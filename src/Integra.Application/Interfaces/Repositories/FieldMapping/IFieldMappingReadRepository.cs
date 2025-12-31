using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Application.FieldMapping;

namespace Integra.Application.Interfaces.Repositories.FieldMapping;

public interface IFieldMappingReadRepository
{
    Task<FieldMappingDto?> GetByIdAsync(Guid fieldMappingId, CancellationToken ct);
}

