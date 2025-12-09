using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public sealed class UnifiedComment : ValueObject
{
    public UnifiedUser Author { get; private set; } = null!;
    public string Body { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private List<ExternalMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalMapping> ExternalMappings
        => _externalMappings.AsReadOnly();

    private UnifiedComment() { }

    private UnifiedComment(UnifiedUser author, string body, DateTime createdAt, DateTime? updatedAt)
    {
        if (author is null)
            throw new DomainException(nameof(author), "Author cannot be null");
        if (body is null)
            throw new DomainException(nameof(body), "Comment body cannot be null");

        Author = author;
        Body = body;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static UnifiedComment Create(UnifiedUser author, string body, DateTime createdAt, DateTime? updatedAt = null)
        => new UnifiedComment(author, body, createdAt, updatedAt);

    public void AddExternalMapping(ExternalMapping mapping)
        => _externalMappings.Add(mapping);
    public void AddExternalMappings(IEnumerable<ExternalMapping> mappings)
        => _externalMappings.AddRange(mappings);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Author;
        yield return Body;
        yield return CreatedAt;
        yield return UpdatedAt ?? DateTime.MinValue;
        foreach (var mapping in _externalMappings)
        {
            yield return mapping;
        }
    }
}
