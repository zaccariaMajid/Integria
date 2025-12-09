using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Integra.Domain.Common;

namespace Integra.Domain.ValueObjects;

public class ProjectSyncSettings : ValueObject
{
    private List<Guid> _includedIntegrationIds = new();
    public IReadOnlyCollection<Guid> IncludedIntegrationIds => _includedIntegrationIds;
    public bool EnableBidirectionalSync { get; private set; }
    private List<string> _includedProjectKeys = new();
    public IReadOnlyCollection<string> IncludedProjectKeys => _includedProjectKeys;
    private List<string> _excludedProjectKeys = new();
    public IReadOnlyCollection<string> ExcludedProjectKeys => _excludedProjectKeys;

    private ProjectSyncSettings() { }

    public ProjectSyncSettings(
        bool enableBidirectionalSync = false)
    {
        EnableBidirectionalSync = enableBidirectionalSync;
    }
    public void AddIncludedIntegrationId(Guid integrationId)
    {
        if (integrationId == Guid.Empty)
            throw new ArgumentException("Integration ID cannot be empty GUID", nameof(integrationId));
        if (!_includedIntegrationIds.Contains(integrationId))
            _includedIntegrationIds.Add(integrationId);
    }
    public void AddIncludedProjectKey(string projectKey)
    {
        if (string.IsNullOrWhiteSpace(projectKey))
            throw new ArgumentException("Project key cannot be null or whitespace", nameof(projectKey));
        if (!_includedProjectKeys.Contains(projectKey))
            _includedProjectKeys.Add(projectKey);
    }
    public void AddExcludedProjectKey(string projectKey)
    {
        if (string.IsNullOrWhiteSpace(projectKey))
            throw new ArgumentException("Project key cannot be null or whitespace", nameof(projectKey));
        if (!_excludedProjectKeys.Contains(projectKey))
            _excludedProjectKeys.Add(projectKey);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return EnableBidirectionalSync;
        foreach (var id in _includedIntegrationIds.OrderBy(id => id))
            yield return id;
        foreach (var key in _includedProjectKeys.OrderBy(key => key))
            yield return key;
        foreach (var key in _excludedProjectKeys.OrderBy(key => key))
            yield return key;
    }
}
