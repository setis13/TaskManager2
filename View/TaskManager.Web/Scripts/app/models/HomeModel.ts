namespace Models {
    export class HomeModel extends ModelBase {
        public Tasks: Array<TaskModel>;

        public SetData(data: any) {
            this.Tasks = new Array();
            for (var i = 0; i < data.Tasks.length; i++) {
                this.Tasks.push(new TaskModel(data.Tasks[i]));
            }
        }
    }
}