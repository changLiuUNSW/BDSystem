(function() {
    'use strict';
    angular.module('app.resource.data')
     .factory('summaryService', summaryService);
    summaryService.$inject = ['$resource'];

    function summaryService($resource) {
        var baseUrl = config.ServerAddress + config.apiprefix + "summary";
        var services= {
            getcontactPersonCount: getcontactPersonCount,
            getSiteCount: getSiteCount
        };
        return services;
        function getcontactPersonCount(params) {
            return $resource(baseUrl + '/contactperson/count').get(params).$promise;
        }

        function getSiteCount(params) {
            return $resource(baseUrl + '/site/count').get(params).$promise;
        }
    }
})();