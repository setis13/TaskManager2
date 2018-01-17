namespace Models {
    export class TaskModel extends BaseModel {
        public CompanyId: string;
        public ProjectId: string;
        public Index: number;
        public Title: string;
        public Description: string;
        public Priority: Enums.TaskPriorityEnum = 0;
        public ActualWork: string;
        public TotalWork: string;
        public Progress: number;
        public Status: Enums.TaskStatusEnum;
        public SubTasks: Array<SubTaskModel>;
        public UserIds: Array<string>;
        public Comments: Array<CommentModel>;

        //extra
        private _totalWork: any; // uses string in modal or number in tempate
        public get TotalWorkHours(): any {
            return this._totalWork;
        }
        public set TotalWorkHours(str: any) {
            this._totalWork = str;
            var value = parseFloat(str);
            this.TotalWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
        }
        //extra
        private _actualWork: any; // uses string in modal or number in tempate
        public get ActualWorkHours(): any {
            return this._actualWork;
        }
        public set ActualWorkHours(str: any) {
            this._actualWork = str;
            var value = parseFloat(str);
            this.ActualWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
        }

        constructor(data: any) {
            super(data);

            this.SubTasks = new Array();
            this.Comments = new Array();
            this.UserIds = new Array();
            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.ProjectId = data.ProjectId;
                this.Index = data.Index;
                this.Title = data.Title;
                this.Description = data.Description !== null ? data.Description : '';
                this.Priority = data.Priority;
                this.ActualWorkHours = moment.duration(data.ActualWork).asHours();
                this.TotalWorkHours = moment.duration(data.TotalWork).asHours();
                this.Progress = data.Progress;
                this.Status = data.Status;
                for (var i = 0; i < data.SubTasks.length; i++) {
                    this.SubTasks.push(new SubTaskModel(data.SubTasks[i]));
                }
                for (var i = 0; i < data.Comments.length; i++) {
                    var comment = new CommentModel(data.Comments[i]);
                    comment.Visible = i > data.Comments.length - Controllers.HomeController.MIN_COMMENTS - 1;
                    this.Comments.push(comment);
                }
                for (var i = 0; i < data.UserIds.length; i++) {
                    this.UserIds.push(data.UserIds[i]);
                }
            }
        }

        public Clone(): TaskModel {
            var clone = new TaskModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.CompanyId = this.CompanyId;
            clone.ProjectId = this.ProjectId;
            clone.Index = this.Index;
            clone.Title = this.Title;
            clone.Description = this.Description;
            clone.Priority = this.Priority;
            clone.ActualWorkHours = this.ActualWorkHours;
            clone.TotalWorkHours = this.TotalWorkHours;
            clone.Progress = this.Progress;
            clone.Status = this.Status;

            // skips subtasks for saving only the task
            //for (var i = 0; i < this.SubTasks.length; i++) {
            //    clone.SubTasks.push(this.SubTasks[i].Clone());
            //}
            for (var i = 0; i < this.UserIds.length; i++) {
                clone.UserIds.push(this.UserIds[i]);
            }

            return clone;
        }
    }
}