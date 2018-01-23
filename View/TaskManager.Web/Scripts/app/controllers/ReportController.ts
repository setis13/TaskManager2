declare var form: any;

namespace Controllers {

    export class ReportController extends BaseController {

        public Model: Models.ProjectsModel;

        static $inject = ["$scope", "$http", "$location"];

        private _projectModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            $scope._OnClick = this._OnClick;

            this.Load();
            $this.HideLoader();

            (<any>$('#date')).calendar({
                type: 'date',
                onChange: function (date, text) {
                   // $this.Model.EditComment.DateMoment = moment(date);
                }
            })/*.calendar("set date", $this.Model.EditComment.DateMoment.toDate())*/;
        }

        public Load = () => {
            this.$scope.Model = this.Model = new Models.Model();

            var $this = this;
            $.ajax({
                url: '/api/Report/Get/',
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowLoader();
                },
                complete() {
                    $this.HideLoader();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Model.Set(result.data);
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