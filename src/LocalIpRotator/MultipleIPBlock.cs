// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;
using System.Net.Sockets;
using System.Numerics;
using LocalIpRotator.Extensions;

namespace LocalIpRotator;

/// <summary>
/// Represents an aggregation of <see cref="IIPBlock" />.
/// </summary>
public class MultipleIPBlock : IIPBlock
{
    private readonly IReadOnlyList<IIPBlock> _ipBlocks;

    /// <summary>
    /// Creates a new instance of <see cref="MultipleIPBlock" /> within the specified ip blocks.
    /// </summary>
    /// <param name="ipBlocks">The multiple ip blocks to combine.</param>
    public MultipleIPBlock(params string[] ipBlocks)
        : this(ipBlocks.AsEnumerable())
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="MultipleIPBlock" /> within the specified ip blocks.
    /// </summary>
    /// <param name="ipBlocks">The multiple ip blocks to combine.</param>
    public MultipleIPBlock(IEnumerable<string> ipBlocks)
        : this(ipBlocks.Select(ipBlock => IPNetwork.Parse(ipBlock)))
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="MultipleIPBlock" /> within the specified ip blocks.
    /// </summary>
    /// <param name="ipNetworks">The multiple <see cref="IPNetwork" /> to combine.</param>
    public MultipleIPBlock(params IPNetwork[] ipNetworks)
        : this(ipNetworks.AsEnumerable())
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="MultipleIPBlock" /> within the specified ip blocks.
    /// </summary>
    /// <param name="ipNetworks">The multiple <see cref="IPNetwork" /> to combine.</param>
    public MultipleIPBlock(IEnumerable<IPNetwork> ipNetworks)
        : this(ipNetworks.Select(ipNetwork => new IPBlock(ipNetwork)))
    {

    }

    /// <summary>
    /// Creates a new instance of <see cref="MultipleIPBlock" /> within the specified ip blocks.
    /// </summary>
    /// <param name="ipBlocks">The multiple <see cref="IIPBlock" /> to combine.</param>
    public MultipleIPBlock(params IIPBlock[] ipBlocks)
        : this(ipBlocks.AsEnumerable())
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="MultipleIPBlock" /> within the specified ip blocks.
    /// </summary>
    /// <param name="ipBlocks">The multiple <see cref="IIPBlock" /> to combine.</param>
    public MultipleIPBlock(IEnumerable<IIPBlock> ipBlocks)
    {
        ArgumentNullException.ThrowIfNull(ipBlocks);

        if (!ipBlocks.Any())
        {
            throw new ArgumentException("Cannot have empty IP blocks.", nameof(ipBlocks));
        }

        var addressFamily = ipBlocks.First().AddressFamily;

        if (ipBlocks.Any(ipBlock => ipBlock.AddressFamily != addressFamily))
        {
            throw new ArgumentException("All IP blocks needs to have the same address family.");
        }

        AddressFamily = addressFamily;
        Count = ipBlocks.Sum(ipBlock => ipBlock.Count);

        _ipBlocks = ipBlocks.ToArray();
    }

    /// <inheridoc />
    public IPAddress this[BigInteger index]
    {
        get
        {
            var blockIndex = 0;

            while (index >= 0)
            {
                if (_ipBlocks.Count <= blockIndex)
                {
                    break;
                }

                var ipBlock = _ipBlocks[blockIndex];

                if (ipBlock.Count > index)
                {
                    return ipBlock[index];
                }

                index -= ipBlock.Count;
                blockIndex++;
            }

            throw new ArgumentOutOfRangeException(nameof(index), "Index out of bounds for the MultipleIPBlock");
        }
    }

    /// <inheridoc />
    public BigInteger Count { get; }

    /// <inheridoc />
    public AddressFamily AddressFamily { get; }
}
