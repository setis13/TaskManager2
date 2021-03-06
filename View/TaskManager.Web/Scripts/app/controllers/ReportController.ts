﻿declare var form: any;
declare var TaskPriorityNames: { [id: number]: string; };
declare var TaskStatusNames: { [id: number]: string; };
declare var start;
declare var end;

namespace Controllers {

    export class ReportController extends BaseController {

        public Model: Models.ReportModel;

        static $inject = ["$scope", "$http", "$location"];

        private _projectModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this.scope.TaskPriorityNames = TaskPriorityNames;
            this.scope.TaskStatusNames = TaskStatusNames;
            this.scope.Model = this.Model = new Models.ReportModel();
            this.Model.Start = start;
            this.Model.End = end;

            $scope.GenerateReport_OnClick = this.GenerateReport_OnClick;

            $scope.CheckNewValue = this.CheckNewValue;

            setTimeout(() => {
                $('#start').calendar({
                    type: 'date',
                    endCalendar: $('#end'),
                    onChange: function (date, text) {
                        $this.Model.Start = moment(date);
                    }
                }).calendar("set date", this.Model.Start.toDate());

                $('#end').calendar({
                    type: 'date',
                    startCalendar: $('#start'),
                    onChange: function (date, text) {
                        $this.Model.End = moment(date);
                    }
                }).calendar("set date", (this.Model.End != null ? $this.Model.End.toDate() : null));

                $('#projects-filter').dropdown();
            });

            this.Load();
        }

        public Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Projects/GetData/',
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowLoader();
                },
                complete() {
                    $this.HideLoader();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetData(result.Data);
                    } else {
                        $this.Error(result.Message);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                }
            });
        }

        public GenerateReport_OnClick = () => {
            this.Generate();
        }

        public Generate = () => {
            var $this = this;
            $.ajax({
                url: '/api/Report/GetData',
                type: 'POST',
                data: {
                    Start: this.Model.Start.format("YYYY-MM-DD"),
                    End: this.Model.End != null ? this.Model.End.format("YYYY-MM-DD") : 'null',
                    ProjectIds: this.Model.SelectedProjectsFilter,
                    IncludeNew: this.Model.IncludeNew,
                    IncludeZero: this.Model.IncludeZero
                },
                beforeSend(xhr) {
                    $this.scope.Generating = true;
                },
                complete() {
                    $this.scope.Generating = false;
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetData2(result.Data);
                    } else {
                        $this.Error(result.Message);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                }
            });
        }
    }
}