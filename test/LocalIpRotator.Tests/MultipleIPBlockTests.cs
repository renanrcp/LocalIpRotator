// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;
using System.Numerics;
using Xunit;

namespace LocalIpRotator.Tests;

public class MultipleIPBlockTests
{
    public static IEnumerable<object[]> CtorParsesIPBlocksCorretlyData()
    {
        yield return new object[]
        {
            new string[]
            {
                "2001:0db8::/32",
                "2001:0db9::/32",
            },
            IPNetwork.Parse("2001:0db8::/32").Usable + IPNetwork.Parse("2001:0db9::/32").Usable,
        };
        yield return new object[]
        {
            new string[]
            {
                "192.168.0.0/24",
                "192.168.0.128/25",
                "192.168.1.1/24"
            },
            IPNetwork.Parse("192.168.0.0/24").Usable + IPNetwork.Parse("192.168.0.128/25").Usable  + IPNetwork.Parse("192.168.1.1/24").Usable,
        };
    }

    [Theory]
    [MemberData(nameof(CtorParsesIPBlocksCorretlyData))]
    public void CtorParsesIPBlocksCorretly(string[] ipBlocks, BigInteger expectedCount)
    {
        // Act
        var result = new MultipleIPBlock(ipBlocks);

        // Assert
        Assert.Equal(expectedCount, result.Count);
    }

    public static IEnumerable<object[]> IndexReturnsNextBlockIPIfHigherThanFirstBlockCountData()
    {
        yield return new object[]
        {
            new MultipleIPBlock("2001:0db8::/32", "2001:0db8::/32"),
            IPNetwork.Parse("2001:0db8::/32").Usable,
            IPNetwork.Parse("2001:0db8::/32").ListIPAddress(FilterEnum.Usable).First(),
        };
        yield return new object[]
        {
            new MultipleIPBlock("192.168.0.0/24", "192.168.0.128/25", "192.168.1.1/24"),
            IPNetwork.Parse("192.168.0.0/24").Usable,
            IPNetwork.Parse("192.168.0.128/25").ListIPAddress(FilterEnum.Usable).First(),
        };
        yield return new object[]
        {
            new MultipleIPBlock("192.168.0.0/24", "192.168.0.128/25", "192.168.1.1/24"),
            IPNetwork.Parse("192.168.0.0/24").Usable + IPNetwork.Parse("192.168.0.128/25").Usable,
            IPNetwork.Parse("192.168.1.1/24").ListIPAddress(FilterEnum.Usable).First(),
        };
    }

    [Theory]
    [MemberData(nameof(IndexReturnsNextBlockIPIfHigherThanFirstBlockCountData))]
    public void IndexReturnsNextBlockIPIfHigherThanFirstBlockCount(MultipleIPBlock ipBlock, BigInteger index, IPAddress expectedAddress)
    {
        // Act
        var result = ipBlock[index];

        // Assert
        Assert.Equal(expectedAddress, result);
    }
}
