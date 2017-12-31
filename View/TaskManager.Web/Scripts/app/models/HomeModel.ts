namespace Models {
    export class HomeModel extends ModelBase {
        public Users: Array<UserModel>;
        public SelectedUsers: Array<UserModel>;
        public Tasks: Array<TaskModel>;
        public EditTask: TaskModel;

        constructor() {
            super();

            this.EditTask = null;
        }

        public SetData(data: any) {
            this.Users = new Array();
            this.Tasks = new Array();
            for (var i = 0; i < data.Tasks.length; i++) {
                this.Tasks.push(new TaskModel(data.Tasks[i]));
            }
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new UserModel(data.Users[i]));
            }
        }
    }
}