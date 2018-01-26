namespace Models {
    export class ReportProjectModel extends ProjectModel {
        public ReportTasks: Array<ReportTaskModel> = new Array();

        public SumActualWork: string;

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

        constructor(data: any) {
            super(data);

            this.SumActualWorkHours = moment.duration(data.SumActualWork).asHours().toFixed(0);

            for (var i = 0; i < data.ReportTasks.length; i++) {
                this.ReportTasks.push(new ReportTaskModel(data.ReportTasks[i]));
            }
        }
    }
}