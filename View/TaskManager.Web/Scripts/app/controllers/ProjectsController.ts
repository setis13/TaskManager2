﻿declare var form: any;

namespace Controllers {

    export class ProjectsController extends BaseController {

        public Model: Models.ProjectsModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            $scope.Create_OnClick = this.Create_OnClick;
            $scope.Edit_OnClick = this.Edit_OnClick;
            $scope.Ok_OnClick = this.Ok_OnClick;
            $scope.Cancel_OnClick = this.Cancel_OnClick;
            $scope.Delete_OnClick = this.Delete_OnClick;

            this.Load();
        }

        public Load = () => {
            this.scope.Model = this.Model = new Models.ProjectsModel();

            var $this = this;
            $.ajax({
                url: '/api/Projects/GetData/',
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowLoader();
                },
                complete() {
                    $this.HideLoader();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetData(result.Data);
                    } else {
                        $this.Error(result.Message);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                }
            });
        }

        public Create_OnClick = () => {
            var project = new Models.ProjectModel(null);
            this.Model.EditProject = project;
        }
        public Edit_OnClick = (project: Models.ProjectModel) => {
            var clone = project.Clone();
            this.Model.EditProject = clone;
        }

        public Ok_OnClick = () => {
            this.Model.EditProject.Error = null;
            if (!super.ValidateForm()) {
                $("#edit-project-modal").modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Projects/Save',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditProject),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        this.Model.EditProject = null;
                        $this.Load();
                    } else {
                        $this.Model.EditProject.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Cancel_OnClick = () => {
            this.Model.EditProject = null;
        }

        public Delete_OnClick = (confirmed: boolean = false) => {
            var $this = this;
            // a confirm modal
            if (this.Model.EditProject.EntityId != null && confirmed == false) {
                $('#confirm-modal>.content>p').html("Please to confirm to delete the project");
                $('#confirm-modal')
                    .modal({
                        allowMultiple: true,
                        closable: false,
                        onApprove: function () {
                            $this.Delete_OnClick(true);
                        }
                    }).modal('show');
                return;
            }

            this.Model.EditProject.Error = null;
            $.ajax({
                url: '/api/Projects/Delete?id=' + this.Model.EditProject.EntityId,
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyDeleting();
                },
                complete() {
                    $this.HideBusyDeleting();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        this.Model.EditProject = null;
                        $this.Load();
                    } else {
                        $this.Model.EditProject.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }
    }
}