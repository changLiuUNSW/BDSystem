(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('contactPersonService', contactPersonService);
    contactPersonService.$inject = ['$resource'];

    function contactPersonService($resource) {
        var baseUrl = config.ServerAddress + config.apiprefix + "contactperson";
        var  service={
            getContactPerson: getContactPerson,
            updateContact: updateContact,
            saveContactPerson: saveContactPerson,
            removeContactPerson: removeContactPerson,
            getPersonHistory: getPersonHistory,
            getAllPersonHistory: getAllPersonHistory,
            removeContactPersonHistory: removeContactPersonHistory,
            changeToNewManager: changeToNewManager
        }
        return service;

        function getContactPerson (params) {
            return $resource(baseUrl + '/:id').get(params).$promise;
        };

        function saveContactPerson(params) {
            if (!params.Id) {
                return $resource(baseUrl).save(params).$promise;
            } else {
                return $resource(baseUrl, {}, {
                    'update': {
                        method: 'PUT'
                    },
                }).update(params).$promise;
            }

        }

        function removeContactPerson (id) {
            return $resource(baseUrl + '/:id', { id: id }, {
                'remove': {
                    method: 'DELETE'
                },
            }).remove().$promise;
        }

        function updateContact(params) {
            return $resource(baseUrl + '/contact', {}, {
                'update': {
                    method: 'PUT'
                },
            }).update(params).$promise;
        };

        function getPersonHistory (params) {
            return $resource(baseUrl + '/history/:personId').get(params).$promise;
        }

        function getAllPersonHistory () {
            return $resource(baseUrl + '/history').get().$promise;
        }

        function removeContactPersonHistory (params) {
            return $resource(baseUrl + '/history', {}, {
                'remove': {
                    method: 'DELETE'
                },
            }).remove(params).$promise;
        }

        function changeToNewManager (params) {
            return $resource(baseUrl + '/history/newmanager').save(params).$promise;
        }


    }
})();