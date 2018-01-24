namespace Models {
    export class ProjectModel extends BaseModel {
        public Title: string;

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.Title = data.Title;
            }
        }

        public Clone(): ProjectModel {
            var clone = new ProjectModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.Title = this.Title;

            return clone;
        }
    }
}