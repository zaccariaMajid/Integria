using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;
using Integra.Domain.Exceptions;
using Integra.Domain.ValueObjects;

namespace Integra.Domain.AggregateRoots;

/// <summary>
/// Have a single point for thinking about a project, even if it exists in multiple systems.
/// Perform project→project migrations (e.g., Jira Project A → Notion DB X).
/// Know which projects are linked to each other in the various tools.
/// Define the scope of syncs (e.g., “only sync this project”).
/// </summary>
public class UnifiedProject : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Guid OwnerId { get; private set; }
    public UnifiedVisibilityType Visibility { get; private set; }

    private readonly List<UnifiedLabel> _labels = new();
    public IReadOnlyCollection<UnifiedLabel> Labels => _labels.AsReadOnly();

    private readonly List<UnifiedCustomField> _customFields = new();
    public IReadOnlyCollection<UnifiedCustomField> CustomFields => _customFields.AsReadOnly();

    private UnifiedProject() { }

    private UnifiedProject(Guid tenantId, string name, string description, Guid ownerId, UnifiedVisibilityType visibility)
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("Tenant ID cannot be empty GUID", nameof(tenantId));
        if (name is null)
            throw new DomainException(nameof(name), "Project name cannot be null");
        if (description is null)
            throw new DomainException(nameof(description), "Project description cannot be null");
        if (ownerId == Guid.Empty)
            throw new DomainException("Owner ID cannot be empty GUID", nameof(ownerId));

        TenantId = tenantId;
        Name = name;
        Description = description;
        OwnerId = ownerId;
        Visibility = visibility;

        AddDomainEvent(new UnifiedProjectCreated(this.Id, this.TenantId));
    }

    public static UnifiedProject Create(Guid tenantId, string name, string description, Guid ownerId, UnifiedVisibilityType visibility)
        => new UnifiedProject(tenantId, name, description, ownerId, visibility);

    public void AddLabel(UnifiedLabel label)
    {
        if (label is null)
            throw new DomainException(nameof(label), "Label cannot be null");
        _labels.Add(label);
        Touch();
        AddDomainEvent(new UnifiedProjectLabelAdded(Id, label.Name));
    }

    public void AddCustomField(UnifiedCustomField customField)
    {
        if (customField is null)
            throw new DomainException(nameof(customField), "Custom field cannot be null");
        _customFields.Add(customField);
        Touch();
        AddDomainEvent(new UnifiedProjectCustomFieldAdded(Id, customField.Name));
    }
}
