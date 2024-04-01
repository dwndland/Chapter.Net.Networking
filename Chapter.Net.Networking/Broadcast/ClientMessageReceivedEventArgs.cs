// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ClientMessageReceivedEventArgs.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net;

namespace Chapter.Net.Networking.Broadcast
{
    /// <summary>
    ///     Raised if a client message just came in and was allowed. Reply has been sent already.
    /// </summary>
    public sealed class ClientMessageReceivedEventArgs : EventArgs
    {
        internal ClientMessageReceivedEventArgs(IPAddress address, string message, ServerConfiguration configuration)
        {
            Address = address;
            Message = message;
            Configuration = configuration;
        }

        /// <summary>
        ///     The IP address of the client.
        /// </summary>
        public IPAddress Address { get; }

        /// <summary>
        ///     The message the client has sent.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     The configuration of the UDP server who got a message.
        /// </summary>
        public ServerConfiguration Configuration { get; }
    }
}