// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Numerics;

namespace LocalIpRotator.Extensions;

/// <summary>
/// Some extensions methods for the <see cref="Random" />.
/// </summary>
public static class RandomExtensions
{
    /// <summary>
    /// Returns a random <see cref="BigInteger" /> that is within a specified range.
    /// </summary>
    /// <param name="random">The randomizer to use to get the random value.</param>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater
    /// than or equal to minValue.</param>
    /// <returns>A <see cref="BigInteger" /> greater than or equal to <paramref name="minValue" /> and less than <paramref name="maxValue" />;
    /// that is, the range of return values includes <paramref name="minValue" /> but not <paramref name="maxValue" />. If <paramref name="minValue" />
    /// equals <paramref name="maxValue" />, <paramref name="minValue" /> is returned.</returns>
    public static BigInteger NextBigInteger(this Random random, BigInteger minValue, BigInteger maxValue)
    {
        if (minValue > maxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(minValue), minValue, $"{nameof(minValue)} is greater than {nameof(maxValue)}.");
        }

        if (minValue == maxValue)
        {
            return minValue;
        }

        var zeroBasedUpperBound = maxValue - 1 - minValue;
        var bytes = zeroBasedUpperBound.ToByteArray();

        byte lastByteMask = 0b11111111;

        for (byte mask = 0b10000000; mask > 0; mask >>= 1, lastByteMask >>= 1)
        {
            if ((bytes[^1] & mask) == mask)
            {
                break;
            }
        }

        while (true)
        {
            random.NextBytes(bytes);

            bytes[^1] &= lastByteMask;

            var result = new BigInteger(bytes);

            if (result <= zeroBasedUpperBound)
            {
                return result + minValue;
            }
        }
    }
}
