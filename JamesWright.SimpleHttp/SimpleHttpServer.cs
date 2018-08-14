using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace JamesWright.SimpleHttp
{
    public class SimpleHttpServer
    {
        private Server server;
        private RouteRepository routeRepository;

        public SimpleHttpServer()
        {
            routeRepository = new RouteRepository();
            this.server = new Server(new Listener(), routeRepository);
        }

        public void Start(string gateway = "localhost", int port = 8005)
        {
            AutoResetEvent keepAlive = new AutoResetEvent(false);
            this.server.StartAsync(gateway, Convert.ToString(port));
            keepAlive.WaitOne();
        }

        public void Get(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Get.Add(new Regex(endpoint), handler);
        }
        public void Get(KeyValuePair<Regex, Action<Request, Response>> endpointHandler)
        {
            this.server.RouteRepository.Get.Add(endpointHandler.Key, endpointHandler.Value);
        }

        public void Post(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Post.Add(new Regex(endpoint), handler);
        }
        public void Post(KeyValuePair<Regex, Action<Request, Response>> endpointHandler)
        {
            this.server.RouteRepository.Post.Add(endpointHandler.Key, endpointHandler.Value);
        }

        public void Put(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Put.Add(new Regex(endpoint), handler);
        }
        public void Put(KeyValuePair<Regex, Action<Request, Response>> endpointHandler)
        {
            this.server.RouteRepository.Put.Add(endpointHandler.Key, endpointHandler.Value);
        }

        public void Delete(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Delete.Add(new Regex(endpoint), handler);
        }
        public void Delete(KeyValuePair<Regex, Action<Request, Response>> endpointHandler)
        {
            this.server.RouteRepository.Delete.Add(endpointHandler.Key, endpointHandler.Value);
        }

        public RouteRepository RouteRepository => routeRepository;
    }
}
