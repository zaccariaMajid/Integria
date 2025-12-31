using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.Integrations.Queries;

public sealed record IntegrationConfigDto
(
    Guid Id,
    Guid TenantId,
    string ProviderType,        // "jira", "notion", "redmine"
    string DisplayName,
    string AuthType,            // "oauth", "token", "basic"
    string CredentialsJson,     // encoded or decoded credentials
    bool IsEnabled
);
