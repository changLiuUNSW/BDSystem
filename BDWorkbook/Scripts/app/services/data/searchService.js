(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('searchService', searchService);
    searchService.$inject = ['$resource'];

    function searchService($resource) {
        var baseUrl = config.ServerAddress + config.apiprefix + "search";
        var searchConfig = {
            //TODO:Always include BusinessType
            'site': [
                { field: 'BusinessType', type: 'text', visible: false, hidden: true },
                { field: 'SiteId', type: 'text', visible: false, hidden: true },
                { field: 'Key', type: 'text', visible: true, navigate: true },
                { field: 'SalesRepList', type: 'list', visible: true, sortable: false },
                { field: 'Company', type: 'text', visible: true },
                { field: 'Unit', type: 'text', visible: false },
                { field: 'Number', type: 'text', visible: true },
                { field: 'Street', type: 'text', visible: true },
                { field: 'Suburb', type: 'text', visible: true },
                { field: 'State', type: 'text', visible: true },
                { field: 'PostCode', type: 'text', visible: true },
                { field: 'Phone', type: 'text', visible: true }
            ],
            //TODO:Always include BusinessType
            'person': [
                { field: 'BusinessType', type: 'text', visible: false, hidden: true },
                { field: 'ContactPersonId', type: 'text', visible: false, hidden: true },
                { field: 'Key', type: 'text', visible: true, navigate: true },
                { field: 'SalesRep', type: 'text', visible: true },
                { field: 'Company', type: 'text', visible: true },
                { field: 'FirstName', type: 'text', visible: true },
                { field: 'LastName', type: 'text', visible: true },
                { field: 'Position', type: 'text', visible: false },
                { field: 'Mobile', type: 'text', visible: true },
                { field: 'DirLine', type: 'text', visible: true },
                { field: 'Email', type: 'text', visible: true },
                { field: 'NextCall', type: 'dateTime', visible: false },
                { field: 'LastCall', type: 'dateTime', visible: false }
            ]
        };

        var currentQuoteConfig = {
            sortableColumns: [
                { field: 'Id', name: 'Id' },
                { field: 'QuoteType', name: 'Type' },
                { field: 'Company', name: 'Company' },
                { field: 'LastContactDate', name: 'Last Contact' },
                { field: 'TotalPW', name: 'Return PW' }
            ],
            overdueList: [
                {
                    field: 'ContactCheckOverDue',
                    name: 'Contact'
                },
                {
                    field: 'DeadCheckOverDue',
                    name: 'Dead'
                },
                {
                    field: 'AjustCheckOverDue',
                    name: 'Ajust'
                }
            ]
        };
        var service = {
            searchPerson: searchPerson,
            searchSite: searchSite,
            searchKey: searchKey,
            searchLead: searchLead,
            searchQuote: searchQuote,
            searchCurrentQuote: searchCurrentQuote,
            currentQuoteConfig: currentQuoteConfig,
            config: searchConfig,
        };
        return service;

        function searchPerson(params) {
            return $resource(baseUrl + "/person", {}, {
                'search': {
                    method: 'POST'
                },
            }).search(params).$promise;
        }


        function searchKey(params) {
            return $resource(baseUrl + '/key').get(params).$promise;
        }


        function searchSite(params) {
            return $resource(baseUrl + "/site", {}, {
                'search': {
                    method: 'POST'
                },
            }).search(params).$promise;
        }

        function searchQuote(params) {
            return $resource(baseUrl + "/quote", {}, {
                'search': {
                    method: 'POST'
                },
            }).search(params).$promise;
        }

        function searchCurrentQuote(params) {
            return $resource(baseUrl + "/currentquote", {}, {
                'search': {
                    method: 'POST'
                },
            }).search(params).$promise;
        }

        function searchLead(params) {
            return $resource(baseUrl + "/lead", {}, {
                'search': {
                    method: 'POST'
                },
            }).search(params).$promise;
        }
    };
})();