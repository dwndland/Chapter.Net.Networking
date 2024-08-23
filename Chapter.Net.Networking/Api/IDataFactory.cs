// -----------------------------------------------------------------------------------------------------------------
// <copyright file="IDataFactory.cs" company="dwndland">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;

namespace Chapter.Net.Networking.Api;

/// <summary>
///     Generates the http content to send by the given data.
/// </summary>
public interface IDataFactory
{
    /// <summary>
    ///     Generates the http content to send by the given data.
    /// </summary>
    /// <param name="data">The data to prepare.</param>
    /// <returns>The http content to send by the given data.</returns>
    HttpContent GenerateHttpContent(object data);
}