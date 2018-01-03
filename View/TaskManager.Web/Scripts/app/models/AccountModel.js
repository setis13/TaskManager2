var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var AccountModel = (function (_super) {
        __extends(AccountModel, _super);
        function AccountModel() {
            _super.call(this);
        }
        AccountModel.prototype.SetData = function (data) {
            this.User = new Models.UserModel(data);
        };
        return AccountModel;
    }(Models.ModelBase));
    Models.AccountModel = AccountModel;
})(Models || (Models = {}));
//# sourceMappingURL=AccountModel.js.map