﻿namespace Models {
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
            this.Date = this._dateMoment.format("YYYY-MM-DD");
        }
        public get ProgressPercent(): number {
            return this.Progress * 100;
        }
        public set ProgressPercent(p: number) {
            this.Progress = p / 100;
        }

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.SubTaskId = data.SubTaskId;
                this.DateMoment = moment(data.Date);
                this.Status = data.Status.toString();
                this.Description = data.Description;
                this.ActualWorkHours = moment.duration(data.ActualWork).asHours().toFixed(1);
                this.Progress = data.Progress;
            } else {
                this.DateMoment = moment(null);
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
            clone.ActualWorkHours = this.ActualWorkHours;
            clone.Progress = this.Progress;

            return clone;
        }
    }
}