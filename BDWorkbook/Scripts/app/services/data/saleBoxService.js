(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('saleBoxService', saleBoxService);
    saleBoxService.$inject = ['$resource'];
    function saleBoxService($resource) {
        var baseUrl = config.ServerAddress + config.apiprefix + "salesbox";
        var service= {
            getZonesByState: getZonesByState,
            getByZoneAndState: getByZoneAndState,
            getSalesbox: getSalesbox
        }
        return service;

        function getSalesbox(params) {
            return $resource(baseUrl).get(params).$promise;
        }

        ///get zone list by state
        function getZonesByState (salesboxes, state) {
            if (!salesboxes || salesboxes.constructor !== Array)
                return salesboxes;

            var zones = [];
            var filtered = getByState(salesboxes, state);

            filtered.forEach(function (salesbox) {
                if (zones.indexOf(salesbox.Sales_Pref) < 0)
                    zones.push(salesbox.Sales_Pref);
            });

            return zones;
        }

        ///get salebox object list by state
        function getByState (salesboxes, state) {
            return salesboxes.filter(function (salesbox) {
                if (salesbox.State === state)
                    return true;

                return false;
            });
        }

        function getByZoneAndState (salesboxes, zone, state) {
            if (!salesboxes || salesboxes.constructor !== Array)
                return salesboxes;

            return salesboxes.filter(function (salesbox) {
                if (salesbox.Sales_Pref === zone &&
                    salesbox.State === state)
                    return true;

                return false;
            });
        }
    }
})();