// -----------------------------------------------------------------------------------------------------------------
// <copyright file="LocalOnlyAttribute.cs" company="my-libraries">
//     Copyright (c) David Wendland. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------------------------------------------------

using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// ReSharper disable once CheckNamespace

namespace Chapter.Net.Networking;

/// <summary>
///     Brings the possibility to limit an controller call to be done by a local software only.
/// </summary>
public class LocalOnlyAttribute : ActionFilterAttribute
{
    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var connection = context.HttpContext.Connection;
        if (!IsLocal(connection))
            context.Result = new ForbidResult();
    }

    private bool IsLocal(ConnectionInfo connection)
    {
        if (connection.RemoteIpAddress != null)
        {
            if (connection.LocalIpAddress != null)
                return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);
            return IPAddress.IsLoopback(connection.RemoteIpAddress);
        }

        // for in memory TestServer or when dealing with default connection info
        return connection.RemoteIpAddress == null && connection.LocalIpAddress == null;
    }
}