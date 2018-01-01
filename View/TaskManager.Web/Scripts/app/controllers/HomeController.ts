declare var form: any;
declare var form2: any;
declare var TaskPriorityNames: { [id: number]: string; };
declare var TaskStatusNames: { [id: number]: string; };

namespace Controllers {

    export class HomeController extends BaseController {

        private taskPriorityClasses: { [index: number]: string } = { 0: 'gray', 1: 'gray', 2: 'yellow', 3: 'orange', 4: 'red' };

        public Model: Models.HomeModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            this.Model = new Models.HomeModel();

            $scope.Model = this.Model;

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

        public Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Home/GetData/',
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.ShowBusySaving();
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

        public TaskPriority_OnClick = () => {
            this.Model.EditTask.Priority = (this.Model.EditTask.Priority + 1) % Object.keys(<any>this.taskPriorityClasses).length;
        }

        public CreateTask_OnClick = () => {
            var task = new Models.TaskModel(null);
            this.Model.EditTask = task;
            (<any>$("#edit-task-modal")).modal({ closable: false }).modal('show');
        }
        public EditTask_OnClick = (task: Models.TaskModel) => {
            var clone = task.Clone();
            this.Model.EditTask = clone;
            (<any>$("#edit-task-modal")).modal({ closable: false }).modal('show');
        }

        public CreateSubTask_OnClick = () => {
            var subtask = new Models.SubTaskModel(null);
            this.Model.EditSubTask = subtask;
            (<any>$("#edit-subtask-modal")).modal({ closable: false }).modal('show');
        }
        public EditSubTask_OnClick = (subtask: Models.SubTaskModel) => {
            var clone = subtask.Clone();
            this.Model.EditSubTask = clone;
            (<any>$("#edit-subtask-modal")).modal({ closable: false }).modal('show');
        }

        public TaskOk_OnClick = () => {
            this.Model.EditTask.Error = null;
            if (!super.ValidateForm(form)) {
                (<any>$("#edit-task-modal")).modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveTask?id=' + this.Model.EditTask.EntityId,
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.ShowBusySaving();
                        $this.Model.EditTask = null;
                        (<any>$("#edit-task-modal")).modal('hide');
                        $this.$scope.$apply();
                    } else {
                        $this.Model.EditTask.Error = result.error;
                        $this.$scope.$apply();
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public TaskCancel_OnClick = () => {
            this.Model.EditTask = null;
            super.ResetForm(form);
            (<any>$("#edit-task-modal")).modal('hide');
        }

        public TaskDelete_OnClick = () => {
            this.Model.EditTask.Error = null;
            var $this = this;
            $.ajax({
                url: '/api/Home/DeleteTask?id=' + this.Model.EditTask.EntityId,
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
                        $this.ShowBusyDeleting();
                        $this.Model.EditTask = null;
                        super.ResetForm(form);
                        (<any>$("#edit-task-modal")).modal('hide');
                        $this.$scope.$apply();
                    } else {
                        $this.Model.EditTask.Error = result.error;
                        $this.$scope.$apply();
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public SubTaskOk_OnClick = () => {
            this.Model.EditSubTask.Error = null;
            if (!super.ValidateForm(form2)) {
                (<any>$("#edit-subtask-modal")).modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveSubTask?id=' + this.Model.EditSubTask.EntityId,
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.ShowBusySaving();
                        this.Model.EditSubTask = null;
                        (<any>$("#edit-subtask-modal")).modal('hide');
                        $this.$scope.$apply();
                    } else {
                        $this.Model.EditSubTask.Error = result.error;
                        $this.$scope.$apply();
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public SubTaskCancel_OnClick = () => {
            this.Model.EditSubTask = null;
            super.ResetForm(form2);
            (<any>$("#edit-subtask-modal")).modal('hide');
        }

        public SubTaskDelete_OnClick = () => {
            this.Model.EditSubTask.Error = null;
            var $this = this;
            $.ajax({
                url: '/api/Home/DeleteSubTask?id=' + this.Model.EditSubTask.EntityId,
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
                        $this.ShowBusyDeleting();
                        $this.Model.EditSubTask = null;
                        super.ResetForm(form2);
                        (<any>$("#edit-subtask-modal")).modal('hide');
                        $this.$scope.$apply();
                    } else {
                        $this.Model.EditSubTask.Error = result.error;
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