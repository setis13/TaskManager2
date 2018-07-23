namespace Models {
    export class ProjectModel extends BaseModel {
        public Title: string;
        public Count: number;

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.Title = data.Title;
                this.Count = data.Count;
            }
        }

        public Clone(): ProjectModel {
            var clone = new ProjectModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.Title = this.Title;
            clone.Count = this.Count;

            return clone;
        }
    }
}