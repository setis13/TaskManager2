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
        public Favorite: boolean;
        public Comments: Array<CommentModel> = new Array();
        public Files: Array<FileModel> = new Array();

        //extra
        private _totalWork: any; // uses string in modal or number in tempate
        public get TotalWorkHours(): string {
            return this._totalWork;
        }
        public set TotalWorkHours(str: string) {
            this._totalWork = str;
            if (str != null && str !== '') {
                var value = parseFloat(str);
                this.TotalWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
            } else {
                this.TotalWork = null;
            }
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

        public GetSubTaskImageFiles(): Array<FileModel> {
            return Enumerable.From(this.Files).Where(e => IsLightGallery(e.FileName)).ToArray();
        }

        public GetSubTaskOtherFiles(): Array<FileModel> {
            return Enumerable.From(this.Files).Where(e => !IsLightGallery(e.FileName)).ToArray();
        }

        public GetCommentsImageFiles(): Array<FileModel> {
            return Enumerable.From(this.Comments).SelectMany(e => e.Files).Where(e => IsLightGallery(e.FileName)).ToArray();
        }

        public GetCommentsOtherFiles(): Array<FileModel> {
            return Enumerable.From(this.Comments).SelectMany(e => e.Files).Where(e => !IsLightGallery(e.FileName)).ToArray();
        }

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.CompanyId = data.CompanyId;
                this.TaskId = data.TaskId;
                this.Order = data.Order;
                this.Title = data.Title;
                this.Description = data.Description !== null ? data.Description : '';
                this.ActualWorkHours = moment.duration(data.ActualWork).asHours();
                this.TotalWorkHours = data.TotalWork != null ? moment.duration(data.TotalWork).asHours().toFixed(1) : null;
                this.Progress = data.Progress;
                this.Status = data.Status;
                this.Favorite = data.Favorite;
                for (var i = 0; data.Comments != null && i < data.Comments.length; i++) {
                    var comment = new CommentModel(data.Comments[i]);
                    comment.Visible = i > data.Comments.length - Controllers.HomeController.MIN_COMMENTS - 1;
                    this.Comments.push(comment);
                }
                for (var i = 0; data.Files != null && i < data.Files.length; i++) {
                    this.Files.push(new FileModel(data.Files[i]));
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
            clone.ActualWorkHours = this.ActualWorkHours;
            clone.TotalWorkHours = this.TotalWorkHours;
            clone.Progress = this.Progress;
            clone.Status = this.Status;
            clone.Favorite = this.Favorite;
            for (var i = 0; i < this.Files.length; i++) {
                clone.Files.push(this.Files[i]);
            }

            return clone;
        }
    }
}