namespace Controllers {

    export class AccountController extends BaseController {

        public Model: Models.AccountModel;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            $scope.Model = this.Model = new Models.AccountModel();
        }
    }
}