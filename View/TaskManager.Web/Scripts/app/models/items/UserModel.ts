namespace Models {
    export class UserModel extends ModelBase {
        public Id: string;
        public UserName: string;
        public Email: string;

        constructor(data: any) {
            super();

            if (data != null) {
                this.Id = data.Id;
                this.UserName = data.UserName;
                this.Email = data.Email;
            } else {
                this.Id = null;
                this.UserName = null;
                this.Email = null;
            }
        }
    }
}