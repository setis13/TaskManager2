declare var form1: any;
declare var form2: any;

namespace Controllers {

    export class CompanyController extends BaseController {

        public Model: Models.Company1Model;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

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
            this.scope.Model = null;

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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        this.scope.Model = $this.Model = new Models.Company1Model();
                        $this.Model.SetData(result.Data);
                    } else {
                        $this.Error(result.Message);
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Load();
                    } else {
                        $this.Error(result.Message);
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Load();
                    } else {
                        $this.Error(result.Message);
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Load();
                    } else {
                        $this.Error(result.Message);
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Load();
                    } else {
                        $this.Error(result.Message);
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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Load();
                    } else {
                        $this.Error(result.Message);
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
        }

        public UserOk_OnClick = () => {
            if (!super.ValidateForm(form1)) {
                $("#find-user-modal").modal("refresh");
                return;
            }
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
                        $this.scope.$apply();
                    },
                    success: (result) => {
                        if (result.Success) {
                            $this.Model.FindUser.Id = result.Data.User.Id;
                            $this.Model.FindUser.Email = result.Data.User.Email;
                            $this.Model.FindUser.UserName = result.Data.User.UserName;
                        } else {
                            $this.Model.FindUser.Error = result.Message;
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
                        $this.scope.$apply();
                    },
                    success: (result) => {
                        if (result.Success) {
                            $this.Model.FindUser = null;
                            $this.Load();
                        } else {
                            $this.Model.FindUser.Error = result.Message;
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
            this.Model.FindUser = null;
        }

        public CreateCompany_OnClick = () => {
            var company = new Models.CompanyModel(null);
            this.Model.EditCompany = company;
        }

        public CompanyOk_OnClick = (subtask: Models.CompanyModel) => {
            this.Model.EditCompany.Error = null;
            if (!super.ValidateForm(form2)) {
                $("#create-company-modal").modal("refresh");
                return;
            }

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
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.EditCompany = null;
                        $this.Load();
                    } else {
                        $this.Model.EditCompany.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public CompanyCancel_OnClick = () => {
            this.Model.EditCompany = null;
        }
    }
}