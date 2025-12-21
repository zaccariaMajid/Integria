using FluentAssertions;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Application.SyncJobs.Commands.CancelSyncJob;
using Integra.Application.SyncJobs.Commands.RetrySyncJob;
using Integra.Application.SyncJobs.Commands.TriggerSyncJob;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Enums;
using Moq;

namespace Integra.UnitTests.SyncJobs;

[TestClass]
public class CommandHandlersTests
{
    [TestMethod]
    public async Task TriggerSyncJob_CreatesJob_WhenRuleEnabled()
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

        var ruleRepo = new Mock<ISyncRuleRepository>();
        ruleRepo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var jobRepo = new Mock<ISyncJobRepository>();

        var handler = new TriggerSyncJobHandler(ruleRepo.Object, jobRepo.Object);
        var cmd = new TriggerSyncJobCommand(Guid.NewGuid(), rule.Id, SyncJobType.FullSync);

        var jobId = await handler.Handle(cmd, CancellationToken.None);

        jobRepo.Verify(r => r.AddAsync(It.Is<SyncJob>(j =>
            j.SyncRuleId == rule.Id &&
            j.TenantId == rule.TenantId &&
            j.JobType == cmd.JobType), It.IsAny<CancellationToken>()), Times.Once);

        jobId.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task TriggerSyncJob_Throws_WhenRuleMissing()
    {
        var ruleRepo = new Mock<ISyncRuleRepository>();
        ruleRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRule?)null);

        var jobRepo = new Mock<ISyncJobRepository>();
        var handler = new TriggerSyncJobHandler(ruleRepo.Object, jobRepo.Object);

        var act = async () => await handler.Handle(
            new TriggerSyncJobCommand(Guid.NewGuid(), Guid.NewGuid(), SyncJobType.FullSync),
            CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task TriggerSyncJob_Throws_WhenRuleDisabled()
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

        var ruleRepo = new Mock<ISyncRuleRepository>();
        ruleRepo.Setup(r => r.GetByIdAsync(rule.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rule);

        var jobRepo = new Mock<ISyncJobRepository>();
        var handler = new TriggerSyncJobHandler(ruleRepo.Object, jobRepo.Object);

        var act = async () => await handler.Handle(
            new TriggerSyncJobCommand(Guid.NewGuid(),rule.Id, SyncJobType.FullSync),
            CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationDomainException>();
    }

    [TestMethod]
    public async Task RetrySyncJob_CreatesRetry_WhenFailed()
    {
        var failedJob = SyncJob.Create(Guid.NewGuid(), Guid.NewGuid(), SyncJobType.FullSync);
        failedJob.Fail("fail");

        var jobRepo = new Mock<ISyncJobRepository>();
        jobRepo.Setup(r => r.GetByIdAsync(failedJob.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(failedJob);

        var handler = new RetrySyncJobHandler(jobRepo.Object);

        var newJobId = await handler.Handle(new RetrySyncJobCommand(failedJob.TenantId, failedJob.Id), CancellationToken.None);

        jobRepo.Verify(r => r.AddAsync(It.Is<SyncJob>(j =>
            j.SyncRuleId == failedJob.SyncRuleId &&
            j.TenantId == failedJob.TenantId &&
            j.JobType == SyncJobType.Retry), It.IsAny<CancellationToken>()), Times.Once);

        newJobId.Should().NotBeEmpty();
    }

    [TestMethod]
    public async Task RetrySyncJob_Throws_WhenNotFailed()
    {
        var job = SyncJob.Create(Guid.NewGuid(), Guid.NewGuid(), SyncJobType.FullSync);

        var jobRepo = new Mock<ISyncJobRepository>();
        jobRepo.Setup(r => r.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(job);

        var handler = new RetrySyncJobHandler(jobRepo.Object);

        var act = async () => await handler.Handle(new RetrySyncJobCommand(job.TenantId, job.Id), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationDomainException>();
    }

    [TestMethod]
    public async Task RetrySyncJob_Throws_WhenMissing()
    {
        var jobRepo = new Mock<ISyncJobRepository>();
        jobRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncJob?)null);

        var handler = new RetrySyncJobHandler(jobRepo.Object);

        var act = async () => await handler.Handle(new RetrySyncJobCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task CancelSyncJob_Deletes_WhenTenantMatches()
    {
        var job = SyncJob.Create(Guid.NewGuid(), Guid.NewGuid(), SyncJobType.FullSync);

        var jobRepo = new Mock<ISyncJobRepository>();
        jobRepo.Setup(r => r.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(job);

        var handler = new CancelSyncJobHandler(jobRepo.Object);

        await handler.Handle(new CancelSyncJobCommand(job.TenantId, job.Id), CancellationToken.None);
    }

    [TestMethod]
    public async Task CancelSyncJob_Throws_WhenNotFound()
    {
        var jobRepo = new Mock<ISyncJobRepository>();
        jobRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncJob?)null);

        var handler = new CancelSyncJobHandler(jobRepo.Object);

        var act = async () => await handler.Handle(
            new CancelSyncJobCommand(Guid.NewGuid(), Guid.NewGuid()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task CancelSyncJob_Throws_WhenTenantMismatch()
    {
        var job = SyncJob.Create(Guid.NewGuid(), Guid.NewGuid(), SyncJobType.FullSync);

        var jobRepo = new Mock<ISyncJobRepository>();
        jobRepo.Setup(r => r.GetByIdAsync(job.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncJob?)null);

        var handler = new CancelSyncJobHandler(jobRepo.Object);

        var act = async () => await handler.Handle(
            new CancelSyncJobCommand(Guid.NewGuid(), job.Id),
            CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }
}
