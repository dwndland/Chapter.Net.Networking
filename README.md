<img src="https://raw.githubusercontent.com/dwndlnd/Chapter.Net.Networking/master/Icon.png" alt="logo" width="64"/>

# Chapter.Net.Networking Library

## Overview
Chapter.Net.Networking provides methods and objects for a more easy network communication.

## Features
- **[LocalOnly] attribute:** Mark a API callback method as allowed to be called local only.
- **Broadcasting:** Start a broadcasting server and clients to find each other on a local network or sending general messages.
- **WebClient** Easy calls to a web API include exception handling and authentication.

## Getting Started

1. **Installation:**
    - Install the Chapter.Net.Networking library via NuGet Package Manager:
    ```bash
    dotnet add package Chapter.Net.Networking
    ```

### **[LocalOnly]**

```csharp
public class SetupController : ApiController
{
    [LocalOnly]
    [HttpPost(Routes.Setup)]
    public void Setup()
    {
    }
}
```

### **Broadcasting**

- Start a server
```csharp
public class BroadcastServer
{
    private readonly IBroadcasting _broadcasting;
    private ServerToken _token;

    public BroadcastServer(IBroadcasting broadcasting)
    {
        _broadcasting = broadcasting;
    }

    public void StartServer()
    {
        _token = _broadcasting.Start(new ServerConfiguration(37455, "Hello Client", s => s == "Hello Server"));
        _broadcasting.ClientMessageReceiving += OnClientMessageReceiving;
        _broadcasting.ClientMessageReceived += OnClientMessageReceived;
    }

    private void OnClientMessageReceiving(object sender, ClientMessageReceivingEventArgs e)
    {
        Console.WriteLine($"'{e.Address}' has send '{e.Message}' and we will reply '{e.Configuration.ResponseMessage}'");
    }

    private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
    {
        Console.WriteLine($"'{e.Address}' has send '{e.Message}' and we replied with '{e.Configuration.ResponseMessage}'");
    }

    public void StopServer()
    {
        _broadcasting.Dispose(_token);
    }
}
```

- Use client to communicate with the server
```csharp
public class BroadcastClient
{
    private readonly IBroadcasting _broadcasting;

    public BroadcastClient(IBroadcasting broadcasting)
    {
        _broadcasting = broadcasting;
    }

    public void SendBroadcast()
    {
        var configuration = new ClientConfiguration(37455, "Hello Server", TimeSpan.FromSeconds(10));
        _broadcasting.Send(configuration, response =>
        {
            Console.WriteLine($"'{response.Address}' as reply to the '{response.Configuration.Message}' with '{response.Message}'");
        });
    }
}
```

### **WebClient**

1. **Prepare http client using the IHttpClientFactory**
The WebClient internally uses the normal HttpClient object.
The IHttpClientFactory is there to create and prepare the http client ready to use.
There is a build in HttpClientFactory which prepares the http client with the media type "application/json".
If you want to use a different media type, or need other preparation on the http client, just implement an own IHttpClientFactory.

2. **Handle exceptions using the IRequestExceptionHandler**
If an http call raises a HttpRequestException or an Exception, the IRequestExceptionHandler is called to decide what to return to the caller of the WebClient.
There is a build in RequestExceptionToTimeoutHandler which converts both exception types to a return HttpStatusCode.RequestTimeout and another build in RequestExceptionThrowHandler which rethrows the exception.
If you want to have a different return or just rethrow, implement an own IRequestExceptionHandler.

3. **Prepare data to send using the IDataFactory**
Whenever you want to send data to the server, sometimes you just want to send a json string, or a multipart.
For that to prepare the data on the call the IDataFactory is called.
There is build in JsonDataFactory which does nothing but serialize everything to json.
To have different types like multipart, just implement the IDataFactory and return the HttpContent by the object type you want to send.

4. **Handle authentication using the ITokenHandler**
The ITokenHandler is responsible to maintain authentication tokens.
The PrepareClient allows to set the authentication on the web client which is called on every API request.
The NeedsTokenRefresh checks if if the API response says we need a new token, and the RequestToken is there just to refresh the token.
There is a build in NoTokenHandler implementation which does no token handling.
That is to use if the token is handled manually from outside or the web API has no token handling at all.

#### WebClient Examples
A. Send data to a web API without Authentication and json only.
    ```csharp
    public class Book
    {
        public string Author { get; set; }
        public string Name { get; set; }
    }
    ```
    ```csharp
    public class BooksService
    {
        private WebClient _client;

        public async Task<bool> Connect()
        {
            var httpClientFactory = new HttpClientFactory("http", "198.168.27.44", 47359);
            var exceptionHandler = new RequestExceptionToTimeoutHandler();
            var tokenHandler = new NoTokenHandler();
            var dataFactory = new JsonDataFactory();

            _client = new WebClient(httpClientFactory, exceptionHandler, tokenHandler, dataFactory);
            return (await _client.GetAsync("api/v1/ping")).IsSuccessStatusCode;
        }

        public async Task SendData(Book book)
        {
            await _client.PostAsync("api/v1/books", book);
        }
    }
    ```

