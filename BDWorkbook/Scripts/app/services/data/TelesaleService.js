(function() {
    'use strict';

    angular.module('app.resource.helper').factory('telesaleService', factory)
        .value('callParams', {
            type: null,
            initial: null,
            siteId: null,
            lastCallId: null
        });

    factory.$inject = ['apiService', '$q', 'ngTableParams', 'userInfo', '$filter'];
    function factory(apiService, $q, ngTableParams, userInfo, $filter) {
        return {
            makeCall: makeCall,
            initNgParams: initNgParams,
            isInRole : isInRole
        }

        function initNgParams(data) {
            var tableParams = new ngTableParams({
                page: 1, // show first page
                count: 5 // count per page
            }, {
                total: data.length, // length of data
                getData: function ($defer, params) {
                    $defer.resolve(data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });

            return tableParams;
        }

        //request a new contact
        function makeCall(param, successCallback, errorCallback) {
            var defer = $q.defer();
            apiService.contact.nextCall(param, function(success) {
                if (angular.isFunction(successCallback)) {
                    successCallback(success);
                }
                defer.resolve(success);
            }, function(error) {
                if (angular.isFunction(errorCallback)) {
                    errorCallback(error);
                }

                defer.reject(error);
            });
            return defer;
        }

        function isInRole(role) {
            var groups = userInfo.group.split(',');

            var found = $filter('find')(groups, function(obj) {
                return obj.toLowerCase() == role.toLowerCase();
            });

            return found != undefined;
        }
    }
})()