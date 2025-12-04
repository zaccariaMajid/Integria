using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.Common;

public abstract record DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
