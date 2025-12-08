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
    public string ExternalValue { get; private set; } = null!;

    private ExternalValueMapping(Guid integrationId, string externalValue)
    {
        if (integrationId == Guid.Empty)
            throw new DomainException("Integration ID cannot be empty.", nameof(integrationId));
        if (string.IsNullOrWhiteSpace(externalValue))
            throw new DomainException("External value cannot be null or empty.", nameof(externalValue));
        IntegrationId = integrationId;
        ExternalValue = externalValue;
    }

    public static ExternalValueMapping Create(Guid integrationId, string externalValue)
        => new ExternalValueMapping(integrationId, externalValue);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalValue;
    }
}
