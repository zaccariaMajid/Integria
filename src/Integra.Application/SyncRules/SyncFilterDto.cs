using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.ValueObjects;

namespace Integra.Application.SyncRules;

public sealed record SyncFilterDto(
    IEnumerable<string>? Statuses,
    IEnumerable<string>? Labels,
    IEnumerable<string>? Priorities,
    IEnumerable<Guid>? Assignees,
    DateTime? UpdatedAfter,
    IEnumerable<string>? CustomFieldNames,
    Guid? OnlyFromIntegration
)
{
    public SyncFilter ToValueObject()
    {
        var filter = SyncFilter.Create(
            updatedAfter: UpdatedAfter,
            onlyFromIntegration: OnlyFromIntegration
        );
        
        filter.AddStatuses(Statuses?.ToList() ?? new List<string>());
        filter.AddLabels(Labels?.ToList() ?? new List<string>());
        filter.AddPriorities(Priorities?.ToList() ?? new List<string>());
        filter.AddAssignees(Assignees?.ToList() ?? new List<Guid>());
        filter.AddCustomFieldNames(CustomFieldNames?.ToList() ?? new List<string>());

        return filter;
    }
}
