(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstOptionalSuppliesCtrl', modalQuoteCostEstOptionalSuppliesCtrl);
    modalQuoteCostEstOptionalSuppliesCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteCostEstOptionalSuppliesCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.optionalSuppliesTab = handle;
        vm.toiTypes = quoteCostEstService.getToiletryTypes();
        vm.toiTypeChoosed = toiTypeChoosed;
        vm.addToils = addToils;
        vm.addToEditor = addToEditor;
        vm.confirm = confirm;
        vm.checkedTois = [];
        vm.editingToils = [];
        vm.toiletries = [];
        vm.selectToi = undefined;
        vm.confirm = confirm;
        vm.cancel = cancel;
        init();
        function init() {
            if (handle == 'editor') {
                vm.editPeriodical = true;
                vm.editingPeriodicals = quoteCostEstService.getEditingOptionalSupps();
            } else if (handle == 'list') {
                vm.addPeriodical = true;
                vm.editingPeriodicals = [];
            }
        };
        function checkTois() {
            if (vm.checkedTois.length <= 0) {
                logger.error('Select at least one toiletry, please.', 'ERROR!');
                return false;
            } else
                return true;
        };
        function toiTypeChoosed() {
            vm.toiletries = [];
            vm.loading = true;
            if (vm.selectToi == null || vm.selectToi == '') {
                vm.toiletries = [];
                vm.loading = false;
                return;
            }
            quoteCostEstService.getToiletriesByPrefix(vm.selectToi).then(function (result) {
                for (var i = 0; i < result.data.length; i++) {
                    var eachTois = {};
                    eachTois.ToiletRequisite = result.data[i];
                    eachTois.ToiletryCode = result.data[i].ItemCode;
                    eachTois.Cost = undefined;
                    eachTois.UnitsPw = undefined;
                    eachTois.Total = undefined;
                    eachTois.admin = undefined;
                    vm.toiletries.push(eachTois);
                }
                vm.loading = false;
                console.log(vm.toiletries);
            }, function (error) {
                logger.serverError(error);
                vm.loading = false;
            });
        };
        function addToils(c) {
            var idx = vm.checkedTois.indexOf(c);
            if (idx > -1) {
                vm.checkedTois.splice(idx, 1);
                c.tick = false;
            } else {
                vm.checkedTois.push(c);
            }
        };
        function addToEditor() {
            if (!checkTois()) {
                return;
            }
            var tempArray = new Array();
            var count = vm.checkedTois.length;
            angular.copy(vm.checkedTois, tempArray);
            vm.editingToils = vm.editingToils.concat(tempArray);
            logger.success('You add ' + count + ' toiletries to editor page.', 'Success');
            vm.checkedTois = [];
            vm.selectedToi = '';
        };
        function confirm() {
            var existSupplies = quoteCostEstService.getCurrentOptionalSupps();
            for (var i = 0; i < vm.editingToils.length; i++) {
                if (existSupplies.indexOf(vm.editingToils[i]) < 0) {
                    existSupplies.push(vm.editingToils[i]);
                }
            }
            
            quoteCostEstService.setCurrentOptionalSupps(existSupplies);
            console.log(existSupplies);
            $modalInstance.close('saveandclosed');
        };
        function cancel() {
            $modalInstance.dismiss('canceled');
        };
    }
})();