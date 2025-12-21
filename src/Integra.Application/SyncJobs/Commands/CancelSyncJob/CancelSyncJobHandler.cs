using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using MediatR;

namespace Integra.Application.SyncJobs.Commands.CancelSyncJob;

public sealed class CancelSyncJobHandler
    : ICommandHandler<CancelSyncJobCommand, Unit>
{
    private readonly ISyncJobRepository _jobRepository;

    public CancelSyncJobHandler(ISyncJobRepository jobRepository)
    {
        _jobRepository = jobRepository;
    }

    public async Task<Unit> Handle(
        CancelSyncJobCommand cmd,
        CancellationToken ct)
    {
        var job = await _jobRepository.GetByIdAsync(cmd.SyncJobId, ct)
            ?? throw new ApplicationNotFoundException("SyncJob not found");

        job.Cancel();

        return Unit.Value;
    }
}
