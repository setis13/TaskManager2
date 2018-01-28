// "0" sets order of compilation

namespace Controllers {

    export abstract class BaseController {

        protected scope: any;
        protected http: angular.IHttpProvider;
        protected location: ng.ILocationService;

        public Model: Models.ModelBase;

        constructor(
            protected $scope: any,
            protected $http: angular.IHttpProvider,
            protected $location: ng.ILocationService) {
            this.scope = $scope;
            this.http = $http;
            this.location = $location;

            this.scope.check = (val, providedInt) => (val & providedInt);
            this.scope.Math = Math;
        }

        private Loading(val: boolean): void {
            this.scope.Loading = val;
        }

        private Saving(val: boolean): void {
            this.scope.Saving = val;
        }

        private Deleting(val: boolean): void {
            this.scope.Deleting = val;
        }

        protected ValidateForm(form2?: any): boolean {
            var result = (form2 != null ? form2() : form()).form('validate form');
            return result;
        }

        protected ShowLoader() {
            $("#loader").parent().addClass("dimmable");
            $("#loader").parent().addClass("dimmed");
            $("#loader").show();
        }

        protected HideLoader() {
            $("#loader").parent().removeClass("dimmable");
            $("#loader").parent().removeClass("dimmed");
            $("#loader").hide();
        }

        // shows busy in a table->tr
        protected ShowBusyRow(row) {
            row.addClass("dimmable dimmed");
            row.find("td:first-child")
                .append($('<div />').attr('class', 'ui active dimmer inverted')
                    .append($('<div />').attr('class', 'ui loader')));
        }

        // hides busy in a table->tr
        protected HideBusyRow(row) {
            row.removeClass("dimmable dimmed");
            row.find("td:first-child > .ui.dimmer").remove();
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

        protected ShowBusyDeleting() {
            this.Deleting(true);
        }

        protected HideBusyDeleting() {
            this.Deleting(false);
        }

        protected Error(message: string) {
            this.Model.Error = message;
        }

        protected UrlForAction(action: string, controller: string = ''): string {
            if (controller === '')
                return '/' + action;
            return '/' + controller + '/' + action;
        }

        private lastKey: { [id: string]: string; } = {};
        private lastValue: { [id: string]: any; } = {};
        // Returns true if value or key is changed.
        // For colored properties of comment
        public CheckNewValue = (propertyName: string, key: string, value: any) => {
            if (value == null) {
                return false;
            }

            if (this.lastKey[propertyName] == null) {
                this.lastKey[propertyName] = key;
                this.lastValue[propertyName] = value;
                return true;
            } else {
                if (this.lastKey[propertyName] === key && this.lastValue[propertyName] === value) {

                    return false;
                } else {
                    this.lastKey[propertyName] = key;
                    this.lastValue[propertyName] = value;
                    return true;
                }
            }
        }

        public ResetCheckNewValue() {
            this.lastKey = {};
            this.lastValue = {};
        }
    }
}