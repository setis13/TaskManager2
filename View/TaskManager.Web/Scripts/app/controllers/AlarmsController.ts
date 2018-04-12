declare var form: any;
declare var AlarmRepeatNames: { [id: number]: string; };

namespace Controllers {

    export class AlarmsController extends BaseController {

        public Model: Models.AlarmsModel;

        static $inject = ["$scope", "$http", "$location"];

        constructor($scope: any, $http: ng.IHttpProvider, $location: ng.ILocationService) {
            super($scope, $http, $location);
            var $this = this;

            $scope.AlarmRepeatNames = AlarmRepeatNames;

            $scope.Create_OnClick = this.Create_OnClick;
            $scope.Edit_OnClick = this.Edit_OnClick;
            $scope.Ok_OnClick = this.Ok_OnClick;
            $scope.Cancel_OnClick = this.Cancel_OnClick;
            $scope.Delete_OnClick = this.Delete_OnClick;
            $scope.AlarmBirthday_OnChange = this.AlarmBirthday_OnChange;

            this.Load();
        }

        public InitAlarmModal() {
            var $this = this;
            setTimeout(() => {
                $('#alarm-repeat-type').dropdown("set selected", this.Model.EditAlarm.RepeatType);
                $('#alarm-date').calendar({
                    type: 'date',
                    minDate: moment().toDate(),
                    onChange: function (date, text) {
                        $this.Model.EditAlarm.DateMoment = moment(date);
                    }
                }).calendar("set date", this.Model.EditAlarm.DateMoment.toDate());
            });
        }

