// -----------------------------------------------------------------------------------------------------------------
// <copyright file="NoTokenHandler.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

namespace Chapter.Net.Networking.Api
{
    /// <inheritdoc />
    public class NoTokenHandler : ITokenHandler
    {
        /// <inheritdoc />
        public void PrepareClient(HttpClient client)
        {
        }

        /// <inheritdoc />
        public bool NeedsTokenRefresh(HttpResponseMessage result)
        {
            return false;
        }

        /// <inheritdoc />
        public Task<bool> RequestToken(HttpClient client)
        {
            return Task.FromResult(false);
        }
    }
}