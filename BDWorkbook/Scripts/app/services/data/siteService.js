(function() {
    'use strict';
    angular.module('app.resource.data')
    .factory('siteService', siteService);
    siteService.$inject = ['$resource'];

    function siteService($resource) {
        var baseUrl = config.ServerAddress + config.apiprefix + "site";
        var services ={
            getSite: getSite,
            getSiteBykey: getSiteByKey,
            updateGroup: updateGroup,
            updateSiteFromAdmin: updateSiteFromAdmin
        };
        return services;
        function getSite (params) {
            return $resource(baseUrl).get(params).$promise;
        };

        function updateGroup(params) {
            return $resource(baseUrl + '/group').save(params).$promise;
        }


        function getSiteByKey (params) {
            return $resource(baseUrl + '/key').get(params).$promise;
        };

        function updateSiteFromAdmin(params) {
            return $resource(baseUrl + '/admin').save(params).$promise;
        }
    }
})();