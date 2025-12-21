using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Enums;

namespace Integra.Application.SyncJobs.Commands.RetrySyncJob;

public sealed class RetrySyncJobHandler
    : ICommandHandler<RetrySyncJobCommand, Guid>
{
    private readonly ISyncJobRepository _jobRepository;

    public RetrySyncJobHandler(ISyncJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Guid> Handle(
        RetrySyncJobCommand cmd,
        CancellationToken ct)
    {
        var failedJob = await _jobRepository.GetByIdAsync(cmd.FailedJobId, ct)
            ?? throw new ApplicationNotFoundException("SyncJob not found");

        if (failedJob.JobStatus != SyncJobStatus.Failed)
            throw new ApplicationDomainException("Only failed jobs can be retried");

        var retryJob = SyncJob.Create(
            syncRuleId: failedJob.SyncRuleId,
            tenantId: failedJob.TenantId,
            jobType: SyncJobType.Retry
        );

        await _jobRepository.AddAsync(retryJob, ct);

        return retryJob.Id;
    }
}
