(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('leadService', leadService);
    leadService.$inject = ['$resource', '$location', '$state'];

    function leadService($resource, $location,$state) {
        var currentDomain = config.clientAddress;
        var baseUrl = config.ServerAddress + config.apiprefix + "lead";
        var services = {
            getLeadDetailUrl: getLeadDetailUrl,
            getAllStatus: getAllStatus,
            getLead: getLead,
            updateAppointment: updateAppointment,
            updateCallback: updateCallback,
            updateQp:updateQp,
            contactNotSuccess: contactNotSuccess,
            quote: quote,
            getAllQpByZone: getAllQpByZone,
            visited:visited,
            cancel:cancel
        }
        return services;

        function getLeadDetailUrl(id) {
            return currentDomain + '/' + $state.href('lead.detail', { id: id });
        }

        function getAllQpByZone(params) {
            return $resource(baseUrl + '/qp').get(params).$promise;
        }


        function quote(id) {
            return $resource(baseUrl + '/quote/:id', { id: id }).save().$promise;
        }

        function updateAppointment(id,date) {
            return $resource(baseUrl + '/appointment/:id', {id:id}, {
                    'update': {
                        method: 'PUT'
                    }
                }
            ).update(date).$promise;
        }

        function updateQp(id, qp) {
            return $resource(baseUrl + '/qp/:id', { id: id }, {
                'update': {
                    method: 'PUT'
                }
            }).update(qp).$promise;
        }


        function updateCallback(id, date) {
            return $resource(baseUrl + '/callback/:id', { id: id }, {
                'update': {
                    method: 'PUT'
                }
            }).update(date).$promise;
        }

        function visited(params) {
            return $resource(baseUrl + '/visited').save(params).$promise;
        }

        function contactNotSuccess(params) {
            return $resource(baseUrl + '/contacted').save(params).$promise;
        }

        function cancel(params) {
            return $resource(baseUrl + '/cancel').save(params).$promise;
        }

        function getAllStatus() {
            return $resource(baseUrl + '/status').get().$promise;
        }

        function getLead(params) {
            return $resource(baseUrl + '/:id').get(params).$promise;
        }
    }
})();