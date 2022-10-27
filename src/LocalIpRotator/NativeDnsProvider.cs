// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;
using System.Net.Sockets;

namespace LocalIpRotator;


/// <summary>
/// A Dns Provider which provide for any IP type.
/// </summary>
public sealed class NativeDnsProvider : IDnsProvider
{
    private NativeDnsProvider()
    {
    }

    /// <summary>
    /// The default instance of the <see cref="NativeDnsProvider" />.
    /// </summary>
    public static readonly NativeDnsProvider Instance = new();

    /// <inheritdoc />
    public Task<IPHostEntry> GetDnsHostEntryAsync(DnsEndPoint dnsEndPoint, CancellationToken cancellationToken = default)
    {
        return Dns.GetHostEntryAsync(dnsEndPoint.Host, AddressFamily.Unspecified, cancellationToken);
    }
}
