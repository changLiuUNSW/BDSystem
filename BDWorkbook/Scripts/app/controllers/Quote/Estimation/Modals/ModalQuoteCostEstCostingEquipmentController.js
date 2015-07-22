(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstCostingEquipmentCtrl', modalQuoteCostEstCostingEquipmentCtrl);
    modalQuoteCostEstCostingEquipmentCtrl.$inject = ['quoteCostEstService', '$modalInstance', 'calculateFix', '$modal', 'handle', '$scope', 'logger'];

    function modalQuoteCostEstCostingEquipmentCtrl(quoteCostEstService, $modalInstance, calculateFix, $modal, handle,$scope, logger) {
        var vm = this;
        vm.costEquEditTab = handle;

        vm.loading = false;
        vm.handleTypeNew = true;
        vm.machineTypes = quoteCostEstService.getMachineTypes();
        vm.machineTypeChoosed = machineTypeChoosed;
        vm.handleType = handle;
        vm.chooseEqui = chooseEqui;
        vm.addEquipment = addEquipment;
        vm.copyToEditing = copyToEditing;
        vm.checkEqus = checkEqus;
        vm.del = del;
        vm.confirm = confirm;
        vm.cancel = cancel;

        vm.selectEquipment = undefined;
        vm.selectedEqu = undefined;
        vm.equipments = undefined;
        vm.editingEquipments = undefined;
        vm.checkedEquipments = [];
        init();

        function cancel() {
            $modalInstance.dismiss();
        };
        function init() {
            if (vm.handleType == 'editor') {
                vm.editEqu = true;
                vm.editingEquipments = quoteCostEstService.getEditingEqus();
            } else if (vm.handleType == 'list') {
                vm.addEqus = true;
                vm.editingEquipments = [];
            }
        };
        function machineTypeChoosed() {
            vm.equipments=[];
            vm.loading = true;
            if (vm.selectEquipment == null || vm.selectEquipment == '') {
                vm.loading = false;
                return;
            }
            quoteCostEstService.getEquipmentsByPrefix(vm.selectEquipment).then(function (result) {
                for (var i = 0; i < result.data.length; i++) {
                    var eachEqu = {};
                    eachEqu.Equipment = result.data[i];
                    eachEqu.EquipmentCode = result.data[i].EquipmentCode;
                    eachEqu.Cost = result.data[i].Cost;
                    eachEqu.UnitsFromSubby = undefined;
                    eachEqu.UnitsFromQuad = undefined;
                    eachEqu.Total = undefined;
                    vm.equipments.push(eachEqu);
                }
                vm.loading = false;
            }, function (error) {
                logger.serverError(error);
                vm.loading = false;
            });
        };
        function chooseEqui(equi) {
            vm.currentEquiText = equi.text;
        };
        function checkEqus() {
            if (vm.checkedEquipments.length <= 0) {
                logger.error('Select at least one equipment, please.', 'ERROR!');
                return false;
            } else
                return true;
        };
        function addEquipment(c) {
            var idx = vm.checkedEquipments.indexOf(c);
            if (idx > -1) {
                vm.checkedEquipments.splice(idx, 1);
                c.tick = false;
            } else {
                vm.checkedEquipments.push(c);
            }
        };
        function copyToEditing() {
            if (!checkEqus()) {
                return;
            }
            var tempArray = [];
            var count = vm.checkedEquipments.length;
            angular.copy(vm.checkedEquipments, tempArray);
            vm.editingEquipments = vm.editingEquipments.concat(tempArray);
            logger.success('You add ' + count + ' equipments to editor page.', 'Success');
            vm.checkedEquipments = [];
        };
        function del(index) {
            if (index > -1) {
                vm.editingEquipments.splice(index, 1);
            }
        };
        function confirm() {
            var existEqus = quoteCostEstService.getCurrentEquipments();
            for (var i = 0; i < vm.editingEquipments.length; i++) {
                if (existEqus.indexOf(vm.editingEquipments[i]) < 0) {
                    existEqus.push(vm.editingEquipments[i]);
                }
            }
            quoteCostEstService.calculateEquipmentsCost(existEqus).then(function (result) {
                quoteCostEstService.setCurrentEquipments(result.data);
                $modalInstance.close('saveandclosed');
            }, function (error) {
                logger.serverError(error);
                return;
            });
        };
    }
})();