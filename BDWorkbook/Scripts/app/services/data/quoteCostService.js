(function () {
    'use strict';
    angular.module('app.resource.data')
        .factory('quoteCostService', quoteCostService);
    quoteCostService.$inject = ['$resource', 'Upload', 'logger'];
    function quoteCostService($resource, $upload,logger) {
        var baseUrl = config.ServerAddress + config.apiprefix + "cost";
        var service = {
            uploadCost: uploadCost,
            download: download,
            finalize: finalize,
            removeCosts: removeCosts,
            createCost: createCost,
            getCost:getCost

        };
        return service;


        function getCost(params) {
            return $resource(baseUrl + '/:id').get(params).$promise;
        }

        function createCost(params) {
            return $resource(baseUrl + '/create').save(params).$promise;
        }

        function removeCosts(params) {
            return $resource(baseUrl + '/bulkdelete').save(params).$promise;
        }


        function finalize(params) {
            return $resource(baseUrl + '/finalize').save(params).$promise;
        }

        function download(type) {
            switch (type.toLowerCase()) {
                case 'nz':
                    window.open(baseUrl + '/download/nz', '_blank', '');
                    break;
                case 'security':
                    window.open(baseUrl + '/download/security', '_blank', '');
                    break;
                default:
                    logger.error('Cannot find corresponding estimation workbook for '+type,'Error');
                    break;
            }
        }


        function uploadCost(cost, file) {
            return $upload.upload({
                url: baseUrl + '/upload',
                data: cost,
                file: file
            });
        }
          
        }

     
    
})();