B. Receive data from the web API without Authentication and json only.
    ```csharp
    public class Book
    {
        public string Author { get; set; }
        public string Name { get; set; }
    }
    ```
    ```csharp
    public class BooksService
    {
        private WebClient _client;

        public async Task<bool> Connect()
        {
            var httpClientFactory = new HttpClientFactory("http", "198.168.27.44", 47359);
            var exceptionHandler = new RequestExceptionToTimeoutHandler();
            var tokenHandler = new NoTokenHandler();
            var dataFactory = new JsonDataFactory();

            _client = new WebClient(httpClientFactory, exceptionHandler, tokenHandler, dataFactory);
            return (await _client.GetAsync("api/v1/ping")).IsSuccessStatusCode;
        }

        public async Task<List<Book>> SearchBooks(string query)
        {
            var result = await _client.GetAsync("api/v1/books?query=" + query);
            return await _client.AsAsync<List<Book>>(result);
        }
    }
    ```

C. Send a file to a web API using Multipart.
    ```csharp
    public class FileUploadRequest
    {
        public string FilePath { get; set; }
        public List<Tuple<string, string>> MetaData { get; } = [];

        public void AddMetaData(string key, string value)
        {
            MetaData.Add(Tuple.Create(key, value));
        }
    }
    ```
    ```csharp
    public class MyDataFactory : JsonDataFactory
    {
        public override HttpContent GenerateHttpContent(object data)
        {
            return data switch
            {
                FileUploadRequest file => CreateMultiPart(file),
                _ => base.GenerateHttpContent(data)
            };
        }

        private HttpContent CreateMultiPart(FileUploadRequest request)
        {
            var multiPart = new MultipartFormDataContent();
            multiPart.Add(new StreamContent(File.OpenRead(request.FilePath)), "files", Path.GetFileName(request.FilePath));
            foreach (var parameter in request.MetaData)
                multiPart.Add(new StringContent(parameter.Item1), parameter.Item2);
            return multiPart;
        }
    }
    ```
    ```csharp
    public class FilesService
    {
        private WebClient _client;

        public async Task<bool> Connect()
        {
            var httpClientFactory = new HttpClientFactory("http", "198.168.27.44", 47359);
            var exceptionHandler = new RequestExceptionToTimeoutHandler();
            var tokenHandler = new NoTokenHandler();
            var dataFactory = new MyDataFactory();

            _client = new WebClient(httpClientFactory, exceptionHandler, tokenHandler, dataFactory);
            return (await _client.GetAsync("api/v1/ping")).IsSuccessStatusCode;
        }

        public async Task<bool> UploadFile(string filePath)
        {
            var data = new FileUploadRequest { FilePath = filePath };
            await using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data.AddMetaData("hash", _hashing.GenerateMD5Hash(stream));
            }
        
            var result = await _client.PostAsync("api/v1/file" + data);
            return result.IsSuccessStatusCode;
        }
    }
    ```

D. Receive a file from a web API using Multipart.
    ```csharp
    public class FilesService
    {
        private WebClient _client;

        public async Task<bool> Connect()
        {
            var httpClientFactory = new HttpClientFactory("http", "198.168.27.44", 47359);
            var exceptionHandler = new RequestExceptionToTimeoutHandler();
            var tokenHandler = new NoTokenHandler();
            var dataFactory = new MyDataFactory();

            _client = new WebClient(httpClientFactory, exceptionHandler, tokenHandler, dataFactory);
            return (await _client.GetAsync("api/v1/ping")).IsSuccessStatusCode;
        }

        public async Task<Stream> DownloadFile()
        {
            var result = await _client.GetAsync("api/v1/file/13");
            switch (result.StatusCode)
            {
                case HttpStatusCode.OK:
                    //var fileName = result.Content.Headers.ContentDisposition.FileName;
                    return await result.Content.ReadAsStreamAsync();
                case HttpStatusCode.NotFound:
                    return null;
                default:
                    throw new Exception("Download file failed.");
            }
        }
    }
    ```

