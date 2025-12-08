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
    // "Status", "Priority"
    public string FieldName { get; private set; } = null!;
    // es. "InProgress"
    public string UnifiedValue { get; private set; } = null!;

    private List<ExternalValueMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalValueMapping> ExternalMappings => _externalMappings.AsReadOnly();

    private FieldMapping() { }

    private FieldMapping(string fieldName, string unifiedValue, IEnumerable<ExternalValueMapping> externalMappings)
    {
        if(string.IsNullOrWhiteSpace(fieldName))
            throw new DomainException("Field name cannot be null or empty.", nameof(fieldName));
        if(string.IsNullOrWhiteSpace(unifiedValue))
            throw new DomainException("Unified value cannot be null or empty.", nameof(unifiedValue));
        FieldName = fieldName;
        UnifiedValue = unifiedValue;
        _externalMappings = externalMappings.ToList();
    }

    public static FieldMapping Create(string fieldName, string unifiedValue, IEnumerable<ExternalValueMapping> externalMappings)
        =>new FieldMapping(fieldName, unifiedValue, externalMappings);

}
