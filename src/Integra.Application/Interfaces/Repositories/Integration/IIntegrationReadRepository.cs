using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Integrations.Queries;

namespace Integra.Application.Interfaces.Repositories.Integration;

public interface IIntegrationReadRepository
{
    Task<IntegrationConfigDto?> GetByIdAsync(Guid integrationId, CancellationToken ct);
}
