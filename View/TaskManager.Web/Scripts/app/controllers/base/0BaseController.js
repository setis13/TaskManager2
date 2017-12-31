// "0" sets order of compilation
var Controllers;
(function (Controllers) {
    var BaseController = (function () {
        function BaseController($scope, $http, $location) {
            var _this = this;
            this.$scope = $scope;
            this.$http = $http;
            this.$location = $location;
            this.UserId = UserId;
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
        BaseController.prototype.ValidateForm = function () {
            if (LoadValidation != undefined) {
                LoadValidation();
                var result = form.form('validate form');
                return result;
            }
            else {
                return true;
            }
        };
        BaseController.prototype.ResetForm = function () {
            if (LoadValidation != undefined) {
                form.form('clear errors');
                $('.ui.error.message').empty();
                $('.ui.form.error').removeClass('error');
            }
        };
        BaseController.prototype.ShowLoader = function () {
            $("#loader").show();
        };
        BaseController.prototype.HideLoader = function () {
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
