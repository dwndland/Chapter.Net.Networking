// -----------------------------------------------------------------------------------------------------------------
// <copyright file="JsonDataFactory.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net.Http;
using System.Text;

namespace Chapter.Net.Networking.Api
{
    /// <inheritdoc />
    public class JsonDataFactory : IDataFactory
    {
        /// <inheritdoc />
        public virtual HttpContent GenerateHttpContent(object data)
        {
#if NET451 || NETSTANDARD2_0
            var jsonParameter = Newtonsoft.Json.JsonConvert.SerializeObject(data);
#else
            var jsonParameter = System.Text.Json.JsonSerializer.Serialize(data);
#endif
            return new StringContent(jsonParameter, Encoding.UTF8, "application/json");
        }
    }
}