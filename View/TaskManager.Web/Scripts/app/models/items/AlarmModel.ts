namespace Models {
    export class AlarmModel extends BaseModel {
        public Title: string;
        public Description: string;
        public Date: string;

        public RepeatType: Enums.AlarmRepeatEnum;
        public RepeatValue: number;
        public Birthday: boolean;

        // extra
        private _dateMoment: moment.Moment;
        public get DateMoment(): moment.Moment {
            return this._dateMoment;
        }
        public set DateMoment(m: moment.Moment) {
            this._dateMoment = m;
            this.Date = this._dateMoment.format("YYYY-MM-DD");
        }
        
        constructor(data: any) {
            super(data);

            if (data != null) {
                this.Title = data.Title;
                this.Description = data.Description !== null ? data.Description : '';
                this.RepeatType = data.RepeatType;
                this.RepeatValue = data.RepeatValue;
                this.Birthday = data.Birthday;
                this.DateMoment = moment(data.Date);
            } else {
                this.Title = '';
                this.Description = '';
                this.RepeatType = null;
                this.RepeatValue = null;
                this.Birthday = false;
                this.DateMoment = moment();
            }
        }

        public Clone(): AlarmModel {
            var clone = new AlarmModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.Title = this.Title;
            clone.Description = this.Description;
            clone.RepeatType = this.RepeatType;
            clone.RepeatValue = this.RepeatValue;
            clone.Birthday = this.Birthday;
            clone.DateMoment = this.DateMoment.clone();

            return clone;
        }
    }
}