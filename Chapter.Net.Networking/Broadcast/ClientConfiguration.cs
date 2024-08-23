// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ClientConfiguration.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;

namespace Chapter.Net.Networking.Broadcast;

/// <summary>
///     The configuration data of the broadcasting client what to send to an UDP broadcasting server on which port.
/// </summary>
public sealed class ClientConfiguration
{
    /// <summary>
    ///     Creates a new instance of ClientConfiguration
    /// </summary>
    /// <param name="port">The port to send the UDP message on.</param>
    /// <param name="message">The message to send on the UDP.</param>
    /// <param name="timeout">The timeout how long to wait for a UDP server response.</param>
    /// <exception cref="ArgumentException">invalid port.</exception>
    /// <exception cref="ArgumentNullException">message is null.</exception>
    /// <exception cref="ArgumentException">timeout cannot be zero.</exception>
    public ClientConfiguration(int port, string message, TimeSpan timeout)
    {
        if (port <= 0)
            throw new ArgumentException("invalid port", nameof(port));
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("message cannot be null or whitespace.", nameof(message));
        if (timeout == TimeSpan.Zero)
            throw new ArgumentException("timeout cannot be zero", nameof(timeout));

        Port = port;
        Message = message;
        Timeout = timeout;
    }

    /// <summary>
    ///     The port to send the UDP message on.
    /// </summary>
    public int Port { get; }

    /// <summary>
    ///     The message to send on the UDP.
    /// </summary>
    public string Message { get; }

    /// <summary>
    ///     The timeout how long to wait for a UDP server response.
    /// </summary>
    public TimeSpan Timeout { get; }
}