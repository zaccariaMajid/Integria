using Integra.Application.Abstractions.Persistence;
using Integra.Domain.AggregateRoots;

namespace Integra.Application.Interfaces.Repositories.SyncRules;

public interface ISyncRuleRepository : IEfRepository<SyncRule>
{
    
}
