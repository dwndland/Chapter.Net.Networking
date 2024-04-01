// -----------------------------------------------------------------------------------------------------------------
// <copyright file="RequestExceptionHandler.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;

namespace Chapter.Net.Networking.Api
{
    /// <inheritdoc />
    public class RequestExceptionToTimeoutHandler : IRequestExceptionHandler
    {
        /// <inheritdoc />
        public HttpResponseMessage Handle(HttpRequestException _)
        {
            return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
        }

        /// <inheritdoc />
        public HttpResponseMessage Handle(Exception _)
        {
            return new HttpResponseMessage(HttpStatusCode.RequestTimeout);
        }
    }
}