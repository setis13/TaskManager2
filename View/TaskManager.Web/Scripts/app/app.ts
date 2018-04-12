// https://stackoverflow.com/questions/15535336/combating-angularjs-executing-controller-twice
angular.module('TaskManagerApp', ['ngSanitize', 'ngRoute', 'BusyDirective', 'ModalDirective', 'ConvertToNumberDirective']);

var app = angular.module('TaskManagerApp');

app.run($rootScope => {
    $rootScope.$on("$routeChangeSuccess", (event, currentRoute, previousRoute) => {
        $rootScope.title = currentRoute.title;
    });
    $rootScope.isActive = function (path) {
        return location.pathname.match(path + "$") ? 'active' : '';
    }
});

app.config([
    '$routeProvider', '$locationProvider',
    ($routeProvider, $locationProvider) => {

        $routeProvider
            .when('/profile',
            {
                title: 'Profile',
                templateUrl: '/Templates/Account/Profile.html',
                controller: 'AccountController',
            })
            .when('/projects',
            {
                title: 'Projects',
                templateUrl: '/Templates/Projects/Index.html',
                controller: 'ProjectsController',
            })
            .when('/company',
            {
                title: 'Company',
                templateUrl: '/Templates/Company/Index.html',
                controller: 'CompanyController',
            })
            .when('/alarms',
            {
                title: 'Alarms',
                templateUrl: '/Templates/Alarms/Index.html',
                controller: 'AlarmsController',
            })
            .when('/report_single',
            {
                title: 'Report, One Day',
                templateUrl: '/Templates/Report/Single.html',
                controller: 'ReportController',
            })
            .when('/report_period',
            {
                title: 'Report, Period',
                templateUrl: '/Templates/Report/Period.html',
                controller: 'ReportController',
            })
            .when('/',
            {
                title: 'Home',
                templateUrl: '/Templates/Home/Index.html',
                controller: 'HomeController',
            })
            ;
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }
]);


app.controller('LoginController', Controllers.LoginController);
app.controller('SignUpController', Controllers.SignUpController);
app.controller('AccountController', Controllers.AccountController);
app.controller('HomeController', Controllers.HomeController);
app.controller('ProjectsController', Controllers.ProjectsController);
app.controller('AlarmsController', Controllers.AlarmsController);
app.controller('CompanyController', Controllers.CompanyController);
app.controller('ReportController', Controllers.ReportController);
app.controller('MenuController', Controllers.MenuController);
