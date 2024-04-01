// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ServerToken.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;

namespace Chapter.Net.Networking.Broadcast
{
    /// <summary>
    ///     The token which represents a single broadcasting server.
    /// </summary>
    public sealed class ServerToken
    {
        private readonly Guid _guid;

        internal ServerToken()
        {
            _guid = Guid.NewGuid();
        }

        private bool Equals(ServerToken other)
        {
            return _guid.Equals(other._guid);
        }

        /// <summary>
        ///     Compares the token with another.
        /// </summary>
        /// <param name="obj">The other token.</param>
        /// <returns>True if the token equals; otherwise false.</returns>
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || (obj is ServerToken other && Equals(other));
        }

        /// <summary>
        ///     Gets the hash code of this token.
        /// </summary>
        /// <returns>The hash code of this token.</returns>
        public override int GetHashCode()
        {
            return _guid.GetHashCode();
        }
    }
}