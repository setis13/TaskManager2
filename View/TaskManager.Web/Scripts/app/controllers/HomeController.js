var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Controllers;
(function (Controllers) {
    var HomeController = (function (_super) {
        __extends(HomeController, _super);
        function HomeController($scope, $http, $location) {
            var _this = this;
            _super.call(this, $scope, $http, $location);
            this.$scope = $scope;
            this.$http = $http;
            this.$location = $location;
            this.Load = function () {
                var $this = _this;
                $.ajax({
                    url: '/api/Home/GetData/',
                    type: 'GET',
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
                            console.log(result);
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
            this.Model = new Models.HomeModel();
            $scope.Model = this.Model;
            this.Load();
        }
        HomeController.$inject = ["$scope", "$http", "$location"];
        return HomeController;
    }(Controllers.BaseController));
    Controllers.HomeController = HomeController;
})(Controllers || (Controllers = {}));
