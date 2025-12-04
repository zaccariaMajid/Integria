using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
