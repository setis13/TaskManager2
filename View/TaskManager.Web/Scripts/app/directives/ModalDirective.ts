var InnerHtmlDictionary: { [id: string]: string; } = {};

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
                    if (InnerHtmlDictionary[(<any>attrs).id] == null) {
                         var html = element.html();
                         for (var i in escapeTags) {
                             html = (<any>html).replaceAll(i, escapeTags[i]);
                         }
                         InnerHtmlDictionary[(<any>attrs).id] = html.toString();
                    } else {
                        element.html('');
                    }
                    if (modelValue != null) {
                        var html = InnerHtmlDictionary[(<any>attrs).id];
                        var linkFn = $compile(html);
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