using System.ComponentModel;

namespace TaskManager.Logic.Enums {
    public enum TaskStatusEnum {
        [Description("Not Started")]
        NotStarted,
        [Description("In Progress")]
        InProgress,
        [Description("Done")]
        Done,
        [Description("Rejected")]
        Rejected,
        [Description("Failed")]
        Failed,
    }
}
