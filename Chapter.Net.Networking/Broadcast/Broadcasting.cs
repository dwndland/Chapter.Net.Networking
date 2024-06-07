// -----------------------------------------------------------------------------------------------------------------
// <copyright file="Broadcasting.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Chapter.Net.Networking.Broadcast.Internal;

namespace Chapter.Net.Networking.Broadcast;

/// <summary>
///     Provides possibilities to launch UDP broadcasting server and send messages to them on the network.
/// </summary>
public sealed class Broadcasting : IBroadcasting
{
    private readonly Dictionary<ServerToken, BroadcastServer> _servers;

    /// <summary>
    ///     Creates a new instance of <see cref="Broadcasting" />.
    /// </summary>
    public Broadcasting()
    {
        _servers = new Dictionary<ServerToken, BroadcastServer>();
    }

    /// <summary>
    ///     Starts a new UDP broadcasting server.
    /// </summary>
    /// <param name="configuration">The configuration of the UDP broadcasting server.</param>
    /// <returns>The token which represents the started UDP server. Use on Dispose to stop it.</returns>
    /// <exception cref="ArgumentNullException">configuration is null.</exception>
    public ServerToken Start(ServerConfiguration configuration)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        var token = new ServerToken();
        var server = new BroadcastServer();
        server.ClientMessageReceiving += OnClientMessageReceiving;
        server.ClientMessageReceived += OnClientMessageReceived;
        server.Run(configuration);
        _servers[token] = server;
        return token;
    }

    /// <summary>
    ///     Raised if an accepted client message came in. Its not replied yet.
    /// </summary>
    public event EventHandler<ClientMessageReceivingEventArgs> ClientMessageReceiving;

    /// <summary>
    ///     Raised if an accepted client message came in. Its replied already.
    /// </summary>
    public event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

    /// <summary>
    ///     Stops all created UDP broadcasting servers.
    /// </summary>
    public void Dispose()
    {
        foreach (var pair in _servers)
        {
            pair.Value.ClientMessageReceiving -= OnClientMessageReceiving;
            pair.Value.ClientMessageReceived -= OnClientMessageReceived;
            pair.Value.Dispose();
        }
    }

    /// <summary>
    ///     Sends a message to a UDP broadcasting server in the network.
    /// </summary>
    /// <param name="configuration">The configuration how and what to send.</param>
    /// <param name="callback">The callback if the server replied.</param>
    /// <exception cref="ArgumentNullException">configuration is null.</exception>
    /// <exception cref="ArgumentNullException">callback is null.</exception>
    public void Send(ClientConfiguration configuration, Action<ServerResponse> callback)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        new BroadcastClient().Run(configuration, callback);
    }

    /// <summary>
    ///     Stops the UDP broadcasting server started by the given token.
    /// </summary>
    /// <param name="token">The token of the UDP broadcasting server to stop.</param>
    /// <exception cref="ArgumentNullException">token is null.</exception>
    public void Dispose(ServerToken token)
    {
        if (token == null)
            throw new ArgumentNullException(nameof(token));

        if (!_servers.TryGetValue(token, out var server))
            return;

        server.ClientMessageReceiving -= OnClientMessageReceiving;
        server.ClientMessageReceived -= OnClientMessageReceived;
        server.Dispose();
        _servers.Remove(token);
    }

    private void OnClientMessageReceiving(object sender, ClientMessageReceivingEventArgs e)
    {
        ClientMessageReceiving?.Invoke(sender, e);
    }

    private void OnClientMessageReceived(object sender, ClientMessageReceivedEventArgs e)
    {
        ClientMessageReceived?.Invoke(sender, e);
    }
}