E. Auto token refresh with call repeat
    ```csharp
    public class Book
    {
        public string Author { get; set; }
        public string Name { get; set; }
    }
    ```
    ```csharp
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    ```
    ```csharp
    public class LoginResult
    {
        public string Token { get; set; }
        public string RepeatToken { get; set; }
    }
    ```
    ```csharp
    public class JwtBearerTokenHandler : ITokenHandler
    {
        private string _token;
        private string _refreshToken;

        public void PrepareClient(HttpClient client)
        {
            var authValue = new AuthenticationHeaderValue("Bearer", _token);
            client.DefaultRequestHeaders.Authorization = authValue;
        }

        public bool NeedsTokenRefresh(HttpResponseMessage result)
        {
            if (result == null)
                return false;

            if (result.StatusCode != HttpStatusCode.Unauthorized)
                return false;

            if (!result.Headers.TryGetValues("Reason", out var reasons))
                return false;

            return reasons.Contains("Token_Expired");
        }

        public async Task<bool> RequestToken(HttpClient client)
        {
            var response = await client.PatchAsync(
                "api/v1.0/sessions/refresh",
                new StringContent(JsonSerializer.Serialize(new { Token = _token, RefreshToken = _refreshToken }), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var resultJson = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LoginResult>(resultJson);
                Keep(result);
                return true;
            }

            return false;
        }

        public void Keep(LoginResult result)
        {
            _token = result.Token;
            _refreshToken = result.RepeatToken;
        }
    }
    ```
    ```csharp
    public class BooksService
    {
        private JwtBearerTokenHandler _tokenHandler;
        private WebClient _client;

        public async Task<bool> Connect()
        {
            var httpClientFactory = new HttpClientFactory("http", "198.168.27.44", 47359);
            var exceptionHandler = new RequestExceptionToTimeoutHandler();
            _tokenHandler = new JwtBearerTokenHandler();
            var dataFactory = new JsonDataFactory();

            _client = new WebClient(httpClientFactory, exceptionHandler, _tokenHandler, dataFactory);
            return (await _client.GetAsync("api/v1/ping")).IsSuccessStatusCode;
        }

        public async Task<bool> Login(string username, string password)
        {
            var loginRequest = new LoginRequest { Username = username, Password = password };
            var result = await _client.PostAsync("api/v1.0/sessions/login", loginRequest);
            if (result.IsSuccessStatusCode)
            {
                var data = await _client.AsAsync<LoginResult>(result);
                _tokenHandler.Keep(data);
                return true;
            }

            return false;
        }

        public async Task<List<Book>> SearchBooks(string query)
        {
            var result = await _client.GetAsync("api/v1/books?query=" + query);
            return await _client.AsAsync<List<Book>>(result);
        }
    }
    ```

f. Manual token refresh by click
    ```csharp
    public class Book
    {
        public string Author { get; set; }
        public string Name { get; set; }
    }
    ```
    ```csharp
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    ```
    ```csharp
    public class LoginResult
    {
        public string Token { get; set; }
        public string RepeatToken { get; set; }
    }
    ```
    ```csharp
    public class JwtBearerTokenHandler : ITokenHandler
    {
        private string _token;
        private string _refreshToken;

        public void PrepareClient(HttpClient client)
        {
            var authValue = new AuthenticationHeaderValue("Bearer", _token);
            client.DefaultRequestHeaders.Authorization = authValue;
        }

        public bool NeedsTokenRefresh(HttpResponseMessage result)
        {
            return false;
        }

        public Task<bool> RequestToken(HttpClient client)
        {
            return Task.FromResult(false);
        }

        public void Keep(LoginResult result)
        {
            _token = result.Token;
            _refreshToken = result.RepeatToken;
        }

        public LoginResult Take()
        {
            return new LoginResult
            {
                Token = _token,
                RepeatToken = _refreshToken
            };
        }
    }
    ```
    ```csharp
    public class BooksService
    {
        private JwtBearerTokenHandler _tokenHandler;
        private WebClient _client;

        public async Task<bool> Connect()
        {
            var httpClientFactory = new HttpClientFactory("http", "198.168.27.44", 47359);
            var exceptionHandler = new RequestExceptionToTimeoutHandler();
            _tokenHandler = new JwtBearerTokenHandler();
            var dataFactory = new JsonDataFactory();

            _client = new WebClient(httpClientFactory, exceptionHandler, _tokenHandler, dataFactory);
            return (await _client.GetAsync("api/v1/ping")).IsSuccessStatusCode;
        }

        public async Task<bool> Login(string username, string password)
        {
            var loginRequest = new LoginRequest { Username = username, Password = password };
            var result = await _client.PostAsync("api/v1.0/sessions/login", loginRequest);
            if (result.IsSuccessStatusCode)
            {
                var data = await _client.AsAsync<LoginResult>(result);
                _tokenHandler.Keep(data);
                return true;
            }

            return false;
        }

        // Triggered by a timer from outside, or login has return also the duration how long the token is valid and then check if its expired.
        public async Task RefreshToken()
        {
            var data = _tokenHandler.Take();
            var result = await _client.PostAsync("api/v1.0/sessions/refresh", data);
            if (result.IsSuccessStatusCode)
            {
                var refreshResult = await _client.AsAsync<LoginResult>(result);
                _tokenHandler.Keep(refreshResult);
            }
        }

        public async Task<List<Book>> SearchBooks(string query)
        {
            var result = await _client.GetAsync("api/v1/books?query=" + query);
            return await _client.AsAsync<List<Book>>(result);
        }
    }
    ```

## Links
* [NuGet](https://www.nuget.org/packages/Chapter.Net.Networking)
* [GitHub](https://github.com/dwndlnd/Chapter.Net.Networking)

## License
Copyright (c) David Wendland. All rights reserved.
Licensed under the MIT License. See LICENSE file in the project root for full license information.
