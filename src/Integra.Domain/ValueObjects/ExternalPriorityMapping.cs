using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public sealed class ExternalPriorityMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }
    public string ExternalName { get; private set; } = null!;
    public string? ExternalId { get; private set; } = null!;
    public object? Metadata { get; private set; }

    private ExternalPriorityMapping(Guid integrationId, string externalName, string? externalId = null, object? metadata = null)
    {
        if(integrationId == Guid.Empty)
            throw new DomainException("IntegrationId cannot be empty.", nameof(integrationId));
        if(string.IsNullOrWhiteSpace(externalName))
            throw new DomainException("ExternalName cannot be null or whitespace.", nameof(externalName));
        IntegrationId = integrationId;
        ExternalName = externalName;
        ExternalId = externalId;
        Metadata = metadata;
    }

    public static ExternalPriorityMapping Create(Guid integrationId, string externalName, string? externalId = null, object? metadata = null)
        => new ExternalPriorityMapping(integrationId, externalName, externalId, metadata);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalName;
        yield return ExternalId ?? string.Empty;
        yield return Metadata ?? string.Empty;
    }
}
