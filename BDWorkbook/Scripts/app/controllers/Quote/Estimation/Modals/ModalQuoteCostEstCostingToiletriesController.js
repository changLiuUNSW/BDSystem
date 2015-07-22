(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteCostEstCostingToiletriesCtrl', modalQuoteCostEstCostingToiletriesCtrl);
    modalQuoteCostEstCostingToiletriesCtrl.$inject = ['quoteCostEstService', '$modalInstance', 'calculateFix', '$modal', 'handle','logger'];

    function modalQuoteCostEstCostingToiletriesCtrl(quoteCostEstService, $modalInstance, calculateFix, $modal, handle, logger) {
        var vm = this;

        vm.estSchool = quoteCostEstService.getSchoolOrNot();;
        vm.toiTypes = quoteCostEstService.getToiletryTypes();
        vm.toiTypeChoosed = toiTypeChoosed;
        vm.costToiEditTab = handle;
        vm.addToils = addToils;
        vm.addToEditor = addToEditor;
        vm.delCheckedToi = delCheckedToi;
        vm.confirm = confirm;
        vm.checkedTois = [];
        vm.editingToils = [];
        vm.toiletries = [];
        vm.selectToi = undefined;
        vm.cancel = cancel;
        init();

        function cancel() {
            $modalInstance.dismiss();
        };
        function checkTois() {
            if (vm.checkedTois.length <= 0) {
                logger.error('Select at least one toiletry, please.', 'ERROR!');
                return false;
            } else
                return true;

        };
        function init() {
            if (vm.handleType == 'editor') {
                vm.editTois = true;
                vm.editingToils = quoteCostEstService.getEditingTois();
            } else if (vm.handleType == 'new') {
                vm.addTois = true;
                vm.editingToils = [];
            }
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
        function delCheckedToi(index) {
            if (index > -1) {
                vm.editingToils.splice(index, 1);
            }
        };
        function confirm() {
            var existTois = quoteCostEstService.getCurrentToils();
            for (var i = 0; i < vm.editingToils.length; i++) {
                if (existTois.indexOf(vm.editingToils[i]) < 0) {
                    existTois.push(vm.editingToils[i]);
                }
            }
            var checkedAdmin = [];
            for (i = 0; i < existTois.length; i++) {
                checkedAdmin.push(existTois[i].admin);
            }
            quoteCostEstService.calculateToiletriesCost(existTois).then(function (result) {
                for (var j = 0; j < existTois.length; j++) {
                    result.data[j].admin = checkedAdmin[j];
                }
                quoteCostEstService.setCurrentToils(result.data);
                $modalInstance.close('saveandclosed');
            }, function (error) {
                logger.serverError(error);
                return;
            });
        };
    };
})();