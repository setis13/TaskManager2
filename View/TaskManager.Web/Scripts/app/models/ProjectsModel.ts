﻿namespace Models {
    export class ProjectsModel extends ModelBase {
        public Projects: Array<ProjectModel>;
        public EditProject: ProjectModel;

        constructor() {
            super();

            this.EditProject = null;
        }

        public SetData(data: any) {
            this.Projects = new Array();
            for (var i = 0; i < data.Projects.length; i++) {
                this.Projects.push(new ProjectModel(data.Projects[i]));
            }
        }
    }
}