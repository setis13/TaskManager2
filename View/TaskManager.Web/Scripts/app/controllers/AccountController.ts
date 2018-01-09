namespace Controllers {

    export class AccountController extends BaseController {

        public Model: Models.AccountModel;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            this.Model = new Models.AccountModel();

            $scope.Model = this.Model;

            this.Load();
        }

        public Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Home/GetData/',
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.ShowBusySaving();
                        $this.$scope.$apply();
                        this.Model.SetData(result.data);
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