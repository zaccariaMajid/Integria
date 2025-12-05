using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.AggregateRoots;

public class RedmineIntegration : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public string RedmineUrl { get; private set; } = null!;
    public string HashedApiKey { get; private set; } = null!;
    public string JsonSettings { get; private set; } = null!;

    private RedmineIntegration() : base() {}

    public RedmineIntegration(
        Guid tenantId,
        string redmineUrl,
        string apiKey,
        string jsonSettings) : base()
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        RedmineUrl = redmineUrl;
        HashedApiKey = apiKey;
        JsonSettings = jsonSettings;
    }

    public RedmineIntegration CreateNew(
        Guid tenantId,
        string redmineUrl,
        string apiKey,
        string jsonSettings)
    {
        if(tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty.", nameof(tenantId));
        
        if(string.IsNullOrWhiteSpace(redmineUrl))
            throw new DomainException("RedmineUrl cannot be empty.", nameof(redmineUrl));
        if(string.IsNullOrWhiteSpace(apiKey))
            throw new DomainException("ApiKey cannot be empty.", nameof(apiKey));

        return new RedmineIntegration(
            tenantId,
            redmineUrl,
            apiKey,
            jsonSettings);
    }
}
