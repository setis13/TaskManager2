angular.module('ConvertToIntDirective', [])
    .directive('convertToNumber', function () {
    function link(scope, element, attrs, ngModel) {
        ngModel.$parsers.push(function (val) {
            if (val === '')
                return null;
            return parseInt(val, 10);
        });
        ngModel.$formatters.push(function (val) {
            if (val === null)
                return '';
            return '' + val;
        });
    }
    return {
        require: 'ngModel',
        link: link
    };
});
