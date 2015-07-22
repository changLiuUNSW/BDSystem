(function() {
    'use strict';

    angular.module('app.resource.helper').factory('telesaleService', factory)
        .value('callParams', {
            //Type: null,
            Initial: null,
            SiteId: null,
            LastCallId: null
        });

    factory.$inject = ['apiService', '$q', 'ngTableParams', 'userInfo', '$filter'];
    function factory(apiService, $q, ngTableParams, userInfo, $filter) {
        return {
            makeCall: makeCall,
            initNgParams: initNgParams,
            initNgParamsAsync: initNgParamsAsync,
        }

        function initNgParams(data) {
            var tableParams = new ngTableParams({
                page: 1, // show first page
                count: 5 // count per page
            }, {
                total: data.length, // length of data
                getData: function ($defer, params) {
                    var orderedData = params.filter() ? $filter('filter')(data, params.filter()) : data;
                    $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });

            return tableParams;
        }

        //initialize a async table params for ng-table
        //defalut to 5 rows per page
        function initNgParamsAsync(asyncFunc) {
            if (!asyncFunc)
                return null;

            var tableParams = new ngTableParams({
                page: 1,
                count: 5
            }, {
                total: 0,
                getData: function ($defer, params) {
                    var filter = params.filter();
                    var searchFields = [];
                    Object.getOwnPropertyNames(filter).forEach(function (name) {
                        searchFields.push({
                            field: name,
                            term: filter[name],
                            type: 'text'
                        });
                    });

                    if (searchFields.length <= 0)
                        return;

                    var searchParam = {
                        PageSize: 5,
                        SearchFields: searchFields,
                        LogicOperator: false,
                    };

                    asyncFunc(searchParam).$promise.then(function (success) {
                        //params.total(success.data.length);
                        $defer.resolve(success.data.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                    });
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
    }
})()