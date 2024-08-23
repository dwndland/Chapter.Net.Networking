// -----------------------------------------------------------------------------------------------------------------
// <copyright file="RequestExceptionThrowHandler.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;

namespace Chapter.Net.Networking.Api;

/// <inheritdoc />
public class RequestExceptionThrowHandler : IRequestExceptionHandler
{
    /// <inheritdoc />
    public HttpResponseMessage Handle(HttpRequestException ex)
    {
        throw ex;
    }

    /// <inheritdoc />
    public HttpResponseMessage Handle(Exception ex)
    {
        throw ex;
    }
}