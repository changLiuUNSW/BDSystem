(function () {
    'use strict';
    angular.module('app.resource.helper')
        .factory('callService', callService);

    callService.$inject = ['apiService', '$q'];

    function callService(apiService, $q) {

        return {
            queue: queue(),
        }

        function queue() {

            return {
                loadAll: loadAll,
                loadTelesale: loadTelesale,
                loadReport: loadReport,
                assign: assign,
                deAssign: deAssign
            };

            function loadAll(tsParam, rptParam) {
                return $q.all([loadTelesale(tsParam), loadReport(rptParam)]);
            }

            function loadTelesale(param) {
                return apiService.telesale.get(param).$promise;
            }

            function loadReport(param) {
                return apiService.contact.report(param).$promise;
            }

            function assign(param, data) {
                return apiService.telesale.assign(param, data).$promise;
            }

            function deAssign(param) {
                return apiService.telesale.deAssign(param).$promise;
            }
        }
    }
})();