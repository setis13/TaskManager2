namespace Models {
    export class MenuModel extends ModelBase {
        public Loaded: boolean = false;
        public Alarms: Array<AlarmModel>;
        public Title: number = null;

        constructor() {
            super();
        }

        public SetData(data: any) {
            this.Loaded = true;
            this.Alarms = new Array();
            for (var i = 0; i < data.Alarms.length; i++) {
                var model = new AlarmModel(data.Alarms[i]);
                this.Alarms.push(model);
                if (this.Title == null) {
                    this.Title = model.DateMoment.diff(moment(moment().format('YYYY-MM-DD') + "T00:00:00"), 'days');
                    continue;
                }
            }
        }
    }
}