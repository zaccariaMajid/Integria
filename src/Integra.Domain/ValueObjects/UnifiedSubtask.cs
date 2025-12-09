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
    public UnifiedTaskStatus Status { get; private set; } = null!;
    public UnifiedUser? Assignee { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private List<ExternalMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalMapping> ExternalMappings
        => _externalMappings.AsReadOnly();

    private UnifiedSubtask() { }

    private UnifiedSubtask(string title, UnifiedUser assignee, DateTime createdAt, DateTime updatedAt, UnifiedTaskStatus status)
    {
        if (title is null)
            throw new DomainException(nameof(title), "Subtask title cannot be null");
        if (updatedAt > createdAt)
            throw new DomainException(nameof(updatedAt), "Subtask update date cannot be greater than creation date");
        Title = title;
        Status = status;
        Assignee = assignee;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static UnifiedSubtask Create(string title, UnifiedUser assignee, DateTime createdAt, DateTime updatedAt, UnifiedTaskStatus status)
        => new UnifiedSubtask(title, assignee, createdAt, updatedAt, status);

    // add single or multiple mappings
    public void AddExternalMapping(ExternalMapping mapping)
        => _externalMappings.Add(mapping);
    public void AddExternalMappings(IEnumerable<ExternalMapping> mappings)
        => _externalMappings.AddRange(mappings);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Status; ;
        yield return CreatedAt;
        yield return UpdatedAt;
        foreach (var mapping in _externalMappings)
        {
            yield return mapping;
        }
    }
}
