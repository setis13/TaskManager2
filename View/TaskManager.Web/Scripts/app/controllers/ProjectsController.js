var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Controllers;
(function (Controllers) {
    var ProjectsController = (function (_super) {
        __extends(ProjectsController, _super);
        function ProjectsController($scope, $http, $location) {
            var _this = this;
            _super.call(this, $scope, $http, $location);
            this.Load = function () {
                var $this = _this;
                $.ajax({
                    url: '/api/Projects/GetData/',
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
            this.Create_OnClick = function () {
                var Project = new Models.ProjectModel(null);
                _this.Model.EditProject = Project;
                _this._projectModal.modal('show');
            };
            this.Edit_OnClick = function (Project) {
                var clone = Project.Clone();
                _this.Model.EditProject = clone;
                _this._projectModal.modal('show');
            };
            this.Ok_OnClick = function () {
                _this.Model.EditProject.Error = null;
                if (!_super.prototype.ValidateForm.call(_this)) {
                    _this._projectModal.modal("refresh");
                    return;
                }
                var $this = _this;
                $.ajax({
                    url: '/api/Projects/Save',
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    data: JSON.stringify(_this.Model.EditProject),
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
                            $this._projectModal.modal('hide');
                        }
                        else {
                            $this.Model.EditProject.Error = result.error;
                            $this.$scope.$apply();
                        }
                    },
                    error: function (jqXhr) {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            };
            this.Cancel_OnClick = function () {
                _this._projectModal.modal('hide');
            };
            this.Delete_OnClick = function () {
                _this.Model.EditProject.Error = null;
                var $this = _this;
                $.ajax({
                    url: '/api/Projects/Delete?id=' + _this.Model.EditProject.EntityId,
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
                            _this._projectModal.modal('hide');
                        }
                        else {
                            $this.Model.EditProject.Error = result.error;
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
            this._projectModal = $("#edit-project-modal").modal({
                closable: false,
                onHidden: function () {
                    $this.Model.EditProject = null;
                    $this.ResetForm();
                    $this.$scope.$apply();
                }
            });
            $scope.Model = this.Model = new Models.ProjectsModel();
            $scope.Create_OnClick = this.Create_OnClick;
            $scope.Edit_OnClick = this.Edit_OnClick;
            $scope.Ok_OnClick = this.Ok_OnClick;
            $scope.Cancel_OnClick = this.Cancel_OnClick;
            $scope.Delete_OnClick = this.Delete_OnClick;
            this.Load();
        }
        ProjectsController.$inject = ["$scope", "$http", "$location"];
        return ProjectsController;
    }(Controllers.BaseController));
    Controllers.ProjectsController = ProjectsController;
})(Controllers || (Controllers = {}));
//# sourceMappingURL=ProjectsController.js.map