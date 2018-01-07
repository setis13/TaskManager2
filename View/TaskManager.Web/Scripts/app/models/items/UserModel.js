var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var UserModel = (function (_super) {
        __extends(UserModel, _super);
        function UserModel(data) {
            _super.call(this);
            this.Id = data.Id;
            this.UserName = data.UserName;
            this.Email = data.Email;
        }
        return UserModel;
    }(Models.ModelBase));
    Models.UserModel = UserModel;
})(Models || (Models = {}));
//# sourceMappingURL=UserModel.js.map