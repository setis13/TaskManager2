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
            _super.apply(this, arguments);
        }
        return HomeModel;
    }(Models.ModelBase));
    Models.HomeModel = HomeModel;
})(Models || (Models = {}));
