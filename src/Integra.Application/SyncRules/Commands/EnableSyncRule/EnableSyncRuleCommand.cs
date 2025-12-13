using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Interfaces;
using MediatR;

namespace Integra.Application.SyncRules.Commands.EnableSyncRule;

public sealed record EnableSyncRuleCommand(Guid SyncRuleId) : ICommand<Unit>;
