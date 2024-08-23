// -----------------------------------------------------------------------------------------------------------------
// <copyright file="IHttpClientFactory.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;

namespace Chapter.Net.Networking.Api;

/// <summary>
///     Generates the http client to use for communication.
/// </summary>
public interface IHttpClientFactory
{
    /// <summary>
    ///     Generates the http client to use for communication.
    /// </summary>
    /// <returns>The generated http client.</returns>
    HttpClient GetClient();
}