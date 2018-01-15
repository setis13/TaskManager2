namespace Models {
    export class CommentModel extends BaseModel {
        public CompanyId: string;
        public TaskId: string;
        public SubTaskId: string;
        public Date: moment.Moment;
        public Status: Enums.TaskStatusEnum;
        public Description: string;
        public ActualWork: string;
        public Progress: number;

        private _actualWork: string;
        public get ActualWorkHours(): string {
            return this._actualWork;
        }
        public set ActualWorkHours(str: string) {
            this._actualWork = str;
            var value = parseFloat(str);
            this.ActualWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
        }

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.SubTaskId = data.SubTaskId;
                this.Date = moment(data.Date);
                this.Status = data.Status;
                this.Description = data.Description;
                this._actualWork = moment.duration(data.ActualWork).asHours().toFixed(1);
                this.Progress = data.Progress;
            } else {
                this.CreatedDate = moment(null);
            }
        }

        public Clone(): CommentModel {
            var clone = new CommentModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.CompanyId = this.CompanyId;
            clone.TaskId = this.TaskId;
            clone.SubTaskId = this.SubTaskId;
            clone.Date = this.Date.clone();
            clone.Status = this.Status;
            clone.Description = this.Description;
            clone._actualWork = this._actualWork;
            clone.Progress = this.Progress;

            return clone;
        }
    }
}