using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integra.Domain.Common;
using Integra.Domain.Enums;
using Integra.Domain.Events;
using Integra.Domain.ValueObjects;

namespace Integra.Domain.AggregateRoots;

/// <summary>
/// contains the unified status of the task
/// allows you to understand if it has changed
/// ensures the consistency of its internal parts (comments, subtasks, etc.)
/// provides a single point from which to perform conversions and mapping
/// </summary>
public class UnifiedTask : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }

    // CORE FIELDS
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public UnifiedTaskStatus Status { get; private set; }
    public UnifiedTaskPriority Priority { get; private set; }

    public UnifiedUser AssignedUser { get; private set; }
    public UnifiedUser ReporterUser { get; private set; }

    // DATES
    public DateTime TaskCreationDate { get; private set; }
    public DateTime TaskUpdateDate { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? CompletionDate { get; private set; }

    // COLLECTIONS
    private List<UnifiedSubtask> _subtasks = new();
    public IReadOnlyList<UnifiedSubtask> Subtasks => _subtasks.AsReadOnly();

    private List<UnifiedComment> _comments = new();
    public IReadOnlyList<UnifiedComment> Comments => _comments.AsReadOnly();

    private List<UnifiedAttachment> _attachments = new();
    public IReadOnlyList<UnifiedAttachment> Attachments => _attachments.AsReadOnly();

    private List<UnifiedLabel> _labels = new();
    public IReadOnlyList<UnifiedLabel> Labels => _labels.AsReadOnly();

    private List<UnifiedCustomField> _customFields = new();
    public IReadOnlyList<UnifiedCustomField> CustomFields => _customFields.AsReadOnly();

    // private List<UnifiedRelations> _relations = new();
    // public IReadOnlyList<UnifiedRelations> Relations => _relations.AsReadOnly();

    private List<ExternalMapping> _externalMappings = new();
    public IReadOnlyList<ExternalMapping> ExternalMappings => _externalMappings.AsReadOnly();

    private UnifiedTask() { }

    private UnifiedTask(
        Guid tenantId,
        Guid projectId,
        string title,
        string description,
        UnifiedTaskStatus status,
        UnifiedTaskPriority priority,
        UnifiedUser assignedUser,
        UnifiedUser reporterUser,
        DateTime taskCreationDate,
        DateTime taskUpdateDate,
        DateTime? dueDate,
        DateTime? startDate,
        DateTime? completionDate)
    {
        if(tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID cannot be empty GUID", nameof(tenantId));
        if(projectId == Guid.Empty)
            throw new ArgumentException("Project ID cannot be empty GUID", nameof(projectId));
        if(title is null)
            throw new ArgumentException("Title cannot be null", nameof(title));
        if(description is null)
            throw new ArgumentException("Description cannot be null", nameof(description));
        if(assignedUser is null)
            throw new ArgumentException("Assigned user cannot be null", nameof(assignedUser));
        if(reporterUser is null)
            throw new ArgumentException("Reporter user cannot be null", nameof(reporterUser));
        if(taskCreationDate == default)
            throw new ArgumentException("Task creation date cannot be default", nameof(taskCreationDate));
        if(taskCreationDate > DateTime.UtcNow)
            throw new ArgumentException("Task creation date cannot be in the future", nameof(taskCreationDate));
        if(taskUpdateDate == default)
            throw new ArgumentException("Task update date cannot be default", nameof(taskUpdateDate));
        if(taskUpdateDate < taskCreationDate)
            throw new ArgumentException("Task update date cannot be before creation date", nameof(taskUpdateDate));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        ProjectId = projectId;
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        AssignedUser = assignedUser;
        ReporterUser = reporterUser;
        TaskCreationDate = taskCreationDate;
        TaskUpdateDate = taskUpdateDate;
        DueDate = dueDate;
        StartDate = startDate;
        CompletionDate = completionDate;

        AddDomainEvent(new UnifiedTaskCreated(Id, projectId, assignedUser.Id, title));
    }

    public static UnifiedTask Create(
        Guid tenantId,
        Guid projectId,
        string title,
        string description,
        UnifiedTaskStatus status,
        UnifiedTaskPriority priority,
        UnifiedUser assignedUser,
        UnifiedUser reporterUser,
        DateTime taskCreationDate,
        DateTime taskUpdateDate,
        DateTime? dueDate = null,
        DateTime? startDate = null,
        DateTime? completionDate = null)
        => new UnifiedTask(
            tenantId,
            projectId,
            title,
            description,
            status,
            priority,
            assignedUser,
            reporterUser,
            taskCreationDate,
            taskUpdateDate,
            dueDate,
            startDate,
            completionDate);
}
