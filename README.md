# SimpleHttp

A basic HTTP server framework written in C#.

## License
[GNU GPL](https://gnu.org/licenses/gpl.html), "giving you legal permission to copy, distribute and/or modify it."

## Feature requests
I'm not actively developing this project at present, but I want to revive it. It is being used in an academic setting. If I can add any functionality to assist your project, feel free to get in touch. Alternatively, I'm open to pull requests.

## Usage

    using JamesWright.SimpleHttp;
    using System.Threading;
    using System.Threading.Tasks;
    
    namespace JamesWright.SimpleHttp.Example
    {
        class Program
        {
            static void Main(string[] args)
            {
                App app = new App();
    
                app.Get(@"^/$", async (req, res) =>
                {
                    res.Content = "<p>You did a GET.</p>";
                    res.ContentType = "text/html";
                    await res.SendAsync();
                });
    
                app.Post(@"^/$", async (req, res) =>
                {
                    res.Content = "<p>You did a POST: " + await req.GetBodyAsync() + "</p>";
                    res.ContentType = "text/html";
                    await res.SendAsync();
                });

                app.Put(@"^/$", async (req, res) =>
                {
                    res.Content = "<p>You did a PUT: " + await req.GetBodyAsync() + "</p>";
                    res.ContentType = "text/html";
                    await res.SendAsync();
                });
    
                app.Delete(@"^/$", async (req, res) =>
                {
                    res.Content = "<p>You did a DELETE: " + await req.GetBodyAsync() + "</p>";
                    res.ContentType = "text/html";
                    await res.SendAsync();
                });
    
                app.Start();
            }
        }
    }
	
	
## APIs

### `App` ###
Represents an application, served over HTTP, and the requests for which it will listen.

#### Public properties and methods #####
##### `void Get(string endpointRegex, Action<Request, Response> handler)` ######
Adds a handler for a HTTP GET request to the requested endpoint matched by regex.

##### `void Post(string endpointRegex, Action<Request, Response> handler)` ######
Adds a handler for a HTTP POST request to the requested endpoint matched by regex.

##### `void Put(string endpointRegex, Action<Request, Response> handler)` ######
Adds a handler for a HTTP PUT request to the requested endpoint matched by regex.

##### `void Delete(string endpointRegex, Action<Request, Response> handler)` ######
Adds a handler for a HTTP DELETE request to the requested endpoint matched by regex.

##### `void Start(string gateway = "localhost", int port = 8005)`#####
Initialises the server and its underlying listener. Port number and listening gateway can be optionally specified.

### `Request` ###
A HTTP request, and its underlying information, that is sent to the server.

#### Public properties and methods #####
##### `string Endpoint { get; }` ######
Returns the endpoint that the Request instance represents e.g. "/".

##### `string[] Parameters { get; }`#####
Contains the parameters sent with the HTTP request. Currently not populated.

##### `async Task<string> GetBodyAsync()` ######
Returns the request's body asynchronously.

##### `NameValueCollection Headers { get; }` #####
Returns collection of request headers.

##### `string[] UserLanguages { get; }` #####
Returns list of user languages (from browser).

##### `NetworkCredential UserIdentity { get; }` #####
Returns object with credentials grabbed from Authorization header.

### `Response` ###
A response to be sent to the user.

#### Public properties and methods #####
##### `string Content { get; set; }` ######
The body content to be returned to the user.

##### `string ContentType { get; set; }`#####
The Internet media type (MIME) of the response e.g. "application/json".

##### `int StatusCode { get; set; }`#####
Status code for response. Default is 200=OK.

##### `async Task SendAsync()` #####
Sends the response asynchronously.

## Roadmap

* Request parameters
* JSON
* NuGet
* HTTPS/SSL
* Memory management
* Unit tests
