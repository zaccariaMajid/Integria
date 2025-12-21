using System.ComponentModel;

namespace Integra.Domain.Enums;

public enum SyncJobStatus
{
    [Description("Pending")]
    Pending = 1,
    [Description("Running")]
    Running = 2,
    [Description("Failed")]
    Failed = 3,
    [Description("Success")]
    Success = 4,
    [Description("Retrying")]
    Retrying = 5,
    [Description("Cancelled")]
    Cancelled = 6
}
