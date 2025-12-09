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
/// contains the unified status of the task
/// allows you to understand if it has changed
/// ensures the consistency of its internal parts (comments, subtasks, etc.)
/// provides a single point from which to perform conversions and mapping
/// </summary>
public sealed class UnifiedTask : AggregateRoot<Guid>
{
    public Guid TenantId { get; private set; }
    public Guid ProjectId { get; private set; }

    // CORE FIELDS
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public UnifiedTaskStatus Status { get; private set; } = null!;
    public UnifiedPriority Priority { get; private set; } = null!;

    public UnifiedUser AssignedUser { get; private set; } = null!;
    public UnifiedUser ReporterUser { get; private set; } = null!;

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

    private List<UnifiedRelation> _relations = new();
    public IReadOnlyList<UnifiedRelation> Relations => _relations.AsReadOnly();

    private List<ExternalMapping> _externalMappings = new();
    public IReadOnlyList<ExternalMapping> ExternalMappings => _externalMappings.AsReadOnly();

    private UnifiedTask() { }

    private UnifiedTask(
        Guid tenantId,
        Guid projectId,
        string title,
        string description,
        UnifiedTaskStatus status,
        UnifiedPriority priority,
        UnifiedUser assignedUser,
        UnifiedUser reporterUser,
        DateTime taskCreationDate,
        DateTime taskUpdateDate,
        DateTime? dueDate,
        DateTime? startDate,
        DateTime? completionDate)
    {
        if (tenantId == Guid.Empty)
            throw new DomainException("Tenant ID cannot be empty GUID", nameof(tenantId));
        if (projectId == Guid.Empty)
            throw new DomainException("Project ID cannot be empty GUID", nameof(projectId));
        if (title is null)
            throw new DomainException("Title cannot be null", nameof(title));
        if (description is null)
            throw new DomainException("Description cannot be null", nameof(description));
        if (assignedUser is null)
            throw new DomainException("Assigned user cannot be null", nameof(assignedUser));
        if (reporterUser is null)
            throw new DomainException("Reporter user cannot be null", nameof(reporterUser));
        if (taskCreationDate == default)
            throw new DomainException("Task creation date cannot be default", nameof(taskCreationDate));
        if (taskCreationDate > DateTime.UtcNow)
            throw new DomainException("Task creation date cannot be in the future", nameof(taskCreationDate));
        if (taskUpdateDate == default)
            throw new DomainException("Task update date cannot be default", nameof(taskUpdateDate));
        if (taskUpdateDate < taskCreationDate)
            throw new DomainException("Task update date cannot be before creation date", nameof(taskUpdateDate));

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

        AddDomainEvent(new UnifiedTaskCreated(Id, projectId, assignedUser.DisplayName, title));
    }

    public static UnifiedTask Create(
        Guid tenantId,
        Guid projectId,
        string title,
        string description,
        UnifiedTaskStatus status,
        UnifiedPriority priority,
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

    public void AddSubtask(UnifiedSubtask subtask)
    {
        if (subtask is null)
            throw new DomainException("Subtask cannot be null", nameof(subtask));

        _subtasks.Add(subtask);
    }

    public void AddLabel(UnifiedLabel label)
    {
        if (label is null)
            throw new DomainException("Label cannot be null", nameof(label));

        _labels.Add(label);
    }

    public void AddCustomField(UnifiedCustomField customField)
    {
        if (customField is null)
            throw new DomainException("Custom field cannot be null", nameof(customField));

        _customFields.Add(customField);
    }
    public void AddComment(UnifiedComment comment)
    {
        if (comment is null)
            throw new DomainException("Comment cannot be null", nameof(comment));

        _comments.Add(comment);
    }
    public void AddRelation(UnifiedRelation relation)
    {
        if (relation is null)
            throw new DomainException("Relation cannot be null", nameof(relation));

        _relations.Add(relation);
    }
    public void AddAttachment(UnifiedAttachment attachment)
    {
        if (attachment is null)
            throw new DomainException("Attachment cannot be null", nameof(attachment));

        _attachments.Add(attachment);
    }
    public void AddExternalMapping(ExternalMapping mapping)
    {
        if (mapping is null)
            throw new DomainException("External mapping cannot be null", nameof(mapping));

        _externalMappings.Add(mapping);
    }

}
