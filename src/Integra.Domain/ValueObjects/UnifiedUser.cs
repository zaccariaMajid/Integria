using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.AggregateRoots;
using Integra.Domain.Common;
using Integra.Domain.Exceptions;

namespace Integra.Domain.ValueObjects;

public class UnifiedUser : ValueObject
{
    public string DisplayName { get; private set; } = null!;
    public string? Email { get; private set; }
    public string? AvatarUrl { get; private set; }
    public List<ExternalMapping> _externalMappings = new();
    public IReadOnlyCollection<ExternalMapping> ExternalMappings 
        => _externalMappings.AsReadOnly();

    private UnifiedUser() { }
    private UnifiedUser(string displayName, string? email, string? avatarUrl, List<ExternalMapping> externalMappings)
    {
        if(string.IsNullOrWhiteSpace(displayName))
            throw new DomainException("Display name cannot be null or empty.", nameof(displayName));
        DisplayName = displayName;
        Email = email;
        AvatarUrl = avatarUrl;
        _externalMappings = externalMappings;
    }

    public static UnifiedUser Create(string displayName, string? email, string? avatarUrl, List<ExternalMapping> externalMappings) 
        => new UnifiedUser(displayName, email, avatarUrl, externalMappings);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return DisplayName;
        yield return Email ?? string.Empty;
        yield return AvatarUrl ?? string.Empty;
        foreach(var mapping in _externalMappings)
        {
            yield return mapping;
        }
    }
}
