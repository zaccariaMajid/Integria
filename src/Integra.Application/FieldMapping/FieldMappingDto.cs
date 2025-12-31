using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.FieldMapping;

public sealed record FieldMappingDto
(
    Guid Id,
    Guid TenantId,
    IReadOnlyList<FieldMappingRuleDto> Rules
);