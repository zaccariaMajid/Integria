using FluentAssertions;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Application.SyncRules.Queries;
using Integra.Application.SyncRules.Queries.ListByIntegration;
using Moq;

namespace Integra.UnitTests.SyncRules;

[TestClass]
public class ListSyncRulesByIntegrationHandlerTests
{
    [TestMethod]
    public async Task Handle_ReturnsList_FromRepository()
    {
        var integrationId = Guid.NewGuid();
        var list = new List<SyncRuleListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncDirection.AtoB, Domain.Enums.SyncScope.Tasks, true, DateTime.UtcNow),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncDirection.BtoA, Domain.Enums.SyncScope.Comments, false, DateTime.UtcNow)
        };

        var readRepository = new Mock<ISyncRuleReadRepository>();
        readRepository
            .Setup(r => r.ListByIntegrationAsync(integrationId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var handler = new ListSyncRulesByIntegrationHandler(readRepository.Object);

        var result = await handler.Handle(new ListSyncRulesByIntegrationQuery(integrationId), CancellationToken.None);

        result.Should().BeEquivalentTo(list);
    }
}
