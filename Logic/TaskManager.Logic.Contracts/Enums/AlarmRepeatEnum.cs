using System.ComponentModel;

namespace TaskManager.Logic.Enums {
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
