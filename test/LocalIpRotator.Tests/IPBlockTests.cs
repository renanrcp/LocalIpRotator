// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;
using Xunit;

namespace LocalIpRotator.Tests;

public class IPBlockTests
{
    public static IEnumerable<object[]> CtorParsesIPBlockCorretlyData()
    {
        yield return new object[]
        {
            "192.168.0.0/24",
            IPNetwork.Parse("192.168.0.0/24"),
        };
        yield return new object[]
        {
            "2aaa:cbbb:2000:6fff::/64",
            IPNetwork.Parse("2aaa:cbbb:2000:6fff::/64"),
        };
        yield return new object[]
        {
            "2aaa:cbbb:2000:6fff::/32",
            IPNetwork.Parse("2aaa:cbbb:2000:6fff::/32"),
        };
    }

    [Theory]
    [MemberData(nameof(CtorParsesIPBlockCorretlyData))]
    public void CtorParsesIPBlockCorretly(string ipBlock, IPNetwork expectedResult)
    {
        // Arrage
        var expectedCount = expectedResult.Usable;
        var expectedFamily = expectedResult.AddressFamily;

        // Act
        var result = new IPBlock(ipBlock);

        // Assert
        Assert.Equal(expectedCount, result.Count);
        Assert.Equal(expectedFamily, result.AddressFamily);
    }
}
