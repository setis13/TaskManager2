namespace Models {
    export class HomeModel extends ModelBase {
        public Loaded: boolean = false;
        public Users: Array<UserModel>;
        public SelectedUsers: Array<UserModel>;
        public Projects: Array<ProjectModel>;
        public Tasks: Array<TaskModel>;
        public EditTask: TaskModel;
        public EditSubTask: SubTaskModel;
        public EditComment: CommentModel;
        public HistoryFilters: { [id: string]: string; };
        public FavoriteFilter: boolean;
        public LastResponsibleIds: Array<string>;
        public LastFavorite: boolean;
        public LastPriority: number;
        public ReportFilter: boolean = false;
        public ShowFilter: boolean = false;
        public AllSubtasksFilter: boolean = false;
        public SelectedHistoryFilter: string = '';
        public SelectedUserFilter: string = '';
        public SelectedProjectFilter: string = '';
        public SortBy: Enums.SortByEnum;
        public Now: moment.Moment = moment();
        public DayHours: number;

        public FilteredTasks: Array<TaskModel>;

        public get TotalTasks() {
            if (this.FilteredTasks != null) {
                return this.FilteredTasks.length;
            } else {
                return "-";
            }
        }
        public get TotalSubtasks() {
            if (this.FilteredTasks != null) {
                return Enumerable.From(this.FilteredTasks).SelectMany(e => e.SubTasks).Count();
            } else {
                return "-";
            }
        }

        constructor() {
            super();

            this.EditTask = null;
            this.EditSubTask = null;
            this.EditComment = null;
        }

        public ApplyClientFilter() {
            var tasks: linq.Enumerable<TaskModel> = Enumerable.From(this.Tasks);
            switch (this.SortBy) {
                case Enums.SortByEnum.TaskId:
                    tasks = tasks.OrderBy(e => e.Index);
                    break;
                case Enums.SortByEnum.TaskIdDesc:
                    tasks = tasks.OrderByDescending(e => e.Index);
                    break;
                case Enums.SortByEnum.Urgency:
                    tasks = tasks.OrderBy(e => e.Priority);
                    break;
                case Enums.SortByEnum.UrgencyDesc:
                    tasks = tasks.OrderByDescending(e => e.Priority);
                    break;
                case Enums.SortByEnum.News:
                    tasks = tasks.OrderBy(t => t.LastModifiedDate.unix());
                    break;
                case Enums.SortByEnum.NewsDesc:
                    tasks = tasks.OrderByDescending(t => t.LastModifiedDate.unix());
                    break;
                default:
                    break;
            }
            // NOTE: moved a filter to html template
            if (this.SelectedUserFilter !== '') {
                tasks = tasks.Where(e => this.FavoriteFilter == false || this.SelectedHistoryFilter != '0' || e.Favorite == this.FavoriteFilter);
            }

            if (this.SelectedUserFilter !== '') {
                tasks = tasks.Where(e => e.UserIds.indexOf(this.SelectedUserFilter) !== -1);
            }

            if (this.SelectedProjectFilter !== '') {
                tasks = tasks.Where(e => e.ProjectId == this.SelectedProjectFilter);
            }

            if (this.FavoriteFilter == true) {
                tasks = tasks.Where(e => e.Favorite);
            }

            this.FilteredTasks = tasks.ToArray();

            if (this.FavoriteFilter == true) {
                for (var i = 0; i < this.FilteredTasks.length; i++) {
                    this.FilteredTasks[i].SubTasks = Enumerable.From(this.FilteredTasks[i].SubTasks).Where(e => e.Favorite).ToArray();
                }
            }

        }

        public SetData(data: any) {
            this.DayHours = 0;
            this.Loaded = true;
            this.SortBy = data.SortBy;
            this.Users = new Array();
            this.Projects = new Array();
            this.Tasks = new Array();
            this.FavoriteFilter = data.FavoriteFilter;
            this.HistoryFilters = {};
            this.LastResponsibleIds = new Array();
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new UserModel(data.Users[i]));
            }
            for (var i = 0; i < data.Projects.length; i++) {
                this.Projects.push(new ProjectModel(data.Projects[i]));
            }
            for (var i = 0; i < data.Tasks.length; i++) {
                this.Tasks.push(new TaskModel(data.Tasks[i]));
            }
            for (var i = 0; i < data.HistoryFilters.length; i++) {
                this.HistoryFilters[moment(data.HistoryFilters[i]).format('MM/DD/YYYY')] =
                    moment(data.HistoryFilters[i]).format('MMM YYYY');
            }
            this.LastResponsibleIds = data.LastResponsibleIds;
            this.LastFavorite = data.LastFavorite;
            this.LastPriority = data.LastPriority;
            this.ApplyClientFilter();

            // calcs day hours
            for (var i = 0; i < this.Tasks.length; i++) {
                var task = this.Tasks[i];
                for (var j = 0; j < task.Comments.length; j++) {
                    var comment = task.Comments[j];
                    if (comment.DateMoment.dayOfYear() == this.Now.dayOfYear()) {
                        this.DayHours += comment.ActualWorkHours;
                    }
                }
                for (var k = 0; k < task.SubTasks.length; k++) {
                    var subtask = task.SubTasks[k];
                    for (var j = 0; j < subtask.Comments.length; j++) {
                        var comment = subtask.Comments[j];
                        if (comment.DateMoment.dayOfYear() == this.Now.dayOfYear()) {
                            this.DayHours += comment.ActualWorkHours;
                        }
                    }
                }
            }
        }

        //private _compareByDate(dt1: moment.Moment, dt2: moment.Moment): boolean {
        //    return dt1.year == dt2.year && dt1.month == dt2.month && dt1.day == dt2.dayOfYearday;
        //}

        public SetSubTasks(subTasks: any) {
            var tmpComments: { [id: string]: Array<CommentModel>; } = {};

            // removes old subtasks
            for (var i = 0; i < subTasks.length; i++) {
                var task = Enumerable.From(this.Tasks).First(e => e.EntityId === subTasks[i].TaskId);
                for (var j = 0; j < task.SubTasks.length; j++) {
                    if (task.SubTasks[j].EntityId === subTasks[i].EntityId) {
                        var removedSubTasks = task.SubTasks.splice(j, 1);
                        // saves comments
                        for (var k = 0; k < removedSubTasks.length; k++) {
                            tmpComments[removedSubTasks[k].EntityId] = removedSubTasks[k].Comments;
                        }
                        break;
                    }
                }
            }
            // adds new subtasks
            for (var i = 0; i < subTasks.length; i++) {
                var task = Enumerable.From(this.Tasks).First(e => e.EntityId === subTasks[i].TaskId);
                var j = 0;
                for (j = 0; j < task.SubTasks.length; j++) {
                    if (task.SubTasks[j].Order > subTasks[i].Order) {
                        break;
                    }
                }
                var newSubTask = new SubTaskModel(subTasks[i]);
                if (tmpComments[newSubTask.EntityId] != undefined) {
                    // restores comments
                    newSubTask.Comments = tmpComments[newSubTask.EntityId];
                }
                task.SubTasks.splice(j, 0, newSubTask);
            }
        }

        public ProjectName(projectId: string): string {
            return Enumerable.From(this.Projects)
                .Where(e => e.EntityId === projectId)
                .Select(e => e.Title)
                .FirstOrDefault(projectId.substr(0, 8));
        }

        public TaskIndexByComment(comment: CommentModel): string {
            if (comment == null) {
                return null;
            }
            var taskId = null;
            var subtaskName = "";
            // takes taskId by subTaskId
            if (comment.SubTaskId != null) {
                var subtask = Enumerable.From(this.Tasks)
                    .SelectMany(e => e.SubTasks)
                    .FirstOrDefault(null, e => e.EntityId === comment.SubTaskId);
                if (subtask != null) {
                    subtaskName = "/" + subtask.Order;
                    taskId = subtask.TaskId;
                } else {
                    return comment.SubTaskId.substr(0, 8);
                }
            }
            // takes taskId
            else if (comment.TaskId != null) {
                taskId = comment.TaskId;
            }

            // gets task title
            if (taskId != null) {
                return Enumerable.From(this.Tasks)
                    .Where(e => e.EntityId === taskId)
                    .Select(e => e.Index.toString())
                    .FirstOrDefault(taskId.substr(0, 8)) + subtaskName;
            } else {
                return "?";
            }
        }

        public UserNames(userIds: Array<string>): string {
            return Enumerable.From(this.Users)
                .Where(e => userIds.indexOf(e.Id) !== -1)
                .Select(e => e.UserName).ToArray().join(", ");
        }

        public UserName(userId: string): string {
            return Enumerable.From(this.Users)
                .Where(e => e.Id === userId)
                .Select(e => e.UserName)
                .FirstOrDefault(userId.substr(0, 8));
        }
    }
}