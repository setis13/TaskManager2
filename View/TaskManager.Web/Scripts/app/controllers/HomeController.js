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
            this.taskPriorityClasses = { 0: 'gray', 1: 'blue', 2: 'yellow', 3: 'orange', 4: 'red' };
            this.Load = function () {
                var $this = _this;
                $.ajax({
                    url: '/api/Home/GetData/',
                    type: 'POST',
                    data: {},
                    beforeSend: function (xhr) {
                        $this.ShowLoader();
                    },
                    complete: function () {
                        $this.HideLoader();
                        $this.$scope.$apply();
                    },
                    success: function (result) {
                        if (result.success) {
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
            this.TaskPriority_OnClick = function () {
                _this.Model.EditTask.Priority = (_this.Model.EditTask.Priority + 1) % Object.keys(_this.taskPriorityClasses).length;
            };
            this.CreateTask_OnClick = function () {
                var task = new Models.TaskModel(null);
                _this.Model.EditTask = task;
                _this._taskModal.modal('show');
            };
            this.EditTask_OnClick = function (task) {
                var clone = task.Clone();
                _this.Model.EditTask = clone;
                _this._taskModal.modal('show');
            };
            this.CreateSubTask_OnClick = function () {
                var subtask = new Models.SubTaskModel(null);
                _this.Model.EditSubTask = subtask;
                _this._subTaskModal.modal('show');
            };
            this.EditSubTask_OnClick = function (subtask) {
                var clone = subtask.Clone();
                _this.Model.EditSubTask = clone;
                _this._subTaskModal.modal('show');
            };
            this.TaskOk_OnClick = function () {
                _this.Model.EditTask.Error = null;
                if (!_super.prototype.ValidateForm.call(_this, form)) {
                    _this._taskModal.modal("refresh");
                    return;
                }
                var $this = _this;
                $.ajax({
                    url: '/api/Home/SaveTask?id=' + _this.Model.EditTask.EntityId,
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
                            _this._taskModal.modal('hide');
                        }
                        else {
                            $this.Model.EditTask.Error = result.error;
                            $this.$scope.$apply();
                        }
                    },
                    error: function (jqXhr) {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            };
            this.TaskCancel_OnClick = function () {
                _this._taskModal.modal('hide');
            };
            this.TaskDelete_OnClick = function () {
                _this.Model.EditTask.Error = null;
                var $this = _this;
                $.ajax({
                    url: '/api/Home/DeleteTask?id=' + _this.Model.EditTask.EntityId,
                    type: 'POST',
                    data: {},
                    beforeSend: function (xhr) {
                        $this.ShowBusyDeleting();
                    },
                    complete: function () {
                        $this.HideBusyDeleting();
                        $this.$scope.$apply();
                    },
                    success: function (result) {
                        if (result.success) {
                            $this.ShowBusyDeleting();
                            _this._taskModal.modal('hide');
                        }
                        else {
                            $this.Model.EditTask.Error = result.error;
                            $this.$scope.$apply();
                        }
                    },
                    error: function (jqXhr) {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            };
            this.SubTaskOk_OnClick = function () {
                _this.Model.EditSubTask.Error = null;
                if (!_super.prototype.ValidateForm.call(_this, form2)) {
                    _this._subTaskModal.modal("refresh");
                    return;
                }
                var $this = _this;
                $.ajax({
                    url: '/api/Home/SaveSubTask?id=' + _this.Model.EditSubTask.EntityId,
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
                            _this._subTaskModal.modal('hide');
                        }
                        else {
                            $this.Model.EditSubTask.Error = result.error;
                            $this.$scope.$apply();
                        }
                    },
                    error: function (jqXhr) {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            };
            this.SubTaskCancel_OnClick = function () {
                _this._subTaskModal.modal('hide');
            };
            this.SubTaskDelete_OnClick = function () {
                _this.Model.EditSubTask.Error = null;
                var $this = _this;
                $.ajax({
                    url: '/api/Home/DeleteSubTask?id=' + _this.Model.EditSubTask.EntityId,
                    type: 'POST',
                    data: {},
                    beforeSend: function (xhr) {
                        $this.ShowBusyDeleting();
                    },
                    complete: function () {
                        $this.HideBusyDeleting();
                        $this.$scope.$apply();
                    },
                    success: function (result) {
                        if (result.success) {
                            $this.ShowBusyDeleting();
                            _this._subTaskModal.modal('hide');
                        }
                        else {
                            $this.Model.EditSubTask.Error = result.error;
                            $this.$scope.$apply();
                        }
                    },
                    error: function (jqXhr) {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            };
            var $this = this;
            this._taskModal = $("#edit-task-modal").modal({
                closable: false,
                onHidden: function () {
                    $this.Model.EditTask = null;
                    $this.ResetForm(form);
                    $this.$scope.$apply();
                }
            });
            this._subTaskModal = $("#edit-subtask-modal").modal({
                closable: false,
                onHidden: function () {
                    $this.Model.EditSubTask = null;
                    $this.ResetForm(form2);
                    $this.$scope.$apply();
                }
            });
            $scope.Model = this.Model = new Models.HomeModel();
            $scope.TaskStatusNames = TaskStatusNames;
            $scope.TaskPriorityNames = TaskPriorityNames;
            $scope.TaskPriorityClasses = this.taskPriorityClasses;
            $scope.TaskPriority_OnClick = this.TaskPriority_OnClick;
            $scope.CreateTask_OnClick = this.CreateTask_OnClick;
            $scope.EditTask_OnClick = this.EditTask_OnClick;
            $scope.TaskOk_OnClick = this.TaskOk_OnClick;
            $scope.TaskCancel_OnClick = this.TaskCancel_OnClick;
            $scope.TaskDelete_OnClick = this.TaskDelete_OnClick;
            $scope.CreateSubTask_OnClick = this.CreateSubTask_OnClick;
            $scope.EditSubTask_OnClick = this.EditSubTask_OnClick;
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
//# sourceMappingURL=HomeController.js.map