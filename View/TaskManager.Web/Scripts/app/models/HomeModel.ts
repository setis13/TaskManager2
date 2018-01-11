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
            for (var i = 0; i < data.Users.length; i++) {
                this.Users.push(new UserModel(data.Users[i]));
            }
            for (var i = 0; i < data.Projects.length; i++) {
                this.Projects.push(new ProjectModel(data.Projects[i]));
            }
            for (var i = 0; i < data.Tasks.length; i++) {
                this.Tasks.push(new TaskModel(data.Tasks[i]));
            }
        }

        public ProjectName(projectId: string): string {
            return Enumerable.From(this.Projects)
                .Where(e => e.EntityId === projectId)
                .Select(e => e.Title)
                .FirstOrDefault(projectId.substr(0, 8));
        }

        public UserNames(userIds: Array<string>): string {
            return Enumerable.From(this.Users)
                .Where(e => userIds.indexOf(e.Id) !== -1)
                .Select(e => e.UserName).ToArray().join(", ");
        }

        public UserName(userId: string): string {
            return Enumerable.From(this.Users)
                .Where(e => e.Id === userId)
                .Select(e => e.UserName)
                .FirstOrDefault(userId.substr(0, 8));
        }
    }
}