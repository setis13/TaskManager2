var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var SignUpModel = (function (_super) {
        __extends(SignUpModel, _super);
        function SignUpModel() {
            _super.apply(this, arguments);
        }
        return SignUpModel;
    }(Models.ModelBase));
    Models.SignUpModel = SignUpModel;
})(Models || (Models = {}));
