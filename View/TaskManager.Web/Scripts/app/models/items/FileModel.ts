namespace Models {
    export class FileModel extends BaseModel {
        public ParentId: string;
        public FileName: string;
        public Size: number;

        private NAME_LEN_LIMIT: number = 24;

        public get DisplayName(): string {
            var index = this.FileName.lastIndexOf('.');
            if (index != -1) {
                if (index > this.NAME_LEN_LIMIT) {
                    return this.FileName.substring(0, index < this.NAME_LEN_LIMIT ? index : this.NAME_LEN_LIMIT) + '~' + this.FileName.substring(index, this.FileName.length);
                } else {
                    return this.FileName;
                }
            } else {
                if (this.FileName.length > this.NAME_LEN_LIMIT) {
                    return this.FileName.substring(0, this.NAME_LEN_LIMIT) + '~';
                } else {
                    return this.FileName;
                }
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