using FluentAssertions;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Application.SyncRules.Queries;
using Integra.Application.SyncRules.Queries.ListByTenant;
using Moq;

namespace Integra.UnitTests.SyncRules;

[TestClass]
public class ListSyncRulesByTenantHandlerTests
{
    private readonly Mock<ISyncRuleReadRepository> _readRepository = new();
    private readonly ListSyncRulesByTenantHandler _handler;

    public ListSyncRulesByTenantHandlerTests()
    {
        _handler = new ListSyncRulesByTenantHandler(_readRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ReturnsList_FromRepository()
    {
        var tenantId = Guid.NewGuid();
        var list = new List<SyncRuleListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncDirection.AtoB, Domain.Enums.SyncScope.Tasks, true, DateTime.UtcNow),
            new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Domain.Enums.SyncDirection.BtoA, Domain.Enums.SyncScope.Comments, false, DateTime.UtcNow)
        };

        _readRepository
            .Setup(r => r.ListByTenantAsync(tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(list);

        var result = await _handler.Handle(new ListSyncRulesByTenantQuery(tenantId), CancellationToken.None);

        result.Should().BeEquivalentTo(list);
    }
}
