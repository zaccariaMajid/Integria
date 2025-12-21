using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;

namespace Integra.Application.SyncJobs.Queries.GetById;

public sealed record GetSyncJobByIdQuery(Guid SyncJobId)
    : IQuery<SyncJobDetailsDto>;
