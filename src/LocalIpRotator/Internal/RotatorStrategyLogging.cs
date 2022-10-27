// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;
using Microsoft.Extensions.Logging;

namespace LocalIpRotator.Internal;

internal static partial class RotatorStrategyLogging
{
    [LoggerMessage(1, LogLevel.Information, "IP: '{Address}' was marked as failed.")]
    public static partial void LogAddressFailed(this ILogger logger, IPAddress address);

    [LoggerMessage(2, LogLevel.Debug, "IP: '{Address}' expiration date is '{Date}'.")]
    public static partial void LogAddressFailedExpiration(this ILogger logger, IPAddress address, DateTimeOffset date);

    [LoggerMessage(3, LogLevel.Information, "All the failed IP addresses are cleared.")]
    public static partial void LogFailedAddressesClear(this ILogger logger);

    [LoggerMessage(4, LogLevel.Information, "IP: '{Address}' was removed from failed addresses.")]
    public static partial void LogAddressFailedRemoved(this ILogger logger, IPAddress address);

    [LoggerMessage(5, LogLevel.Debug, "IP: '{Address}' is invalid because it is in failed addresses.")]
    public static partial void LogAddressIsInvalid(this ILogger logger, IPAddress address);

    [LoggerMessage(6, LogLevel.Debug, "IP: '{Address}' is because because it is not in failed addresses.")]
    public static partial void LogAddressIsValid(this ILogger logger, IPAddress address);

    [LoggerMessage(7, LogLevel.Debug, "IP: '{Address}' was chosen.")]
    public static partial void LogAddressChosen(this ILogger logger, IPAddress address);
}
