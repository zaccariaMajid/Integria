using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Abstractions;
using Integra.Application.Interfaces.Repositories;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.SyncRules.CreateSyncRule;

public sealed class CreateSyncRuleHandler 
    : ICommandHandler<CreateSyncRuleCommand, SyncRuleDto>
{
    private readonly ISyncRuleRepository _repository;
    // private readonly IUnitOfWork _unitOfWork;

    public CreateSyncRuleHandler(
        ISyncRuleRepository repository
        // IUnitOfWork unitOfWork
        )
    {
        _repository = repository;
        // _unitOfWork = unitOfWork;
    }

    public async Task<SyncRuleDto> HandleAsync(CreateSyncRuleCommand command, CancellationToken ct)
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
        // await _unitOfWork.SaveChangesAsync(ct);

        return SyncRuleDto.FromDomain(rule);
    }
}
