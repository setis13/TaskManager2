//app.factory('Project', ['$http', function ($http) {
//    function Project(ProjectData) {
//        if (ProjectData) {
//            this.setData(ProjectData);
//        }
//    };
//    Project.prototype = {
//        setData: function (ProjectData) {
//            angular.extend(this, ProjectData);
//        },

//        create: function () {
//            return { EntityId: '00000000-0000-0000-0000-000000000000', Name: 'New Name' }
//        },

//        query: function () {
//            console.log("query");
//        },


//        load: function (id) {
//            var scope = this;
//            $http.get('Projects/Load/' + projectId).success(function (ProjectData) {
//                scope.setData(ProjectData);
//            });
//        },
//        delete: function () {
//            $http.delete('Projects/Delete/' + projectId);
//        },
//        save: function () {
//            $http.put('Projects/Save/' + projectId, this);
//        }
//    };
//    return Project;
//}]);