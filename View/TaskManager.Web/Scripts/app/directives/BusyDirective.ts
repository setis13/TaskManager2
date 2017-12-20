angular.module('BusyDirective', [])
    .directive('lbBusy', function () {
        function link(scope, element, attrs) {
            scope.$watch(attrs.lbBusy, function (value) {
                if (value) {
                    // bug with 'loading' and 'disabled'
                    // https://github.com/Semantic-Org/Semantic-UI/issues/1744
                    element.removeClass("primary");
                    element.addClass("readonly");
                    element.addClass("loading");
                } else {
                    element.addClass("primary");
                    element.removeClass("loading");
                    element.removeClass("readonly");
                }
            });
        }
        return { link: link };
    })

