(function() {
    'use strict';
    angular.module('app.resource.auth')
        .factory('authInterceptorService', authInterceptorService);
    authInterceptorService.$inject = ['$q', '$localStorage'];
    function authInterceptorService($q, $localStorage) {
        var services= {
            request: request
        }
        return services;
        function request (config) {
            config.headers = config.headers || {};
            if ($localStorage.authorizationData) {
                config.headers.Authorization = 'Bearer ' + $localStorage.authorizationData.token;
            }
            return config;
        }
    }
})();