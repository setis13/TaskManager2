namespace Models {
    export class TaskModel extends BaseModel {
        public CompanyId: string;
        public ProjectId: string;
        public Index: number;
        public Title: string;
        public Description: string;
        public Priority: Enums.TaskPriorityEnum = 0;
        public ActualWork: moment.Duration;
        public TotalWork: moment.Duration;
        public Progress: number;
        public Status: Enums.TaskStatusEnum;
        public SubTasks: Array<SubTaskModel>;
        //public Comments: Array<CommentModel>;

        constructor(data: any) {
            super(data);

            this.SubTasks = new Array();
            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.ProjectId = data.ProjectId;
                this.Index = data.Index;
                this.Title = data.Title;
                this.Description = data.Description;
                this.Priority = data.Priority;
                this.ActualWork = moment.duration(data.ActualWork);
                this.TotalWork = moment.duration(data.TotalWork);
                this.Progress = data.Progress;
                this.Status = data.Status;
                for (var i = 0; i < data.SubTasks.length; i++) {
                    this.SubTasks.push(new SubTaskModel(data.SubTasks[i]));
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

            return clone;
        }
    }
}