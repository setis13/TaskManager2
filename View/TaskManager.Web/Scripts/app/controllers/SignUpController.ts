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
            $scope.Model = this.Model;
        }

        protected SignUp = () => {
            if (!super.Validate()) {
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
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.ShowBusySaving();
                        $this.$scope.$apply();
                        window.location.href = result.data.ReturnUrl;
                    } else {
                        $this.Error(result.error);
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