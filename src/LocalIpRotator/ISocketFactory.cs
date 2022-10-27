// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net.Sockets;

namespace LocalIpRotator;

/// <summary>
/// A factory to create <see cref="Socket" />.
/// </summary>
public interface ISocketFactory
{
    /// <summary>
    /// Creates a new instance of <see cref="Socket" />.
    /// </summary>
    /// <returns>A new instance of <see cref="Socket" />.</returns>
    Socket CreateSocket();
}
