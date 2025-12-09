namespace Integra.Domain.Enums;

public enum SyncScope
{
    ProjectMetadata,    // name, description, visibility
    Labels,             // project and task labels
    CustomFields,       // definitions + values
    Tasks,              // core fields: title, desc, status, priority
    Comments,
    Attachments,
    SubTasks,
    Relations,
    All                 // include everything
}
