using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;

public sealed class ExternalUserMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }
    public string ExternalUserId { get; private set; } = null!;
    public string ExternalUserName { get; private set; } = null!;

    private ExternalUserMapping() { }

    public ExternalUserMapping(Guid integrationId, string externalUserId, string externalUserName)
    {
        if (string.IsNullOrWhiteSpace(externalUserId))
            throw new ArgumentException("External user ID cannot be null or empty.", nameof(externalUserId));
        if (string.IsNullOrWhiteSpace(externalUserName))
            throw new ArgumentException("External user name cannot be null or empty.", nameof(externalUserName));
        IntegrationId = integrationId;
        ExternalUserId = externalUserId;
        ExternalUserName = externalUserName;
    }

    public static ExternalUserMapping Create(Guid integrationId, string externalUserId, string externalUserName)
        => new ExternalUserMapping(integrationId, externalUserId, externalUserName);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalUserId;
        yield return ExternalUserName;
    }
}
