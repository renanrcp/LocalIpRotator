// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net.Sockets;
using Xunit;

namespace LocalIpRotator.Tests;

public class NativeSocketFactoryTests
{
    [Fact]
    public void CreateCreatesSocketWithTypeStreamAndTcpProtocolAndNoDelayTrue()
    {
        // Arrange
        var factory = NativeSocketFactory.Instance;

        // Act
        var result = factory.CreateSocket();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(SocketType.Stream, result.SocketType);
        Assert.Equal(ProtocolType.Tcp, result.ProtocolType);
        Assert.True(result.NoDelay);
    }
}
