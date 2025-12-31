using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.Integrations.Queries;

namespace Integra.Application.Interfaces.Providers;

public interface IProviderConnectorFactory
{
    IProviderConnector Create(IntegrationConfigDto integration);
}
