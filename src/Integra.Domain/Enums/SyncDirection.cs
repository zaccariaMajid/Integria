using System.ComponentModel;

namespace Integra.Domain.Enums;

public enum SyncDirection
{
    [Description("A to B")]
    AtoB = 1,
    [Description("B to A")]
    BtoA = 2,
    [Description("Bidirectional")]
    Bidirectional = 3
}
