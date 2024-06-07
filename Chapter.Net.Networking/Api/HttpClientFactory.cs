// -----------------------------------------------------------------------------------------------------------------
// <copyright file="HttpClientFactory.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Chapter.Net.Networking.Api;

/// <inheritdoc />
public class HttpClientFactory : IHttpClientFactory
{
    private readonly Uri _address;

    /// <summary>
    ///     Creates a new instance of the HttpClientFactory.
    /// </summary>
    /// <param name="address">The server address with protocol and port.</param>
    public HttpClientFactory(string address)
    {
        _address = new Uri(address);
    }

    /// <summary>
    ///     Creates a new instance of the HttpClientFactory.
    /// </summary>
    /// <param name="address">The server address with protocol and port.</param>
    public HttpClientFactory(Uri address)
    {
        _address = address;
    }

    /// <summary>
    ///     Creates a new instance of the HttpClientFactory.
    /// </summary>
    /// <param name="protocol">The protocol.</param>
    /// <param name="address">The address.</param>
    /// <param name="port">The port.</param>
    public HttpClientFactory(string protocol, string address, uint port)
    {
        _address = new Uri($"{protocol}://{address}:{port}/");
    }

    /// <inheritdoc />
    public HttpClient GetClient()
    {
        var client = new HttpClient { BaseAddress = _address };
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return client;
    }
}