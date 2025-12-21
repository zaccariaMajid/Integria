using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Integra.Application.SyncJobs.Queries.GetById;

namespace Integra.Application.Validators;

public sealed class GetSyncJobByIdValidator
    : AbstractValidator<GetSyncJobByIdQuery>
{
    public GetSyncJobByIdValidator()
    {
        RuleFor(x => x.SyncJobId).NotEmpty();
    }
}
