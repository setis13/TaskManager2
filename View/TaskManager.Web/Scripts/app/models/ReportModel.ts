namespace Models {
    export class ReportModel extends ModelBase {
        public Start: moment.Moment = null;
        public End: moment.Moment = null;
        public ShowStatus: boolean = true;
        public ReportProjects: Array<ReportProjectModel> = null;

        constructor() {
            super();

        }

        public SetData(data: any) {
            this.ReportProjects = new Array();
            for (var i = 0; i < data.ReportProjects.length; i++) {
                this.ReportProjects.push(new ReportProjectModel(data.ReportProjects[i]));
            }
        }
    }
}