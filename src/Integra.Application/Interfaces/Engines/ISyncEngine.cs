using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.Interfaces.Engines;

public interface ISyncEngine
{
    Task ExecuteAsync(SyncJob job, SyncRule rule, CancellationToken ct);
}
