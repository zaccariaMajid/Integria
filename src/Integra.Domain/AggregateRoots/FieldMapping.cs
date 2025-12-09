using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;
using Integra.Domain.ValueObjects;

namespace Integra.Domain.AggregateRoots;

public sealed class FieldMapping : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }

    private readonly List<ExternalStatusMapping> _statusMappings = new();
    public IReadOnlyCollection<ExternalStatusMapping> StatusMappings => _statusMappings.AsReadOnly();

    private readonly List<ExternalPriorityMapping> _priorityMappings = new();
    public IReadOnlyCollection<ExternalPriorityMapping> PriorityMappings => _priorityMappings.AsReadOnly();

    private readonly List<ExternalValueMapping> _customFieldValueMappings = new();
    public IReadOnlyCollection<ExternalValueMapping> CustomFieldValueMappings => _customFieldValueMappings.AsReadOnly();

    private FieldMapping() { }

    private FieldMapping(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty GUID", nameof(tenantId));

        TenantId = tenantId;
    }

    public static FieldMapping Create(Guid tenantId)
        => new FieldMapping(tenantId);

    // --------------------------
    // STATUS mapping
    // --------------------------
    public void AddStatusMapping(ExternalStatusMapping mapping)
    {
        _statusMappings.Add(mapping);
        Touch();
    }

    public UnifiedTaskStatus MapStatusFromProvider(Guid integrationId, string externalName)
    {
        var mapping = _statusMappings
            .FirstOrDefault(x =>
                x.IntegrationId == integrationId &&
                x.ExternalName.Equals(externalName, StringComparison.OrdinalIgnoreCase)
            );

        if (mapping is null)
            throw new DomainException($"No mapping for {externalName}");

        return mapping.UnifiedTaskStatus;
    }


    public string MapStatusToProvider(UnifiedTaskStatus unifiedStatus, Guid integrationId)
    {
        var direct = _statusMappings.FirstOrDefault(m =>
            m.IntegrationId == integrationId &&
            m.UnifiedTaskStatus.Name.Equals(unifiedStatus.Name, StringComparison.OrdinalIgnoreCase));

        if (direct != null)
            return direct.ExternalName;

        var byCategory = _statusMappings.FirstOrDefault(m =>
            m.IntegrationId == integrationId &&
            m.UnifiedTaskStatus.Category == unifiedStatus.Category);

        if (byCategory != null)
            return byCategory.ExternalName;

        var providerDefault = _statusMappings
            .Where(m => m.IntegrationId == integrationId)
            .FirstOrDefault();

        if (providerDefault != null)
            return providerDefault.ExternalName;

        throw new DomainException(
            $"No mapping found to convert unified status '{unifiedStatus.Name}' for provider {integrationId}"
        );
    }


    // --------------------------
    // PRIORITY mapping
    // --------------------------
    public void AddPriorityMapping(ExternalPriorityMapping mapping)
    {
        _priorityMappings.Add(mapping);
        Touch();
    }

    public UnifiedPriority MapPriorityFromProvider(Guid integrationId, string externalPriority)
    {
        var match = _priorityMappings
            .Where(m => m.IntegrationId == integrationId)
            .FirstOrDefault(m => m.ExternalName.Equals(externalPriority, StringComparison.OrdinalIgnoreCase));

        if (match == null)
            throw new DomainException($"No mapping found for priority '{externalPriority}'");

        return match.UnifiedPriority;
    }

    public string MapPriorityToProvider(UnifiedPriority priority, Guid integrationId)
    {
        var match = _priorityMappings
            .FirstOrDefault(m => m.UnifiedPriority.Equals(priority) && m.IntegrationId == integrationId);

        if (match == null)
            throw new DomainException($"No provider mapping found for unified priority {priority.Name}");

        return match.ExternalName;
    }

    // --------------------------
    // CUSTOM FIELD VALUES mapping
    // --------------------------
    public void AddCustomFieldValueMapping(ExternalValueMapping mapping)
    {
        _customFieldValueMappings.Add(mapping);
        Touch();
    }

    public object? MapCustomFieldValueFromProvider(Guid integrationId, string fieldName, string externalValue)
    {
        var match = _customFieldValueMappings.FirstOrDefault(m =>
            m.IntegrationId == integrationId &&
            m.FieldName == fieldName &&
            m.ExternalValue == externalValue
        );

        return match?.UnifiedValue;
    }


    public string? MapCustomFieldValueToProvider(Guid integrationId, string fieldName, object? unifiedValue)
    {
        var match = _customFieldValueMappings
            .Where(m => m.IntegrationId == integrationId && m.FieldName == fieldName)
            .FirstOrDefault(m => Equals(m.UnifiedValue, unifiedValue));

        return match?.ExternalValue;
    }

}
