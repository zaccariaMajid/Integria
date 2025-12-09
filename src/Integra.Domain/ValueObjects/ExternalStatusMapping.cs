using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;

public class ExternalStatusMapping : ValueObject
{
    public Guid IntegrationId { get; private set; }
    public string ExternalName { get; private set; } = null!; // es: Jira category / Redmine ID
    public string ExternalType { get; private set; } = null!; // es: {"id": 5}, {"category": "To Do"}

    private ExternalStatusMapping() { }

    private ExternalStatusMapping(Guid integrationId,  string externalName, string externalType)
    {
        IntegrationId = integrationId;
        ExternalName = externalName;
        ExternalType = externalType;
    }

    public static ExternalStatusMapping Create(Guid integrationId, string externalName, string externalType)
        => new ExternalStatusMapping(integrationId, externalName, externalType);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IntegrationId;
        yield return ExternalName;
        yield return ExternalType;
    }
}
