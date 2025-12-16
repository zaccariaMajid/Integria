using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Integra.Application.SyncRules.Commands.UpdateSyncRule;

namespace Integra.Application.Validators;

public class UpdateSyncRuleValidator
    : AbstractValidator<UpdateSyncRuleCommand>
{
    public UpdateSyncRuleValidator()
    {
        RuleFor(x => x.SyncRuleId)
            .NotEmpty().WithMessage("SyncRuleId is required.");
        RuleFor(x => x.Scope)
            .IsInEnum().WithMessage("Scope must be a valid enum value.");
        RuleFor(x => x.ConflictPolicy)
            .IsInEnum().WithMessage("ConflictPolicy must be a valid enum value.");
    }
}
