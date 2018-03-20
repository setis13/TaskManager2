namespace Models {
    export class FileModel extends BaseModel {
        public ParentId: string;
        public FileName: string;
        public Size: number;

        public get DisplayName(): string {
            if (this.FileName.length > 16) {
                var index = this.FileName.lastIndexOf('.');
                if (index != -1) {
                    return this.FileName.substring(0, index < 16 ? index : 16) + '~' + this.FileName.substring(index, this.FileName.length);
                } else {
                    return this.FileName.substring(0, 16) + '~';
                }
            } else {
                return this.FileName;
            }
        }

        constructor(data: any) {
            super(data);

            if (data != null) {
                this.ParentId = data.ParentId;
                this.FileName = data.FileName;
                this.Size = data.Size;
            }
        }

        public Clone(): FileModel {
            var clone = new FileModel(null);

            clone.EntityId = this.EntityId;
            clone.CreatedDate = this.CreatedDate.clone();

            clone.ParentId = this.ParentId;
            clone.FileName = this.FileName;
            clone.Size = this.Size;

            return clone;
        }
    }
}