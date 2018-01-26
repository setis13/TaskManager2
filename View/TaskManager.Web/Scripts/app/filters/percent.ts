app.filter('percent', function ($sce) {
    return function (value: number, is_xhtml) {
        return value * 100;
    }
});