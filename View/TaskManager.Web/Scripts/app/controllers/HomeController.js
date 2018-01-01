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
            this.taskPriorityClasses = { 0: 'gray', 1: 'gray', 2: 'yellow', 3: 'orange', 4: 'red' };
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
            this.CreateTask_OnClick = function () {
                var task = new Models.TaskModel(null);
                _this.Model.EditTask = task;
                $("#edit-task-modal").modal({ closable: false }).modal('show');
            };
            this.EditTask_OnClick = function (task) {
                var clone = task.Clone();
                _this.Model.EditTask = clone;
                $("#edit-task-modal").modal({ closable: false }).modal('show');
            };
            this.CreateSubTask_OnClick = function () {
                var subtask = new Models.SubTaskModel(null);
                _this.Model.EditSubTask = subtask;
                $("#edit-subtask-modal").modal({ closable: false }).modal('show');
            };
            this.EditSubTask_OnClick = function (subtask) {
                var clone = subtask.Clone();
                _this.Model.EditSubTask = clone;
                $("#edit-subtask-modal").modal({ closable: false }).modal('show');
            };
            this.TaskPriority_OnClick = function () {
                _this.Model.EditTask.Priority = (_this.Model.EditTask.Priority + 1) % Object.keys(_this.taskPriorityClasses).length;
            };
            this.TaskOk_OnClick = function () {
                if (!_super.prototype.ValidateForm.call(_this, form)) {
                    $("#edit-task-modal").modal("refresh");
                    return;
                }
                //var $this = this;
                //$.ajax({
                //    url: '/api/Home/DeleteTask/',
                //    type: 'POST',
                //    data: {},
                //    beforeSend(xhr) {
                //        $this.ShowBusySaving();
                //    },
                //    complete() {
                //        $this.HideBusySaving();
                //        $this.$scope.$apply();
                //    },
                //    success: (result) => {
                //        if (result.success) {
                //            $this.ShowBusySaving();
                //            $this.$scope.$apply();
                //        } else {
                //            $this.Error(result.error);
                //        }
                //    },
                //    error: (jqXhr) => {
                //        console.error(jqXhr.statusText);
                //        $this.Error(jqXhr.statusText);
                //        $this.$scope.$apply();
                //    }
                //});
                _this.Model.EditTask = null;
                $("#edit-task-modal").modal('hide');
            };
            this.TaskCancel_OnClick = function () {
                _this.Model.EditTask = null;
                _super.prototype.ResetForm.call(_this, form);
                $("#edit-task-modal").modal('hide');
            };
            this.TaskDelete_OnClick = function () {
                _this.TaskCancel_OnClick();
            };
            this.SubTaskOk_OnClick = function () {
                if (!_super.prototype.ValidateForm.call(_this, form2)) {
                    $("#edit-subtask-modal").modal("refresh");
                    return;
                }
                _this.Model.EditSubTask = null;
                $("#edit-subtask-modal").modal('hide');
            };
            this.SubTaskCancel_OnClick = function () {
                _this.Model.EditSubTask = null;
                _super.prototype.ResetForm.call(_this, form2);
                $("#edit-subtask-modal").modal('hide');
            };
            this.SubTaskDelete_OnClick = function () {
                _this.SubTaskCancel_OnClick();
            };
            this.Model = new Models.HomeModel();
            $scope.Model = this.Model;
            $scope.TaskStatusNames = TaskStatusNames;
            $scope.TaskPriorityNames = TaskPriorityNames;
            $scope.TaskPriorityClasses = this.taskPriorityClasses;
            $scope.CreateTask_OnClick = this.CreateTask_OnClick;
            $scope.EditTask_OnClick = this.EditTask_OnClick;
            $scope.CreateSubTask_OnClick = this.CreateSubTask_OnClick;
            $scope.EditSubTask_OnClick = this.EditSubTask_OnClick;
            $scope.TaskPriority_OnClick = this.TaskPriority_OnClick;
            $scope.TaskOk_OnClick = this.TaskOk_OnClick;
            $scope.TaskCancel_OnClick = this.TaskCancel_OnClick;
            $scope.TaskDelete_OnClick = this.TaskDelete_OnClick;
            $scope.SubTaskOk_OnClick = this.SubTaskOk_OnClick;
            $scope.SubTaskCancel_OnClick = this.SubTaskCancel_OnClick;
            $scope.SubTaskDelete_OnClick = this.SubTaskDelete_OnClick;
            this.Load();
        }
        HomeController.$inject = ["$scope", "$http", "$location"];
        return HomeController;
    }(Controllers.BaseController));
    Controllers.HomeController = HomeController;
})(Controllers || (Controllers = {}));
