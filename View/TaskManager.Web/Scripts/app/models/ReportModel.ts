namespace Models {
    export class ReportModel extends ModelBase {
        public Loaded: boolean = false;
        public Projects: Array<ProjectModel>;
        public Start: moment.Moment = null;
        public End: moment.Moment = null;
        public ShowStatus: boolean = true;
        public ReportProjects: Array<ReportProjectModel> = null;
        public SumActualWork: string;
        public SelectedProjectsFilter: Array<string> = Array();

        //extra
        private _sumActualWork: any; // uses string in modal or number in tempate
        public get SumActualWorkHours(): any {
            return this._sumActualWork;
        }
        public set SumActualWorkHours(str: any) {
            this._sumActualWork = str;
            var value = parseFloat(str);
            this.SumActualWork = moment.duration(!isNaN(value) ? value : 0, "hours").format("d.hh:mm:ss", <any>{ trim: false });
        }

        constructor() {
            super();
        }

        public SetData(data: any) {
            this.Loaded = true;
            this.Projects = new Array();
            for (var i = 0; i < data.Projects.length; i++) {
                this.Projects.push(new ProjectModel(data.Projects[i]));
            }
        }

        public SetData2(data: any) {
            this.ReportProjects = new Array();
            for (var i = 0; i < data.ReportProjects.length; i++) {
                this.ReportProjects.push(new ReportProjectModel(data.ReportProjects[i]));
            }
            this.SumActualWorkHours = moment.duration(data.SumActualWork).asHours().toFixed(0);
        }
    }
}