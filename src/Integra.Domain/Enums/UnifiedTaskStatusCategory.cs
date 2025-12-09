using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Domain.Enums;

public enum UnifiedTaskStatusCategory
{
    NotStarted,
    Active,
    Waiting,
    Completed,
    Cancelled
}
