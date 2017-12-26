var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Controllers;
(function (Controllers) {
    var LoginController = (function (_super) {
        __extends(LoginController, _super);
        function LoginController($scope, $http, $location) {
            var _this = this;
            _super.call(this, $scope, $http, $location);
            this.$scope = $scope;
            this.$http = $http;
            this.$location = $location;
            this.Login = function () {
                if (!_super.prototype.Validate.call(_this)) {
                    return;
                }
                var $this = _this;
                $.ajax({
                    url: '/api/Account/Login/',
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
                            window.location.href = result.data.ReturnUrl;
                        }
                        else {
                            $this.Error(result.error);
                            if (result.data && result.data.ReturnUrl) {
                                setTimeout(function () { window.location.href = result.data.ReturnUrl; }, 1000);
                            }
                        }
                    },
                    error: function (jqXhr) {
                        $this.Error(jqXhr.statusText);
                        $this.$scope.$apply();
                    }
                });
            };
            this.Model = new Models.LoginModel();
            $scope.Login = this.Login;
            $scope.Model = this.Model;
        }
        LoginController.$inject = ["$scope", "$http", "$location"];
        return LoginController;
    }(Controllers.BaseController));
    Controllers.LoginController = LoginController;
})(Controllers || (Controllers = {}));
