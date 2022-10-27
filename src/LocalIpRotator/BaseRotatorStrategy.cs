// Licensed to the LocalIpRotator under one or more agreements.
// LocalIpRotator licenses this file to you under the MIT license.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using LocalIpRotator.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace LocalIpRotator;

/// <summary>
/// An abstract class to rotator strategies.
/// </summary>
public abstract class BaseRotatorStrategy : IIPRotatorStrategy
{
    /// <summary>
    /// The default duration that an <see cref="IPAddress" /> in a strategy will consider as failed.
    /// </summary>
    /// <remarks>
    /// The default duration is 7 days.
    /// </remarks>
    public static readonly TimeSpan DEFAULT_FAILED_IP_DURATION = TimeSpan.FromDays(7);

    /// <summary>
    /// The size of an IPV6/64 block.
    /// </summary>
    public static readonly BigInteger IPV6_BLOCK_64_SIZE = BigInteger.Pow(2, 64);

    private readonly ILogger _logger;
    private readonly TimeSpan _failedIPDuration;
    private readonly ConcurrentDictionary<IPAddress, long> _failedAddresses;
    private readonly Lazy<IReadOnlyCollection<IPAddress>> _failedAddressesKeys;

    private IPAddress? _lastUsedAddress;

    /// <summary>
    /// Creates a new instance of <see cref="BaseRotatorStrategy" />.
    /// </summary>
    /// <param name="ipBlock">The <see cref="IIPBlock" /> for this strategy.</param>
    /// <param name="logger">A logger to log ip rotation info.</param>
    /// <param name="failedIPDuration">The duration that an <see cref="IPAddress" /> in this strategy will consider as failed.</param>
    protected BaseRotatorStrategy(IIPBlock ipBlock, ILogger? logger = null, TimeSpan? failedIPDuration = null)
    {
        ArgumentNullException.ThrowIfNull(ipBlock);

        IPBlock = ipBlock;
        _logger = logger ?? NullLogger.Instance;
        _failedIPDuration = failedIPDuration ?? DEFAULT_FAILED_IP_DURATION;

        _failedAddresses = new();
        _failedAddressesKeys = new Lazy<IReadOnlyCollection<IPAddress>>(() =>
        {
            return new ReadOnlyListWrapper(_failedAddresses);
        }, true);
    }

    /// <inheritdoc />
    public IIPBlock IPBlock { get; }

    /// <summary>
    /// Gets all the failed address and their UTC expiration date in Unix Timestamp.
    /// </summary>
    public IReadOnlyDictionary<IPAddress, long> FailedAddresses => _failedAddresses;

    /// <inheritdoc />
    public IPAddress? LastUsedAddress
    {
        get => Volatile.Read(ref _lastUsedAddress);
        private set => Volatile.Write(ref _lastUsedAddress, value);
    }

    IReadOnlyCollection<IPAddress> IIPRotatorStrategy.FailedAddresses
    {
        get
        {
            return _failedAddressesKeys.Value;
        }
    }

    /// <inheritdoc />
    public void AddFailedAddress(IPAddress address)
    {
        var cacheExpiration = DateTimeOffset.UtcNow.Add(_failedIPDuration);

        _ = _failedAddresses.TryAdd(address, cacheExpiration.ToUnixTimeMilliseconds());

        _logger.LogAddressFailed(address);

        _logger.LogAddressFailedExpiration(address, cacheExpiration);

        OnAddressFailure(address);
    }

    /// <inheritdoc />
    public void ClearFailedAddresses()
    {
        _failedAddresses.Clear();

        _logger.LogFailedAddressesClear();
    }

    /// <inheritdoc />
    public virtual Task<IPHostEntry> GetDnsHostEntryAsync(DnsEndPoint dnsEndPoint, CancellationToken cancellationToken = default)
    {
        return Dns.GetHostEntryAsync(dnsEndPoint.Host, IPBlock.AddressFamily, cancellationToken);
    }

    /// <inheritdoc />
    public IPAddress GetIPAddress()
    {
        var ipAddress = GetIPAddressCore();

        LastUsedAddress = ipAddress;

        _logger.LogAddressChosen(ipAddress);

        return ipAddress;
    }

    /// <inheritdoc />
    public void RemoveFailedAddress(IPAddress address)
    {
        if (_failedAddresses.TryRemove(address, out _))
        {
            _logger.LogAddressFailedRemoved(address);
        }
    }

    /// <summary>
    /// Gets an <see cref="IPAddress" /> by this strategy.
    /// </summary>
    /// <returns>An <see cref="IPAddress" /> by this strategy.</returns>
    protected internal abstract IPAddress GetIPAddressCore();

    /// <summary>
    /// Check if the <paramref name="address" /> is valid considering the all failed addresses.
    /// </summary>
    /// <param name="address">The <see cref="IPAddress" /> to be check.</param>
    /// <returns><see langword="true" /> if the <paramref name="address" /> is not marked as failed, otherwise <see langword="false" />.</returns>
    protected internal bool IsValidAddress(IPAddress address)
    {
        if (!_failedAddresses.TryGetValue(address, out var expireDateMs))
        {
            _logger.LogAddressIsValid(address);

            return true;
        }

        if (expireDateMs < DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        {
            RemoveFailedAddress(address);

            _logger.LogAddressIsValid(address);

            return true;
        }

        _logger.LogAddressIsInvalid(address);

        return false;
    }

    /// <summary>
    /// Method called when an <see cref="IPAddress" /> was marked as failed.
    /// </summary>
    /// <param name="address">The <see cref="IPAddress" /> that was marked as failed.</param>
    protected internal virtual void OnAddressFailure(IPAddress address)
    {
    }


    private readonly struct ReadOnlyListWrapper : IReadOnlyCollection<IPAddress>
    {
        private readonly ConcurrentDictionary<IPAddress, long> _failedAddresses;

        public ReadOnlyListWrapper(ConcurrentDictionary<IPAddress, long> failedAddresses)
        {
            _failedAddresses = failedAddresses;
        }

        public int Count => _failedAddresses.Count;


        public IEnumerator<IPAddress> GetEnumerator()
        {
            return _failedAddresses.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
