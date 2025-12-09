namespace Integra.Domain.Enums;

public enum UnifiedRelationType
{
    Blocks,         // A blocks B
    BlockedBy,      // A is blocked by B
    RelatesTo,      // Generic relation
    Duplicates,     // A duplicates B
    DuplicatedBy,   // A is duplicated by B
    Precedes,       // A precedes B
    Follows,        // A follows B
    Parent,         // Parent of
    Child           // Child of
}
