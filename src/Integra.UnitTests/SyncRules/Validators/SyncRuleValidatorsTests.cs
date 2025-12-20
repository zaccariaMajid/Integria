using FluentValidation.TestHelper;
using Integra.Application.SyncRules;
using Integra.Application.SyncRules.Commands.CreateSyncRule;
using Integra.Application.SyncRules.Commands.UpdateSyncRule;
using Integra.Application.SyncRules.Queries.ListByIntegration;
using Integra.Application.SyncRules.Queries.ListByTenant;
using Integra.Application.Validators;
using Integra.Domain.Enums;

namespace Integra.UnitTests.SyncRules.Validators;

[TestClass]
public class SyncRuleValidatorsTests
{
    private readonly CreateSyncRuleValidator _createValidator = new();
    private readonly UpdateSyncRuleValidator _updateValidator = new();
    private readonly ListSyncRulesByTenantValidator _listValidator = new();
    private readonly ListSyncRulesByIntegrationValidator _listByIntegrationValidator = new();

    [TestMethod]
    public void CreateSyncRuleValidator_Fails_When_FieldMappingIdMissing()
    {
        var command = new CreateSyncRuleCommand(
            TenantId: Guid.NewGuid(),
            SourceIntegrationId: Guid.NewGuid(),
            TargetIntegrationId: Guid.NewGuid(),
            Direction: SyncDirection.Bidirectional,
            Scope: SyncScope.Tasks,
            Filter: null,
            ConflictPolicy: SyncConflictPolicy.PreferSource,
            FieldMappingId: Guid.Empty,
            SyncScheduleId: null);

        var result = _createValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FieldMappingId);
    }

    [TestMethod]
    public void UpdateSyncRuleValidator_Fails_When_FieldMappingIdMissing()
    {
        var command = new UpdateSyncRuleCommand(
            SyncRuleId: Guid.NewGuid(),
            Scope: SyncScope.Tasks,
            Filter: null,
            ConflictPolicy: SyncConflictPolicy.PreferSource,
            FieldMappingId: Guid.Empty);

        var result = _updateValidator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.FieldMappingId);
    }

    [TestMethod]
    public void ListSyncRulesByTenantValidator_Fails_When_TenantMissing()
    {
        var query = new ListSyncRulesByTenantQuery(Guid.Empty);

        var result = _listValidator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.TenantId);
    }

    [TestMethod]
    public void ListSyncRulesByTenantValidator_Passes_With_Tenant()
    {
        var query = new ListSyncRulesByTenantQuery(Guid.NewGuid());

        var result = _listValidator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [TestMethod]
    public void ListSyncRulesByIntegrationValidator_Fails_When_IntegrationMissing()
    {
        var query = new ListSyncRulesByIntegrationQuery(Guid.Empty);

        var result = _listByIntegrationValidator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.IntegrationId);
    }

    [TestMethod]
    public void ListSyncRulesByIntegrationValidator_Passes_With_Integration()
    {
        var query = new ListSyncRulesByIntegrationQuery(Guid.NewGuid());

        var result = _listByIntegrationValidator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
