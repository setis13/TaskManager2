namespace Controllers {

    export class AccountController extends BaseController {

        public Model: Models.AccountModel;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            $scope.Model = this.Model = new Models.AccountModel();

            var $this = this;

            $scope.ChangePassword_OnClick = this.ChangePassword_OnClick;
            $scope.ChangePasswordOk_OnClick = this.ChangePasswordOk_OnClick;
            $scope.ChangePasswordCancel_OnClick = this.ChangePasswordCancel_OnClick;
        }

        public ChangePassword_OnClick = () => {
            this.Model.ChangePassword = new Models.ChangePasswordModel();
        }

        public ChangePasswordOk_OnClick = () => {
            this.Model.ChangePassword.Error = null;
            if (!super.ValidateForm()) {
                $("#change-password-modal").modal("refresh");
                return;
            }

            var $this = this;
            $.ajax({
                url: '/api/Account/ChangePassword',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.ChangePassword),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Model.ChangePassword = null;
                        toastr.success("Password was changed");
                    } else {
                        $this.Model.ChangePassword.Error = result.error;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public ChangePasswordCancel_OnClick = () => {
            this.Model.ChangePassword = null;
        }
    }
}