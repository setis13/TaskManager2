namespace Models {
    export class BaseModel extends ModelBase {

        public EntityId: string;

        constructor(data: any) {
            super();
            if (data != null) {
                this.EntityId = data.EntityId;
            }
        }
    }
}