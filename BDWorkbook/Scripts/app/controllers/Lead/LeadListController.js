(function () {
    'use strict';
    angular.module('app.Lead.controllers')
        .controller('leadListCtrl', leadListCtrl);
    leadListCtrl.$inject = ['leadService', 'logger'];

    function leadListCtrl(leadService, logger) {
        var vm = this;
        vm.statusList = [];
        init();
        function getAllStatus() {
            return leadService.getAllStatus().then(function (result) {
                vm.statusList = result.data;
            });
        }

        function init() {
            getAllStatus()
            .catch(function(error) {
                logger.serverError(error);
            });

        }
    }
})();