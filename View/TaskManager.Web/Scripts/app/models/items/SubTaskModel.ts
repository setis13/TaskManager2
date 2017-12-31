namespace Models {
    export class SubTaskModel extends BaseModel {
        public CompanyId: string;
        public TaskId: string;
        public Order: number;
        public Title: string;
        public Description: string;
        public ActualWork: moment.Duration;
        public TotalWork: moment.Duration;
        public Progress: number;
        public Status: Enums.TaskStatusEnum;
        //public Comments: Array<CommentModel>;

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.Order = data.Order;
                this.Title = data.Title;
                this.Description = data.Description;
                this.ActualWork = moment.duration(data.ActualWork);
                this.TotalWork = moment.duration(data.TotalWork);
                this.Progress = data.Progress;
                this.Status = data.Status;
                //this.Comments = new Array();
                //for (var i = 0; i < data.Comments.length; i++) {
                //    this.Comments.push(new CommentsModel(data.subtasks[i]));
                //}
            }
        }

        public Clone(): SubTaskModel {
            var clone = new SubTaskModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.CompanyId = this.CompanyId;
            clone.TaskId = this.TaskId;
            clone.Order = this.Order;
            clone.Title = this.Title;
            clone.Description = this.Description;
            clone.ActualWork = this.ActualWork;
            clone.TotalWork = this.TotalWork;
            clone.Progress = this.Progress;
            clone.Status = this.Status;

            return clone;
        }
    }
}