using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public class UnifiedCustomField : ValueObject
{
    public string Name;
    public CustomFieldType Type;
    public object? Value;
    private List<ExternalFieldMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalFieldMapping> ExternalMappings
        => _externalMappings.AsReadOnly();

    private UnifiedCustomField(string name, CustomFieldType type, object? value = null)
    {
        if (name is null)
            throw new DomainException(nameof(name), "Custom field name cannot be null");
        Name = name;
        Type = type;
        Value = value;
    }

    public static UnifiedCustomField Create(string name, CustomFieldType type, object? value = null)
        => new UnifiedCustomField(name, type, value);

    public void AddExternalMapping(ExternalFieldMapping mapping)
        => _externalMappings.Add(mapping);
    public void AddExternalMappings(IEnumerable<ExternalFieldMapping> mappings)
        => _externalMappings.AddRange(mappings);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Type;
        yield return Value ?? string.Empty;
        foreach (var mapping in _externalMappings)
        {
            yield return mapping;
        }
    }
}
