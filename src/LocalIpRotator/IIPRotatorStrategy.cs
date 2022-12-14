// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System.Net;

namespace LocalIpRotator;

/// <summary>
/// Represents an IP Rotator strategy
/// </summary>
public interface IIPRotatorStrategy : IDnsProvider
{
    /// <summary>
    /// The <see cref="IIPBlock" /> assigned to this strategy.
    /// </summary>
    IIPBlock IPBlock { get; }

    /// <summary>
    /// All the failed addresses in this strategy.
    /// </summary>
    public IReadOnlyCollection<IPAddress> FailedAddresses { get; }

    /// <summary>
    /// The last used address by this strategy.
    /// </summary>
    IPAddress? LastUsedAddress { get; }

    /// <summary>
    /// Add an <see cref="IPAddress" /> as failed for this strategy.
    /// </summary>
    /// <param name="address">The address to be added as failed for this strategy.</param>
    void AddFailedAddress(IPAddress address);

    /// <summary>
    /// Remove a failed <see cref="IPAddress" /> in this strategy.
    /// </summary>
    /// <param name="address">The failed addres to be removed in this strategy.</param>
    void RemoveFailedAddress(IPAddress address);

    /// <summary>
    /// Clear all failed addresses in this strategy.
    /// </summary>
    void ClearFailedAddresses();

    /// <summary>
    /// Gets an <see cref="IPAddress" /> by this strategy.
    /// </summary>
    /// <returns>An <see cref="IPAddress" /> by this strategy.</returns>
    IPAddress GetIPAddress();
}
