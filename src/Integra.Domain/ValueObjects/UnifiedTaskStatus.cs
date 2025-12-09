using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public sealed class UnifiedTaskStatus : ValueObject
{
    public string Name { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public UnifiedTaskStatusCategory Category { get; private set; }
    public List<ExternalStatusMapping> _externalStatusMappings = new();
    public IReadOnlyList<ExternalStatusMapping> ExternalMappings => _externalStatusMappings.AsReadOnly();

    private UnifiedTaskStatus() { }

    private UnifiedTaskStatus(string name, string color, UnifiedTaskStatusCategory category)
    {
        if (name is null)
            throw new DomainException("Status name cannot be null", nameof(name));
        if (color is null)
            throw new DomainException("Status color cannot be null", nameof(color));
        Name = name;
        Color = color;
        Category = category;
    }
    public static UnifiedTaskStatus Create(string name, string color, UnifiedTaskStatusCategory category)
        => new UnifiedTaskStatus(name, color, category);

    public void AddExternalMapping(ExternalStatusMapping mapping)
    {
        if (mapping is null)
            throw new DomainException("External mapping cannot be null", nameof(mapping));
        _externalStatusMappings.Add(mapping);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Color;
        yield return Category;
        foreach (var mapping in _externalStatusMappings)
            yield return mapping;
    }
}
