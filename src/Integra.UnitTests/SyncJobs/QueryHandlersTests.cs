using FluentAssertions;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncJobs;
using Integra.Application.SyncJobs.Queries.GetById;
using Integra.Application.SyncJobs.Queries.ListRecentSyncJobs;
using Integra.Application.SyncJobs.Queries.ListSyncJobsByRule;
using Integra.Application.SyncJobs.Queries;
using Moq;

namespace Integra.UnitTests.SyncJobs;

[TestClass]
public class QueryHandlersTests
{
    [TestMethod]
    public async Task GetSyncJobById_ReturnsDto_WhenFound()
    {
        var id = Guid.NewGuid();
        var dto = new SyncJobDetailsDto(
            id,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Domain.Enums.SyncJobType.FullSync,
            Domain.Enums.SyncJobStatus.Running,
            10,
            null,
            DateTime.UtcNow,
            null,
            DateTime.UtcNow);

        var repo = new Mock<ISyncJobReadRepository>();
        repo.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var handler = new GetSyncJobByIdHandler(repo.Object);

        var result = await handler.Handle(new GetSyncJobByIdQuery(id), CancellationToken.None);

        result.Should().Be(dto);
    }

    [TestMethod]
    public async Task GetSyncJobById_Throws_WhenMissing()
    {
        var repo = new Mock<ISyncJobReadRepository>();
        repo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncJobDetailsDto?)null);

        var handler = new GetSyncJobByIdHandler(repo.Object);

        var act = async () => await handler.Handle(new GetSyncJobByIdQuery(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }

    [TestMethod]
    public async Task ListRecentSyncJobs_Returns_List()
    {
        var tenantId = Guid.NewGuid();
        var list = new List<SyncJobListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncJobType.FullSync, Domain.Enums.SyncJobStatus.Running, 50, DateTime.UtcNow, null),
            new(Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncJobType.Retry, Domain.Enums.SyncJobStatus.Pending, 0, DateTime.UtcNow, DateTime.UtcNow)
        };

        var repo = new Mock<ISyncJobReadRepository>();
        repo.Setup(r => r.ListRecentAsync(tenantId, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var handler = new ListRecentSyncJobsHandler(repo.Object);

        var result = await handler.Handle(new ListRecentSyncJobsQuery(tenantId, 10), CancellationToken.None);

        result.Should().BeEquivalentTo(list);
    }

    [TestMethod]
    public async Task ListSyncJobsByRule_Returns_List()
    {
        var ruleId = Guid.NewGuid();
        var list = new List<SyncJobListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncJobType.FullSync, Domain.Enums.SyncJobStatus.Running, 10, DateTime.UtcNow, DateTime.UtcNow),
            new(Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncJobType.Retry, Domain.Enums.SyncJobStatus.Failed, 100, DateTime.UtcNow, DateTime.UtcNow)
        };

        var repo = new Mock<ISyncJobReadRepository>();
        repo.Setup(r => r.ListBySyncRuleAsync(ruleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var handler = new ListSyncJobsByRuleHandler(repo.Object);

        var result = await handler.Handle(new ListSyncJobsByRuleQuery(ruleId), CancellationToken.None);

        result.Should().BeEquivalentTo(list);
    }
}
