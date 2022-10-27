// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Numerics;
using LocalIpRotator.Extensions;
using Xunit;

namespace LocalIpRotator.Tests.Extensions;

public class EnumerableExtensionsTests
{
    public static IEnumerable<object[]> SumSumsAllValuesInEnumerableData()
    {
        yield return new object[]
        {
            new BigInteger[]
            {
                new BigInteger(450),
                new BigInteger(550),
            },
            1000,
        };
        yield return new object[]
        {
            new BigInteger[]
            {
                new BigInteger(1500),
                new BigInteger(500),
            },
            2000,
        };
        yield return new object[]
        {
            new BigInteger[]
            {
                new BigInteger(-1000),
                new BigInteger(2000),
            },
            1000,
        };
    }

    [Theory]
    [MemberData(nameof(SumSumsAllValuesInEnumerableData))]
    public void SumSumsAllValuesInEnumerable(IEnumerable<BigInteger> enumerable, BigInteger expectedResult)
    {
        // Act
        var result = EnumerableExtensions.Sum(enumerable, x => x);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}
