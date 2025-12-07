using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public class UnifiedSubtask : ValueObject
{
    public string Title { get; private set; } = null!;
    public UnifiedTaskStatus Status { get; private set; }

    private List<ExternalMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalMapping> ExternalMappings => _externalMappings;

    private UnifiedSubtask() { }

    private UnifiedSubtask(string title, UnifiedTaskStatus status)
    {
        if(title is null)
            throw new DomainException(nameof(title), "Subtask title cannot be null");
        Title = title;
        Status = status;
    }

    public static UnifiedSubtask Create(string title, UnifiedTaskStatus status)
        => new UnifiedSubtask(title, status);

    // add single or multiple mappings
    public void AddExternalMapping(ExternalMapping mapping) => _externalMappings.Add(mapping);
    public void AddExternalMappings(IEnumerable<ExternalMapping> mappings) => _externalMappings.AddRange(mappings);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Status;
        foreach (var mapping in _externalMappings)
        {
            yield return mapping;
        }
    }
}
