var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var ProjectsModel = (function (_super) {
        __extends(ProjectsModel, _super);
        function ProjectsModel() {
            _super.call(this);
            this.EditProject = null;
        }
        ProjectsModel.prototype.SetData = function (data) {
            this.Projects = new Array();
            for (var i = 0; i < data.Projects.length; i++) {
                this.Projects.push(new Models.ProjectModel(data.Projects[i]));
            }
        };
        return ProjectsModel;
    }(Models.ModelBase));
    Models.ProjectsModel = ProjectsModel;
})(Models || (Models = {}));
//# sourceMappingURL=ProjectsModel.js.map