(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstCostingPeriodicalsCtrl', modalQuoteCostEstCostingPeriodicalsCtrl);
    modalQuoteCostEstCostingPeriodicalsCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteCostEstCostingPeriodicalsCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.costPerdEditTab = handle;
        vm.rowsCount = undefined;
        vm.addRows = addRows;
        vm.addedPeriodicals = [];
        vm.delCheckedPed = delCheckedPed;
        vm.confirm = confirm;
        vm.cancel = cancel;
        init();
        function init() {
            if (handle == 'editor') {
                vm.editPerd = true;
                vm.addedPeriodicals = quoteCostEstService.getEditingPerds();
            } else if (handle == 'fillrows') {
                vm.addPerd = true;
                vm.addedPeriodicals = [];
            }
        };

        function addRows() {
            if (vm.rowsCount > 0) {
                vm.addedPeriodicals = [];
                for (var i = 0; i < vm.rowsCount; i++) {
                    var newPeriodicals = {
                        Description: undefined,
                        CostPerTime: undefined,
                        FreqPa: undefined,
                        CostPa: undefined,
                        Itemise: undefined
                    };
                    vm.addedPeriodicals.push(newPeriodicals);
                }
                vm.editPerd = true;
            }
        };
        function delCheckedPed() {
            
        };
        function confirm() {
            var existPerds = quoteCostEstService.getCurrentPeriodicals();
            for (var i = 0; i < vm.addedPeriodicals.length; i++) {
                if (existPerds.indexOf(vm.addedPeriodicals[i]) < 0) {
                    existPerds.push(vm.addedPeriodicals[i]);
                }
            }
            for (i = 0; i < existPerds.length; i++) {
                var costPa = calculateFix.mul(existPerds[i].CostPerTime, existPerds[i].FreqPa);
                existPerds[i].costPa = costPa;
            }
            quoteCostEstService.setCurrentPeriodicals(existPerds);
            $modalInstance.close('saveandclosed');
        };
        function cancel() {
            $modalInstance.dismiss('canceled');
        };
    }
})();