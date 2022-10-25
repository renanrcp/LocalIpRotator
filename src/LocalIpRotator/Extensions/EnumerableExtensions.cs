// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Numerics;

namespace LocalIpRotator.Extensions;

/// <summary>
/// Some extensions methods for the <see cref="IEnumerable{T}" />.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Computes the sum of the sequence of <see cref="BigInteger" /> values that are obtained by
    /// invoking a transform function on each element of the input sequence.
    /// </summary>
    /// <param name="source">A sequence of values that are used to calculate a sum.</param>
    /// <param name="selector">A transform function to apply to each element.</param>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <returns>The sum of the projected values.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="selector" /> is null.</exception>
    /// <exception cref="OverflowException">The sum is larger than the max value of the <see cref="BigInteger" />.</exception>
    public static BigInteger Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, BigInteger> selector)
    {
        if (source == null)
        {
            ArgumentNullException.ThrowIfNull(source);
        }

        if (selector == null)
        {
            ArgumentNullException.ThrowIfNull(selector);
        }

        var sum = BigInteger.Zero;

        checked
        {
            foreach (var item in source)
            {
                sum += selector(item);
            }
        }

        return sum;
    }
}
