using FluentAssertions;
using Integra.Application.Exceptions;
using Integra.Application.Interfaces.Repositories.SyncRules;
using Integra.Application.SyncRules.Queries.GetById;
using Moq;

namespace Integra.UnitTests.SyncRules;

[TestClass]
public class GetSyncRuleByIdHandlerTests
{
    private readonly Mock<ISyncRuleReadRepository> _readRepository = new();
    private readonly GetSyncRuleByIdHandler _handler;

    public GetSyncRuleByIdHandlerTests()
    {
        _handler = new GetSyncRuleByIdHandler(_readRepository.Object);
    }

    [TestMethod]
    public async Task Handle_ReturnsDto_WhenFound()
    {
        var id = Guid.NewGuid();
        var dto = new SyncRuleDetailsDto(
            id,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Domain.Enums.SyncDirection.Bidirectional,
            Domain.Enums.SyncScope.Tasks,
            Domain.Enums.SyncConflictPolicy.PreferSource,
            Guid.NewGuid(),
            Guid.NewGuid(),
            true,
            DateTime.UtcNow,
            DateTime.UtcNow);

        _readRepository
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var result = await _handler.Handle(new GetSyncRuleByIdQuery(id), CancellationToken.None);

        result.Should().Be(dto);
    }

    [TestMethod]
    public async Task Handle_Throws_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _readRepository
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SyncRuleDetailsDto?)null);

        var act = async () => await _handler.Handle(new GetSyncRuleByIdQuery(id), CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationNotFoundException>();
    }
}
