using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Integra.Application.SyncRules.Queries.ListByTenant;

namespace Integra.Application.Validators;

public sealed class ListSyncRulesByTenantValidator
    : AbstractValidator<ListSyncRulesByTenantQuery>
{
    public ListSyncRulesByTenantValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty();
    }
}
