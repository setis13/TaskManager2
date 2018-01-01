var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Models;
(function (Models) {
    var BaseModel = (function (_super) {
        __extends(BaseModel, _super);
        function BaseModel(data) {
            _super.call(this);
            if (data != null) {
                this.EntityId = data.EntityId;
                this.CreatedDate = moment(data.CreatedDate);
            }
        }
        return BaseModel;
    }(Models.ModelBase));
    Models.BaseModel = BaseModel;
})(Models || (Models = {}));
//# sourceMappingURL=1BaseModel.js.map