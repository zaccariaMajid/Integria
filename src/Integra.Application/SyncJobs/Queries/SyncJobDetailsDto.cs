using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Enums;

namespace Integra.Application.SyncJobs.Queries;

public sealed record SyncJobDetailsDto
(
    Guid Id,
    Guid TenantId,
    Guid SyncRuleId,
    SyncJobType JobType,
    SyncJobStatus JobStatus,
    int ProgressPercentage,
    string? ErrorMessage,
    DateTime StartedOn,
    DateTime? CompletedOn,
    DateTime? LastUpdatedOn
);