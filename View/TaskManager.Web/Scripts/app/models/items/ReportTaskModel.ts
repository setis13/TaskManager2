namespace Models {
    export class ReportTaskModel extends TaskModel {
        public ReportSubTasks: Array<ReportSubTaskModel> = new Array();
        public ReportComments: Array<ReportCommentModel> = new Array();

        constructor(data: any) {
            super(data);

            for (var i = 0; i < data.ReportSubTasks.length; i++) {
                this.ReportSubTasks.push(new ReportSubTaskModel(data.ReportSubTasks[i]));
            }

            for (var i = 0; i < data.ReportComments.length; i++) {
                this.ReportComments.push(new ReportCommentModel(data.ReportComments[i]));
            }
        }
    }
}