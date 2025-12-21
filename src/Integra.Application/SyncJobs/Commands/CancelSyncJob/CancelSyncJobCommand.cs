using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.SyncJobs.Commands.CancelSyncJob;

public sealed record CancelSyncJobCommand(
    Guid TenantId,
    Guid SyncJobId
) : ICommand<Unit>;
