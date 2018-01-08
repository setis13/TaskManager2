namespace Models {
    export class ProjectModel extends BaseModel {
        public Name: string;

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.Name = data.Name;
            }
        }

        public Clone(): ProjectModel {
            var clone = new ProjectModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.Name = this.Name;

            return clone;
        }
    }
}