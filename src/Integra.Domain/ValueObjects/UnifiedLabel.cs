using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public class UnifiedLabel : ValueObject
{
    public string Name { get; private set; } = null!;
    public string Color { get; private set; } = null!;
    public List<ExternalMapping> _externalMappings = new();
    public IReadOnlyList<ExternalMapping> ExternalMappings => _externalMappings.AsReadOnly();

    private UnifiedLabel() { }

    private UnifiedLabel(string name, string color)
    {
        if (name is null)
            throw new DomainException("Label name cannot be null", nameof(name));
        if (color is null)
            throw new DomainException("Label color cannot be null", nameof(color));
        Name = name;
        Color = color;
    }

    public static UnifiedLabel Create(string name, string color)
        => new UnifiedLabel(name, color);

    public void AddExternalMapping(ExternalMapping mapping)
    {
        if (mapping is null)
            throw new DomainException("External mapping cannot be null", nameof(mapping));
        _externalMappings.Add(mapping);
    }
    public void RemoveExternalMapping(ExternalMapping mapping)
    {
        if (mapping is null)
            throw new DomainException("External mapping cannot be null", nameof(mapping));
        _externalMappings.Remove(mapping);
    }
    public void ClearExternalMappings()
    {
        _externalMappings.Clear();
    }
    public void UpdateColor(string color)
    {
        if (color is null)
            throw new DomainException("Label color cannot be null", nameof(color));
        Color = color;
    }
    public void UpdateName(string name)
    {
        if (name is null)
            throw new DomainException("Label name cannot be null", nameof(name));
        Name = name;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Color;
    }
}
