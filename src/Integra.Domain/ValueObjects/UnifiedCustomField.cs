using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Enums;

namespace Integra.Domain.ValueObjects;

public class UnifiedCustomField
{
    public string Name;
    public CustomFieldType Type;
    public object? Value;
    private List<ExternalFieldMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalFieldMapping> ExternalMappings => _externalMappings;

    private UnifiedCustomField(string name, CustomFieldType type, object? value = null)
    {
        Name = name;
        Type = type;
        Value = value;
    }

    public static UnifiedCustomField Create(string name, CustomFieldType type, object? value = null)
        => new UnifiedCustomField(name, type, value);

    // add single or multiple mappings
    public void AddExternalMapping(ExternalFieldMapping mapping) => _externalMappings.Add(mapping);
    public void AddExternalMappings(IEnumerable<ExternalFieldMapping> mappings) => _externalMappings.AddRange(mappings);
}
