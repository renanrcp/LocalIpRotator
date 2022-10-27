// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace LocalIpRotator;

/// <summary>
/// Represents a block of IP addresses.
/// </summary>
public class IPBlock : IIPBlock
{
    private readonly IPAddressCollection _ipAddresses;
    private readonly IPNetwork _ipNetwork;

    /// <summary>
    /// Creates a new instance of <see cref="IPBlock" /> within the specified <paramref name="ipBlock" />.
    /// </summary>
    /// <param name="ipBlock">The IP block to be used in this block.</param>
    public IPBlock(string ipBlock) : this(IPNetwork.Parse(ipBlock))
    {
    }

    /// <summary>
    /// Creates a new instance of <see cref="IPBlock" /> within the specified <see cref="IPNetwork" />.
    /// </summary>
    /// <param name="ipNetwork">The <see cref="IPNetwork" /> to be used in this block.</param>
    public IPBlock(IPNetwork ipNetwork)
    {
        ArgumentNullException.ThrowIfNull(ipNetwork);

        _ipNetwork = ipNetwork;
        _ipAddresses = _ipNetwork.ListIPAddress(FilterEnum.Usable);
    }

    /// <inheridoc />
    public BigInteger Count => _ipNetwork.Usable;

    /// <inheridoc />
    public IPAddress this[BigInteger index] => _ipAddresses[index];

    /// <inheridoc />
    public AddressFamily AddressFamily => _ipNetwork.AddressFamily;

    /// <inheridoc />
    public IEnumerator<IPAddress> GetEnumerator()
    {
        return _ipAddresses;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _ipAddresses;
    }
}
