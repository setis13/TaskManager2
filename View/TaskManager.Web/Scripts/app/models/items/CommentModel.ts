namespace Models {
    export class CommentModel extends BaseModel {
        public CompanyId: string;
        public TaskId: string;
        public SubTaskId: string;
        public Date: string;
        public Status: Enums.TaskStatusEnum;
        public ActualWork: string;
        public Progress: number;
        public Description: string;

        private _actualWork: string;
        public get ActualWorkHours(): string {
            return this._actualWork;
        }
        public set ActualWorkHours(str: string) {
            this._actualWork = str;
            var value = parseFloat(str);
            this.ActualWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
        }

        private _dateMoment: moment.Moment;
        public get DateMoment(): moment.Moment {
            return this._dateMoment;
        }
        public set DateMoment(m: moment.Moment) {
            this._dateMoment = m;
            this.Date = this._dateMoment.toString();
            console.log(this.Date);
        }

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.SubTaskId = data.SubTaskId;
                this.DateMoment = moment(data.Date);
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
            clone.DateMoment = this.DateMoment.clone();
            clone.Status = this.Status;
            clone.Description = this.Description;
            clone._actualWork = this._actualWork;
            clone.Progress = this.Progress;

            return clone;
        }
    }
}