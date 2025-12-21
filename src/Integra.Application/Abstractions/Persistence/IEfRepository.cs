using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.Abstractions.Persistence;

public interface IEfRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(T Entity, CancellationToken ct = default);
    Task UpdateAsync(T Entity, CancellationToken ct = default);
    Task DeleteAsync(T Entity, CancellationToken ct = default);
}
