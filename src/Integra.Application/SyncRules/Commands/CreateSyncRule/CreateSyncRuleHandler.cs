using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Abstractions;
using Integra.Application.Interfaces.Repositories;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.SyncRules.Commands.CreateSyncRule;

public sealed class CreateSyncRuleHandler
    : ICommandHandler<CreateSyncRuleCommand, SyncRuleDto>
{
    private readonly ISyncRuleRepository _repository;

    public CreateSyncRuleHandler(
        ISyncRuleRepository repository
        )
    {
        _repository = repository;
    }

    public async Task<SyncRuleDto> Handle(CreateSyncRuleCommand command, CancellationToken ct)
    {
        // DTO → ValueObject
        var filter = command.Filter?.ToValueObject();

        // Domain entity creation
        var rule = SyncRule.Create(
            tenantId: command.TenantId,
            sourceIntegrationId: command.SourceIntegrationId,
            targetIntegrationId: command.TargetIntegrationId,
            direction: command.Direction,
            scope: command.Scope,
            filter: filter,
            conflictPolicy: command.ConflictPolicy,
            fieldMappingId: command.FieldMappingId,
            syncScheduleId: command.SyncScheduleId
        );

        await _repository.AddAsync(rule);

        return SyncRuleDto.FromDomain(rule);
    }
}
