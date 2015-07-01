(function() {
    'use strict';
    angular.module('app.resource.helper')
        .factory('formatter', formatter);
    formatter.$inject = ['$filter'];
    function formatter($filter) {
        var service = {
            Date: parseDate
        };
        return service;
        function parseDate(date) {
            return $filter('date')(date, 'yyyy-MM-dd');
        }
    }
})();