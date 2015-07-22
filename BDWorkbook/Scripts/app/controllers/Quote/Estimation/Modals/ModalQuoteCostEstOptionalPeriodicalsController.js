(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstOptionalPeriodicalsCtrl', modalQuoteCostEstOptionalPeriodicalsCtrl);
    modalQuoteCostEstOptionalPeriodicalsCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteCostEstOptionalPeriodicalsCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.modalTitle = 'Please fill the specific number of rows you want to add';
        vm.noTextLabel = 'NO. of rows for optional periodicals info';
        vm.optionalPeriodicalsTab = handle;
        vm.rowsCount = undefined;
        vm.addRows = addRows;
        vm.editingPeriodicals = [];
        vm.delCheckedPed = delCheckedPed;
        vm.confirm = confirm;
        vm.cancel = cancel;
        init();
        function init() {
            if (handle == 'editor') {
                vm.editPeriodical = true;
                vm.editingPeriodicals = quoteCostEstService.getEditingOptionalPerds();
            } else if (handle == 'list') {
                vm.addPeriodical = true;
                vm.editingPeriodicals = [];
            }
        };

        function addRows() {
            if (vm.rowsCount > 0) {
                vm.editingPeriodicals = [];
                for (var i = 0; i < vm.rowsCount; i++) {
                    var newPeriodicals = {
                        description: undefined,
                        cost: undefined,
                        margin: undefined,
                        clientPrice: undefined
                    };
                    vm.editingPeriodicals.push(newPeriodicals);
                }
                vm.editPeriodical = true;
            }
        };
        function delCheckedPed() {

        };
        function confirm() {
            var existPerds = quoteCostEstService.getCurrentOptionalPerds();
            for (var i = 0; i < vm.editingPeriodicals.length; i++) {
                if (existPerds.indexOf(vm.editingPeriodicals[i]) < 0) {
                    existPerds.push(vm.editingPeriodicals[i]);
                }
            }
            for (i = 0; i < existPerds.length; i++) {
                var clientPrice = calculateFix.add(existPerds[i].cost, existPerds[i].margin);
                existPerds[i].clientPrice = clientPrice;
            }
            quoteCostEstService.setCurrentOptionalPerds(existPerds);
            console.log(existPerds);
            $modalInstance.close('saveandclosed');
        };
        function cancel() {
            $modalInstance.dismiss('canceled');
        };
    }
})();