using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;

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
    public DateTime StartedOn { get; private set; }
    public DateTime? CompletedOn { get; private set; }
    public DateTime? LastUpdatedOn { get; private set; }
    private SyncJob() : base() { }

    private SyncJob(
        Guid syncRuleId,
        Guid tenantId,
        SyncJobType jobType,
        SyncJobStatus jobStatus,
        int progressPercentage,
        string? errorMessage,
        DateTime startedOn,
        DateTime? completedOn,
        DateTime? lastUpdatedOn) : base()
    {
        Id = Guid.NewGuid();
        SyncRuleId = syncRuleId;
        TenantId = tenantId;
        JobType = jobType;
        JobStatus = jobStatus;
        ProgressPercentage = progressPercentage;
        ErrorMessage = errorMessage;
        StartedOn = startedOn;
        CompletedOn = completedOn;
        LastUpdatedOn = lastUpdatedOn;

        AddDomainEvent(new SyncJobCreated(Id));
    }

    public SyncJob Create(
        Guid syncRuleId,
        Guid tenantId,
        SyncJobType jobType,
        SyncJobStatus jobStatus,
        int progressPercentage,
        string? errorMessage,
        DateTime startedOn,
        DateTime? completedOn,
        DateTime? lastUpdatedOn)
    {
        if(syncRuleId == Guid.Empty)
            throw new ArgumentException("SyncRuleId cannot be empty", nameof(syncRuleId));
        if(tenantId == Guid.Empty)
            throw new ArgumentException("TenantId cannot be empty", nameof(tenantId));

        return new SyncJob(
            syncRuleId,
            tenantId,
            jobType,
            jobStatus,
            progressPercentage,
            errorMessage,
            startedOn,
            completedOn,
            lastUpdatedOn);
    }
}
