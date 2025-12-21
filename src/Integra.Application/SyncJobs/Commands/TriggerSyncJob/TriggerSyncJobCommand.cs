using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using Integra.Domain.Enums;

namespace Integra.Application.SyncJobs.Commands.TriggerSyncJob;

public sealed record TriggerSyncJobCommand(
    Guid TenantId,
    Guid SyncRuleId,
    SyncJobType JobType
) : ICommand<Guid>;