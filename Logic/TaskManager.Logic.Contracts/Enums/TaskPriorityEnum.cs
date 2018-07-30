using System.ComponentModel;

namespace TaskManager.Logic.Enums {
    public enum TaskPriorityEnum {
        [Description("Very Low")]
        VeryLow,
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
