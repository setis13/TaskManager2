declare var form: any;
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

        private taskPriorityClasses: { [index: number]: string } = { 0: 'gray', 1: 'blue', 2: 'yellow', 3: 'orange', 4: 'red' };
        private sortByNames: { [index: number]: string } = { 0: 'none', 1: 'Task#', 2: 'Task#', 3: 'Urgency', 4: 'Urgency', 5: 'News', 6: 'News' };

        public Model: Models.HomeModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);

            var $this = this;

            setTimeout(() => {
                $("#history-filter").dropdown();
                $('#user-filter').dropdown();
                $('#project-filter').dropdown();
            });

            $scope.Model = this.Model = new Models.HomeModel();

            $scope.TaskStatusNames = TaskStatusNames;
            $scope.TaskPriorityNames = TaskPriorityNames;
            $scope.TaskPriorityClasses = this.taskPriorityClasses;
            $scope.SortByNames = this.sortByNames;

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
            $scope.ResetCheckNewValue = this.ResetCheckNewValue;

            $scope.HistoryFilter_OnChange = this.HistoryFilter_OnChange;
            $scope.UserFilter_OnChange = this.UserFilter_OnChange;
            $scope.ProjectFilter_OnChange = this.ProjectFilter_OnChange;
            $scope.CommentStatus_OnChange = this.CommentStatus_OnChange;

            $scope.ClearFilters_OnClick = this.ClearFilters_OnClick;
            $scope.SortBy_OnClick = this.SortBy_OnClick;

            $scope.TaskFavorite_OnClick = this.TaskFavorite_OnClick;
            $scope.SubTaskFavorite_OnClick = this.SubTaskFavorite_OnClick;
            $scope.FavoriteFilter_OnClick = this.FavoriteFilter_OnClick;

            $scope.AttachFiles_OnClick = this.AttachFiles_OnClick;
            $scope.RemoveFile_OnClick = this.RemoveFile_OnClick;
            $scope.SizeName = SizeName;

            this.Load();
        }

        public InitTaskModal() {
            setTimeout(() => {
                $('#task-title').unbind('input'); // event was busy
                $('#task-description').unbind('input'); // event was busy
                $('#task-total-hours').unbind('input'); // event was busy
                $('#task-project').dropdown("set selected", this.Model.EditTask.ProjectId);
                // NOTE: костыль!
                $('#task-users').val('').dropdown("set selected", Enumerable.From(this.Model.EditTask.UserIds).Select(e => "string:" + e).ToArray());
            });
        }
        public InitSubTaskModal() {
            setTimeout(() => {
                $('#subtask-title').unbind('input'); // event was busy
                $('#subtask-description').unbind('input'); // event was busy
                $('#subtask-total-hours').unbind('input'); // event was busy
            });
        }
        public InitCommentModal() {
            var $this = this;
            setTimeout(() => {
                $('#comment-description').unbind('input'); // event was busy
                $('#comment-status').dropdown("set selected", this.Model.EditComment.Status);
                $('#comment-date').calendar({
                    type: 'date',
                    onChange: function (date, text) {
                        $this.Model.EditComment.DateMoment = moment(date);
                    }
                }).calendar("set date", this.Model.EditComment.DateMoment.toDate());
            });
        }

        public InitComment() {
            setTimeout(() => {
                (<any>$(".timeago")).timeago();
                (<any>$(".lightgallery")).lightGallery();
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetData(result.Data);
                        $this.InitComment();
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

        public HistoryFilter_OnChange = () => {
            this.Load();
        }

        public UserFilter_OnChange = () => {
            this.Model.ApplyClientFilter();
            this.InitComment();
        }

        public ProjectFilter_OnChange = () => {
            this.Model.ApplyClientFilter();
            this.InitComment();
        }

        public ClearFilters_OnClick = () => {
            setTimeout(() => {
                $("#history-filter").dropdown('restore defaults');
                $("#user-filter").dropdown('restore defaults');
                $("#project-filter").dropdown('restore defaults');
                $('#history-filter').parent().find('.text.default').html($('#history-filter > option[value=""]').html());
                $('#user-filter').parent().find('.text.default').html($('#user-filter > option[value=""]').html());
                $('#project-filter').parent().find('.text.default').html($('#project-filter > option[value=""]').html());
            });
        }

        public FavoriteFilter_OnClick = () => {
            var $this = this;
            $.ajax({
                url: '/api/Account/InvertFavoriteFilter',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $("#favorite-filter").removeClass("favorite");
                    $("#favorite-filter").addClass("spinner loading");
                },
                complete() {
                    $("#favorite-filter").addClass("favorite");
                    $("#favorite-filter").removeClass("spinner loading");
                },
                success: (result) => {
                    if (result.Success === true) {
                        this.Model.FavoriteFilter = result.Data.FavoriteFilter;
                        this.Model.ApplyClientFilter();
                        this.InitComment();
                    } else {
                        $this.Model.Error = result.Message;
                    }
                    $this.scope.$apply();
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public TaskFavorite_OnClick = (task: Models.TaskModel) => {
            var $this = this;
            $.ajax({
                url: '/api/Home/InvertTaskFavorite?id=' + task.EntityId,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $("#favorite-" + task.EntityId).removeClass("favorite");
                    $("#favorite-" + task.EntityId).addClass("spinner loading");
                },
                complete() {
                    $("#favorite-" + task.EntityId).addClass("favorite");
                    $("#favorite-" + task.EntityId).removeClass("spinner loading");
                },
                success: (result) => {
                    if (result.Success === true) {
                        task.Favorite = result.Data.Favorite;
                        if (result.Data.Favorite == false) {
                            // unfavorite subtasks
                            for (var i in task.SubTasks) {
                                var subtask = task.SubTasks[i];
                                task.SubTasks[i].Favorite = false;
                            }
                        }
                        this.Model.ApplyClientFilter();
                        this.InitComment();
                    } else {
                        $this.Model.Error = result.Message;
                    }
                    $this.scope.$apply();
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public SubTaskFavorite_OnClick = (task: Models.TaskModel, subtask: Models.SubTaskModel) => {
            var $this = this;
            $.ajax({
                url: '/api/Home/InvertSubTaskFavorite?taskId=' + subtask.TaskId + "&subtaskId=" + subtask.EntityId,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $("#favorite-" + subtask.EntityId).removeClass("favorite");
                    $("#favorite-" + subtask.EntityId).addClass("spinner loading");
                },
                complete() {
                    $("#favorite-" + subtask.EntityId).addClass("favorite");
                    $("#favorite-" + subtask.EntityId).removeClass("spinner loading");
                },
                success: (result) => {
                    if (result.Success === true) {
                        subtask.Favorite = result.Data.Favorite;
                        if (result.Data.Favorite == true) {
                            // favorite task
                            task.Favorite = true;
                        }
                        this.Model.ApplyClientFilter();
                        this.InitComment();
                    } else {
                        $this.Model.Error = result.Message;
                    }
                    $this.scope.$apply();
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public SortBy_OnClick = () => {
            switch (this.Model.SortBy) {
                case Enums.SortByEnum.UrgencyDesc:
                    this.Model.SortBy = Enums.SortByEnum.TaskIdDesc;
                    break;
                case Enums.SortByEnum.TaskIdDesc:
                    this.Model.SortBy = Enums.SortByEnum.NewsDesc;
                    break;
                case Enums.SortByEnum.NewsDesc:
                    this.Model.SortBy = Enums.SortByEnum.UrgencyDesc;
                    break;
                default:
                    this.Model.SortBy = Enums.SortByEnum.UrgencyDesc;
                    break;
            }
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
                    if (result.Success === false) {
                        $this.Model.Error = result.Message;
                        $this.scope.$apply();
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
            // sets default responsible
            task.UserIds = this.Model.LastResponsibleIds;
            // sets default favorite
            task.Favorite = this.Model.LastFavorite;
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
            // sets default favorite
            task.Favorite = this.Model.LastFavorite;
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
            if (task.Comments.length > 0) {
                comment.Progress = task.Progress;
                comment.Status = task.Status;
            }
            this.Model.EditComment = comment;
            this.InitCommentModal();
        }
        public AddSubTaskComment_OnClick = (subtask: Models.SubTaskModel) => {
            var comment = new Models.CommentModel(null);
            comment.SubTaskId = subtask.EntityId;
            if (subtask.Comments.length > 0) {
                comment.Progress = subtask.Progress;
                comment.Status = subtask.Status;
            }
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetSubTasks(result.Data.SubTasks);
                        $this.InitComment();
                    } else {
                        $this.Error(result.Message);
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetSubTasks(result.Data.SubTasks);
                        $this.InitComment();
                    } else {
                        $this.Error(result.Message);
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
                data: JSON.stringify($this.Model.EditTask),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.EditTask.EntityId = result.Data.EntityId;
                        $this.Task_UploadFiles($this.Model.EditTask,
                            () => {
                                $this.Model.LastResponsibleIds = this.Model.EditTask.UserIds;
                                $this.Model.LastFavorite = this.Model.EditTask.Favorite;
                                $this.Model.EditTask = null;
                                $this.Load();
                            }
                        );
                    } else {
                        $this.HideBusySaving();
                        $this.Model.EditTask.Error = result.Message;
                        $this.scope.$apply();
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

        public TaskDelete_OnClick = (confirmed: boolean = false) => {
            var $this = this;
            // a confirm modal
            if (this.Model.EditTask.EntityId != null && confirmed == false) {
                $('#confirm-modal>.content>p').html("Please to confirm to delete the task");
                $('#confirm-modal')
                    .modal({
                        allowMultiple: true,
                        closable: false,
                        onApprove: function () {
                            $this.TaskDelete_OnClick(true);
                            $this.scope.$apply();
                        }
                    }).modal('show');
                return;
            }

            this.Model.EditTask.Error = null;
            $.ajax({
                url: '/api/Home/DeleteTask?id=' + this.Model.EditTask.EntityId,
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
                        $this.Model.EditTask = null;
                        $this.Load();
                    } else {
                        $this.Model.EditTask.Error = result.Message;
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
                success: (result) => {
                    if (result.Success) {
                        $this.Model.EditSubTask.EntityId = result.Data.EntityId;
                        $this.Task_UploadFiles($this.Model.EditSubTask,
                            () => {
                                $this.Model.LastFavorite = this.Model.EditSubTask.Favorite;
                                $this.Model.EditSubTask = null;
                                $this.Load();
                            }
                        );
                    } else {
                        $this.HideBusySaving();
                        $this.Model.EditSubTask.Error = result.Message;
                        $this.scope.$apply();
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

        public SubTaskDelete_OnClick = (confirmed: boolean = false) => {
            var $this = this;
            // a confirm modal
            if (this.Model.EditSubTask.EntityId != null && confirmed == false) {
                $('#confirm-modal>.content>p').html("Please to confirm to delete the task");
                $('#confirm-modal')
                    .modal({
                        allowMultiple: true,
                        closable: false,
                        onApprove: function () {
                            $this.SubTaskDelete_OnClick(true);
                            $this.scope.$apply();
                        }
                    }).modal('show');
                return;
            }

            this.Model.EditSubTask.Error = null;
            $.ajax({
                url: '/api/Home/DeleteSubTask?id=' + this.Model.EditSubTask.EntityId,
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
                        $this.Model.EditSubTask = null;
                        $this.Load();
                    } else {
                        $this.Model.EditSubTask.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public CommentOk_OnClick = (confirmed: boolean = false) => {
            this.Model.EditComment.Error = null;
            if (!super.ValidateForm(form3)) {
                $("#edit-comment-modal").modal("refresh");
                return;
            }

            var $this = this;
            // a confirm modal
            if (this.Model.EditComment.EntityId != null && confirmed == false) {
                $('#confirm-modal>.content>p').html("Please to confirm to update the comment");
                $('#confirm-modal')
                    .modal({
                        allowMultiple: true,
                        closable: false,
                        onApprove: () => {
                            $this.CommentOk_OnClick(true);
                            $this.scope.$apply();
                        }
                    }).modal('show');
                return;
            }
            $.ajax({
                url: '/api/Home/SaveComment',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditComment),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.EditComment.EntityId = result.Data.EntityId;
                        $this.Task_UploadFiles($this.Model.EditComment,
                            () => {
                                $this.Model.EditComment = null;
                                $this.Load();
                            }
                        );
                    } else {
                        $this.HideBusySaving();
                        $this.Model.EditComment.Error = result.Message;
                        $this.scope.$apply();
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

        public CommentDelete_OnClick = (confirmed: boolean = false) => {
            var $this = this;
            // a confirm modal
            if (this.Model.EditComment.EntityId != null && confirmed == false) {
                $('#confirm-modal>.content>p').html("Please to confirm to delete the comment");
                $('#confirm-modal')
                    .modal({
                        allowMultiple: true,
                        closable: false,
                        onApprove: function () {
                            $this.CommentDelete_OnClick(true);
                            $this.scope.$apply();
                        }
                    }).modal('show');
                return;
            }

            this.Model.EditComment.Error = null;
            $.ajax({
                url: '/api/Home/DeleteComment?id=' + this.Model.EditComment.EntityId,
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
                        $this.Model.EditComment = null;
                        $this.Load();
                    } else {
                        $this.Model.EditComment.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public CommentStatus_OnChange = () => {
            if (this.Model.EditComment.Status == Enums.TaskStatusEnum.Done) {
                this.Model.EditComment.Progress = 1;
            }
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

        // ** FILES ** //

        public Task_UploadFiles(model: Models.BaseModel, actionCompleted: any) {
            var $this = this;
            var fileModels = <Array<Models.FileModel>>(<any>model).Files;
            // select new files
            var upload = new Array();
            var elem = $("#task-files-field");
            var i = 0;
            while (elem.has((<any>String).format("#task-file{0}", i)).length) {
                var files = (<any>document.getElementById('task-file' + i)).files;
                // by files
                for (var j in files) {
                    var file = files[j];
                    if (file.constructor.name === 'File') {
                        for (var k in fileModels) {
                            var fileModel = fileModels[k];
                            if (fileModel.FileName == file.name) {
                                upload.push(file);
                            }
                        }
                    }
                }
                i++;
            }
            // uploads new files
            var data = new FormData();
            for (var j in fileModels) {
                var fileModel = fileModels[j];
                var flag: boolean = false;
                for (var i = 0; i < upload.length; i++) {
                    if (upload[i].name == fileModel.FileName) {
                        data.append(upload[i].name, upload[i]);
                        flag = true;
                        break;
                    }
                }
                // a existing file
                if (flag == false) {
                    data.append(fileModel.FileName, "");
                }
            }
            $.ajax({
                url: '/api/File/Attach',
                type: 'POST',
                processData: false,
                contentType: false,
                data: data,
                dataType: 'json',
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                    xhr.setRequestHeader("EntityId", model.EntityId);
                },
                complete() {
                    $this.HideBusySaving();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        actionCompleted();
                    } else {
                        model.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public AttachFiles_OnClick = (files: Array<Models.FileModel>) => {
            // gets field
            var elem = $("#task-files-field");
            var i = 0;
            // adds new a input files
            while (elem.has((<any>String).format("#task-file{0}", i)).length) {
                i++;
            }

            var action = (arg) => {
                for (var i in arg.target.files) {
                    var file = arg.target.files[i];
                    if (file.size != 0) {
                        if (file.constructor.name === 'File') {
                            // cheks by exists
                            var break1 = false;
                            for (var j in files) {
                                if (files[j].FileName == file.name) {
                                    toastr.warning((<any>String).format("file '{0}' already exists", file.name));
                                    break1 = true;
                                    break;
                                }
                            }
                            if (break1) continue;
                            var model = new Models.FileModel(null);
                            model.FileName = file.name;
                            model.Size = file.size;
                            files.push(model);
                        }
                    } else {
                        toastr.error("can not upload an empty file");
                    }
                }
                this.scope.$apply();
            }

            elem.append($('<input multiple/>').attr('type', 'file').attr('id', 'task-file' + i).on("change", action).hide().click());
        }

        public RemoveFile_OnClick = (files: Array<Models.FileModel>, file: Models.FileModel) => {
            for (var i in files) {
                if (files[i] == file) {
                    files.splice(<any>i, 1);
                }
            }
        }
    }
}