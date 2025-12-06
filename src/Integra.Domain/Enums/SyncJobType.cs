using System.ComponentModel;

namespace Integra.Domain.Enums;

public enum SyncJobType
{
    [Description("Full Sync")]
    FullSync = 1,
    [Description("Incremental Sync")]
    IncrementalSync = 2,
    [Description("Conflict Resolution")]
    ConflictResolution = 3
}
