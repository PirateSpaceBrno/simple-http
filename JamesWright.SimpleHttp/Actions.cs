using System;

namespace JamesWright.SimpleHttp
{
    public static class Actions
    {
        public static Action<Request, Response> Error404 = new Action<Request, Response>(async (req, res) =>
        {
            res.Content = "Error 404 - Not found";
            res.ContentType = ContentTypes.Html;
            res.StatusCode = StatusCodes.ClientError.NotFound;
            await res.SendAsync();
        });

        public static object FirstOrDefault(Func<object, bool> p)
        {
            throw new NotImplementedException();
        }
    }
}
