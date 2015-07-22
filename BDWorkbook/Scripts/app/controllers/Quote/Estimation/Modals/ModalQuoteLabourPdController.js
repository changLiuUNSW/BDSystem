(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteLabourPeriodicalsCtrl', modalQuoteLabourPeriodicalsCtrl);
    modalQuoteLabourPeriodicalsCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteLabourPeriodicalsCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.loading = true;
        vm.editingPeriodicals = undefined;
        vm.modalTitle = 'Please fill the specific number of rows you want to add';
        vm.noTextLabel = 'NO. of rows for periodicals performed by Quad Labour';
        vm.workType = undefined;
        vm.rowsCount = undefined;
        vm.addRows = addRows;
        vm.del = del;
        vm.confirm = confirm;
        vm.cancel = cancel;
        init();
        function init() {
            quoteCostEstService.getAllWorkType().then(function (result) {
                vm.workType = result.data;
                vm.loading = false;
                if (handle == 'editor') {
                    vm.editPeriodsicals = true;
                    vm.editingPeriodicals = quoteCostEstService.getEditingLabourPeriodcials();
                } else if (handle == 'new') {
                    vm.addPeriodicals = true;
                    vm.editingPeriodicals = [];
                }
            }, function (error) {
                logger.serverError(error);
                vm.loading = false;
            });
            
        };
        function addRows() {
            if (vm.rowsCount > 0) {
                vm.editingPeriodicals = [];
                for (var i = 0; i < vm.rowsCount; i++) {
                    var workType = [];
                    angular.copy(vm.workType, workType);
                    var row = {
                        Description: undefined,
                        Material: undefined,
                        rate:undefined,
                        Hours: undefined,
                        workType:workType,
                        AddToPricePage: undefined,
                        Frequency: undefined,
                        ourprice: 0,
                        pricePw: 0,
                        mcostPw: 0
                    };
                    vm.editingPeriodicals.push(row);
                }
                vm.editPeriodsicals = true;
            }
        };
        function del(index) {
            if (index > -1) {
                vm.editingPeriodicals.splice(index, 1);
            }
        };
        function confirm() {
            var cost = [];
            var existRows = quoteCostEstService.getCurrentLabourPeriodcials();
            for (var i = 0; i < vm.editingPeriodicals.length; i++) {
                if (existRows.indexOf(vm.editingPeriodicals[i]) < 0) {
                    existRows.push(vm.editingPeriodicals[i]);
                }
            }
            for (i = 0; i < existRows.length; i++) {
                var row = {
                    LabourRateId: existRows[i].rate.Id,
                    LabourRate: existRows[i].rate,
                    AddToPricePage: existRows[i].AddToPricePage=="true"?true:false,
                    Hours: existRows[i].Hours,
                    Frequency: existRows[i].Frequency,
                    Material: existRows[i].Material,
                    WeeksToInvoice:52
                };
                cost.push(row);
            }
            quoteCostEstService.calculateLabourPeriodicalsCost(cost).then(function (result) {
                for (i = 0; i < result.data.length; i++) {
                    existRows[i].pricePw = result.data[i].Wage;
                    existRows[i].ourprice = calculateFix.mul(result.data[i].LabourRate.Weekdays, result.data[i].Hours);
                    existRows[i].mcostPw = calculateFix.mul(result.data[i].Material, result.data[i].Frequency);
                }
                quoteCostEstService.setCurrentLabourPeriodcials(existRows);
                $modalInstance.close('saveandclosed');
            }, function (error) {
                logger.serverError(error);
                return;
            });
        };
        function cancel() {
            $modalInstance.dismiss('canceled');
        };
    }
})();