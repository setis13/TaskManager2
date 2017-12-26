namespace Models {
    export class TaskModel extends ModelBase {
        public CompanyId: string;
        public ProjectId: string;
        public Index: number;
        public Title: string;
        public Description: string;
        public Priority: Enums.TaskPriorityEnum;
        public ActualWork: moment.Duration;
        public TotalWork: moment.Duration;
        public Progress: number;
        public Status: Enums.TaskStatusEnum;
    }
}