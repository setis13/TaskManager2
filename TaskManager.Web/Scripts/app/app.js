angular.module('TaskManagerApp', ['ui.router', 'ngResource']);

var app = angular.module('TaskManagerApp');
app.config(function ($stateProvider, $httpProvider) {
    $stateProvider
    .state('projects', {
        controller: 'ProjectController'
    })
    .state('subprojects', {
        controller: 'SubprojectController'
    });
}).run(function ($state) {
    $state.go('projects');
});