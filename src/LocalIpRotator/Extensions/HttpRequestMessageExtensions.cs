// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;

namespace LocalIpRotator.Extensions;

/// <summary>
/// Some extensions methods for the <see cref="HttpRequestMessage" />.
/// </summary>
public static class HttpRequestMessageExtensions
{
    private const string AddressContextRequestKey = "IPRotatorStrategy::LocalUsedAddres";

    /// <summary>
    /// Set a local address in the request context.
    /// </summary>
    /// <remarks>
    /// This doesn't sets the address to be used in the request, just indicates which has been chosen.
    /// </remarks>
    /// <param name="requestMessage">The request to set in context.</param>
    /// <param name="address">The address to be set.</param>
    public static void SetAddressInContext(this HttpRequestMessage requestMessage, IPAddress address)
    {
        requestMessage.Options.Set(new HttpRequestOptionsKey<IPAddress>(AddressContextRequestKey), address);
    }

    /// <summary>
    /// Try get a local address from the request context.
    /// </summary>
    /// <remarks>
    /// This doesn't returns the address used in the request, just indicates which has been chosen.
    /// </remarks>
    /// <param name="requestMessage">The request to get from context.</param>
    /// <param name="address">The out address.</param>
    /// <returns><see langword="true" /> if can get the address from the request context otherwise <see langword="false" />.</returns>
    public static bool TryGetAddressFromContext(this HttpRequestMessage requestMessage, out IPAddress? address)
    {
        return requestMessage.Options.TryGetValue(new HttpRequestOptionsKey<IPAddress>(AddressContextRequestKey), out address);
    }
}
