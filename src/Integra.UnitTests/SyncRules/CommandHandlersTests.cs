using FluentAssertions;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Application.SyncRules.Commands.AssignScheduleToSyncRule;
using Integra.Application.SyncRules.Commands.CreateSyncRule;
using Integra.Application.SyncRules.Commands.DisableSyncRule;
using Integra.Application.SyncRules.Commands.EnableSyncRule;
using Integra.Application.SyncRules.Commands.RemoveScheduleFromSyncRule;
using Integra.Application.SyncRules.Commands.UpdateSyncRule;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Enums;
using Moq;

namespace Integra.UnitTests.SyncRules;

[TestClass]
public class CommandHandlersTests
{
    [TestMethod]
    public async Task CreateSyncRuleHandler_AddsRule_AndReturnsDto()
    {
        var repo = new Mock<ISyncRuleRepository>();
        var handler = new CreateSyncRuleHandler(repo.Object);
        var command = new CreateSyncRuleCommand(
            TenantId: Guid.NewGuid(),
            SourceIntegrationId: Guid.NewGuid(),
            TargetIntegrationId: Guid.NewGuid(),
            Direction: SyncDirection.Bidirectional,
            Scope: SyncScope.Tasks,
            Filter: null,
            ConflictPolicy: SyncConflictPolicy.PreferSource,
            FieldMappingId: Guid.NewGuid(),
            SyncScheduleId: null);

        var dto = await handler.Handle(command, CancellationToken.None);

        repo.Verify(r => r.AddAsync(It.Is<SyncRule>(rule =>
            rule.TenantId == command.TenantId &&
            rule.SourceIntegrationId == command.SourceIntegrationId &&
            rule.TargetIntegrationId == command.TargetIntegrationId &&
            rule.Direction == command.Direction &&
            rule.Scope == command.Scope &&
            rule.ConflictPolicy == command.ConflictPolicy &&
            rule.FieldMappingId == command.FieldMappingId &&
            rule.SyncScheduleId == command.SyncScheduleId &&
            rule.IsEnabled)), Times.Once);

        dto.TenantId.Should().Be(command.TenantId);
        dto.SourceIntegrationId.Should().Be(command.SourceIntegrationId);
        dto.TargetIntegrationId.Should().Be(command.TargetIntegrationId);
        dto.Direction.Should().Be(command.Direction);
        dto.Scope.Should().Be(command.Scope);
        dto.ConflictPolicy.Should().Be(command.ConflictPolicy);
        dto.FieldMappingId.Should().Be(command.FieldMappingId);
        dto.ScheduleId.Should().Be(command.SyncScheduleId);
        dto.IsEnabled.Should().BeTrue();
    }

    [TestMethod]
    public async Task UpdateSyncRuleHandler_UpdatesConfiguration()
    {
        var rule = SyncRule.Create(
            tenantId: Guid.NewGuid(),
            sourceIntegrationId: Guid.NewGuid(),
            targetIntegrationId: Guid.NewGuid(),
            direction: SyncDirection.AtoB,
            scope: SyncScope.Tasks,
            filter: null,
            conflictPolicy: SyncConflictPolicy.PreferSource,
            fieldMappingId: Guid.NewGuid(),
            syncScheduleId: null);

        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var handler = new UpdateSyncRuleHandler(repo.Object);
        var cmd = new UpdateSyncRuleCommand(
            SyncRuleId: rule.Id,
            Scope: SyncScope.Comments,
            Filter: null,
            ConflictPolicy: SyncConflictPolicy.PreferTarget,
            FieldMappingId: Guid.NewGuid());

        await handler.Handle(cmd, CancellationToken.None);

        rule.Scope.Should().Be(cmd.Scope);
        rule.ConflictPolicy.Should().Be(cmd.ConflictPolicy);
        rule.FieldMappingId.Should().Be(cmd.FieldMappingId);
    }

