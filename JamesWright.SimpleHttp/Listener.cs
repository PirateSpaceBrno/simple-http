using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
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

            if (routes == null)
                return false;

            // Check by Regex match all stored routes, keep it
            var route = routes.FirstOrDefault(x => x.Key.IsMatch(request.Endpoint));
            //var route = new KeyValuePair<Regex, Action<Request, Response>>();
            //foreach (var x in routes)
            //{
            //    var dd = request.Endpoint;
            //    if (x.Key.Match(dd).Success)
            //    {
            //        var fgf = "";
            //    }
            //}

            if (route.Equals(new KeyValuePair<Regex, Action<Request, Response>>()))
                return false;

            // TODO: Thread pooling!
            await Task.Run(() =>
                {
                    route.Value(request, new Response(context.Response));
                    //routes[request.Endpoint](request, new Response(context.Response));
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
