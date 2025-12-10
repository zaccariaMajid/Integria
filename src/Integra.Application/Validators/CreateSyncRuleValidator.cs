using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Integra.Application.SyncRules.CreateSyncRule;

namespace Integra.Application.Validators;

public class CreateSyncRuleValidator
    : AbstractValidator<CreateSyncRuleCommand>
{
    public CreateSyncRuleValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage("TenantId is required.");
        RuleFor(x => x.SourceIntegrationId)
            .NotEmpty().WithMessage("SourceIntegrationId is required.");
        RuleFor(x => x.TargetIntegrationId)
            .NotEmpty().WithMessage("TargetIntegrationId is required.");
        RuleFor(x => x.Direction)
            .IsInEnum().WithMessage("Direction must be a valid enum value.");
        RuleFor(x => x.Scope)
            .IsInEnum().WithMessage("Scope must be a valid enum value.");
        RuleFor(x => x.ConflictPolicy)
            .IsInEnum().WithMessage("ConflictPolicy must be a valid enum value.");
    }
}
