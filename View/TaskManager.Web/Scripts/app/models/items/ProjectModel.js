var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var ProjectModel = (function (_super) {
        __extends(ProjectModel, _super);
        function ProjectModel(data) {
            _super.call(this, data);
            if (data != null) {
                this.Name = data.Name;
            }
        }
        ProjectModel.prototype.Clone = function () {
            var clone = new ProjectModel(null);
            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();
            clone.Name = this.Name;
            return clone;
        };
        return ProjectModel;
    }(Models.BaseModel));
    Models.ProjectModel = ProjectModel;
})(Models || (Models = {}));
