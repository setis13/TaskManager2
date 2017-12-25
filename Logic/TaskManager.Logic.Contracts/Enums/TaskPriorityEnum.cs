using System.ComponentModel;

namespace TaskManager.Logic.Contracts.Enums {
    public enum TaskPriorityEnum {
        [Description("Low")]
        Low,
        [Description("Medium")]
        Medium,
        [Description("High")]
        High,
        [Description("Critical")]
        Critical,
    }
}
