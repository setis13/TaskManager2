namespace Models {
    export class UserModel extends ModelBase {
        public Id: string;
        public UserName: string;
        public Email: string;

        constructor(data: any) {
            super();
            this.Id = data.Id;
            this.UserName = data.UserName;
            this.Email = data.Email;
        }
    }
}