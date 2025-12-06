using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;
using Integra.Domain.Exceptions;
using Integra.Domain.ExtensionMethods;

namespace Integra.Domain.AggregateRoots;
/// <summary>
/// Define who synchronizes what
/// Map projects, types, statuses, priorities
/// Indicate whether the sync is: unidirectional, bidirectional or initial migration only
/// Set the frequency
/// Enable/disable the rule
/// </summary>
public sealed class SyncRule : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid SourceIntegrationId { get; private set; }
    public Guid TargetIntegrationId { get; private set; }
    public SyncDirection SyncDirection { get; private set; }
    public string Scope { get; private set; } = null!;
    public string MappingJson { get; private set; } = null!;
    public TimeSpan Frequency { get; private set; }
    public bool IsEnabled { get; private set; }
    public DateTime? LastRunOn { get; private set; }

    private SyncRule() : base() { }

    private SyncRule(
        Guid tenantId,
        Guid sourceIntegrationId,
        Guid targetIntegrationId,
        SyncDirection syncDirection,
        string scope,
        string mappingJson,
        TimeSpan frequency,
        bool isEnabled,
        DateTime? lastRunOn) : base()
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        SourceIntegrationId = sourceIntegrationId;
        TargetIntegrationId = targetIntegrationId;
        SyncDirection = syncDirection;
        Scope = scope;
        MappingJson = mappingJson;
        Frequency = frequency;
        IsEnabled = isEnabled;
        LastRunOn = lastRunOn;

        AddDomainEvent(new SyncRuleCreated(Id, sourceIntegrationId, targetIntegrationId, syncDirection.GetEnumDescription()));
    }

    public SyncRule Create(
        Guid tenantId,
        Guid sourceIntegrationId,
        Guid targetIntegrationId,
        SyncDirection syncDirection,
        string scope,
        string mappingJson,
        TimeSpan frequency,
        bool isEnabled,
        DateTime? lastRunOn)
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty.", nameof(tenantId));
        if (sourceIntegrationId == Guid.Empty)
            throw new DomainException("SourceIntegrationId cannot be empty.", nameof(sourceIntegrationId));
        if (targetIntegrationId == Guid.Empty)
            throw new DomainException("TargetIntegrationId cannot be empty.", nameof(targetIntegrationId));
        if (string.IsNullOrWhiteSpace(scope))
            throw new DomainException("Scope cannot be null or empty.", nameof(scope));
        if (string.IsNullOrWhiteSpace(mappingJson))
            throw new DomainException("MappingJson cannot be null or empty.", nameof(mappingJson));
        if (frequency <= TimeSpan.Zero)
            throw new DomainException("Frequency must be greater than zero.", nameof(frequency));
        return new SyncRule(
            tenantId,
            sourceIntegrationId,
            targetIntegrationId,
            syncDirection,
            scope,
            mappingJson,
            frequency,
            isEnabled,
            lastRunOn);
    }
}
