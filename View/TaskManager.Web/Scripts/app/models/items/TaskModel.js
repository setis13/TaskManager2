var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var TaskModel = (function (_super) {
        __extends(TaskModel, _super);
        //public Comments: Array<CommentModel>;
        function TaskModel(data) {
            _super.call(this, data);
            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.ProjectId = data.ProjectId;
                this.Index = data.Index;
                this.Title = data.Title;
                this.Description = data.Description;
                this.Priority = data.Priority;
                this.ActualWork = moment.duration(data.ActualWork);
                this.TotalWork = moment.duration(data.TotalWork);
                this.Progress = data.Progress;
                this.Status = data.Status;
                this.SubTasks = new Array();
                for (var i = 0; i < data.SubTasks.length; i++) {
                    this.SubTasks.push(new Models.SubTaskModel(data.SubTasks[i]));
                    console.log("add SubTask");
                }
            }
        }
        return TaskModel;
    }(Models.BaseModel));
    Models.TaskModel = TaskModel;
})(Models || (Models = {}));
