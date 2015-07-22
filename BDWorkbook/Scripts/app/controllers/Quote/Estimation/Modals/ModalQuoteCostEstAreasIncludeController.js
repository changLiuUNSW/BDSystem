(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstAreasIncludeCtrl', modalQuoteCostEstAreasIncludeCtrl);
    modalQuoteCostEstAreasIncludeCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteCostEstAreasIncludeCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.editingIncludeAreas = undefined;
        vm.rowsCount = undefined;
        vm.addRows = addRows;
        vm.confirm = confirm;
        vm.cancel = cancel;
        init();
        function init() {
            if (handle == 'editor') {
                vm.editAreaInc = true;
                vm.editingIncludeAreas = quoteCostEstService.getEditingAreasInclude();
            } else if (handle == 'new') {
                vm.addAreaInc = true;
                vm.editingIncludeAreas = [];
            }
        };

        function addRows() {
            if (vm.rowsCount > 0) {
                vm.editingIncludeAreas = [];
                for (var i = 0; i < vm.rowsCount; i++) {
                    var newArea = {
                        Description: undefined
                    };
                    vm.editingIncludeAreas.push(newArea);
                }
                vm.editAreaInc = true;
            }
        };
        function delCheckedPed() {
            
        };
        function confirm() {
            var existAreas = quoteCostEstService.getCurrentAreasInclude();
            for (var i = 0; i < vm.editingIncludeAreas.length; i++) {
                if (existAreas.indexOf(vm.editingIncludeAreas[i]) < 0) {
                    existAreas.push(vm.editingIncludeAreas[i]);
                }
            }
            quoteCostEstService.setCurrentAreasInclude(existAreas);
            $modalInstance.close('saveandclosed');
        };
        function cancel() {
            $modalInstance.dismiss('canceled');
        };
    }
})();