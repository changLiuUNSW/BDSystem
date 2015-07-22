(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstAreasRowsCtrl', modalQuoteCostEstAreasRowsCtrl);
    modalQuoteCostEstAreasRowsCtrl.$inject = ['$modalInstance','logger', '$modal', '$scope', '$state', 'handle'];

    function modalQuoteCostEstAreasRowsCtrl($modalInstance,logger, $modal, $scope, $state, handle) {
        var vm = this;
        vm.rows = undefined;
        vm.areaIncludeOrNot = handle;
        vm.apply = apply;
        vm.cancel = cancel;
        function apply() {
            $modalInstance.close(vm.rows);
        };
        function cancel() {
            $modalInstance.dismiss();
        };
    }

})();