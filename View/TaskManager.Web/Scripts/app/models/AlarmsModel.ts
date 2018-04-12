namespace Models {
    export class AlarmsModel extends ModelBase {
        public Loaded: boolean = false;
        public Alarms: Array<AlarmModel>;
        public EditAlarm: AlarmModel;

        constructor() {
            super();

            this.EditAlarm = null;
        }

        public SetData(data: any) {
            this.Loaded = true;
            this.Alarms = new Array();
            for (var i = 0; i < data.Alarms.length; i++) {
                this.Alarms.push(new AlarmModel(data.Alarms[i]));
            }
        }
    }
}