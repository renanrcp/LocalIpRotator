// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace LocalIpRotator;

/// <summary>
/// Represents a block of IP addresses.
/// </summary>
public interface IIPBlock
{
    /// <summary>
    /// Total number of IP addresses in this block.
    /// </summary>
    public BigInteger Count { get; }

    /// <summary>
    /// Get an IP address in the specific index.
    /// </summary>
    public IPAddress this[BigInteger index] { get; }

    /// <summary>
    /// The <see cref="System.Net.Sockets.AddressFamily" /> type of this block.
    /// </summary>
    public AddressFamily AddressFamily { get; }
}
