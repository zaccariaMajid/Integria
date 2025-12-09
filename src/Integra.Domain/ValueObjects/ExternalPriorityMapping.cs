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

    // (es: "Highest", "High", "Urgent", "5")
    public string ExternalName { get; private set; } = null!;

    // (es: Jira Priority ID)
    public string? ExternalId { get; private set; }

    public UnifiedPriority UnifiedPriority { get; private set; } = null!;

    // (es. color, index)
    public object? Metadata { get; private set; }

    private ExternalPriorityMapping() { }

    private ExternalPriorityMapping(
        Guid integrationId,
        string externalName,
        UnifiedPriority unifiedPriority,
        string? externalId = null,
        object? metadata = null)
    {
        if (integrationId == Guid.Empty)
            throw new DomainException("IntegrationId cannot be empty.", nameof(integrationId));

        if (string.IsNullOrWhiteSpace(externalName))
            throw new DomainException("ExternalName cannot be null or whitespace.", nameof(externalName));

        if (unifiedPriority is null)
            throw new DomainException("UnifiedPriority cannot be null.", nameof(unifiedPriority));

        IntegrationId = integrationId;
        ExternalName = externalName;
        UnifiedPriority = unifiedPriority;
        ExternalId = externalId;
        Metadata = metadata;
    }

    public static ExternalPriorityMapping Create(
        Guid integrationId,
        string externalName,
        UnifiedPriority unifiedPriority,
        string? externalId = null,
        object? metadata = null)
        => new ExternalPriorityMapping(integrationId, externalName, unifiedPriority, externalId, metadata);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalName;
        yield return UnifiedPriority;     // ← mandatory!
    }
}
