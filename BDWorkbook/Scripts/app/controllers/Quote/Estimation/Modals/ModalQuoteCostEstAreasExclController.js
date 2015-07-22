(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstAreasExclCtrl', modalQuoteCostEstAreasExclCtrl);
    modalQuoteCostEstAreasExclCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteCostEstAreasExclCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.editingExclAreas = undefined;
        vm.rowsCount = undefined;
        vm.addRows = addRows;
        vm.confirm = confirm;
        vm.cancel = cancel;
        init();
        function init() {
            if (handle == 'editor') {
                vm.editAreaExcl = true;
                vm.editingExclAreas = quoteCostEstService.getEditingAreasExcl();
            } else if (handle == 'new') {
                vm.addAreaExcl = true;
                vm.editingExclAreas = [];
            }
        };

        function addRows() {
            if (vm.rowsCount > 0) {
                vm.editingExclAreas = [];
                for (var i = 0; i < vm.rowsCount; i++) {
                    var newArea = {
                        Description: undefined
                    };
                    vm.editingExclAreas.push(newArea);
                }
                vm.editAreaExcl = true;
            }
        };
        function delCheckedPed() {
            
        };
        function confirm() {
            var existAreas = quoteCostEstService.getCurrentAreasExcl();
            for (var i = 0; i < vm.editingExclAreas.length; i++) {
                if (existAreas.indexOf(vm.editingExclAreas[i]) < 0) {
                    existAreas.push(vm.editingExclAreas[i]);
                }
            }
            quoteCostEstService.setCurrentAreasExcl(existAreas);
            $modalInstance.close('saveandclosed');
        };
        function cancel() {
            $modalInstance.dismiss('canceled');
        };
    }
})();