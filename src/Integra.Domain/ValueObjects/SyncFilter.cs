using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;

public sealed class SyncFilter : ValueObject
{
    // Filtra per status (unificati)
    private List<string> _statuses = new();
    public IReadOnlyCollection<string> Statuses => _statuses.AsReadOnly();

    // Filtra per label
    private List<string> _labels = new();
    public IReadOnlyCollection<string> Labels => _labels.AsReadOnly();

    // Filtra per priority
    private List<string> _priorities = new();
    public IReadOnlyCollection<string> Priorities => _priorities.AsReadOnly();

    // Filtra per assignee (UnifiedUser.Id)
    private List<Guid> _assignees = new();
    public IReadOnlyCollection<Guid> Assignees => _assignees.AsReadOnly();

    // Solo elementi modificati dopo una certa data
    public DateTime? UpdatedAfter { get; private set; }

    // Filtra per custom fields specifici
    private List<string> _customFieldNames = new();
    public IReadOnlyCollection<string>? CustomFieldNames => _customFieldNames.AsReadOnly();

    // Filtra per integrazione (es: sincronizza solo elementi provenienti da Notion)
    public Guid? OnlyFromIntegration { get; private set; }

    private SyncFilter() { }

    private SyncFilter(
        DateTime? updatedAfter,
        Guid? onlyFromIntegration)
    {
        UpdatedAfter = updatedAfter;
        OnlyFromIntegration = onlyFromIntegration;
    }

    public static SyncFilter Create(
        DateTime? updatedAfter = null,
        Guid? onlyFromIntegration = null)
    {
        return new SyncFilter(
            updatedAfter,
            onlyFromIntegration);
    }
    public void AddStatus(string status)
    {
        _statuses.Add(status);
    }
    public void AddLabel(string label)
    {
        _labels.Add(label);
    }
    public void AddPriority(string priority)
    {
        _priorities.Add(priority);
    }
    public void AddAssignee(Guid assigneeId)
    {
        _assignees.Add(assigneeId);
    }
    public void AddCustomFieldName(string fieldName)
    {
        _customFieldNames.Add(fieldName);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var status in _statuses)
            yield return status;
        foreach (var label in _labels)
            yield return label;
        foreach (var priority in _priorities)
            yield return priority;
        foreach (var assignee in _assignees)
            yield return assignee;
        yield return UpdatedAfter ?? DateTime.MinValue;
        foreach (var fieldName in _customFieldNames)
            yield return fieldName;
        yield return OnlyFromIntegration ?? Guid.Empty;
    }
}

