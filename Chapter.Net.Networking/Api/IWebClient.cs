// -----------------------------------------------------------------------------------------------------------------
// <copyright file="IWebClient.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;

namespace Chapter.Net.Networking.Api
{
    /// <summary>
    ///     Provides easy way to call a web api.
    /// </summary>
    public interface IWebClient
    {
        /// <summary>
        ///     Executes http get.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The http get return value.</returns>
        Task<HttpResponseMessage> GetAsync(string route);

        /// <summary>
        ///     Executes http put without a parameter.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The http put return value.</returns>
        Task<HttpResponseMessage> PutAsync(string route);

        /// <summary>
        ///     Executes http put with a parameter.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="parameter">The parameter to transmit.</param>
        /// <returns>The http put return value.</returns>
        Task<HttpResponseMessage> PutAsync(string route, object parameter);

        /// <summary>
        ///     Executes http delete.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The http post return value.</returns>
        Task<HttpResponseMessage> DeleteAsync(string route);

        /// <summary>
        ///     Executes http post without a parameter.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The http post return value.</returns>
        Task<HttpResponseMessage> PostAsync(string route);

        /// <summary>
        ///     Executes http post wit a parameter.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="parameter">The parameter to transmit.</param>
        /// <returns>The http get return value.</returns>
        Task<HttpResponseMessage> PostAsync(string route, object parameter);

#if !NET451 && !NETSTANDARD2_0
        /// <summary>
        ///     Executes http patch without a parameter.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The http patch return value.</returns>
        Task<HttpResponseMessage> PatchAsync(string route);

        /// <summary>
        ///     Executes http patch with a parameter.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <param name="parameter">The parameter to transmit.</param>
        /// <returns>The http patch return value.</returns>
        Task<HttpResponseMessage> PatchAsync(string route, object parameter);
#endif
        /// <summary>
        ///     Deserializes the content of the given The http response message from a previous call to an object.
        /// </summary>
        /// <typeparam name="T">The expected target type.</typeparam>
        /// <param name="result">The http response message from a previous call.</param>
        /// <returns>The deserialized object.</returns>
        Task<T> AsAsync<T>(HttpResponseMessage result);
    }
}