        private InitCalendar() {
            var $this = this;
            var models: Array<Models.AlarmModel> = this.Model.Alarms;
            var today = moment();

            $('#calendar').html('');
            $('#calendar').calendar({
                firstDayOfWeek: 1,
                inline: true,
                type: 'date',
                mode: 'day',
                disableYear: true,
                disableMonth: true,
                multiMonth: 10,
                formatter: {
                    cell: function (cell, date1: Date, cellOptions) {
                        var dateStr = moment(date1).format("YYYY-MM-DD");
                        var date: moment.Moment = moment(dateStr);

                        $(cell[0]).attr('date', dateStr);
                        // creates alarm by a double click
                        $(cell[0]).on('dblclick', (sender) => {
                            $this.Create_OnClick(moment($(sender.currentTarget).attr('date')));
                            $this.scope.$apply();
                        });

                        if (today.diff(date, 'days') > 0) {
                            cell[0].classList.add('tomorrow');
                        }
                        for (var i in models) {
                            var model = models[i];
                            if (cell[0].classList.contains('disabled') == false) {
                                // adds mark
                                if (model.RepeatValue == null) {
                                    if (model.DateMoment.date() == date.date() &&
                                        model.DateMoment.month() == date.month() &&
                                        model.DateMoment.year() == date.year()) {
                                        $this.Mark(cell[0], model);
                                    }
                                } else {
                                    switch (model.RepeatType) {
                                        case Enums.AlarmRepeatEnum.Days:
                                            if (model.DateMoment.diff(date, 'days') % model.RepeatValue == 0) {
                                                $this.Mark(cell[0], model);
                                            }
                                            break;
                                        case Enums.AlarmRepeatEnum.Weeks:
                                            if (model.DateMoment.diff(date, 'days') % (model.RepeatValue * 7) == 0) {
                                                $this.Mark(cell[0], model);
                                            }
                                            break;
                                        case Enums.AlarmRepeatEnum.Months:
                                            if (model.DateMoment.date() == date.date() &&
                                                (model.DateMoment.month() - date.month()) % model.RepeatValue == 0) {
                                                $this.Mark(cell[0], model);
                                            }
                                            break;
                                        case Enums.AlarmRepeatEnum.Years:
                                            if (model.DateMoment.date() == date.date() &&
                                                model.DateMoment.month() == date.month() &&
                                                (model.DateMoment.year() - date.year()) % model.RepeatValue == 0) {
                                                $this.Mark(cell[0], model);
                                            }
                                            break;
                                        default:
                                            if (model.DateMoment.date() == date.date() &&
                                                model.DateMoment.month() == date.month() &&
                                                model.DateMoment.year() == date.year()) {
                                                $this.Mark(cell[0], model);
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            });
            //$('#calendar div.calendar').off('mousedown');
            //$('#calendar div.calendar').off('mouseup');
            $('#calendar div.calendar').off('mouseover');
            //$('#calendar div.calendar').off('click');
            //$('#calendar div.calendar').off('keydown');
            //$('#calendar div.calendar').off('touchend');
            //$('#calendar div.calendar').off('touchstart');
        }

        private Mark = (elem: any, model: Models.AlarmModel) => {
            // marks cell
            elem.classList.add('mark');
            // sets popup
            (<any>$(elem)).popup({ position: 'bottom center', title: model.Title, content: model.Description });
            // adds id attr
            $(elem).attr('alarm_id', model.EntityId);
            // adds event handler
            $(elem).on('click', (sender) => {
                this.Edit_OnClick(this.GetModelById($(sender.currentTarget).attr('alarm_id')));
                this.scope.$apply();
            });
        }

        private GetModelById(alarmId: string): Models.AlarmModel {
            return Enumerable.From(this.Model.Alarms).First(e => e.EntityId == alarmId);
        }

        public Load = () => {
            this.scope.Model = this.Model = new Models.AlarmsModel();

            var $this = this;
            $.ajax({
                url: '/api/Alarms/GetData/',
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
                        $this.Model.SetData(result.Data);
                        $this.InitCalendar();
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

        public Create_OnClick = (date: moment.Moment) => {
            var alarm = new Models.AlarmModel(null);
            if (date != null) {
                alarm.DateMoment = date;
            }
            this.Model.EditAlarm = alarm;
            this.InitAlarmModal();
        }
        public Edit_OnClick = (alarm: Models.AlarmModel) => {
            var clone = alarm.Clone();
            this.Model.EditAlarm = clone;
            this.InitAlarmModal();
        }

        public Ok_OnClick = () => {
            this.Model.EditAlarm.Error = null;
            if (!super.ValidateForm()) {
                $("#edit-alarm-modal").modal("refresh");
                return;
            }
            if (this.Model.EditAlarm.RepeatValue == null) {
                this.Model.EditAlarm.RepeatType = null;
            }

            var $this = this;
            $.ajax({
                url: '/api/Alarms/Save',
                type: 'POST',
                contentType: 'application/json',
                dataType: 'json',
                data: JSON.stringify(this.Model.EditAlarm),
                beforeSend(xhr) {
                    $this.ShowBusySaving();
                },
                complete() {
                    $this.HideBusySaving();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        this.Model.EditAlarm = null;
                        $this.Load();
                        angular.element($(".main.menu")).controller().Load();
                    } else {
                        $this.Model.EditAlarm.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        public Cancel_OnClick = () => {
            this.Model.EditAlarm = null;
        }

        public Delete_OnClick = (confirmed: boolean = false) => {
            var $this = this;
            // a confirm modal
            if (this.Model.EditAlarm.EntityId != null && confirmed == false) {
                $('#confirm-modal>.content>p').html("Please to confirm to delete the alarm");
                $('#confirm-modal')
                    .modal({
                        allowMultiple: true,
                        closable: false,
                        onApprove: function () {
                            $this.Delete_OnClick(true);
                            $this.scope.$apply();
                        }
                    }).modal('show');
                return;
            }

            this.Model.EditAlarm.Error = null;
            $.ajax({
                url: '/api/Alarms/Delete?id=' + this.Model.EditAlarm.EntityId,
                type: 'POST',
                data: {},
                beforeSend(xhr) {
                    $this.ShowBusyDeleting();
                },
                complete() {
                    $this.HideBusyDeleting();
                    $this.scope.$apply();
                },
                success: (result) => {
                    if (result.Success) {
                        this.Model.EditAlarm = null;
                        $this.Load();
                        angular.element($(".main.menu")).controller().Load();
                    } else {
                        $this.Model.EditAlarm.Error = result.Message;
                    }
                },
                error: (jqXhr) => {
                    console.error(jqXhr.statusText);
                    toastr.error(jqXhr.statusText);
                }
            });
        }

        private AlarmBirthday_OnChange = () => {
            if (this.Model.EditAlarm.Birthday) {
                this.Model.EditAlarm.RepeatType = Enums.AlarmRepeatEnum.Years;
                this.Model.EditAlarm.RepeatValue = 1;
                setTimeout(() =>
                    $('#alarm-repeat-type').dropdown("set selected", 'string:' + Enums.AlarmRepeatEnum.Years)
                );
            } else {
                this.Model.EditAlarm.RepeatType = null;
                this.Model.EditAlarm.RepeatValue = null;
                setTimeout(() =>
                    $("#alarm-repeat-type").dropdown('restore defaults')
                );
            }
        }
    }
}