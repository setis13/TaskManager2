var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var AccountUserModel = (function (_super) {
        __extends(AccountUserModel, _super);
        function AccountUserModel(data) {
            _super.call(this);
            this.Email = data.Email;
        }
        return AccountUserModel;
    }(Models.ModelBase));
    Models.AccountUserModel = AccountUserModel;
})(Models || (Models = {}));
//# sourceMappingURL=AccountUserModel.js.map