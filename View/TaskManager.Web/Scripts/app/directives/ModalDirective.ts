var InnerHrmlDictionary: { [id: string]: string; } = {};

angular.module('ModalDirective', [])
    .directive('modal', function ($compile) {
        return {
            restrict: 'E',
            replace: true,
            transclude: true,
            require: 'ngModel',
            template: '<div class="ui modal" ng-transclude></div>',
            link: function (scope, element, attrs, ngModel) {
                if ((<any>attrs).id == undefined) {
                    console.error("modal doesn't have 'id'");
                    return;
                }
                scope.$watch(function () {
                    return (<any>ngModel).$modelValue;
                }, function (modelValue) {
                    if (InnerHrmlDictionary[(<any>attrs).id] == null) {
                        InnerHrmlDictionary[(<any>attrs).id] = element.html();
                    } else {
                        element.html('');
                    }
                    if (modelValue != null) {
                        var linkFn = $compile(InnerHrmlDictionary[(<any>attrs).id]);
                        var content = linkFn(scope);
                        element.append(content);
                    }
                    (<any>element).modal({ closable: false });
                    (<any>element).modal(modelValue != null ? 'show' : 'hide');
                });
                scope.$on('$destroy', function () {
                    element.remove();
                });
            }
        }
    });