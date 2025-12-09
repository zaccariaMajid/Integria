using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.AggregateRoots;

public sealed class SyncSchedule : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }

    // Frequenza semplice (ogni X)
    public TimeSpan? Frequency { get; private set; }

    // Espressione cron (alternativa a Frequency)
    public string? CronExpression { get; private set; }

    // Stato schedule
    public bool IsEnabled { get; private set; }

    // Tracciamento esecuzioni
    public DateTime? LastRun { get; private set; }
    public DateTime? NextRun { get; private set; }

    // Retry logic
    public int MaxRetries { get; private set; }
    public TimeSpan RetryBackoff { get; private set; }

    private SyncSchedule() { }

    private SyncSchedule(
        Guid tenantId,
        TimeSpan? frequency,
        string? cronExpression,
        bool isEnabled,
        int maxRetries,
        TimeSpan retryBackoff)
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("TenantId cannot be empty.", nameof(tenantId));

        if (frequency == null && string.IsNullOrWhiteSpace(cronExpression))
            throw new DomainException("A schedule must have either Frequency or CronExpression.");

        if (frequency != null && frequency <= TimeSpan.Zero)
            throw new DomainException("Frequency must be greater than zero.");

        TenantId = tenantId;
        Frequency = frequency;
        CronExpression = cronExpression;
        IsEnabled = isEnabled;
        MaxRetries = maxRetries;
        RetryBackoff = retryBackoff;
    }

    public static SyncSchedule CreateInterval(
        Guid tenantId,
        TimeSpan frequency,
        bool isEnabled = true,
        int maxRetries = 3,
        TimeSpan? retryBackoff = null)
    {
        return new SyncSchedule(
            tenantId,
            frequency,
            null,
            isEnabled,
            maxRetries,
            retryBackoff ?? TimeSpan.FromSeconds(30)
        );
    }

    public static SyncSchedule CreateCron(
        Guid tenantId,
        string cronExpression,
        bool isEnabled = true,
        int maxRetries = 3,
        TimeSpan? retryBackoff = null)
    {
        return new SyncSchedule(
            tenantId,
            null,
            cronExpression,
            isEnabled,
            maxRetries,
            retryBackoff ?? TimeSpan.FromSeconds(30)
        );
    }

    public void MarkRun(DateTime runTime, DateTime nextRun)
    {
        LastRun = runTime;
        NextRun = nextRun;
    }

    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;

    // private DateTime? ComputeNextRun(DateTime from)
    // {
    //     if (Frequency != null)
    //         return from + Frequency;

    //     if (!string.IsNullOrWhiteSpace(CronExpression))
    //     {
    //         var next = CronParser.GetNextOccurrence(CronExpression, from);
    //         return next;
    //     }

    //     return null;
    // }
}
