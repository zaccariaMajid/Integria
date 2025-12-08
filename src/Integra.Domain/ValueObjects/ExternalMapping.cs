using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Events;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public class ExternalMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }
    public string ExternalId { get; private set; } = null!;
    public string? ExternalKey { get; private set; }
    public string? Url { get; private set; }
    public DateTime LastSyncedAt { get; private set; }
    public string? RevisionHash { get; private set; }

    private ExternalMapping() { }

    private ExternalMapping(Guid integrationId, string externalId, string? externalKey = null, string? url = null, DateTime? lastSyncedAt = null, string? revisionHash = null)
    {
        if(integrationId == Guid.Empty)
            throw new DomainException(nameof(integrationId), "Integration ID cannot be empty GUID");
        if(externalId is null)
            throw new DomainException(nameof(externalId), "External ID cannot be null");

        IntegrationId = integrationId;
        ExternalId = externalId;
        ExternalKey = externalKey;
        Url = url;
        LastSyncedAt = lastSyncedAt ?? DateTime.UtcNow;
        RevisionHash = revisionHash;
    }

    public static ExternalMapping Create(Guid integrationId, string externalId, string? externalKey = null, string? url = null, DateTime? lastSyncedAt = null, string? revisionHash = null)
        => new ExternalMapping(integrationId, externalId, externalKey, url, lastSyncedAt, revisionHash);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalId;
        yield return ExternalKey ?? string.Empty;
        yield return Url ?? string.Empty;
        yield return RevisionHash ?? string.Empty;
    }
}
