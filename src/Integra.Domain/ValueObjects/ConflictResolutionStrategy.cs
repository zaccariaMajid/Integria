using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;

namespace Integra.Domain.ValueObjects;

public sealed class ConflictResolutionStrategy : ValueObject
{
    public SyncConflictPolicy Policy { get; private set; }

    public ConflictResolutionStrategy(SyncConflictPolicy policy)
    {
        Policy = policy;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Policy;
    }
}
