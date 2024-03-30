// -----------------------------------------------------------------------------------------------------------------
// <copyright file="IRequestExceptionHandler.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;

namespace Chapter.Net.Networking.Api;

/// <summary>
///     Handles exceptions created by the web client calls.
/// </summary>
public interface IRequestExceptionHandler
{
    /// <summary>
    ///     Handles the HttpRequestException created by the web client calls.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>The object to use as the web client response.</returns>
    HttpResponseMessage Handle(HttpRequestException exception);

    /// <summary>
    ///     Handles the Exception created by the web client calls.
    /// </summary>
    /// <param name="exception">The exception.</param>
    /// <returns>The object to use as the web client response.</returns>
    HttpResponseMessage Handle(Exception exception);
}