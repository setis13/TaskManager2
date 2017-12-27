// "0" sets order of compilation

namespace Controllers {

    export abstract class BaseController {

        protected scope: any;
        protected http: angular.IHttpProvider;
        protected location: ng.ILocationService;

        public Model: Models.ModelBase;

        protected UserId: string = UserId;

        constructor(
            protected $scope: any,
            protected $http: angular.IHttpProvider,
            protected $location: ng.ILocationService) {
            this.scope = $scope;
            this.http = $http;
            this.location = $location;

            this.scope.isActive = this.isActive;

            this.$scope.check = (val, providedInt) => (val & providedInt);
            this.$scope.Math = Math;
        }

        private Loading(val: boolean): void {
            this.scope.Loading = val;
        }

        private Saving(val: boolean): void {
            this.scope.Saving = val;
        }

        protected Validate(): boolean {
            if (LoadValidation != undefined) {
                LoadValidation();
                var result = form.form('validate form');
                return result;
            } else {
                return true;
            }
        }

        protected ShowLoader() {
            $("#loader").show();
        }

        protected HideLoader() {
            $("#loader").hide();
        }

        protected DeleteLoader() {
            $("#loader").remove();
        }

        protected ShowBusySaving() {
            this.Saving(true);
        }

        protected HideBusySaving() {
            this.Saving(false);
        }

        protected Error(message: string) {
            this.Model.Error = message;
        }

        protected isActive = (path) => {
            return this.location.path().match(path + "$") ? 'active' : '';
        }

        protected UrlForAction(action: string, controller: string = ''): string {
            if (controller === '')
                return '/' + action;
            return '/' + controller + '/' + action;
        }

    }
}