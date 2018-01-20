namespace Models {
    export class Company1Model extends ModelBase {
        public Company: CompanyModel;
        public InvitedCompany: CompanyModel;
        public Users: Array<UserModel>;
        public InvitationUsers: Array<UserModel>;

        constructor() {
            super();
        }

        public SetData(data: any) {
            this.Company = data.Company != null ? new CompanyModel(data.Company) : null;
            this.InvitedCompany = data.InvitedCompany != null ? new CompanyModel(data.InvitedCompany) : null;
            this.Users = new Array();
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new UserModel(data.Users[i]));
            }
            this.InvitationUsers = new Array();
            for (var i = 0; i < data.InvitationUsers.length; i++) {
                this.InvitationUsers.push(new UserModel(data.InvitationUsers[i]));
            }
        }
    }
}