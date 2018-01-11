namespace Models {
    export class HomeModel extends ModelBase {
        public Users: Array<UserModel>;
        public SelectedUsers: Array<UserModel>;
        public Projects: Array<ProjectModel>;
        public Tasks: Array<TaskModel>;
        public EditTask: TaskModel;
        public EditSubTask: SubTaskModel;

        constructor() {
            super();

            this.EditTask = null;
            this.EditSubTask = null;
        }

        public SetData(data: any) {
            this.Users = new Array();
            this.Projects = new Array();
            this.Tasks = new Array();
            for (var i = 0; i < data.Projects.length; i++) {
                this.Projects.push(new ProjectModel(data.Projects[i]));
            }
            for (var i = 0; i < data.Tasks.length; i++) {
                this.Tasks.push(new TaskModel(data.Tasks[i]));
            }
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new UserModel(data.Users[i]));
            }
        }

        public ProjectName(projectId: string) {
            if (projectId != null) {
                return Enumerable.From(this.Projects)
                    .Where(e => e.EntityId === projectId)
                    .Select(e => e.Title)
                    .FirstOrDefault(projectId.substr(0, 8));
            } else {
                return "";
            }
        }
    }
}