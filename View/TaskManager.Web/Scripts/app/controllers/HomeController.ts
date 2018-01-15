declare var form: any;
declare var form2: any;
declare var form3: any;
declare var TaskPriorityNames: { [id: number]: string; };
declare var TaskStatusNames: { [id: number]: string; };

namespace Controllers {

    export class HomeController extends BaseController {

        private taskPriorityClasses: { [index: number]: string } =
        { 0: 'gray', 1: 'blue', 2: 'yellow', 3: 'orange', 4: 'red' };

        public Model: Models.HomeModel;

        static $inject = ["$scope", "$http", "$location"];

        private _taskModal: any;
        private _subTaskModal: any;
        private _commentModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this._taskModal = (<any>$("#edit-task-modal")).modal({
                closable: false,
                onHidden() {
                    $this.Model.EditTask = null;
                    $this.ResetForm(form);
                    $this.$scope.$apply();

                    //  resets dropdown
                    (<any>$('#task-users')).dropdown("restore defaults");
                    (<any>$('#project')).dropdown("restore defaults");
                    // NOTES: default text was changed after selected because this code restores it
                    (<any>$('#project')).parent().find('.text.default').html($('#project > option[value=""]').html());
                },
                onShow() {
                    // NOTES: event.onVisibly works withount timeout, but has delay
                    // sets selected value in dropdown
                    setTimeout(() => {
                        if ($('#task-users').val().length > 0) {
                            (<any>$('#task-users')).dropdown("set selected", $('#task-users').val());
                        }
                        if ($('#project').val() !== "") {
                            (<any>$('#project')).dropdown("set selected", $('#project').val());
                        }
                    });
                }
            });
            this._subTaskModal = (<any>$("#edit-subtask-modal")).modal({
                closable: false,
                onHidden() {
                    $this.Model.EditSubTask = null;
                    $this.ResetForm(form2);
                    $this.$scope.$apply();
                }
            });
            this._commentModal = (<any>$("#edit-comment-modal")).modal({
                closable: false,
                onShow() {
                    (<any>$('#date')).calendar({
                        type: 'date'
                    });
                },
                onHidden() {
                    $this.Model.EditComment = null;
                    $this.ResetForm(form3);
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
            $scope.UpSubTask_OnClick = this.UpSubTask_OnClick;
            $scope.DownSubTask_OnClick = this.DownSubTask_OnClick;
            $scope.SubTaskOk_OnClick = this.SubTaskOk_OnClick;
            $scope.SubTaskCancel_OnClick = this.SubTaskCancel_OnClick;
            $scope.SubTaskDelete_OnClick = this.SubTaskDelete_OnClick;

            $scope.AddTaskComment_OnClick = this.AddTaskComment_OnClick;
            $scope.AddSubTaskComment_OnClick = this.AddSubTaskComment_OnClick;

            this.Load();
        }

        public Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Home/GetData/',
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
            this._taskModal.modal('show');
        }
        public EditTask_OnClick = (task: Models.TaskModel) => {
            var clone = task.Clone();
            this.Model.EditTask = clone;
            this._taskModal.modal('show');
        }

        public CreateSubTask_OnClick = (task: Models.TaskModel) => {
            var subtask = new Models.SubTaskModel(null);
            subtask.TaskId = task.EntityId;
            this.Model.EditSubTask = subtask;
            this._subTaskModal.modal('show');
        }
        public EditSubTask_OnClick = (subtask: Models.SubTaskModel) => {
            var clone = subtask.Clone();
            this.Model.EditSubTask = clone;
            this._subTaskModal.modal('show');
        }

        public AddTaskComment_OnClick = (task: Models.TaskModel) => {
            var comment = new Models.CommentModel(null);
            comment.TaskId = task.EntityId;
            this.Model.EditComment = comment;
            this._commentModal.modal('show');
        }
        public AddSubTaskComment_OnClick = (subtask: Models.SubTaskModel) => {
            var comment = new Models.CommentModel(null);
            comment.SubTaskId = subtask.EntityId;
            this.Model.EditComment = comment;
            this._commentModal.modal('show');
        }
        public EditTaskComment_OnClick = (task: Models.TaskModel, comment: Models.CommentModel) => {
            var clone = comment.Clone();
            this.Model.EditComment = clone;
            this._commentModal.modal('show');
        }
        public EditSubTaskComment_OnClick = (task: Models.SubTaskModel, comment: Models.CommentModel) => {
            var clone = comment.Clone();
            this.Model.EditComment = clone;
            this._commentModal.modal('show');
        }

        public UpSubTask_OnClick = (subtask: Models.SubTaskModel) => {
            var $this = this;
            $.ajax({
                url: '/api/Home/UpSubTask?id=' + subtask.EntityId,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyRow($("#subtask-" + subtask.EntityId));
                },
                complete() {
                    $this.HideBusyRow($("#subtask-" + subtask.EntityId));
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Model.SetSubTasks(result.data.SubTasks);
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public DownSubTask_OnClick = (subtask: Models.SubTaskModel) => {
            var $this = this;
            $.ajax({
                url: '/api/Home/DownSubTask?id=' + subtask.EntityId,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyRow($("#subtask-" + subtask.EntityId));
                },
                complete() {
                    $this.HideBusyRow($("#subtask-" + subtask.EntityId));
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Model.SetSubTasks(result.data.SubTasks);
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public TaskOk_OnClick = () => {
            this.Model.EditTask.Error = null;
            if (!super.ValidateForm(form)) {
                this._taskModal.modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveTask?id=' + this.Model.EditTask.EntityId,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditTask),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        this._taskModal.modal('hide');
                        $this.Load();
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
            this._taskModal.modal('hide');
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
                        this._taskModal.modal('hide');
                        $this.Load();
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
                this._subTaskModal.modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveSubTask?id=' + this.Model.EditSubTask.EntityId,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditSubTask),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        this._subTaskModal.modal('hide');
                        $this.Load();
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
            this._subTaskModal.modal('hide');
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
                        this._subTaskModal.modal('hide');
                        $this.Load();
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