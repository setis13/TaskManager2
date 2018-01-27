app.filter('set_progress_data_percent', function ($sce) {
    return function (task: Models.BaseModel, is_xhtml) {
        var percent = (<any>task).Progress * 100;
        $(<string>("#progress-" + task.EntityId)).progress({ percent: percent, showActivity: false });
        return percent;
    }
});