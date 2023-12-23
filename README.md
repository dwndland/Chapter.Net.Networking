# Chapter.Net.Networking Library

## Overview
Chapter.Net.Networking provides methods and objects for a more easy network communication.

## Features
- **[LocalOnly] attribute:** Mark a API callback method as allowed to be called local only.
- **Broadcasting:** Start a broadcasting server and clients to find each other on a local network or sending general messages.

## Getting Started

1. **Installation:**
    - Install the Chapter.Net.Networking library via NuGet Package Manager:
    ```bash
    dotnet add package Chapter.Net.Networking
    ```

2. **[LocalOnly]:**
    - Usage
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

2. **Broadcasting:**
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

## Links
* [NuGet](https://www.nuget.org/packages/Chapter.Net.Networking)
* [GitHub](https://github.com/dwndland/Chapter.Net.Networking)

## License
Copyright (c) David Wendland. All rights reserved.
Licensed under the MIT License. See LICENSE file in the project root for full license information.
