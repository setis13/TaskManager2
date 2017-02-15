angular.module('TaskManagerApp', ['ui.router', 'ngResource', 'TaskManagerApp.controllers'/*, 'TaskManagerApp.services'*/]);

var app = angular.module('TaskManagerApp');
app.config(function ($stateProvider, $httpProvider) {
    $stateProvider.state('projects', {
        //url: '/Projects',
        //templateUrl: '/Scripts/app/templates/projects.html',
        controller: 'ProjectController'
    })/*.state('viewProject', {
        url: '/Projects/Project:id',
        templateUrl: '/Scripts/app/templates/project-view.html',
        controller: 'ProjectViewController'
    }).state('newProject', {
        url: '/Projects/New',
        templateUrl: '/Scripts/app/templates/project-add.html',
        controller: 'ProjectCreateController'
    }).state('editProject', {
        url: '/Projects/Edit:id',
        templateUrl: '/Scripts/app/templates/project-edit.html',
        controller: 'ProjectEditController'
    })*/;
}).run(function ($state) {
    $state.go('projects');
});