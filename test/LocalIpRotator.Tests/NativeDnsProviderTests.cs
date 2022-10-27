// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;
using Xunit;

namespace LocalIpRotator.Tests;

public class NativeDnsProviderTests
{
    [Fact]
    public async Task GetDnsHostEntryAsyncReturnsDnsOfAllIpTypes()
    {
        // Arrange
        var host = "google.com";

        var provider = NativeDnsProvider.Instance;

        var expectedResult = await Dns.GetHostEntryAsync(host, AddressFamily.Unspecified);

        // Act
        var result = await provider.GetDnsHostEntryAsync(new DnsEndPoint(host, 443));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResult, result, new IPHostEntryComparer());
    }

    private class IPHostEntryComparer : IEqualityComparer<IPHostEntry>
    {
        public bool Equals(IPHostEntry? x, IPHostEntry? y)
        {
            return
                x != null &&
                y != null &&
                x.HostName.Equals(y.HostName, StringComparison.Ordinal) &&
                x.Aliases.SequenceEqual(y.Aliases) &&
                x.AddressList.SequenceEqual(y.AddressList);
        }

        public int GetHashCode([DisallowNull] IPHostEntry obj)
        {
            return obj.GetHashCode();
        }
    }
}
