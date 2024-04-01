// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ServerConfiguration.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;

namespace Chapter.Net.Networking.Broadcast
{
    /// <summary>
    ///     The server configuration when to response on what port.
    /// </summary>
    public sealed class ServerConfiguration
    {
        /// <summary>
        ///     Creates a new instance of <see cref="ServerConfiguration" />.
        /// </summary>
        /// <param name="port">The port the server listens to.</param>
        /// <param name="responseMessage">The message to reply if the filter confirmed.</param>
        /// <param name="filter">The filter on what message to reply.</param>
        /// <exception cref="ArgumentException">invalid port.</exception>
        /// <exception cref="ArgumentNullException">responseMessage is null.</exception>
        /// <exception cref="ArgumentNullException">filter is null.</exception>
        public ServerConfiguration(int port, string responseMessage, Func<string, bool> filter)
        {
            if (port <= 0)
                throw new ArgumentException("invalid port", nameof(port));
            if (string.IsNullOrWhiteSpace(responseMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(responseMessage));

            Port = port;
            ResponseMessage = responseMessage;
            Filter = filter ?? throw new ArgumentNullException(nameof(filter));
        }

        /// <summary>
        ///     The port the server listens to.
        /// </summary>
        public int Port { get; }

        /// <summary>
        ///     The message to reply if the filter confirmed.
        /// </summary>
        public string ResponseMessage { get; }

        /// <summary>
        ///     The filter on what message to reply.
        /// </summary>
        public Func<string, bool> Filter { get; }
    }
}