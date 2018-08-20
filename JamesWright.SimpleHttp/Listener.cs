using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JamesWright.SimpleHttp
{
    class Listener
    {
        private HttpListener httpListener;
        private HttpListenerContext context;

        public Listener()
        {
            this.httpListener = new HttpListener();
        }

        public async Task StartAsync(string gateway, string port, RouteRepository routeRepository)
        {
            this.httpListener.Prefixes.Add(string.Format("http://{0}:{1}/", gateway, port));
            this.httpListener.Start();

            Console.WriteLine("Listening for requests on gateway {0} port {1}.", gateway, port);

            Request request = await GetNextRequestAsync();

            while (request != null)
            {
                Console.WriteLine("{0}: {1} {2}", DateTime.Now, request.Method, request.Endpoint);

                if (!await TryRespondAsync(request, routeRepository))
                    Console.WriteLine("HTTP 404 for {0}.", request.Endpoint);

                request = await GetNextRequestAsync();
            }
        }

        private async Task<bool> TryRespondAsync(Request request, RouteRepository routeRepository)
        {
            Dictionary<Regex, Action<Request, Response>> routes = routeRepository.GetRoutes(request.Method);

            // Enable CORS
            var response = context.Response;
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, Accept, X-Requested-With");

            if (routes == null && request.Method != Methods.Options)
            {
                return false;
            }

            var endpoint = request.Endpoint;
            endpoint = endpoint.Split('?')[0];

            // Check by Regex match all stored routes, keep it
            KeyValuePair < Regex, Action < Request, Response >> route = new KeyValuePair<Regex, Action<Request, Response>>();
            if (request.Method != Methods.Options)
            {
                route = routes.FirstOrDefault(x => x.Key.IsMatch(endpoint));
            }
            else
            {
                route = routeRepository.GetRoutes(Methods.Get).FirstOrDefault(x => x.Key.IsMatch("/"));
            }

            // If route does not exist, return 404 
            if (route.Equals(new KeyValuePair<Regex, Action<Request, Response>>()))
            {
                await Task.Run(() =>
                {
                    try
                    {
                        Actions.Error404(request, new Response(response));
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine($"WARN - {ex.Message}");
                    }
                });

                return false;
            }

            // TODO: Thread pooling!
            await Task.Run(() =>
                {
                    try
                    {
                        
                        route.Value(request, new Response(response));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"WARN - {ex.Message}");
                    }
                });

            return true;
        }

        private async Task<Request> GetNextRequestAsync()
        {
            try
            {
                this.context = await this.httpListener.GetContextAsync();
                HttpListenerRequest httpRequest = this.context.Request;
                return new Request(httpRequest);
            }
            catch (Exception)
            {
                //TODO: output/log exception
                return null;
            }
        }
    }
}
