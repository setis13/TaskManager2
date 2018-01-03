namespace Models {
    export class AccountModel extends ModelBase {
        public User: UserModel;

        constructor() {
            super();
        }

        public SetData(data: any) {
            this.User = new UserModel(data);
        }
    }
}