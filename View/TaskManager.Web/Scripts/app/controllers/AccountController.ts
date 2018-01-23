namespace Controllers {

    export class AccountController extends BaseController {

        public Model: Models.AccountModel;

        private _changePasswordModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            $scope.Model = this.Model = new Models.AccountModel();

            var $this = this;
            this._changePasswordModal = (<any>$("#change-password-modal")).modal({
                closable: false,
                onHidden() {
                    $this.Model.ChangePassword = null;
                    $this.ResetForm();
                    $this.$scope.$apply();
                }
            });

            $scope.ChangePassword_OnClick = this.ChangePassword_OnClick;
            $scope.ChangePasswordOk_OnClick = this.ChangePasswordOk_OnClick;
            $scope.ChangePasswordCancel_OnClick = this.ChangePasswordCancel_OnClick;
        }

        public ChangePassword_OnClick = () => {
            this.Model.ChangePassword = new Models.ChangePasswordModel();
            this._changePasswordModal.modal('show');
        }

        public ChangePasswordOk_OnClick = () => {
            this.Model.ChangePassword.Error = null;
            if (!super.ValidateForm()) {
                this._changePasswordModal.modal("refresh");
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
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this._changePasswordModal.modal('hide');
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
            this._changePasswordModal.modal('hide');
        }
    }
}