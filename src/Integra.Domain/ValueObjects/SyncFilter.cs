using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;

public sealed class SyncFilter : ValueObject
{
    // Filtra per status (unificati)
    public IReadOnlyCollection<string>? Statuses { get; private set; }

    // Filtra per label
    public IReadOnlyCollection<string>? Labels { get; private set; }

    // Filtra per priority
    public IReadOnlyCollection<string>? Priorities { get; private set; }

    // Filtra per assignee (UnifiedUser.Id)
    public IReadOnlyCollection<Guid>? Assignees { get; private set; }

    // Solo elementi modificati dopo una certa data
    public DateTime? UpdatedAfter { get; private set; }

    // Filtra per custom fields specifici
    public IReadOnlyCollection<string>? CustomFieldNames { get; private set; }

    // Filtra per integrazione (es: sincronizza solo elementi provenienti da Notion)
    public Guid? OnlyFromIntegration { get; private set; }

    private SyncFilter() { }

    private SyncFilter(
        IReadOnlyCollection<string>? statuses,
        IReadOnlyCollection<string>? labels,
        IReadOnlyCollection<string>? priorities,
        IReadOnlyCollection<Guid>? assignees,
        DateTime? updatedAfter,
        IReadOnlyCollection<string>? customFieldNames,
        Guid? onlyFromIntegration)
    {
        Statuses = statuses;
        Labels = labels;
        Priorities = priorities;
        Assignees = assignees;
        UpdatedAfter = updatedAfter;
        CustomFieldNames = customFieldNames;
        OnlyFromIntegration = onlyFromIntegration;
    }

    public static SyncFilter Create(
        IReadOnlyCollection<string>? statuses = null,
        IReadOnlyCollection<string>? labels = null,
        IReadOnlyCollection<string>? priorities = null,
        IReadOnlyCollection<Guid>? assignees = null,
        DateTime? updatedAfter = null,
        IReadOnlyCollection<string>? customFieldNames = null,
        Guid? onlyFromIntegration = null)
    {
        return new SyncFilter(
            statuses,
            labels,
            priorities,
            assignees,
            updatedAfter,
            customFieldNames,
            onlyFromIntegration);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Statuses != null ? string.Join(",", Statuses) : null;
        yield return Labels != null ? string.Join(",", Labels) : null;
        yield return Priorities != null ? string.Join(",", Priorities) : null;
        yield return Assignees != null ? string.Join(",", Assignees) : null;
        yield return UpdatedAfter;
        yield return CustomFieldNames != null ? string.Join(",", CustomFieldNames) : null;
        yield return OnlyFromIntegration;
    }
}

