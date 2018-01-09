// "0" sets order of compilation
var Controllers;
(function (Controllers) {
    var BaseController = (function () {
        function BaseController($scope, $http, $location) {
            var _this = this;
            this.$scope = $scope;
            this.$http = $http;
            this.$location = $location;
            this.isActive = function (path) {
                return _this.location.path().match(path + "$") ? 'active' : '';
            };
            this.scope = $scope;
            this.http = $http;
            this.location = $location;
            this.scope.isActive = this.isActive;
            this.$scope.check = function (val, providedInt) { return (val & providedInt); };
            this.$scope.Math = Math;
        }
        BaseController.prototype.Loading = function (val) {
            this.scope.Loading = val;
        };
        BaseController.prototype.Saving = function (val) {
            this.scope.Saving = val;
        };
        BaseController.prototype.Deleting = function (val) {
            this.scope.Deleting = val;
        };
        BaseController.prototype.ValidateForm = function (form2) {
            var result = (form2 != null ? form2 : form).form('validate form');
            return result;
        };
        BaseController.prototype.ResetForm = function (form2) {
            var frm = (form2 != null ? form2 : form);
            // ERR: Error: [$rootScope:inprog] $apply already in progress. 
            // error occured in dropdown
            // Replaced by ".field.error".remove class
            //frm.form('clear');
            frm.find('.field.error').removeClass('error');
            frm.find('.ui.error.message').empty();
            frm.removeClass('error');
        };
        BaseController.prototype.ShowLoader = function () {
            $("#loader").parent().addClass("dimmable");
            $("#loader").parent().addClass("dimmed");
            $("#loader").show();
        };
        BaseController.prototype.HideLoader = function () {
            $("#loader").parent().removeClass("dimmable");
            $("#loader").parent().removeClass("dimmed");
            $("#loader").hide();
        };
        BaseController.prototype.DeleteLoader = function () {
            $("#loader").remove();
        };
        BaseController.prototype.ShowBusySaving = function () {
            this.Saving(true);
        };
        BaseController.prototype.HideBusySaving = function () {
            this.Saving(false);
        };
        BaseController.prototype.ShowBusyDeleting = function () {
            this.Deleting(true);
        };
        BaseController.prototype.HideBusyDeleting = function () {
            this.Deleting(false);
        };
        BaseController.prototype.Error = function (message) {
            this.Model.Error = message;
        };
        BaseController.prototype.UrlForAction = function (action, controller) {
            if (controller === void 0) { controller = ''; }
            if (controller === '')
                return '/' + action;
            return '/' + controller + '/' + action;
        };
        return BaseController;
    }());
    Controllers.BaseController = BaseController;
})(Controllers || (Controllers = {}));
//# sourceMappingURL=0BaseController.js.map