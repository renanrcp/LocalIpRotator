// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;
using System.Net;
using NSubstitute;
using Xunit;

namespace LocalIpRotator.Tests;

public class BaseRotatorStrategyTests
{
    [Fact]
    public void GetIPAddressReturnsChildIPAddressAndSetLastUsedAddress()
    {
        // Arrange
        var address = IPAddress.Parse("192.168.0.1");

        var strategy = Substitute.For<BaseRotatorStrategy>(new IPBlock("192.168.0.0/24"), null, null);

        _ = strategy.GetIPAddressCore().Returns(address);

        // Act
        var result = strategy.GetIPAddress();

        // Assert
        Assert.Equal(address, result);
        Assert.Equal(result, strategy.LastUsedAddress);
    }

    [Fact]
    public void AddFailedAddressAddAddressFailedAndCallsOnAddressFailure()
    {
        // Arrange
        var address = IPAddress.Parse("192.168.0.1");

        var strategy = Substitute.For<BaseRotatorStrategy>(new IPBlock("192.168.0.0/24"), null, null);

        // Act
        strategy.AddFailedAddress(address);


        // Assert
        strategy.Received().OnAddressFailure(address);
        Assert.NotEmpty(strategy.FailedAddresses);
        Assert.Equal(address, strategy.FailedAddresses.First().Key);
    }

    [Fact]
    public void ClearFailedAddressesClearsFailedAddress()
    {
        // Arrange
        var strategy = Substitute.For<BaseRotatorStrategy>(new IPBlock("192.168.0.0/24"), null, null);

        strategy.AddFailedAddress(IPAddress.Parse("192.168.0.1"));

        // Act
        strategy.ClearFailedAddresses();

        // Assert
        Assert.Empty(strategy.FailedAddresses);
    }

    [Fact]
    public void IsValidAddressReturnsTrueIfAddressIsNotOnFailedAddresses()
    {
        // Arrange
        var address = IPAddress.Parse("192.168.0.1");

        var strategy = Substitute.For<BaseRotatorStrategy>(new IPBlock("192.168.0.0/24"), null, null);

        // Act
        var result = strategy.IsValidAddress(address);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsValidAddressReturnsTrueAndRemoveFromFailedIfAddressIsOnFailedAddressesButExpired()
    {
        // Arrange
        var address = IPAddress.Parse("192.168.0.1");
        var expirationTime = TimeSpan.FromMilliseconds(10);

        var strategy = Substitute.For<BaseRotatorStrategy>(new IPBlock("192.168.0.0/24"), null, expirationTime);

        strategy.AddFailedAddress(address);

        await Task.Delay(expirationTime);

        // Act
        var result = strategy.IsValidAddress(address);

        // Assert
        Assert.True(result);
        Assert.Empty(strategy.FailedAddresses);
    }

    [Fact]
    public void IsValidAddressReturnsFalseIfAddressIsOnFailedAddresses()
    {
        // Arrange
        var address = IPAddress.Parse("192.168.0.1");

        var strategy = Substitute.For<BaseRotatorStrategy>(new IPBlock("192.168.0.0/24"), null, null);

        strategy.AddFailedAddress(address);

        // Act
        var result = strategy.IsValidAddress(address);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetDnsHostEntryAsyncReturnsDnsHostEntryAccordingWithIPBlockFamily()
    {
        // Arrange
        var host = "google.com";
        var dnsEndPoint = new DnsEndPoint(host, 443);
        var ipBlock = new IPBlock("192.168.0.0/24");

        var expectedIPEntry = await Dns.GetHostEntryAsync(host, ipBlock.AddressFamily);

        var strategy = Substitute.For<BaseRotatorStrategy>(ipBlock, null, null);

        strategy.When(x => x.GetDnsHostEntryAsync(dnsEndPoint)).CallBase();

        // Act
        var result = await strategy.GetDnsHostEntryAsync(dnsEndPoint);

        // Assert
        Assert.Equal(expectedIPEntry, result, new IPHostEntryComparer());
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
