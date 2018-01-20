declare var form: any;

namespace Controllers {

    export class CompanyController extends BaseController {

        public Model: Models.Company1Model;

        static $inject = ["$scope", "$http", "$location"];

        private _findModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this._findModal = (<any>$("#find-user-modal")).modal({
                closable: false,
                onHidden() {
                    $this.ResetForm();
                    $this.$scope.$apply();
                }
            });

            $scope.UserId = UserId;
            $scope.Leave_OnClick = this.Leave_OnClick;
            $scope.Decline_OnClick = this.Decline_OnClick;
            $scope.FindUser_OnClick = this.FindUser_OnClick;
            $scope.Accept_OnClick = this.Accept_OnClick;
            $scope.Reject_OnClick = this.Reject_OnClick;
            $scope.Remove_OnClick = this.Remove_OnClick;

            this.Load();
        }

        public Load = () => {
            this.$scope.Model = this.Model = new Models.Company1Model();

            var $this = this;
            $.ajax({
                url: '/api/Company/GetData/',
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
                        $this.$scope.$apply();
                        $this.Model.SetData(result.data);
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                    $this.$scope.$apply();
                }
            });
        }
    }
}