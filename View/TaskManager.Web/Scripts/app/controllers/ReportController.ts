declare var form: any;
declare var TaskPriorityNames: { [id: number]: string; };
declare var TaskStatusNames: { [id: number]: string; };

namespace Controllers {

    export class ReportController extends BaseController {

        public Model: Models.ReportSingleModel;

        static $inject = ["$scope", "$http", "$location"];

        private _projectModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this.$scope.TaskPriorityNames = TaskPriorityNames;
            this.$scope.TaskStatusNames = TaskStatusNames;
            this.$scope.Model = this.Model = new Models.ReportSingleModel();

            $scope.GenerateSingle_OnClick = this.GenerateSingle_OnClick;

            (<any>$('#date')).calendar({
                type: 'date',
                onChange: function (date, text) {
                    $this.Model.Date = moment(date);
                }
            }).calendar("set date", $this.Model.Date.toDate());
        }

        public GenerateSingle_OnClick = () => {
            this.Load();
        }

        public Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Report/GetSingle?date=' + this.Model.Date.format("YYYY-MM-DD"),
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.$scope.Generating = true;
                },
                complete() {
                    $this.$scope.Generating = false;
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Model.SetData(result.data);
                    } else {
                        $this.Error(result.error);
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