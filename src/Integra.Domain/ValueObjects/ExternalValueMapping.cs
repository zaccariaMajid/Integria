using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public sealed class ExternalValueMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }

    // (es: "Severity", "Customer Type")
    public string FieldName { get; private set; } = null!;

    // (es: "Urgent", "Low", "VIP")
    public string ExternalValue { get; private set; } = null!;

    // (es: "High", "Medium", "Low")
    public object UnifiedValue { get; private set; } = null!;

    // (ID, colore, indice, ecc.)
    public object? Metadata { get; private set; }

    private ExternalValueMapping() { }

    private ExternalValueMapping(
        Guid integrationId,
        string fieldName,
        string externalValue,
        object unifiedValue,
        object? metadata)
    {
        if (integrationId == Guid.Empty)
            throw new DomainException("Integration ID cannot be empty.", nameof(integrationId));

        if (string.IsNullOrWhiteSpace(fieldName))
            throw new DomainException("Field name cannot be empty.", nameof(fieldName));

        if (string.IsNullOrWhiteSpace(externalValue))
            throw new DomainException("External value cannot be empty.", nameof(externalValue));

        IntegrationId = integrationId;
        FieldName = fieldName;
        ExternalValue = externalValue;
        UnifiedValue = unifiedValue;
        Metadata = metadata;
    }

    public static ExternalValueMapping Create(
        Guid integrationId,
        string fieldName,
        string externalValue,
        object unifiedValue,
        object? metadata = null)
        => new ExternalValueMapping(integrationId, fieldName, externalValue, unifiedValue, metadata);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return FieldName;
        yield return ExternalValue;
        yield return UnifiedValue!;
    }
}
