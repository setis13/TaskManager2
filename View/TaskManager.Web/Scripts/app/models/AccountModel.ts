namespace Models {
    export class AccountModel extends ModelBase {
        public User: UserModel;
        public ChangePassword: ChangePasswordModel;

        constructor() {
            super();
        }

        public SetData(data: any) {
            this.User = new UserModel(data);
        }
    }

    export class ChangePasswordModel extends ModelBase {
        constructor() {
            super();
        }

        public OldPassword: string;
        public NewPassword: string;
    }
}