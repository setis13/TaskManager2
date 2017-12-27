var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var SubTaskModel = (function (_super) {
        __extends(SubTaskModel, _super);
        //public Comments: Array<CommentModel>;
        function SubTaskModel(data) {
            _super.call(this, data);
            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.Order = data.Order;
                this.Title = data.Title;
                this.Description = data.Description;
                this.ActualWork = moment.duration(data.ActualWork);
                this.TotalWork = moment.duration(data.TotalWork);
                this.Progress = data.Progress;
                this.Status = data.Status;
            }
        }
        return SubTaskModel;
    }(Models.BaseModel));
    Models.SubTaskModel = SubTaskModel;
})(Models || (Models = {}));
