using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.Interfaces.Providers;

public sealed record ExternalTaskDto
(
    string ExternalId,
    string ExternalKey,
    IReadOnlyDictionary<string, object?> Fields,
    DateTime UpdatedAt
);

