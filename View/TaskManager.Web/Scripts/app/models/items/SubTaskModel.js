var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var SubTaskModel = (function (_super) {
        __extends(SubTaskModel, _super);
        function SubTaskModel() {
            _super.apply(this, arguments);
        }
        return SubTaskModel;
    }(Models.ModelBase));
    Models.SubTaskModel = SubTaskModel;
})(Models || (Models = {}));
