namespace JamesWright.SimpleHttp
{
    public static class StatusCodes
    {
        public static class Success
        {
            public const int Ok = 200;
            public const int Created = 201;
            public const int NoContent = 204;
        }

        public static class Redirection
        {
            public const int MovedPermanently = 301;
            public const int NotModified = 304;
        }

        public static class ClientError
        {
            public const int BadRequest = 400;
            public const int Unauthorized = 401;
            public const int Forbidden = 403;
            public const int NotFound = 404;
            public const int Gone = 410;
        }

        public static class ServerError
        {
            public const int InternalServerError = 500;
            public const int ServiceUnavailable = 503;
        }
    }
}