    [TestMethod]
    public async Task UpdateSyncRuleHandler_Throws_WhenNotFound()
    {
        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRule?)null);

        var handler = new UpdateSyncRuleHandler(repo.Object);
        var cmd = new UpdateSyncRuleCommand(
            SyncRuleId: Guid.NewGuid(),
            Scope: SyncScope.Comments,
            Filter: null,
            ConflictPolicy: SyncConflictPolicy.PreferTarget,
            FieldMappingId: Guid.NewGuid());

        var act = async () => await handler.Handle(cmd, CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task EnableSyncRuleHandler_EnablesRule()
    {
        var rule = SyncRule.Create(
            tenantId: Guid.NewGuid(),
            sourceIntegrationId: Guid.NewGuid(),
            targetIntegrationId: Guid.NewGuid(),
            direction: SyncDirection.AtoB,
            scope: SyncScope.Tasks,
            filter: null,
            conflictPolicy: SyncConflictPolicy.PreferSource,
            fieldMappingId: Guid.NewGuid(),
            syncScheduleId: null,
            isEnabled: false);

        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var handler = new EnableSyncRuleHandler(repo.Object);

        await handler.Handle(new EnableSyncRuleCommand(rule.Id), CancellationToken.None);

        rule.IsEnabled.Should().BeTrue();
    }

    [TestMethod]
    public async Task EnableSyncRuleHandler_Throws_WhenNotFound()
    {
        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRule?)null);

        var handler = new EnableSyncRuleHandler(repo.Object);

        var act = async () => await handler.Handle(new EnableSyncRuleCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task DisableSyncRuleHandler_DisablesRule()
    {
        var rule = SyncRule.Create(
            tenantId: Guid.NewGuid(),
            sourceIntegrationId: Guid.NewGuid(),
            targetIntegrationId: Guid.NewGuid(),
            direction: SyncDirection.AtoB,
            scope: SyncScope.Tasks,
            filter: null,
            conflictPolicy: SyncConflictPolicy.PreferSource,
            fieldMappingId: Guid.NewGuid(),
            syncScheduleId: null,
            isEnabled: true);

        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var handler = new DisableSyncRuleHandler(repo.Object);

        await handler.Handle(new DisableSyncRuleCommand(rule.Id), CancellationToken.None);

        rule.IsEnabled.Should().BeFalse();
    }

    [TestMethod]
    public async Task DisableSyncRuleHandler_Throws_WhenNotFound()
    {
        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRule?)null);

        var handler = new DisableSyncRuleHandler(repo.Object);

        var act = async () => await handler.Handle(new DisableSyncRuleCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task AssignScheduleToSyncRuleHandler_AssignsSchedule()
    {
        var rule = SyncRule.Create(
            tenantId: Guid.NewGuid(),
            sourceIntegrationId: Guid.NewGuid(),
            targetIntegrationId: Guid.NewGuid(),
            direction: SyncDirection.AtoB,
            scope: SyncScope.Tasks,
            filter: null,
            conflictPolicy: SyncConflictPolicy.PreferSource,
            fieldMappingId: Guid.NewGuid(),
            syncScheduleId: null);

        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var handler = new AssignScheduleToSyncRuleHandler(repo.Object);
        var scheduleId = Guid.NewGuid();

        await handler.Handle(new AssignScheduleToSyncRuleCommand(rule.Id, scheduleId), CancellationToken.None);

        rule.SyncScheduleId.Should().Be(scheduleId);
    }

    [TestMethod]
    public async Task AssignScheduleToSyncRuleHandler_Throws_WhenNotFound()
    {
        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRule?)null);

        var handler = new AssignScheduleToSyncRuleHandler(repo.Object);

        var act = async () => await handler.Handle(
            new AssignScheduleToSyncRuleCommand(Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task RemoveScheduleFromSyncRuleHandler_RemovesSchedule()
    {
        var scheduleId = Guid.NewGuid();
        var rule = SyncRule.Create(
            tenantId: Guid.NewGuid(),
            sourceIntegrationId: Guid.NewGuid(),
            targetIntegrationId: Guid.NewGuid(),
            direction: SyncDirection.AtoB,
            scope: SyncScope.Tasks,
            filter: null,
            conflictPolicy: SyncConflictPolicy.PreferSource,
            fieldMappingId: Guid.NewGuid(),
            syncScheduleId: scheduleId);

        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var handler = new RemoveScheduleFromSyncRuleHandler(repo.Object);

        await handler.Handle(new RemoveScheduleFromSyncRuleCommand(rule.Id), CancellationToken.None);

        rule.SyncScheduleId.Should().BeNull();
    }

    [TestMethod]
    public async Task RemoveScheduleFromSyncRuleHandler_Throws_WhenNotFound()
    {
        var repo = new Mock<ISyncRuleRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRule?)null);

        var handler = new RemoveScheduleFromSyncRuleHandler(repo.Object);

        var act = async () => await handler.Handle(
            new RemoveScheduleFromSyncRuleCommand(Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }
}
