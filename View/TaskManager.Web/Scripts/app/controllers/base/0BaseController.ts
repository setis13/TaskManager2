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

        private Deleting(val: boolean): void {
            this.scope.Deleting = val;
        }

        protected ValidateForm(form2?: any): boolean {
            var result = (form2 != null ? form2 : form).form('validate form');
            return result;
        }

        protected ResetForm(form2?: any) {
            var frm = (form2 != null ? form2 : form);
            // ERR: Error: [$rootScope:inprog] $apply already in progress. 
            // error occured in dropdown
            // Replaced by ".field.error".remove class
            //frm.form('clear');
            frm.find('.field.error').removeClass('error');
            frm.find('.ui.error.message').empty();
            frm.removeClass('error');
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