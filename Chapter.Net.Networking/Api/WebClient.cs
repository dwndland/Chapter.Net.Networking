// -----------------------------------------------------------------------------------------------------------------
// <copyright file="WebClient.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chapter.Net.Networking.Api;

/// <inheritdoc />
public class WebClient : IWebClient
{
    private readonly Lazy<HttpClient> _client;
    private readonly IDataFactory _dataFactory;
    private readonly IRequestExceptionHandler _exceptionHandler;
    private readonly ITokenHandler _tokenHandler;

    /// <summary>
    ///     Creates a new instance of the WebClient.
    /// </summary>
    /// <param name="httpClientFactory">The factory to create the http client.</param>
    /// <param name="exceptionHandler"></param>
    /// <param name="tokenHandler"></param>
    /// <param name="dataFactory"></param>
    public WebClient(IHttpClientFactory httpClientFactory, IRequestExceptionHandler exceptionHandler, ITokenHandler tokenHandler, IDataFactory dataFactory)
    {
        _exceptionHandler = exceptionHandler;
        _tokenHandler = tokenHandler;
        _dataFactory = dataFactory;
        _client = new Lazy<HttpClient>(httpClientFactory.GetClient);
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> GetAsync(string route)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.GetAsync(route);
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.GetAsync(route);

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PutAsync(string route)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.PutAsync(route, null);
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.PutAsync(route, null);

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PutAsync(string route, object parameter)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.PutAsync(route, _dataFactory.GenerateHttpContent(parameter));
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.PutAsync(route, _dataFactory.GenerateHttpContent(parameter));

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> DeleteAsync(string route)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.DeleteAsync(route);
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.DeleteAsync(route);

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostAsync(string route)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.PostAsync(route, null);
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.PostAsync(route, null);

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PostAsync(string route, object parameter)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.PostAsync(route, _dataFactory.GenerateHttpContent(parameter));
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.PostAsync(route, _dataFactory.GenerateHttpContent(parameter));

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PatchAsync(string route)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.PatchAsync(route, null);
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.PatchAsync(route, null);

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<HttpResponseMessage> PatchAsync(string route, object parameter)
    {
        try
        {
            _tokenHandler.PrepareClient(_client.Value);
            var result = await _client.Value.PatchAsync(route, _dataFactory.GenerateHttpContent(parameter));
            if (_tokenHandler.NeedsTokenRefresh(result))
                if (await _tokenHandler.RequestToken(_client.Value))
                    result = await _client.Value.PatchAsync(route, _dataFactory.GenerateHttpContent(parameter));

            return result;
        }
        catch (HttpRequestException ex)
        {
            return _exceptionHandler.Handle(ex);
        }
        catch (Exception ex)
        {
            return _exceptionHandler.Handle(ex);
        }
    }

    /// <inheritdoc />
    public async Task<T> AsAsync<T>(HttpResponseMessage result)
    {
        var resultJson = await result.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(resultJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}