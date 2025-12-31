using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Engines;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Domain.Enums;
using MediatR;

namespace Integra.Application.SyncJobs.Commands.ExecuteSyncJob;

public sealed class ExecuteSyncJobHandler : ICommandHandler<ExecuteSyncJobCommand, Unit>
{
    private readonly ISyncJobRepository _jobRepo;         // write
    private readonly ISyncRuleRepository _ruleRepo;       // write (rule state)
    private readonly ISyncEngine _engine;                 // orchestrator

    public ExecuteSyncJobHandler(
        ISyncJobRepository jobRepo,
        ISyncRuleRepository ruleRepo,
        ISyncEngine engine)
    {
        _jobRepo = jobRepo;
        _ruleRepo = ruleRepo;
        _engine = engine;
    }

    public async Task<Unit> Handle(ExecuteSyncJobCommand cmd, CancellationToken ct)
    {
        var job = await _jobRepo.GetByIdAsync(cmd.JobId, ct)
            ?? throw new ApplicationNotFoundException("SyncJob not found");

        if (job.JobStatus is SyncJobStatus.Success or SyncJobStatus.Failed or SyncJobStatus.Canceled)
            return Unit.Value; // idempotenza: job già concluso

        // Get the rule
        var rule = await _ruleRepo.GetByIdAsync(job.SyncRuleId, ct)
            ?? throw new ApplicationNotFoundException("SyncRule not found");

        if (!rule.IsEnabled)
        {
            job.Fail("SyncRule is disabled");
            return Unit.Value;
        }

        job.Start();

        try
        {
            await _engine.ExecuteAsync(job, rule, ct);
            job.Complete();
        }
        catch (OperationCanceledException)
        {
            // cancellation token triggered
            job.Cancel();
        }
        catch (JobCanceledException)
        {
            // cancel requested
            job.Cancel();
        }
        catch (Exception ex)
        {
            job.Fail(ex.Message);
            throw; // handled by behaviour
        }

        return Unit.Value;
    }
}
