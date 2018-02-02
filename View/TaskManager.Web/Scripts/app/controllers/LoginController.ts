namespace Controllers {

    export class LoginController extends BaseController {

        public Model: Models.LoginModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor(
            protected $scope: any,
            protected $http: ng.IHttpProvider,
            protected $location: ng.ILocationService) {
            super($scope, $http, $location);
            this.Model = new Models.LoginModel();

            $scope.Login = this.Login;
            $scope.Model = this.Model;
        }

        protected Login = () => {
            if (!super.ValidateForm()) {
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Account/Login/',
                type: 'POST',
                data: this.Model,
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        window.location.href = result.Data.ReturnUrl;
                    } else {
                        $this.Error(result.Message);
                        if (result.Data && result.Data.ReturnUrl) {
                            setTimeout(() => { window.location.href = result.Data.ReturnUrl; }, 1000);
                        }
                    }
                },
                error: (jqXhr) => {
                    $this.Error(jqXhr.statusText);
                }
            });
        }
    }
}