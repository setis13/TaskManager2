declare var form1: any;
declare var form2: any;

namespace Controllers {

    export class CompanyController extends BaseController {

        public Model: Models.Company1Model;

        static $inject = ["$scope", "$http", "$location"];

        private _findModal: any;
        private _createModal: any;

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this._findModal = (<any>$("#find-user-modal")).modal({
                closable: false,
                onHidden() {
                    $this.ResetForm(form1);
                    $this.$scope.$apply();
                }
            });

            this._createModal = (<any>$("#create-company-modal")).modal({
                closable: false,
                onHidden() {
                    $this.ResetForm(form2);
                    $this.$scope.$apply();
                }
            });

            $scope.UserId = UserId;

            $scope.Leave_OnClick = this.Leave_OnClick;
            $scope.Accept_OnClick = this.Accept_OnClick;
            $scope.Reject_OnClick = this.Reject_OnClick;

            $scope.FindUser_OnClick = this.FindUser_OnClick;
            $scope.CreateCompany_OnClick = this.CreateCompany_OnClick;

            $scope.Remove_OnClick = this.Remove_OnClick;
            $scope.Cancel_OnClick = this.Cancel_OnClick;

            $scope.UserCancel_OnClick = this.UserCancel_OnClick;
            $scope.UserOk_OnClick = this.UserOk_OnClick;

            $scope.CompanyCancel_OnClick = this.CompanyCancel_OnClick;
            $scope.CompanyOk_OnClick = this.CompanyOk_OnClick;

            this.Load();
        }

        public Load = () => {
            this.$scope.Model = null;

            var $this = this;
            $.ajax({
                url: '/api/Company/GetData/',
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowLoader();
                },
                complete() {
                    $this.HideLoader();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        this.$scope.Model = $this.Model = new Models.Company1Model();
                        $this.Model.SetData(result.data);
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    $this.Error(jqXhr.statusText);
                }
            });
        }

        public Leave_OnClick = () => {
            if (!confirm("Please confirm to leave from the company")) {
                return;
            }
            var $this = this;
            $.ajax({
                url: '/api/Company/LeaveCompany',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.scope.Leaving = true;
                },
                complete() {
                    $this.scope.Leaving = false;
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Load();
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Accept_OnClick = (user: Models.UserModel) => {
            var $this = this;
            $.ajax({
                url: '/api/Company/AcceptCompany',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.scope.Accepting = true;
                },
                complete() {
                    $this.scope.Accepting = false;
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Load();
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Reject_OnClick = (user: Models.UserModel) => {
            if (!confirm("Please confirm to reject an invitation")) {
                return;
            }
            var $this = this;
            $.ajax({
                url: '/api/Company/RejectCompany',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.scope.Rejecting = true;
                },
                complete() {
                    $this.scope.Rejecting = false;
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Load();
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Remove_OnClick = (user: Models.UserModel) => {
            if (!confirm("Please confirm to remove the user")) {
                return;
            }
            var $this = this;
            $.ajax({
                url: '/api/Company/RemoveUser?id=' + user.Id,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyRow($("#user-" + user.Id));
                },
                complete() {
                    $this.ShowBusyRow($("#user-" + user.Id));
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Load();
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Cancel_OnClick = (user: Models.UserModel) => {
            if (!confirm("Please confirm to cancel an invitation")) {
                return;
            }
            var $this = this;
            $.ajax({
                url: '/api/Company/CancelInvitation?id=' + user.Id,
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyRow($("#user-" + user.Id));
                },
                complete() {
                    $this.ShowBusyRow($("#user-" + user.Id));
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this.Load();
                    } else {
                        $this.Error(result.error);
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public FindUser_OnClick = () => {
            var user = new Models.UserModel(null);
            this.Model.FindUser = user;
            this._findModal.modal('show');
        }

        public UserOk_OnClick = () => {
            var $this = this;
            if (this.Model.FindUser.Id == null) {
                $.ajax({
                    url: '/api/Company/FindUser',
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    data: JSON.stringify(this.Model.FindUser),
                    beforeSend(xhr) {
                        $this.ShowBusySaving();
                    },
                    complete() {
                        $this.HideBusySaving();
                        $this.$scope.$apply();
                    },
                    success: (result) => {
                        if (result.success) {
                            $this.Model.FindUser.Id = result.data.User.Id;
                            $this.Model.FindUser.Email = result.data.User.Email;
                            $this.Model.FindUser.UserName = result.data.User.UserName;
                        } else {
                            $this.Model.FindUser.Error = result.error;
                        }
                    },
                    error: (jqXhr) => {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            } else {
                $.ajax({
                    url: '/api/Company/InviteUser?id=' + this.Model.FindUser.Id,
                    type: 'POST',
                    contentType: 'application/json',
                    dataType: 'json',
                    data: {},
                    beforeSend(xhr) {
                        $this.ShowBusySaving();
                    },
                    complete() {
                        $this.HideBusySaving();
                        $this.$scope.$apply();
                    },
                    success: (result) => {
                        if (result.success) {
                            $this._findModal.modal('hide');
                            $this.Load();
                        } else {
                            $this.Model.FindUser.Error = result.error;
                        }
                    },
                    error: (jqXhr) => {
                        console.error(jqXhr.statusText);
                        toastr.error(jqXhr.statusText);
                    }
                });
            }
        }

        public UserCancel_OnClick = () => {
            this._findModal.modal('hide');
            this.Model.FindUser = null;
        }

        public CreateCompany_OnClick = () => {
            var company = new Models.CompanyModel(null);
            this.Model.EditCompany = company;
            this._createModal.modal('show');
        }

        public CompanyOk_OnClick = (subtask: Models.CompanyModel) => {
            var $this = this;
            $.ajax({
                url: '/api/Company/CreateCompany',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditCompany),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.$scope.$apply();
                },
                success: (result) => {
                    if (result.success) {
                        $this._createModal.modal('hide');
                        $this.Load();
                    } else {
                        $this.Model.EditCompany.Error = result.error;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public CompanyCancel_OnClick = () => {
            this._createModal.modal('hide');
            this.Model.EditCompany = null;
        }
    }
}