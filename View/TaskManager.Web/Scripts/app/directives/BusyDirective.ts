var buttonClasses = [
    "primary",
    "red",
    "orange",
    "yellow",
    "olive",
    "green",
    "teal",
    "blue",
    "violet",
    "purple",
    "pink",
    "brown",
    "grey",
    "black"];

angular.module('BusyDirective', [])
    .directive('lbBusy', function () {
        var buttonClass = null;

        function link(scope, element, attrs) {
            scope.$watch(attrs.lbBusy, function (value) {
                if (value === true) {
                    // bug with 'loading' and 'disabled'
                    // https://github.com/Semantic-Org/Semantic-UI/issues/1744
                    // finds a color of button
                    for (var i = 0; i < buttonClasses.length; i++) {
                        if (element.hasClass(buttonClasses[i])) {
                            buttonClass = buttonClasses[i];
                            // removes the color
                            element.removeClass(buttonClass);
                            break;
                        }
                    }
                    element.addClass("readonly");
                    element.addClass("loading");
                } else if (value === false) {
                    // restores the color
                    if (buttonClass != null) {
                        element.addClass(buttonClass);
                    }
                    element.removeClass("loading");
                    element.removeClass("readonly");
                }
            });
        }
        return { link: link };
    })


    .directive('modal', function ($compile) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            require: 'ngModel',
            template: '<div class="ui modal" ng-transclude></div>',
            link: function (scope, element, attrs, ngModel) {
                (<any>element).modal({
                    onHide: function () {
                        (<any>ngModel).$setViewValue(false);
                    }
                });
                scope.$watch(function () {
                    return (<any>ngModel).$modelValue;
                }, function (modelValue) {
                    (<any>element).modal(modelValue ? 'show' : 'hide');
                    console.log(<any>"toogle modal:" + modelValue);
                    if (modelValue && inner != null) {
                        var linkFn = $compile(inner);
                        var content = linkFn(scope);
                        element.append(content);
                    } else {
                        if (inner == null) {
                            inner = element.html();
                        }
                            element.html('');
                    }
                });
                scope.$on('$destroy', function () {
                    (<any>element).modal('hide');
                    element.remove();
                    console.log("destroy");
                });
            }
        }
    });

var inner = null;