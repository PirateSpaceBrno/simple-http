using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace JamesWright.SimpleHttp
{
    public class SimpleHttpServer
    {
        private Server server;

        public SimpleHttpServer()
        {
            this.server = new Server(new Listener(), new RouteRepository());
        }

        public void Start (string gateway = "localhost", int port = 8005)
        {
            AutoResetEvent keepAlive = new AutoResetEvent(false);
            this.server.StartAsync(gateway, Convert.ToString(port));
            keepAlive.WaitOne();
        }

        public void Get(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Get.Add(new Regex(endpoint), handler);
        }

        public void Post(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Post.Add(new Regex(endpoint), handler);
        }

        public void Put(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Put.Add(new Regex(endpoint), handler);
        }

        public void Delete(string endpoint, Action<Request, Response> handler)
        {
            this.server.RouteRepository.Delete.Add(new Regex(endpoint), handler);
        }
    }
}
