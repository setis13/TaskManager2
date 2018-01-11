namespace Models {
    export class BaseModel extends ModelBase {

        public EntityId: string;
        public CreatedById: string;
        public CreatedDate: moment.Moment;

        constructor(data: any) {
            super();
            if (data != null) {
                this.EntityId = data.EntityId;
                this.CreatedById = data.CreatedById;
                this.CreatedDate = moment(data.CreatedDate);
            } else {
                this.CreatedDate = moment();
            }
        }
    }
}