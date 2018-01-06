var Enums;
(function (Enums) {
    (function (TaskStatusEnum) {
        //[Description("Not Started")]
        TaskStatusEnum[TaskStatusEnum["NotStarted"] = 0] = "NotStarted";
        //[Description("In Progress")]
        TaskStatusEnum[TaskStatusEnum["InProgress"] = 1] = "InProgress";
        //[Description("Done")]
        TaskStatusEnum[TaskStatusEnum["Done"] = 2] = "Done";
        //[Description("Rejected")]
        TaskStatusEnum[TaskStatusEnum["Rejected"] = 3] = "Rejected";
        //[Description("Failed")]
        TaskStatusEnum[TaskStatusEnum["Failed"] = 4] = "Failed";
    })(Enums.TaskStatusEnum || (Enums.TaskStatusEnum = {}));
    var TaskStatusEnum = Enums.TaskStatusEnum;
})(Enums || (Enums = {}));
