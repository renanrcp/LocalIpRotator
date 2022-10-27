// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net.Sockets;

namespace LocalIpRotator;

/// <summary>
/// A socket factory which natively creates <see cref="Socket" />.
/// </summary>
public sealed class NativeSocketFactory : ISocketFactory
{
    private NativeSocketFactory()
    {

    }

    /// <summary>
    /// The default instance of the <see cref="NativeSocketFactory" />.
    /// </summary>
    public static readonly NativeSocketFactory Instance = new();

    /// <inheritdoc />
    public Socket CreateSocket()
    {
        return new Socket(SocketType.Stream, ProtocolType.Tcp)
        {
            NoDelay = true,
        };
    }
}
