using System.Net;

namespace JamesWright.SimpleHttp
{
    static class Methods
    {
        public const string Get = WebRequestMethods.Http.Get;
        public const string Post = WebRequestMethods.Http.Post;
        public const string Put = WebRequestMethods.Http.Put;
        public const string Delete = "DELETE";
        public const string Options = "OPTIONS";
    }
}
