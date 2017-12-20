
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
            if (!super.Validate()) {
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
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        window.location.href = result.data.ReturnUrl;
                    } else {
                        $this.Error(result.error);
                        if (result.data && result.data.ReturnUrl) {
                            setTimeout(() => { window.location.href = result.data.ReturnUrl; }, 1000);
                        }
                    }
                },
                error: (jqXhr) => {
                    $this.Error(jqXhr.statusText);
                    $this.$scope.$apply();
                }
            });
        }
    }
}