using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integra.Application.FieldMapping;

public sealed record FieldMappingRuleDto
(
    string UnifiedField,        // es: "status"
    string SourceField,         // es: "jira.status.id"
    string TargetField,         // es: "notion.select"
    string? TransformExpression // opzionale (string, enum, jsonata, ecc.)
);