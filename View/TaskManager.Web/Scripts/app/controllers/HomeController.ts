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
            $scope.CreateTask_OnClick = this.CreateTask_OnClick;
            $scope.EditTask_OnClick = this.EditTask_OnClick;
            $scope.TaskPriority_OnClick = this.TaskPriority_OnClick;
            $scope.Ok_OnClick = this.Ok_OnClick;
            $scope.Cancel_OnClick = this.Cancel_OnClick;

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

        public CreateTask_OnClick = () => {
            var task = new Models.TaskModel(null);
            this.Model.EditTask = task;
            (<any>$("#edit-modal")).modal({ closable: false }).modal('show');
        }

        public EditTask_OnClick = (task: Models.TaskModel) => {
            var clone = task.Clone();
            this.Model.EditTask = clone;
            (<any>$("#edit-modal")).modal({ closable: false }).modal('show');
        }

        public TaskPriority_OnClick = () => {
            this.Model.EditTask.Priority = (this.Model.EditTask.Priority + 1) % Object.keys(<any>this.taskPriorityClasses).length;
        }

        public Ok_OnClick = () => {
            if (!super.ValidateForm()) {
                (<any>$("#edit-modal")).modal("refresh");
                return;
            }
            this.Model.EditTask = null;
            (<any>$("#edit-modal")).modal('hide');
        }

        public Cancel_OnClick = () => {
            this.Model.EditTask = null;
            super.ResetForm();
            (<any>$("#edit-modal")).modal('hide');
        }
    }
}