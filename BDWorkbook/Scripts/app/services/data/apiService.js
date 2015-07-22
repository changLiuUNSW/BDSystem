(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('apiService', apiService);
    apiService.$inject = ['$http', '$resource'];

    function apiService($http, $resource) {
        var url = config.ServerAddress + config.apiprefix;
        return {
            //contact table
            contact: $resource(url + 'contact/:contactId/:param/:paramId/:param2/:param2Id',
            {},
            {
                'nextCall': {
                    method: 'POST',
                    params: { param: 'call/next' }
                },

                'endCall': {
                    method: 'POST',
                    params: { param: 'call/end' }
                },

                'assignContactPerson': {
                    method: 'PUT',
                    params: {
                        param: 'AssignContactPerson',
                    }
                },
                'report': {
                    method: 'GET',
                    params: { param: 'report' }
                },
                'reportHistory': {
                    method: 'GET',
                    params: { param: 'report', param2: 'history' }
                },
                'updateNote': {
                    method: 'PUT',
                    params: {
                        param: 'note'
                    }
                },
                'addCallLine': {
                    method: 'POST',
                    params: { param: 'CallLine' }
                },
                'removeCallLine': {
                    method: 'Delete',
                    params: {
                        param: 'CallLine',
                        paramId: '@Id'
                    }
                }
            }),

            //site
            search: $resource(url + 'search/:table', {}, {
                'externalManager': {
                    method: 'POST',
                    params: {
                        table: 'externalManager',
                    }
                }
            }),

            contactPerson: $resource(url + 'contactperson/:param', {}, {
                'pendingUpdate': {
                    method: 'PUT',
                    params: {
                        param: 'pendingUpdate'
                    }
                }
            }),


            //lead table
            lead: $resource(url + 'lead/:id',
            {},
            {
                'update': {
                    method: 'PUT'
                }
            }),

            //alloction table
            allocation: $resource(url + 'allocation/:param/:id',
            {},
            {
                'update': {
                    method: 'PUT'
                }
            }),

            //zone table
            salesbox: $resource(url + 'salesbox/:id/:param',
            {},
            {
                'update': {
                    method: 'PUT'
                },
                'getZones': {
                    method: 'GET',
                    params: { param: 'zone' }
                },
                'getStates': {
                    method: 'GET',
                    params: { param: 'state' }
                },
                'zoneAllocation': {
                    method: 'GET',
                    params: { param: 'zoneAllocation' }
                }
            }),

            //quad personal lead priority
            leadPersonal: $resource(url + 'leadPersonal/:id/:param/:paramId',
            {},
            {
                'names': {
                    method: 'GET',
                    params: { param: 'names' }
                },
                'update': {
                    method: 'PUT'
                },
                'batchUpdate': {
                    method: 'PUT',
                    params: {
                        param: 'batch'
                    }
                },
                'getShift': {
                    method: 'GET',
                    params: {
                        param: 'shift'
                    }
                },
                'updateShift': {
                    method: 'PUT',
                    params: {
                        param: 'shift'
                    }
                }
            }),

            //telesale
            telesale: $resource(url + 'telesale/:id/:param/:paramId',
            { id: '@id', paramId: '@paramId' },
            {
                'update': {
                    method: 'PUT'
                },
                'assign': {
                    method: 'POST',
                    params: { param: 'assign' }
                },
                'deAssign': {
                    method: 'DELETE',
                    params: { param: 'deassign' }
                }
            }),

            quoteSpec: $resource(url + 'quotespec/:specId/:param/:paramId',
            {}, {
                'updateFrequency': {
                    method: 'PUT',
                    params: {
                        param: 'SpecItem'
                    }
                }
            }),

            quoteCost: $resource(url + 'cost/:id/:param/:paramId', {
            
            }, {
                equipments: {
                    method: 'GET',
                    params: {
                        param: 'equipment'
                    }
                }
            }),

            user: $resource(url + 'user/:id/:param/:paramId', {}, {

            })
    };
    }
})();