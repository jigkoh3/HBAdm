var hbAdmApp = angular.module('hbAdmApp', ['ngRoute', 'LocalStorageModule','ngLoadingSpinner','SignalR']);

hbAdmApp.config(function ($routeProvider) {
    $routeProvider
    .when('/dashboard', {
        templateUrl: 'app/views/dashboard.html',
        controller: 'DashboardController'
    })
    .when('/login', {
        templateUrl: 'app/views/auth/login.html',
        controller: 'loginController'
    })
    .when('/associate', {
        templateUrl: 'app/views/auth/associate.html',
        controller: 'associateController'
    });
    $routeProvider.otherwise({
        redirectTo: '/login'
    });
});

var serviceBase = '';
hbAdmApp.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngAuthApp'
});

hbAdmApp.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

hbAdmApp.run(['authService', function (authService) {
    authService.fillAuthData();
}]);