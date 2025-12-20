using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Integra.Application.SyncRules.Queries.ListByIntegration;

namespace Integra.Application.Validators;

public sealed class ListSyncRulesByIntegrationValidator
    : AbstractValidator<ListSyncRulesByIntegrationQuery>
{
    public ListSyncRulesByIntegrationValidator()
    {
        RuleFor(x => x.IntegrationId).NotEmpty();
    }
}