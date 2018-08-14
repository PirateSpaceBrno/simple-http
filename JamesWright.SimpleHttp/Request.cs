using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JamesWright.SimpleHttp
{
    public class Request
    {
        private HttpListenerRequest httpRequest;
        private string body;

        internal Request(HttpListenerRequest httpRequest)
        {
            this.httpRequest = httpRequest;
        }

        public string[] Parameters { get; private set; }


        public string Endpoint
        {
            get { return this.httpRequest.RawUrl; }
        }

        public string Method
        {
            get { return this.httpRequest.HttpMethod; }
        }

        public NameValueCollection Headers
        {
            get { return this.httpRequest.Headers; }
        }

        public string[] UserLanguages
        {
            get { return this.httpRequest.UserLanguages; }
        }

        public NetworkCredential UserIdentity
        {
            get
            {
                var auth = this.httpRequest.Headers["Authorization"];

                if (auth != null && auth.StartsWith("Basic", StringComparison.InvariantCultureIgnoreCase))
                {
                    //Extract credentials
                    string encodedUsernamePassword = auth.Substring("Basic ".Length).Trim();

                    //the coding should be iso or you could use ASCII and UTF-8 decoder
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                    int seperatorIndex = usernamePassword.IndexOf(':');

                    var result = new NetworkCredential()
                    {
                        Domain = "creds",
                        UserName = usernamePassword.Substring(0, seperatorIndex),
                        Password = usernamePassword.Substring(seperatorIndex + 1)
                    };

                    return result;
                }
                else if (auth != null && auth.StartsWith("Token", StringComparison.InvariantCultureIgnoreCase))
                {
                    //Extract token
                    string token = auth.Substring("Token ".Length).Trim();

                    return new NetworkCredential()
                    {
                        Domain = "token",
                        Password = token
                    };
                }
                else
                {
                    return null;
                }
            }
        }


        public async Task<string> GetBodyAsync()
        {
            //TODO: handle exceptions
            if (Method == Methods.Get || !this.httpRequest.HasEntityBody)
                return null;

            if (this.body == null)
            {
                byte[] buffer = new byte[this.httpRequest.ContentLength64];
                using (Stream inputStream = this.httpRequest.InputStream)
                {
                    await inputStream.ReadAsync(buffer, 0, buffer.Length);
                }

                this.body = Encoding.UTF8.GetString(buffer);
            }

            return this.body;
        }
    }
}
