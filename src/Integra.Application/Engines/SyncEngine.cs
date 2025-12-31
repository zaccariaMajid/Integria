using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Engines;
using Integra.Application.Interfaces.Providers;
using Integra.Application.Interfaces.Repositories.FieldMapping;
using Integra.Application.Interfaces.Repositories.Integration;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Enums;

namespace Integra.Application.Engines;
public sealed class SyncEngine : ISyncEngine
{
    private readonly IProviderConnectorFactory _connectorFactory;
    private readonly IFieldMappingReadRepository _mappingRepo;
    private readonly IIntegrationReadRepository _integrationRepo;
    private readonly ISyncJobRepository _jobRepo;

    public SyncEngine(
        IProviderConnectorFactory connectorFactory,
        IFieldMappingReadRepository mappingRepo,
        IIntegrationReadRepository integrationRepo,
        ISyncJobRepository jobRepo)
    {
        _connectorFactory = connectorFactory;
        _mappingRepo = mappingRepo;
        _integrationRepo = integrationRepo;
        _jobRepo = jobRepo;
    }

    public async Task ExecuteAsync(SyncJob job, SyncRule rule, CancellationToken ct)
    {
        // 1) Load mapping + integrations
        var mapping = await _mappingRepo.GetByIdAsync(rule.FieldMappingId, ct)
            ?? throw new ApplicationNotFoundException("FieldMapping not found");

        var sourceIntegration = await _integrationRepo.GetByIdAsync(rule.SourceIntegrationId, ct)
            ?? throw new ApplicationNotFoundException("Source integration not found");

        var targetIntegration = await _integrationRepo.GetByIdAsync(rule.TargetIntegrationId, ct)
            ?? throw new ApplicationNotFoundException("Target integration not found");

        var source = _connectorFactory.Create(sourceIntegration);
        var target = _connectorFactory.Create(targetIntegration);

        // 2) Determine checkpoint/cursor (opzionale)
        // var cursor = await _checkpointRepo.Get(job.Id) ...

        // 3) Pull in chunks
        const int chunkSize = 50;
        int processed = 0;

        await foreach (var chunk in source.PullTasksChunkedAsync(rule.Scope, chunkSize, ct))
        {
            // 3.1 check cancellation
            await EnsureNotCanceledAsync(job.Id, ct);

            // 3.2 Normalize + Map + Diff + Push
            foreach (var srcTask in chunk)
            {
                await EnsureNotCanceledAsync(job.Id, ct);

                var unified = mapping.NormalizeFromSource(srcTask, sourceIntegration);

                // diff with last sync state
                var targetPayload = mapping.DenormalizeToTarget(unified, targetIntegration);

                await target.UpsertTaskAsync(targetPayload, ct);

                processed++;
                if (processed % 10 == 0)
                {
                    job.ReportProgress(CalcProgress(processed));
                    await _jobRepo.UpdateAsync(job, ct);
                }
            }

            // 3.3 checkpoint after chunk (optional)
        }

        // 4) final progress
        job.ReportProgress(100);
    }

    private static int CalcProgress(int processed)
    {
        // progress per chunk
        return Math.Min(99, processed / 10);
    }

    private async Task EnsureNotCanceledAsync(Guid jobId, CancellationToken ct)
    {
        // Simple approach: reload job state every N steps (or use a cached flag)
        // Ideally: a lightweight repository or a distributed cache.
        var current = await _jobRepo.GetByIdAsync(jobId, ct);
        if (current is null) return;

        if (current.JobStatus == SyncJobStatus.Canceled)
            throw new JobCanceledException("Job is already canceled");
    }
}
