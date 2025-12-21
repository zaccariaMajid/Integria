using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Integra.Application.SyncJobs.Queries.ListRecentSyncJobs;

namespace Integra.Application.Validators;

public sealed class ListRecentSyncJobsValidator
    : AbstractValidator<ListRecentSyncJobsQuery>
{
    public ListRecentSyncJobsValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
        RuleFor(x => x.Take).GreaterThan(0).LessThanOrEqualTo(100);
    }
}