namespace Models {
    export class Company1Model extends ModelBase {
        public Company: CompanyModel;
        public Users: Array<UserModel>;
        public InvitedUsers: Array<UserModel>;

        public EditCompany: CompanyModel;
        public FindUser: UserModel;

        constructor() {
            super();
        }

        public SetData(data: any) {
            this.Company = data.Company != null ? new CompanyModel(data.Company) : null;
            this.Users = new Array();
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new UserModel(data.Users[i]));
            }
            this.InvitedUsers = new Array();
            for (var i = 0; i < data.InvitedUsers.length; i++) {
                this.InvitedUsers.push(new UserModel(data.InvitedUsers[i]));
            }
        }
    }
}