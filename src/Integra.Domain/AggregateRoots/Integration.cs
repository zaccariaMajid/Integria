using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;
using Integra.Domain.Exceptions;
using Integra.Domain.ExtensionMethods;

namespace Integra.Domain.AggregateRoots;

/// <summary>
/// Stores credentials, endpoints, access info
/// Ensures security (encrypted API keys)
/// Exposes configurations for data extraction and writing
/// Validates that the integration is working
/// </summary>
public sealed class Integration : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public IntegrationType IntegrationType { get; private set; }
    public string SystemUrl { get; private set; } = null!;
    public string ApiKeyHash { get; private set; } = null!;
    public DateTime ConnectedOn { get; private set; }
    public string ConfigurationJson { get; private set; } = null!;
    public int RateLimitPerMinute { get; private set; }

    private Integration() : base() { }

    private Integration(
        Guid tenantId,
        IntegrationType integrationType,
        string systemUrl,
        string apiKeyHash,
        DateTime connectedOn,
        string configurationJson,
        int rateLimitPerMinute) : base()
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(systemUrl))
            throw new DomainException("SystemUrl cannot be null or empty.", nameof(systemUrl));
        if (string.IsNullOrWhiteSpace(apiKeyHash))
            throw new DomainException("ApiKeyHash cannot be null or empty.", nameof(apiKeyHash));
        if (rateLimitPerMinute <= 0)
            throw new DomainException("RateLimitPerMinute must be greater than zero.", nameof(rateLimitPerMinute));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        IntegrationType = integrationType;
        SystemUrl = systemUrl;
        ApiKeyHash = apiKeyHash;
        ConnectedOn = connectedOn;
        ConfigurationJson = configurationJson;
        RateLimitPerMinute = rateLimitPerMinute;


        AddDomainEvent(new IntegrationCreated(Id, integrationType.GetEnumDescription()));
    }

    public Integration Create(
        Guid tenantId,
        IntegrationType integrationType,
        string systemUrl,
        string apiKeyHash,
        DateTime connectedOn,
        string configurationJson,
        int rateLimitPerMinute) => new Integration(
            tenantId,
            integrationType,
            systemUrl,
            apiKeyHash,
            connectedOn,
            configurationJson,
            rateLimitPerMinute);
}
