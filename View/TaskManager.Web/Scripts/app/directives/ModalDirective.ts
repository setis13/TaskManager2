var InnerHrmlDictionary: { [id: string]: string; } = {};

var escapeTags: { [excape: string]: string; } = {};
escapeTags['ng-repeat1'] = 'ng-repeat';

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
                         var html = element.html();
                         for (var i in escapeTags) {
                             html = (<any>html).replaceAll(i, escapeTags[i]);
                         }
                        InnerHrmlDictionary[(<any>attrs).id] = html;
                    } else {
                        element.html('');
                    }
                    if (modelValue != null) {
                        var linkFn = $compile(InnerHrmlDictionary[(<any>attrs).id]);
                        var content = linkFn(scope);
                        element.append(content);
                    } else {
                        element.html('');
                    }
                    (<any>element).modal({ closable: false, autofocus: false });
                    (<any>element).modal(modelValue != null ? 'show' : 'hide');
                });
                scope.$on('$destroy', function () {
                    element.remove();
                });
            }
        }
    });