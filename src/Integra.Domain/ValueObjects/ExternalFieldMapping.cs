using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public sealed class ExternalFieldMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }
    public string ExternalName { get; private set; } = null!;
    public string? ExternalType { get; private set; }
    public object? AdditionalInfo { get; private set; }

    private ExternalFieldMapping() { }
    private ExternalFieldMapping(Guid integrationId, string externalName, string? externalType = null, object? additionalInfo = null)
    {
        if(externalName is null)
            throw new DomainException(nameof(externalName), "External name cannot be null");
        if(integrationId == Guid.Empty)
            throw new DomainException(nameof(integrationId), "Integration ID cannot be empty GUID");

        IntegrationId = integrationId;
        ExternalName = externalName;
        ExternalType = externalType;
        AdditionalInfo = additionalInfo;
    }

    public static ExternalFieldMapping Create(Guid integrationId, string externalName, string? externalType = null, object? additionalInfo = null)
        => new ExternalFieldMapping(integrationId, externalName, externalType, additionalInfo);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalName;
        yield return ExternalType ?? string.Empty;
        yield return AdditionalInfo ?? string.Empty;
    }
}
