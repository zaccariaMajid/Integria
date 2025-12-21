using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Abstractions.Persistence;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.Interfaces.Repositories.SyncJobs;

public interface ISyncJobRepository : IEfRepository<SyncJob>
{
}
