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
            this.Load = function () {
                var $this = _this;
                $.ajax({
                    url: '/api/Home/GetData/',
                    type: 'POST',
                    data: {},
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
                            _this.Model.SetData(result.data);
                        }
                        else {
                            $this.Error(result.error);
                        }
                    },
                    error: function (jqXhr) {
                        console.error(jqXhr.statusText);
                        $this.Error(jqXhr.statusText);
                        $this.$scope.$apply();
                    }
                });
            };
            this.CreateTask = function () {
                $("#edit-modal").modal({ closable: false }).modal('show');
            };
            this.Model = new Models.HomeModel();
            $scope.Model = this.Model;
            $scope.TaskStatusNames = TaskStatusNames;
            $scope.TaskPriorityNames = TaskPriorityNames;
            $scope.CreateTask = this.CreateTask;
            this.Load();
        }
        HomeController.$inject = ["$scope", "$http", "$location"];
        return HomeController;
    }(Controllers.BaseController));
    Controllers.HomeController = HomeController;
})(Controllers || (Controllers = {}));
