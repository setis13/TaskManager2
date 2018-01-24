namespace Models {
    export class ReportSingleModel extends ModelBase {
        public Date: moment.Moment;
        public ReportProjects: Array<ReportProjectModel>;

        constructor() {
            super();

            this.Date = moment();
        }

        public SetData(data: any) {
            this.ReportProjects = new Array();
            for (var i = 0; i < data.ReportProjects.length; i++) {
                this.ReportProjects.push(new ReportProjectModel(data.ReportProjects[i]));
            }
        }
    }
}