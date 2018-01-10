declare var form: any;

namespace Controllers {

    export class ProjectsController extends BaseController {

        public Model: Models.ProjectsModel;

        static $inject = ["$scope", "$http", "$location"];

        private _projectModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this._projectModal = (<any>$("#edit-project-modal")).modal({
                closable: false,
                onHidden() {
                    $this.Model.EditProject = null;
                    $this.ResetForm();
                    $this.$scope.$apply();
                }
            });

            $scope.Create_OnClick = this.Create_OnClick;
            $scope.Edit_OnClick = this.Edit_OnClick;
            $scope.Ok_OnClick = this.Ok_OnClick;
            $scope.Cancel_OnClick = this.Cancel_OnClick;
            $scope.Delete_OnClick = this.Delete_OnClick;

            this.Load();
        }

        public Load = () => {
            this.$scope.Model = this.Model = new Models.ProjectsModel();

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
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.$scope.$apply();
                        this.Model.SetData(result.data);
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                    $this.$scope.$apply();
                }
            });
        }

        public Create_OnClick = () => {
            var Project = new Models.ProjectModel(null);
            this.Model.EditProject = Project;
            this._projectModal.modal('show');
        }
        public Edit_OnClick = (Project: Models.ProjectModel) => {
            var clone = Project.Clone();
            this.Model.EditProject = clone;
            this._projectModal.modal('show');
        }

        public Ok_OnClick = () => {
            this.Model.EditProject.Error = null;
            if (!super.ValidateForm()) {
                this._projectModal.modal("refresh");
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
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this._projectModal.modal('hide');
                        $this.Load();
                    } else {
                        $this.Model.EditProject.Error = result.error;
                        $this.$scope.$apply();
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Cancel_OnClick = () => {
            this._projectModal.modal('hide');
        }

        public Delete_OnClick = () => {
            this.Model.EditProject.Error = null;
            var $this = this;
            $.ajax({
                url: '/api/Projects/Delete?id=' + this.Model.EditProject.EntityId,
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyDeleting();
                },
                complete() {
                    $this.HideBusyDeleting();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        this._projectModal.modal('hide');
                        $this.Load();
                    } else {
                        $this.Model.EditProject.Error = result.error;
                        $this.$scope.$apply();
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