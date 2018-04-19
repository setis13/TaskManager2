namespace Controllers {

    export class MenuController extends BaseController {

        public Model: Models.MenuModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            this.Load();
        }

        public Load = () => {
            this.scope.Model = this.Model = new Models.MenuModel();
            var $this = this;
            $.ajax({
                url: '/api/Alarms/GetNearAlarms?date=' + moment().format('YYYY-MM-DD'),
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyLoading();
                },
                complete() {
                    $this.HideBusyLoading();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        $this.Model.SetData(result.Data);
                        if ($this.Model.Alarms.length > 0) {
                            setTimeout(() =>
                                (<any>$('.alarm.item'))
                                    .popup({
                                        html: $('#calendar-popup').html(),
                                        position: 'bottom center',
                                        hoverable: true,
                                        show: 100
                                    }));
                        }
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
    }
}