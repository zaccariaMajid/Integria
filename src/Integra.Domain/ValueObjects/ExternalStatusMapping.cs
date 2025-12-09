using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;
public sealed class ExternalStatusMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }
    public string ExternalName { get; private set; } = null!;
    public UnifiedTaskStatus UnifiedTaskStatus { get; private set; } = null!;
    public object? Metadata { get; private set; }  // <-- più flessibile di ExternalType

    private ExternalStatusMapping() { }

    private ExternalStatusMapping(
        Guid integrationId,
        string externalName,
        UnifiedTaskStatus unifiedTaskStatus,
        object? metadata
    )
    {
        IntegrationId = integrationId;
        ExternalName = externalName;
        UnifiedTaskStatus = unifiedTaskStatus;
        Metadata = metadata;
    }

    public static ExternalStatusMapping Create(
        Guid integrationId,
        string externalName,
        UnifiedTaskStatus unifiedTaskStatus,
        object? metadata = null
    )
        => new ExternalStatusMapping(integrationId, externalName, unifiedTaskStatus, metadata);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalName;
        yield return UnifiedTaskStatus;
    }
}
