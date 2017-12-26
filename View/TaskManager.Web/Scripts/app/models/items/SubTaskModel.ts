namespace Models {
    export class SubTaskModel extends ModelBase {
        public CompanyId: string;
        public TaskId: string;
        public Order: number;
        public Title: string;
        public Description: string;
        public ActualWork: moment.Duration;
        public TotalWork: moment.Duration;
        public Progress: number;
        public Status: Enums.TaskStatusEnum;
    }
}