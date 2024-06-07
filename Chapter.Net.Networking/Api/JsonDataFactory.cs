// -----------------------------------------------------------------------------------------------------------------
// <copyright file="JsonDataFactory.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Chapter.Net.Networking.Api;

/// <inheritdoc />
public class JsonDataFactory : IDataFactory
{
    /// <inheritdoc />
    public virtual HttpContent GenerateHttpContent(object data)
    {
        var jsonParameter = JsonSerializer.Serialize(data);
        return new StringContent(jsonParameter, Encoding.UTF8, "application/json");
    }
}