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

    private UnifiedLabel() { }

    private UnifiedLabel(string name, string color)
    {
        if(name is null)
            throw new DomainException("Label name cannot be null",nameof(name));
        if(color is null)
            throw new DomainException("Label color cannot be null",nameof(color));
        Name = name;
        Color = color;
    }

    public static UnifiedLabel Create(string name, string color) => new UnifiedLabel(name, color);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
