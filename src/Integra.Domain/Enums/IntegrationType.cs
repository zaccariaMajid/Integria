using System.ComponentModel;

namespace Integra.Domain.Enums;

public enum IntegrationType
{
    [Description("Redmine")]
    Redmine = 1,
    [Description("Jira")]
    Jira = 2,
    [Description("ClickUp")]
    ClickUp = 3,
    [Description("Trello")]
    Trello = 4,
    [Description("Linear")]
    Linear = 5
}
