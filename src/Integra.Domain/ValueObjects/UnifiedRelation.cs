using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public class UnifiedRelation : ValueObject
{
    public Guid TargetTaskId { get; private set; }
    public UnifiedRelationType RelationType { get; private set; }
    public List<ExternalMapping> _externalMappings = new();
    public IReadOnlyList<ExternalMapping> ExternalMappings => _externalMappings.AsReadOnly();

    private UnifiedRelation(Guid targetTaskId, UnifiedRelationType relationType, List<ExternalMapping>? externalMappings = null)
    {
        if(targetTaskId == Guid.Empty)
            throw new DomainException("TargetTaskId cannot be empty.", nameof(targetTaskId));

        TargetTaskId = targetTaskId;
        RelationType = relationType;
        if (externalMappings is not null)
            _externalMappings = externalMappings;
    }

    public static UnifiedRelation Create(Guid targetTaskId, UnifiedRelationType relationType, List<ExternalMapping>? externalMappings = null)
        => new UnifiedRelation(targetTaskId,  relationType, externalMappings);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TargetTaskId;
        yield return RelationType;
        foreach (var mapping in _externalMappings)
            yield return mapping;
    }
}
