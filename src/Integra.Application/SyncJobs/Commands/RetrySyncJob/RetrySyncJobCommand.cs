using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;

namespace Integra.Application.SyncJobs.Commands.RetrySyncJob;

public sealed record RetrySyncJobCommand(
    Guid TenantId,
    Guid FailedJobId
) : ICommand<Guid>;