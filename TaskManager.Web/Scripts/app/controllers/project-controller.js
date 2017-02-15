angular.module('TaskManagerApp.controllers', []).controller('ProjectController', function ($scope, $compile, $element, Project) {

    $scope.SubmitInProgress = false;

    $scope.Project = null;

    $scope.DataTable = null;

    $scope.Init = function () {
        var dt = $('#dtProjects').on('processing.dt', function (e, settings, processing) {
            $('#processingIndicator').css('display', processing ? 'block' : 'none');
        }).dataTable({
            "sort": false,
            "paging": false,
            "ordering": false,
            "searching": false,
            "serverSide": false,
            "ajax": {
                "dataType": 'json',
                "url": '/api/Project/GetAll',
                "type": "GET"
            },
            "columns": [
                {
                    "data": "EntityId",
                    "render": function (data, type, row) {
                        return "<span id='" + data + "'>" + data.substr(0, 8) + "</span>";
                    }
                },
                {
                    "data": "CreatedDate",
                    "render": function (data, type, row) {
                        return localFormat(new Date(data));
                    }
                },
                {
                    "data": "LastModifiedDate",
                    "render": function (data, type, row) {
                        return localFormat(new Date(data));
                    }
                },
                { "data": "Name" },
                {
                    "data": "EntityId",
                    "orderable": false,
                    "render": function (data, type, row) {
                        return '<a href="#" data-toggle="modal" data-target="#dialogEditProject" ng-click="Edit(\'' + data + '\')">Edit</a>';
                    }
                }
            ],
            "fnRowCallback": function (nRow) {
                $compile(nRow)($scope);
            }
        });
        $scope.DataTable = dt.DataTable();
    };

    $scope.Add = function () {
        $scope.Project = { EntityId: '00000000-0000-0000-0000-000000000000', Name: 'New Name' }
    }

    $scope.Edit = function (projectId) {
        $scope.SubmitInProgress = true;

        $.ajax({
            url: '/api/Project/GetById/' + projectId,
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            success: (result) => {
                $scope.SubmitInProgress = false;
                $scope.$apply();
                if (result.success) {
                    $scope.Project = result.data;
                    $scope.$apply();
                } else {
                    toastr.error(result.error, "Loading Project");
                }
            },
            error: (jqXhr) => {
                $scope.SubmitInProgress = false;
                $scope.$apply();
                toastr.error(jqXhr.statusText, "Loading Project");
            }
        });
    }

    $scope.Delete = function (projectId) {
        $scope.SubmitInProgress = true;
        $.ajax({
            url: '/api/Project/Delete/' + projectId,
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            success: (result) => {
                $scope.SubmitInProgress = false;
                $scope.$apply();
                if (result.success) {
                    $scope.Project = null;
                    $scope.$apply();
                    $scope.DataTable.ajax.reload();
                    $("#dialogEditProject").modal('hide');
                    toastr.success(result.message, "Delete Project");
                } else {
                    toastr.error(result.error, "Delete Project");
                }
            },
            error: (jqXhr) => {
                $scope.SubmitInProgress = false;
                $scope.$apply();
                toastr.error(jqXhr.statusText, "Delete Project");
            }
        });
    }

    $scope.Submit = function (project) {
        if (!$scope.projectForm.$invalid) {
            $scope.SubmitInProgress = true;

            $.ajax({
                url: '/api/Project/Save',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(project),
                success: (result) => {
                    $scope.SubmitInProgress = false;
                    $scope.$apply();
                    if (result.success) {
                        $scope.Project = null;
                        $scope.$apply();
                        $("#dialogEditProject").modal('hide');
                        toastr.success(result.message, "Save Project");
                        var projects = $scope.DataTable.rows().data();
                        for (var i in projects) {
                            if (projects[i].EntityId === result.data.EntityId) {
                                $scope.DataTable.row(i).data(result.data).draw();
                                break;
                            }
                        }
                    } else {
                        toastr.error(result.error, "Save Project");
                    }
                },
                error: (jqXhr) => {
                    $scope.SubmitInProgress = false;
                    $scope.$apply();
                    toastr.error(jqXhr.statusText, "Save Project");
                }
            });
        }
    }
})