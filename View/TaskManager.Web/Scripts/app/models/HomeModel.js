var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var HomeModel = (function (_super) {
        __extends(HomeModel, _super);
        function HomeModel() {
            _super.call(this);
            this.EditTask = null;
            this.EditSubTask = null;
        }
        HomeModel.prototype.SetData = function (data) {
            this.Users = new Array();
            this.Tasks = new Array();
            for (var i = 0; i < data.Tasks.length; i++) {
                this.Tasks.push(new Models.TaskModel(data.Tasks[i]));
            }
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new Models.UserModel(data.Users[i]));
            }
        };
        return HomeModel;
    }(Models.ModelBase));
    Models.HomeModel = HomeModel;
})(Models || (Models = {}));
//# sourceMappingURL=HomeModel.js.map