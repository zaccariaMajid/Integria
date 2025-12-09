using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;

namespace Integra.Domain.ValueObjects;

public class UnifiedPriority : ValueObject
{
    public string Name { get; private set; } = null!;
    public int Level { get; private set; }
    public UnifiedPriorityCategory Category { get; private set; }
    public string ColorHex { get; private set; } = null!;
    public List<ExternalPriorityMapping> _externalMappings { get; private set; } = new();
    public IReadOnlyList<ExternalPriorityMapping> ExternalMappings => _externalMappings.AsReadOnly();
    private UnifiedPriority(string name, int level, UnifiedPriorityCategory category, string colorHex)
    {
        Name = name;
        Level = level;
        Category = category;
        ColorHex = colorHex;
    }

    public static UnifiedPriority Create(string name, int level, UnifiedPriorityCategory category, string colorHex)
        => new UnifiedPriority(name, level, category, colorHex);
    
    public void AddExternalMapping(ExternalPriorityMapping mapping)
    {
        if(mapping == null)
            throw new ArgumentNullException(nameof(mapping));
        _externalMappings.Add(mapping);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Level;
        yield return Category;
        yield return ColorHex;
        foreach (var mapping in _externalMappings)
            yield return mapping;
    }
}
