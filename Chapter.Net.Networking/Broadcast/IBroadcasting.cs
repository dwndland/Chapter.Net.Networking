// -----------------------------------------------------------------------------------------------------------------
// <copyright file="IBroadcasting.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;

namespace Chapter.Net.Networking.Broadcast;

/// <summary>
///     Provides possibilities to launch UDP broadcasting server and send messages to them on the network.
/// </summary>
public interface IBroadcasting : IDisposable
{
    /// <summary>
    ///     Starts a new UDP broadcasting server.
    /// </summary>
    /// <param name="configuration">The configuration of the UDP broadcasting server.</param>
    /// <returns>The token which represents the started UDP server. Use on Dispose to stop it.</returns>
    ServerToken Start(ServerConfiguration configuration);

    /// <summary>
    ///     Raised if an accepted client message came in. Its not replied yet.
    /// </summary>
    event EventHandler<ClientMessageReceivingEventArgs> ClientMessageReceiving;

    /// <summary>
    ///     Raised if an accepted client message came in. Its replied already.
    /// </summary>
    event EventHandler<ClientMessageReceivedEventArgs> ClientMessageReceived;

    /// <summary>
    ///     Sends a message to a UDP broadcasting server in the network.
    /// </summary>
    /// <param name="configuration">The configuration how and what to send.</param>
    /// <param name="callback">The callback if the server replied.</param>
    void Send(ClientConfiguration configuration, Action<ServerResponse> callback);

    /// <summary>
    ///     Stops the UDP broadcasting server started by the given token.
    /// </summary>
    /// <param name="token">The token of the UDP broadcasting server to stop.</param>
    void Dispose(ServerToken token);
}