// https://stackoverflow.com/questions/15535336/combating-angularjs-executing-controller-twice
angular.module('TaskManagerApp', ['ngSanitize', 'ngRoute', 'BusyDirective']);

var app = angular.module('TaskManagerApp');

app.run($rootScope => {
    $rootScope.$on("$routeChangeSuccess", (event, currentRoute, previousRoute) => {
        $rootScope.title = currentRoute.title;
    });
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
//app.controller('CompanyController', Controllers.CompanyController);
