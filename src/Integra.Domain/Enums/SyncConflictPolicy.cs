using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Domain.Enums;

public enum SyncConflictPolicy
{
    LastWriteWins = 1,

    PreferSource = 2,
    PreferTarget = 3,

    MostAdvancedStatusWins = 4,

    HighestPriorityWins = 5,

    SmartMerge = 6
}
