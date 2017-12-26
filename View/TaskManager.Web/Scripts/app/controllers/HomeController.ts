namespace Controllers {
    export class HomeController extends BaseController {

        public Model: Models.HomeModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor(
            protected $scope: any,
            protected $http: ng.IHttpProvider,
            protected $location: ng.ILocationService) {
            super($scope, $http, $location);
            this.Model = new Models.HomeModel();

            $scope.Model = this.Model;

            this.Load();
        }

        protected Load = () => {
            var $this = this;
            $.ajax({
                url: '/api/Home/GetData/',
                type: 'GET',
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
                        console.log(result);
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