namespace Controllers {
    export class SignUpController extends BaseController {

        public Model: Models.SignUpModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor(
            protected $scope: any,
            protected $http: ng.IHttpProvider,
            protected $location: ng.ILocationService) {
            super($scope, $http, $location);
            this.Model = new Models.SignUpModel();

            $scope.SignUp = this.SignUp;
            $scope.GoToHome = () => window.location.href = "/";
            $scope.Model = this.Model;
        }

        protected SignUp = () => {
            if (!super.ValidateForm()) {
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Account/Register/',
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
                        $this.ShowBusySaving();
                        window.location.href = result.Data.ReturnUrl;
                    } else {
                        $this.Error(result.Message);
                    }
                },
                error: (jqXhr) => {
                    $this.Error(jqXhr.statusText);
                }
            });
        }
    }
}