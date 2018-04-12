using System.ComponentModel;

namespace TaskManager.Logic.Contracts.Enums {
    public enum AlarmRepeatEnum {
        [Description("Days")]
        Days,
        [Description("Weeks")]
        Weeks,
        [Description("Months")]
        Months,
        [Description("Years")]
        Years,
    }
}
