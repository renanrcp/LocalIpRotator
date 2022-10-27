// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;

namespace LocalIpRotator;

/// <summary>
/// A Dns Provider.
/// </summary>
public interface IDnsProvider
{
    /// <summary>
    /// Gets a Dns <see cref="IPHostEntry" /> for the specified <paramref name="dnsEndPoint" />.
    /// </summary>
    /// <param name="dnsEndPoint">The host address to get the Dns <see cref="IPHostEntry" />.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to signal the asynchronous operation should
    /// be canceled.</param>
    /// <returns>A Dns <see cref="IPHostEntry" /> for the specified <paramref name="dnsEndPoint" />.</returns>
    public Task<IPHostEntry> GetDnsHostEntryAsync(DnsEndPoint dnsEndPoint, CancellationToken cancellationToken = default);
}
