// -----------------------------------------------------------------------------------------------------------------
// <copyright file="BroadcastServer.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter.Net.Networking.Broadcast.Internal;

internal sealed class BroadcastServer : IDisposable
{
    private UdpClient _server;
    private CancellationTokenSource _tokenSource;

    public void Dispose()
    {
        _server.Close();
        _tokenSource.Cancel();
    }

    internal event EventHandler<ClientMessageReceivingEventArgs> ClientMessageReceiving;

    internal event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

    internal void Run(ServerConfiguration configuration)
    {
        _tokenSource = new CancellationTokenSource();
        var cancelToken = _tokenSource.Token;

        Task.Run(() =>
        {
            _server = new UdpClient(configuration.Port);
            var response = Encoding.ASCII.GetBytes(configuration.ResponseMessage);

            while (true)
                try
                {
                    var client = new IPEndPoint(IPAddress.Any, 0);

                    if (cancelToken.IsCancellationRequested)
                        cancelToken.ThrowIfCancellationRequested();

                    var messageData = _server.Receive(ref client);

                    if (cancelToken.IsCancellationRequested)
                        cancelToken.ThrowIfCancellationRequested();

                    var message = Encoding.ASCII.GetString(messageData);

                    OnClientMessageReceiving(client.Address, message, configuration);

                    if (configuration.Filter(message))
                    {
                        OnClientMessageReceived(client.Address, message, configuration);
                        _server.Send(response, response.Length, client);
                    }
                }
                catch (ObjectDisposedException)
                {
                    //_server.Close which cancels the _server.Receive
                    _tokenSource.Dispose();
                    break;
                }
                catch (SocketException)
                {
                    //_server.Close which cancels the _server.Receive
                    _tokenSource.Dispose();
                    break;
                }
                catch (OperationCanceledException)
                {
                    _tokenSource.Dispose();
                    break;
                }
                catch
                {
                    // any other error, retry
                }
        }, cancelToken);
    }

    private void OnClientMessageReceiving(IPAddress address, string message, ServerConfiguration configuration)
    {
        ClientMessageReceiving?.Invoke(this, new ClientMessageReceivingEventArgs(address, message, configuration));
    }

    private void OnClientMessageReceived(IPAddress address, string message, ServerConfiguration configuration)
    {
        ClientMessageReceived?.Invoke(this, new ClientMessageReceivedEventArgs(address, message, configuration));
    }
}