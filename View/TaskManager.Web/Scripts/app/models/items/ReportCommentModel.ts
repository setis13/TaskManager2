namespace Models {
    export class ReportCommentModel extends CommentModel {

        public DeltaProgress: number;

        constructor(data: any) {
            super(data);

            this.DeltaProgress = data.DeltaProgress;
        }
    }
}