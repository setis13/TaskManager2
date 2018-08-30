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
        public Show: boolean;
        public Files: Array<FileModel> = new Array();

        // extra
        public Visible: boolean = true;
        // extra
        private _actualWork: string;
        public get ActualWorkHours(): string {
            return this._actualWork;
        }
        public set ActualWorkHours(str: string) {
            this._actualWork = str;
            if (str != null && str !== '') {
                var value = parseFloat(str);
                this.ActualWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
            } else {
                this.ActualWork = null;
            }
        }
        // extra
        private _dateMoment: moment.Moment;
        public get DateMoment(): moment.Moment {
            return this._dateMoment;
        }
        public set DateMoment(m: moment.Moment) {
            this._dateMoment = m;
            this.Date = this._dateMoment.format("YYYY-MM-DD");
        }
        // extra
        public get ProgressPercent(): number {
            if (this.Progress === null) {
                return null;
            } else {
                return Math.round(this.Progress * 100);
            }
        }
        public set ProgressPercent(p: number) {
            if (p != null && p !== <any>'') {
                this.Progress = p / 100;
            } else {
                this.Progress = null;
            }
        }

        public GetCommentImageFiles(): Array<FileModel> {
            return Enumerable.From(this.Files).Where(e => IsLightGallery(e.FileName)).ToArray();
        }

        public GetCommentOtherFiles(): Array<FileModel> {
            return Enumerable.From(this.Files).Where(e => !IsLightGallery(e.FileName)).ToArray();
        }

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.SubTaskId = data.SubTaskId;
                this.DateMoment = moment(data.Date);
                this.Status = data.Status.toString();
                this.Description = data.Description !== null ? data.Description : '';
                this.ActualWorkHours = data.ActualWork != null ? moment.duration(data.ActualWork).asHours().toFixed(1) : null;
                this.Progress = data.Progress;
                this.Show = data.Show;
                for (var i = 0; data.Files != null && i < data.Files.length; i++) {
                    this.Files.push(new FileModel(data.Files[i]));
                }
            } else {
                this.CompanyId = null;
                this.TaskId = null;
                this.SubTaskId = null;
                this.DateMoment = moment();
                this.Status = Enums.TaskStatusEnum.InProgress; // default status
                this.Description = '';
                this.ActualWorkHours = null;
                this.Progress = null;
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
            clone.Show = this.Show;
            for (var i = 0; i < this.Files.length; i++) {
                clone.Files.push(this.Files[i]);
            }

            return clone;
        }
    }
}