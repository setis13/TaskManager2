namespace Models {
    export class CompanyModel extends BaseModel {
        public Name: string;

        constructor(data: any) {
            super(data);

            this.Name = data != null ? data.Name : null;
        }
    }
}