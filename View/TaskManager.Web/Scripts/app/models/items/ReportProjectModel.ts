namespace Models {
    export class ReportProjectModel extends ProjectModel {
        public ReportTasks: Array<ReportTaskModel> = new Array();

        constructor(data: any) {
            super(data);

            for (var i = 0; i < data.ReportTasks.length; i++) {
                this.ReportTasks.push(new ReportTaskModel(data.ReportTasks[i]));
            }
        }
    }
}