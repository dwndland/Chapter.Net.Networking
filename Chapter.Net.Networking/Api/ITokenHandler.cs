// -----------------------------------------------------------------------------------------------------------------
// <copyright file="ITokenHandler.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

namespace Chapter.Net.Networking.Api
{
    /// <summary>
    ///     Takes care about api security tokens.
    /// </summary>
    public interface ITokenHandler
    {
        /// <summary>
        ///     Prepares the http client for the next call.
        /// </summary>
        /// <param name="client">The http client to prepare.</param>
        void PrepareClient(HttpClient client);

        /// <summary>
        ///     Checks if the call result says that the token need a refresh.
        /// </summary>
        /// <param name="result">The response message from the previous call.</param>
        /// <returns>True if the token is expired.</returns>
        bool NeedsTokenRefresh(HttpResponseMessage result);

        /// <summary>
        ///     Refreshes the connection token.
        /// </summary>
        /// <param name="client">The http client.</param>
        /// <returns>True if the token is updated and the previous call can be repeated.</returns>
        Task<bool> RequestToken(HttpClient client);
    }
}