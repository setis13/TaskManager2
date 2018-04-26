namespace Models {
    export class BaseModel extends ModelBase {

        public EntityId: string;
        public CreatedById: string;
        public CreatedDate: moment.Moment;
        public LastModifiedById: string;
        public LastModifiedDate: moment.Moment;

        // hides a edit link in older comments
        public CheckByAge(): boolean {
            return moment().diff(this.CreatedDate, "hours") < 12;
        }

        constructor(data: any) {
            super();
            if (data != null) {
                this.EntityId = data.EntityId;
                this.CreatedById = data.CreatedById;
                this.CreatedDate = moment.utc(data.CreatedDate);
                this.LastModifiedById = data.LastModifiedById;
                this.LastModifiedDate = moment.utc(data.LastModifiedDate);
            } else {
                this.CreatedDate = moment();
                this.LastModifiedDate = moment();
            }
        }
    }
}