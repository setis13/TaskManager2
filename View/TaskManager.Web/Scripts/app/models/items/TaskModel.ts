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
        //public Comments: Array<CommentModel>;
        
        //extra
        public get TotalWorkHours(): string {
            return moment.duration(this.TotalWork).asHours().toFixed(1);
        }
        public set TotalWorkHours(str: string) {
            var value = parseFloat(str);
            this.TotalWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d [days] hh:mm:ss");
        }

        constructor(data: any) {
            super(data);

            this.SubTasks = new Array();
            this.UserIds = new Array();
            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.ProjectId = data.ProjectId;
                this.Index = data.Index;
                this.Title = data.Title;
                this.Description = data.Description;
                this.Priority = data.Priority;
                this.ActualWork = moment.duration(data.ActualWork).format("d [days] hh:mm:ss");
                this.TotalWork = moment.duration(data.TotalWork).format("d [days] hh:mm:ss");
                this.Progress = data.Progress;
                this.Status = data.Status;
                for (var i = 0; i < data.SubTasks.length; i++) {
                    this.SubTasks.push(new SubTaskModel(data.SubTasks[i]));
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
            clone.ActualWork = this.ActualWork;
            clone.TotalWork = this.TotalWork;
            clone.Progress = this.Progress;
            clone.Status = this.Status;

            for (var i = 0; i < this.SubTasks.length; i++) {
                clone.SubTasks.push(this.SubTasks[i].Clone());
            }
            for (var i = 0; i < this.UserIds.length; i++) {
                clone.UserIds.push(this.UserIds[i]);
            }

            return clone;
        }
    }
}