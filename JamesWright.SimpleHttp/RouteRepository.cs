using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JamesWright.SimpleHttp
{
    class RouteRepository
    {
        public Dictionary<Regex, Action<Request, Response>> Get { get; private set; }
        public Dictionary<Regex, Action<Request, Response>> Post { get; private set; }
        public Dictionary<Regex, Action<Request, Response>> Put { get; private set; }
        public Dictionary<Regex, Action<Request, Response>> Delete { get; private set; }

        public RouteRepository()
        {
            Get = new Dictionary<Regex, Action<Request, Response>>();
            Post = new Dictionary<Regex, Action<Request, Response>>();
            Put = new Dictionary<Regex, Action<Request, Response>>();
            Delete = new Dictionary<Regex, Action<Request, Response>>();
        }

        public Dictionary<Regex, Action<Request, Response>> GetRoutes(string method)
        {
            switch (method)
            {
                case Methods.Get:
                    return Get;
                case Methods.Post:
                    return Post;
                case Methods.Put:
                    return Put;
                case Methods.Delete:
                    return Delete;
                default:
                    return null;
            }
        }
    }
}
