app.controller('SubprojectController', function ($scope, $compile, $element) {

    $scope.SubmitInProgress = false;

    $scope.Subproject = null;

    $scope.DataTable = null;

    LoadProjects();

    $scope.Init = function () {
        var dt = $('#dtSubprojects').on('processing.dt', function (e, settings, processing) {
            $('#processingIndicator').css('display', processing ? 'block' : 'none');
        }).dataTable({
            "sort": false,
            "paging": false,
            "ordering": false,
            "searching": false,
            "serverSide": false,
            "ajax": {
                "dataType": 'json',
                "url": '/api/Subproject/GetAll',
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
                {
                    "data": "ProjectId",
                    "render": function (data, type, row) {
                        return GetProjectName(data);
                    }
                },
                { "data": "Name" },//title
                { "data": "Hours" },
                {
                    "data": "EntityId",
                    "orderable": false,
                    "render": function (data, type, row) {
                        return '<a href="#" data-toggle="modal" data-target="#dialogEditSubproject" ng-click="Edit(\'' + data + '\')">Edit</a>';
                    }
                }
            ],
            "fnRowCallback": function (nRow) {
                $compile(nRow)($scope);
            }
        });
        $scope.DataTable = dt.DataTable();
    };

    function LoadProjects() {
        $.ajax({
            url: '/api/Project/GetAll',
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            success: (result) => {
                if (result.success) {
                    $scope.Projects = result.data;
                } else {
                    toastr.error(result.error, "Loading Titles");
                }
            },
            error: (jqXhr) => {
                toastr.error(jqXhr.statusText, "Loading Titles");
            }
        });
    }

    function GetProjectName(titleId) {
        for (var i in $scope.Projects) {
            if ($scope.Projects[i].EntityId === titleId) {
                return $scope.Projects[i].Name;
            }
        }
        return 'undefined';
    }

    $scope.Add = function () {
        $scope.Subproject = { EntityId: '00000000-0000-0000-0000-000000000000', Name: 'New Name' }
    }

    $scope.Edit = function (subprojectId) {
        $scope.SubmitInProgress = true;

        $.ajax({
            url: '/api/Subproject/GetById/' + subprojectId,
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            success: (result) => {
                $scope.SubmitInProgress = false;
                $scope.$apply();
                if (result.success) {
                    $scope.Subproject = result.data;
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

    $scope.Delete = function (subprojectId) {
        $scope.SubmitInProgress = true;
        $.ajax({
            url: '/api/Subproject/Delete/' + subprojectId,
            type: 'GET',
            contentType: 'application/json',
            dataType: 'json',
            success: (result) => {
                $scope.SubmitInProgress = false;
                $scope.$apply();
                if (result.success) {
                    $scope.Subproject = null;
                    $scope.$apply();
                    $scope.DataTable.ajax.reload();
                    $("#dialogEditSubproject").modal('hide');
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

    $scope.Submit = function (subproject) {
        if (!$scope.subprojectForm.$invalid) {
            $scope.SubmitInProgress = true;

            $.ajax({
                url: '/api/Subproject/Save',
                type: 'POST',
                contentType: 'application/json; charset=UTF-8',
                dataType: 'json',
                data: JSON.stringify(subproject),
                success: (result) => {
                    $scope.SubmitInProgress = false;
                    $scope.$apply();
                    if (result.success) {
                        $scope.Subproject = null;
                        $scope.$apply();
                        $("#dialogEditSubproject").modal('hide');
                        toastr.success(result.message, "Save Project");
                        $scope.DataTable.ajax.reload();
                        //var subprojects = $scope.DataTable.rows().data();
                        //for (var i in subprojects) {
                        //    if (subprojects[i].EntityId === result.data.EntityId) {
                        //        $scope.DataTable.row(i).data(result.data).draw();
                        //        break;
                        //    }
                        //}
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