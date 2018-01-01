var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var TaskModel = (function (_super) {
        __extends(TaskModel, _super);
        function TaskModel(data) {
            _super.call(this, data);
            this.Priority = 0;
            this.SubTasks = new Array();
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
                for (var i = 0; i < data.SubTasks.length; i++) {
                    this.SubTasks.push(new Models.SubTaskModel(data.SubTasks[i]));
                }
            }
        }
        Object.defineProperty(TaskModel.prototype, "TotalWorkHours", {
            //public Comments: Array<CommentModel>;
            //extra
            get: function () {
                if (this.TotalWork != null) {
                    return this.TotalWork.asHours().toString();
                }
                else {
                    return null;
                }
            },
            set: function (str) {
                var value = parseInt(str);
                this.TotalWork = moment.duration(!isNaN(value) ? value : 0, "hours");
            },
            enumerable: true,
            configurable: true
        });
        TaskModel.prototype.Clone = function () {
            var clone = new TaskModel(null);
            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();
            clone.CompanyId = this.CompanyId;
            clone.ProjectId = this.ProjectId;
            clone.Index = this.Index;
            clone.Title = this.Title;
            clone.Description = this.Description;
            clone.Priority = this.Priority;
            clone.ActualWork = this.ActualWork;
            clone.TotalWork = this.TotalWork;
            clone.Progress = this.Progress;
            clone.Status = this.Status;
            for (var i = 0; i < this.SubTasks.length; i++) {
                clone.SubTasks.push(this.SubTasks[i].Clone());
            }
            return clone;
        };
        return TaskModel;
    }(Models.BaseModel));
    Models.TaskModel = TaskModel;
})(Models || (Models = {}));
