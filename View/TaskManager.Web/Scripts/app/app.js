// https://stackoverflow.com/questions/15535336/combating-angularjs-executing-controller-twice
angular.module('TaskManagerApp', ['ngSanitize', 'ngRoute', 'BusyDirective']);
var app = angular.module('TaskManagerApp');
app.run(function ($rootScope) {
    $rootScope.$on("$routeChangeSuccess", function (event, currentRoute, previousRoute) {
        $rootScope.title = currentRoute.title;
    });
});
app.config([
    '$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/profile', {
            title: 'Profile',
            templateUrl: '/Templates/Account/Profile.html',
            controller: 'AccountController',
        })
            .when('/', {
            title: 'Home',
            templateUrl: '/Templates/Home/Index.html',
            controller: 'HomeController',
        });
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }
]);
app.controller('LoginController', Controllers.LoginController);
app.controller('SignUpController', Controllers.SignUpController);
app.controller('HomeController', Controllers.HomeController);
app.controller('AccountController', Controllers.AccountController);
