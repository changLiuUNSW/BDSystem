// config
var app =
    angular.module('app')
        .config(
            [

                '$stateProvider', '$urlRouterProvider', '$controllerProvider', '$compileProvider', '$filterProvider', '$provide', '$httpProvider', 'cfpLoadingBarProvider', 'uiSelectConfig',
                function($stateProvider, $urlRouterProvider, $controllerProvider, $compileProvider, $filterProvider, $provide, $httpProvider, cfpLoadingBarProvider, uiSelectConfig) {
                    //http://blog.thoughtram.io/angularjs/2015/01/14/exploring-angular-1.3-speed-up-with-applyAsync.html
                    $httpProvider.useApplyAsync(true);
                    uiSelectConfig.theme = 'bootstrap';
                    cfpLoadingBarProvider.includeSpinner = false;
                    //IsAjax Request for asp.net 
//                    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';
                    $httpProvider.interceptors.push('authInterceptorService');
                    app.controller = $controllerProvider.register;
                    app.directive = $compileProvider.directive;
                    app.filter = $filterProvider.register;
                    app.factory = $provide.factory;
                    app.service = $provide.service;
                    app.constant = $provide.constant;
                    app.value = $provide.value;

                }
            ]
        ).value('userInfo', {
            userName: undefined,
            group: undefined
        });