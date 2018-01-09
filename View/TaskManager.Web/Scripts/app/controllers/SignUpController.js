var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Controllers;
(function (Controllers) {
    var SignUpController = (function (_super) {
        __extends(SignUpController, _super);
        function SignUpController($scope, $http, $location) {
            var _this = this;
            _super.call(this, $scope, $http, $location);
            this.$scope = $scope;
            this.$http = $http;
            this.$location = $location;
            this.SignUp = function () {
                if (!_super.prototype.ValidateForm.call(_this)) {
                    return;
                }
                var $this = _this;
                $.ajax({
                    url: '/api/Account/Register/',
                    type: 'POST',
                    data: _this.Model,
                    beforeSend: function (xhr) {
                        $this.ShowBusySaving();
                    },
                    complete: function () {
                        $this.HideBusySaving();
                        $this.$scope.$apply();
                    },
                    success: function (result) {
                        if (result.success) {
                            $this.ShowBusySaving();
                            $this.$scope.$apply();
                            window.location.href = result.data.ReturnUrl;
                        }
                        else {
                            $this.Error(result.error);
                        }
                    },
                    error: function (jqXhr) {
                        $this.Error(jqXhr.statusText);
                        $this.$scope.$apply();
                    }
                });
            };
            this.Model = new Models.SignUpModel();
            $scope.SignUp = this.SignUp;
            $scope.Model = this.Model;
        }
        SignUpController.$inject = ["$scope", "$http", "$location"];
        return SignUpController;
    }(Controllers.BaseController));
    Controllers.SignUpController = SignUpController;
})(Controllers || (Controllers = {}));
//# sourceMappingURL=SignUpController.js.map