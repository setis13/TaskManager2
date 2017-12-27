namespace Models {
    export class AccountUserModel extends ModelBase {
        public Email: string;

        constructor(data: any) {
            super();
            this.Email = data.Email;
        }
    }
}