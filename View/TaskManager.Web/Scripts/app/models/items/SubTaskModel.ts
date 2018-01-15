namespace Models {
    export class SubTaskModel extends BaseModel {
        public CompanyId: string;
        public TaskId: string;
        public Order: number;
        public Title: string;
        public Description: string;
        public ActualWork: string;
        public TotalWork: string;
        public Progress: number;
        public Status: Enums.TaskStatusEnum;
        public Comments: Array<CommentModel>;

        //extra
        private _totalWork: string;
        public get TotalWorkHours(): string {
            return this._totalWork;
        }
        public set TotalWorkHours(str: string) {
            this._totalWork = str;
            var value = parseFloat(str);
            this.TotalWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
        }
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
                this.Order = data.Order;
                this.Title = data.Title;
                this.Description = data.Description;
                this._actualWork = moment.duration(data.ActualWork).asHours().toFixed(1);
                this._totalWork = moment.duration(data.TotalWork).asHours().toFixed(1);
                this.Progress = data.Progress;
                this.Status = data.Status;
                this.Comments = new Array();
                for (var i = 0; i < data.Comments.length; i++) {
                    this.Comments.push(new CommentModel(data.Comments[i]));
                }
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
            clone._actualWork = this._actualWork;
            clone._totalWork = this._totalWork;
            clone.Progress = this.Progress;
            clone.Status = this.Status;

            return clone;
        }
    }
}