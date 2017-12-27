// https://stackoverflow.com/questions/15535336/combating-angularjs-executing-controller-twice
angular.module('TaskManagerApp', ['ngSanitize', 'ngRoute', 'BusyDirective']);
var app = angular.module('TaskManagerApp');
app.config([
    '$routeProvider', '$locationProvider',
    function ($routeProvider, $locationProvider) {
        $routeProvider
            .when('/', {
            templateUrl: '/Templates/Home.html',
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
