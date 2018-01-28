﻿declare var form: any;
declare var form2: any;
declare var form3: any;
declare var taskUsersDropdown: any;
declare var projectDropdown: any;
declare var statusDropdown: any;
declare var historyFilterDropdown: any;
declare var TaskPriorityNames: { [id: number]: string; };
declare var TaskStatusNames: { [id: number]: string; };

namespace Controllers {

    export class HomeController extends BaseController {

        private taskPriorityClasses: { [index: number]: string } =
        { 0: 'gray', 1: 'blue', 2: 'yellow', 3: 'orange', 4: 'red' };

        public Model: Models.HomeModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);

            var $this = this;

            setTimeout(() => {
                $("#history-filter").dropdown();
                $('#user-filter').dropdown();
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
            $scope.EditTaskComment_OnClick = this.EditTaskComment_OnClick;
            $scope.EditSubTaskComment_OnClick = this.EditSubTaskComment_OnClick;
            $scope.CommentOk_OnClick = this.CommentOk_OnClick;
            $scope.CommentCancel_OnClick = this.CommentCancel_OnClick;
            $scope.CommentDelete_OnClick = this.CommentDelete_OnClick;

            $scope.ToggleAllComments_OnClick = this.ToggleAllComments_OnClick;
            $scope.ShowedAllComments = this.ShowedAllComments;
            $scope.CheckNewValue = this.CheckNewValue;

            $scope.HistoryFilter_OnChange = this.HistoryFilter_OnChange;
            $scope.UserFilter_OnChange = this.UserFilter_OnChange;

            $scope.ClearFilters_OnClick = this.ClearFilters_OnClick;
            $scope.SortByTaskId_OnClick = this.SortByTaskId_OnClick;
            $scope.SortByUrgency_OnClick = this.SortByUrgency_OnClick;

            this.Load();
        }

        public InitTaskModal() {
            setTimeout(() => {
                $('#project').dropdown("set selected", this.Model.EditTask.ProjectId);
                $('#task-users').dropdown("set selected", this.Model.EditTask.UserIds);
            });
        }
        public InitSubTaskModal() {
        }
        public InitCommentModal() {
            var $this = this;
            setTimeout(() => {
                $('#status').dropdown("set selected", this.Model.EditComment.Status);
                $('#date').calendar({
                    type: 'date',
                    onChange: function (date, text) {
                        $this.Model.EditComment.DateMoment = moment(date);
                    }
                }).calendar("set date", this.Model.EditComment.DateMoment.toDate());
            });
        }

        public Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Home/GetData/' + (this.Model.SelectedHistoryFilter !== '0' ? '?historyFilter=' + this.Model.SelectedHistoryFilter : ''),
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
                        $this.Model.SetData(result.data);
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                }
            });
        }

        public HistoryFilter_OnChange = () => {
            super.ResetCheckNewValue();
            this.Load();
        }

        public UserFilter_OnChange = () => {
            super.ResetCheckNewValue();
            this.Model.ApplyClientFilter();
        }

        public ClearFilters_OnClick = () => {
            super.ResetCheckNewValue();
            setTimeout(() => {
                $("#history-filter").dropdown('restore defaults');
                $("#user-filter").dropdown('restore defaults');
                $('#history-filter').parent().find('.text.default').html($('#history-filter > option[value=""]').html());
                $('#user-filter').parent().find('.text.default').html($('#user-filter > option[value=""]').html());
            });
        }

        public SortByTaskId_OnClick = () => {
            this.Model.SortBy = this.Model.SortBy != Enums.SortByEnum.TaskIdDesc ?
                Enums.SortByEnum.TaskIdDesc : Enums.SortByEnum.TaskId;
            this.Model.ApplyClientFilter();
            this.SaveSorting();

        }
        public SortByUrgency_OnClick = () => {
            this.Model.SortBy = this.Model.SortBy != Enums.SortByEnum.UrgencyDesc ?
                Enums.SortByEnum.UrgencyDesc : Enums.SortByEnum.Urgency;
            this.Model.ApplyClientFilter();
            this.SaveSorting();
        }

        private SaveSorting() {
            var $this = this;
            $.ajax({
                url: '/api/Account/SaveSorting?sortby=' + this.Model.SortBy,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                success: (result) => {
                    if (result.success == false) {
                        $this.Model.Error = result.error;
                        $this.$scope.$apply();
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public TaskPriority_OnClick = () => {
            this.Model.EditTask.Priority = (this.Model.EditTask.Priority + 1) % Object.keys(<any>this.taskPriorityClasses).length;
        }

        public CreateTask_OnClick = () => {
            var task = new Models.TaskModel(null);
            this.Model.EditTask = task;
            this.InitTaskModal();
        }
        public EditTask_OnClick = (task: Models.TaskModel) => {
            var clone = task.Clone();
            this.Model.EditTask = clone;
            this.InitTaskModal();
        }

        public CreateSubTask_OnClick = (task: Models.TaskModel) => {
            var subtask = new Models.SubTaskModel(null);
            subtask.TaskId = task.EntityId;
            this.Model.EditSubTask = subtask;
            this.InitSubTaskModal();
        }
        public EditSubTask_OnClick = (subtask: Models.SubTaskModel) => {
            var clone = subtask.Clone();
            this.Model.EditSubTask = clone;
            this.InitSubTaskModal();
        }

        public AddTaskComment_OnClick = (task: Models.TaskModel) => {
            var comment = new Models.CommentModel(null);
            comment.TaskId = task.EntityId;
            this.Model.EditComment = comment;
            this.InitCommentModal();
        }
        public AddSubTaskComment_OnClick = (subtask: Models.SubTaskModel) => {
            var comment = new Models.CommentModel(null);
            comment.SubTaskId = subtask.EntityId;
            this.Model.EditComment = comment;
            this.InitCommentModal();
        }
        public EditTaskComment_OnClick = (task: Models.TaskModel, comment: Models.CommentModel) => {
            var clone = comment.Clone();
            this.Model.EditComment = clone;
            this.InitCommentModal();
        }
        public EditSubTaskComment_OnClick = (subtask: Models.SubTaskModel, comment: Models.CommentModel) => {
            var clone = comment.Clone();
            this.Model.EditComment = clone;
            this.InitCommentModal();
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
                $("#edit-task-modal").modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveTask',
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
                        $this.Model.EditTask = null;
                        $this.Load();
                    } else {
                        $this.Model.EditTask.Error = result.error;
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
                        $this.Model.EditTask = null;
                        $this.Load();
                    } else {
                        $this.Model.EditTask.Error = result.error;
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
                $("#edit-subtask-modal").modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveSubTask',
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
                        $this.Model.EditSubTask = null;
                        $this.Load();
                    } else {
                        $this.Model.EditSubTask.Error = result.error;
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
                        $this.Model.EditSubTask = null;
                        $this.Load();
                    } else {
                        $this.Model.EditSubTask.Error = result.error;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public CommentOk_OnClick = () => {
            this.Model.EditComment.Error = null;
            if (!super.ValidateForm(form3)) {
                $("#edit-comment-modal").modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Home/SaveComment',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditComment),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Model.EditComment = null;
                        $this.Load();
                    } else {
                        $this.Model.EditComment.Error = result.error;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public CommentCancel_OnClick = () => {
            this.Model.EditComment = null;
        }

        public CommentDelete_OnClick = () => {
            this.Model.EditComment.Error = null;
            var $this = this;
            $.ajax({
                url: '/api/Home/DeleteComment?id=' + this.Model.EditComment.EntityId,
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
                        $this.Model.EditComment = null;
                        $this.Load();
                    } else {
                        $this.Model.EditComment.Error = result.error;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public static MIN_COMMENTS = 3;
        // Scope. Array of taskIds that show all comments.
        private ShowedAllComments: Array<string> = new Array();
        public ToggleAllComments_OnClick = (taskId: string, comments: Array<Models.CommentModel>) => {
            if (this.ShowedAllComments.indexOf(taskId) !== -1) {
                this.ShowedAllComments.splice($.inArray(taskId, this.ShowedAllComments), 1);

                for (var i = 0; i < comments.length; i++) {
                    comments[i].Visible = i > comments.length - HomeController.MIN_COMMENTS - 1;
                }
            } else {
                this.ShowedAllComments.push(taskId);

                for (var i = 0; i < comments.length; i++) {
                    comments[i].Visible = true;
                }
            }
        }
    }
}