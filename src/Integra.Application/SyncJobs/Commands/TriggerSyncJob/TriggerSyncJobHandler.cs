using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.SyncJobs.Commands.TriggerSyncJob;

public sealed class TriggerSyncJobHandler
    : ICommandHandler<TriggerSyncJobCommand, Guid>
{
    private readonly ISyncRuleRepository _syncRuleRepository;
    private readonly ISyncJobRepository _syncJobRepository;

    public TriggerSyncJobHandler(
        ISyncRuleRepository syncRuleRepository,
        ISyncJobRepository syncJobRepository)
    {
        _syncRuleRepository = syncRuleRepository;
        _syncJobRepository = syncJobRepository;
    }

    public async Task<Guid> Handle(
        TriggerSyncJobCommand cmd,
        CancellationToken ct)
    {
        // Verify if the SyncRule exists and is enabled
        var rule = await _syncRuleRepository.GetByIdAsync(cmd.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException("SyncRule not found");

        if (!rule.IsEnabled)
            throw new ApplicationDomainException("SyncRule is disabled");

        // Create the job
        var job = SyncJob.Create(
            syncRuleId: rule.Id,
            tenantId: rule.TenantId,
            jobType: cmd.JobType
        );

        // Save the job
        await _syncJobRepository.AddAsync(job, ct);

        return job.Id;
    }
}
