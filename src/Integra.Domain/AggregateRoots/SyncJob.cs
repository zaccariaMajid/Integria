using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;
using Integra.Domain.Exceptions;

namespace Integra.Domain.AggregateRoots;

/// <summary>
/// Represent a scheduled or executed job
/// Manage sync status (pending, running, failed, success)
/// Contain relevant logs
/// Allow retries and restarts
/// </summary>
public sealed class SyncJob : AggregateRoot<Guid>
{
    public Guid SyncRuleId { get; private set; }
    public Guid TenantId { get; private set; }

    public SyncJobType JobType { get; private set; }
    public SyncJobStatus JobStatus { get; private set; }

    public int ProgressPercentage { get; private set; }
    public string? ErrorMessage { get; private set; }

    public int RetryCount { get; private set; }

    public DateTime StartedOn { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public DateTime LastUpdatedOn { get; private set; }

    private SyncJob() { }

    private SyncJob(Guid syncRuleId, Guid tenantId, SyncJobType jobType)
    {
        if (syncRuleId == Guid.Empty)
            throw new DomainException("SyncRuleId cannot be empty");
        if (tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty");

        Id = Guid.NewGuid();
        SyncRuleId = syncRuleId;
        TenantId = tenantId;
        JobType = jobType;

        JobStatus = SyncJobStatus.Pending;
        StartedOn = DateTime.UtcNow;
        LastUpdatedOn = StartedOn;

        AddDomainEvent(new SyncJobCreated(Id, SyncRuleId, JobType));
    }

    public static SyncJob Create(Guid syncRuleId, Guid tenantId, SyncJobType jobType)
        => new SyncJob(syncRuleId, tenantId, jobType);

    // --------------------------
    // State transitions
    // --------------------------

    public void Start()
    {
        JobStatus = SyncJobStatus.Running;
        StartedOn = DateTime.UtcNow;
        LastUpdatedOn = StartedOn;

        AddDomainEvent(new SyncJobStarted(Id));
    }

    public void ReportProgress(int progress)
    {
        if (progress < 0 || progress > 100)
            throw new DomainException("Progress must be between 0 and 100");

        ProgressPercentage = progress;
        LastUpdatedOn = DateTime.UtcNow;
    }

    public void Complete()
    {
        JobStatus = SyncJobStatus.Success;
        ProgressPercentage = 100;
        CompletedOn = DateTime.UtcNow;
        LastUpdatedOn = CompletedOn.Value;

        AddDomainEvent(new SyncJobCompleted(Id));
    }

    public void Fail(string message)
    {
        JobStatus = SyncJobStatus.Failed;
        ErrorMessage = message;
        CompletedOn = DateTime.UtcNow;
        LastUpdatedOn = CompletedOn.Value;

        AddDomainEvent(new SyncJobFailed(Id, message));
    }

    public void Retry()
    {
        RetryCount++;
        JobStatus = SyncJobStatus.Retrying;
        LastUpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new SyncJobRetried(Id, RetryCount));
    }
}
