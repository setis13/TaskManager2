namespace Models {
    export class ReportSubTaskModel extends SubTaskModel {
        public ReportComments: Array<ReportCommentModel> = new Array();

        constructor(data: any) {
            super(data);

            for (var i = 0; i < data.ReportComments.length; i++) {
                this.ReportComments.push(new ReportCommentModel(data.ReportComments[i]));
            }
        }
    }
}