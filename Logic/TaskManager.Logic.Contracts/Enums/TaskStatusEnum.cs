using System.ComponentModel;

namespace TaskManager.Logic.Contracts.Enums {
    public enum TaskStatusEnum {
        [Description("Created")]
        Created,
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
