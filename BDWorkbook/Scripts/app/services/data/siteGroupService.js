(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('siteGroupService', siteGroupService);
    siteGroupService.$inject = ['$resource'];
    function siteGroupService($resource) {
        var baseUrl = config.ServerAddress + config.apiprefix + "group";
        var services = {
            getGroup: getGroup,
            getAllGroup: getAllGroup,
            saveGroup: saveGroup,
            searchGroup: searchGroup,
            removeGroup: removeGroup,
            removeSites: removeSites
    }
        return services;


        function saveGroup(params) {
            if (!params.Id) {
                return $resource(baseUrl).save(params).$promise;
            }
            return $resource(baseUrl, {}, {
                'update': {
                    method: 'PUT'
                },
            }).update(params).$promise;
        }

        function removeGroup(id) {
            return $resource(baseUrl + '/:id', { id: id }, {
                'remove': {
                    method: 'DELETE'
                },
            }).remove().$promise;
        }

        function getAllGroup(params) {
            return $resource(baseUrl).get(params).$promise;
        }

        function removeSites(id,siteIds) {
            return $resource(baseUrl + '/:id/site', { id: id }, {
                'remove': {
                    method: 'DELETE'
                },
            }).remove(siteIds).$promise;
        }


        function getGroup(params) {
            return $resource(baseUrl + '/:id').get(params).$promise;
        }
        function searchGroup (params) {
            return $resource(baseUrl + '/search').get(params).$promise;
        }

    }
})();