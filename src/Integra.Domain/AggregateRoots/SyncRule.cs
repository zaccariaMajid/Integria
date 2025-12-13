using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;
using Integra.Domain.Exceptions;
using Integra.Domain.ExtensionMethods;
using Integra.Domain.ValueObjects;

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

    // Provider coinvolti
    public Guid SourceIntegrationId { get; private set; }
    public Guid TargetIntegrationId { get; private set; }

    // Direzione della sincronizzazione
    public SyncDirection Direction { get; private set; }

    // Scope (Tasks, Comments, Labels, ecc.)
    public SyncScope Scope { get; private set; }

    // Filtri opzionali (labels, statuses, updatedAfter...)
    public SyncFilter? Filter { get; private set; }

    // Come risolvere conflitti tra versioni
    public SyncConflictPolicy ConflictPolicy { get; private set; }

    // Quale set di mapping utilizzare
    public Guid FieldMappingId { get; private set; }

    // Scheduling è gestito da un aggregate SEPARATO → SyncSchedule
    public Guid? SyncScheduleId { get; private set; }

    public bool IsEnabled { get; private set; }

    private SyncRule() { }

    private SyncRule(
        Guid tenantId,
        Guid sourceIntegrationId,
        Guid targetIntegrationId,
        SyncDirection direction,
        SyncScope scope,
        SyncFilter? filter,
        SyncConflictPolicy conflictPolicy,
        Guid fieldMappingId,
        Guid? syncScheduleId,
        bool isEnabled)
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty", nameof(tenantId));

        if (sourceIntegrationId == Guid.Empty)
            throw new DomainException("SourceIntegrationId cannot be empty", nameof(sourceIntegrationId));

        if (targetIntegrationId == Guid.Empty)
            throw new DomainException("TargetIntegrationId cannot be empty", nameof(targetIntegrationId));

        TenantId = tenantId;
        SourceIntegrationId = sourceIntegrationId;
        TargetIntegrationId = targetIntegrationId;
        Direction = direction;
        Scope = scope;
        Filter = filter;
        ConflictPolicy = conflictPolicy;
        FieldMappingId = fieldMappingId;
        SyncScheduleId = syncScheduleId;
        IsEnabled = isEnabled;

        AddDomainEvent(new SyncRuleCreated(Id, SourceIntegrationId, TargetIntegrationId, Direction.GetEnumDescription()));
    }

    public static SyncRule Create(
        Guid tenantId,
        Guid sourceIntegrationId,
        Guid targetIntegrationId,
        SyncDirection direction,
        SyncScope scope,
        SyncFilter? filter,
        SyncConflictPolicy conflictPolicy,
        Guid fieldMappingId,
        Guid? syncScheduleId,
        bool isEnabled = true)
    {
        return new SyncRule(
            tenantId,
            sourceIntegrationId,
            targetIntegrationId,
            direction,
            scope,
            filter,
            conflictPolicy,
            fieldMappingId,
            syncScheduleId,
            isEnabled
        );
    }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    public void AssignSchedule(Guid scheduleId)
        => SyncScheduleId = scheduleId;
    public void RemoveSchedule() => SyncScheduleId = null;
    public void UpdateConfiguration(
        SyncScope scope,
        SyncFilter? filter,
        SyncConflictPolicy conflictPolicy,
        Guid fieldMappingId)
    {
        if (fieldMappingId == Guid.Empty)
            throw new DomainException("FieldMappingId cannot be empty", nameof(fieldMappingId));

        Scope = scope;
        Filter = filter;
        ConflictPolicy = conflictPolicy;
        FieldMappingId = fieldMappingId;
    }
}
