(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('modalQuoteLabourEstCtrl', modalQuoteLabourEstCtrl);
    modalQuoteLabourEstCtrl.$inject = ['quoteCostEstService', '$modalInstance', '$modal', 'handle', '$scope', 'logger', 'calculateFix'];

    function modalQuoteLabourEstCtrl(quoteCostEstService, $modalInstance, $modal, handle, $scope, logger, calculateFix) {
        var vm = this;
        vm.loading = true;
        vm.editingEstInfos = undefined;
        vm.modalTitle = 'Please fill the specific number of rows you want to add';
        vm.noTextLabel = 'NO. of rows for estimation info';
        vm.workType = undefined;
        vm.tickLabour = tickLabour;
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
                    vm.editEstimateInfo = true;
                    vm.editingEstInfos = quoteCostEstService.getEditingLabourEst();
                } else if (handle == 'new') {
                    vm.addEstimateInfo = true;
                    vm.editingEstInfos = [];
                }
            }, function (error) {
                logger.serverError(error);
                vm.loading = false;
            });
            
        };
        function tickLabour(c) {
            var idx = vm.selectLabours.indexOf(c);
            if (idx > -1) {
                vm.selectLabours.splice(idx, 1);
            } else {
                vm.selectLabours.push(c);
            }
        };
        function addRows() {
            if (vm.rowsCount > 0) {
                vm.editingEstInfos = [];
                for (var i = 0; i < vm.rowsCount; i++) {
                    var workType = [];
                    angular.copy(vm.workType, workType);
                    var row = {
                        Description: undefined,
                        workdays: [
                        {
                            text: 'Mon-Fri',
                            value: 'weekdays',
                            checked:false
                        }, {
                            text: 'Sat',
                            value: 'sat',
                            checked: false
                        }, {
                            text: 'Sun',
                            value: 'sun',
                            checked: false
                        }, {
                            text: 'P/H',
                            value: 'holiday',
                            checked: false
                        }],
                        checkedDays:[],
                        rate: undefined,
                        dayspw: undefined,
                        workType:workType,
                        WeekdayTotal:0,
                        SaturdayTotal:0,
                        SundayTotal:0,
                        HolidayTotal:0,
                        mins: {
                            weekdays: null,
                            saturday: null,
                            sunday: null,
                            holiday: null
                        },
                        typeChoosed: function (each,index) {
                        },
                        checkdays: function (each, index) {
                            var idx = each.checkedDays.indexOf(each.workdays[index]);
                            if (idx > -1) {
                                each.checkedDays.splice(idx, 1);
                            } else {
                                each.checkedDays.push(each.workdays[index]);
                            }
                        }
                    };
                    vm.editingEstInfos.push(row);
                }
                vm.editEstimateInfo = true;
            }
        };
        function del(index) {
            if (index > -1) {
                vm.editingEstInfos.splice(index, 1);
            }
        };
        function confirm() {
            var labourCost = [];
            var existRows = quoteCostEstService.getCurrentLabourEst();
            for (var i = 0; i < vm.editingEstInfos.length; i++) {
                if (existRows.indexOf(vm.editingEstInfos[i]) < 0) {
                    existRows.push(vm.editingEstInfos[i]);
                }
            }
            for (i = 0; i < existRows.length; i++) {
                var row = {
                    LabourRateId: existRows[i].rate.Id,
                    LabourRate: existRows[i].rate,
                    DaysPerWeek: existRows[i].dayspw,
                    MinsOnWeekDays: existRows[i].mins.weekdays,
                    MinsOnSat: existRows[i].mins.saturday,
                    MinsOnSun: existRows[i].mins.sunday,
                    MinsOnHoliday: existRows[i].mins.holiday,
                    HolidayFactor: 1.5,
                    Labour:{WeeksToInvoice:52}
                };
                labourCost.push(row);
            }
            quoteCostEstService.calculateLabourCost(labourCost).then(function (result) {
                console.log(result.data);
                for (i = 0; i < result.data.length; i++) {
                    existRows[i].WeekdayTotal = result.data[i].WeekdayTotal;
                    existRows[i].SaturdayTotal = result.data[i].SaturdayTotal;
                    existRows[i].SundayTotal = result.data[i].SundayTotal;
                    existRows[i].HolidayTotal = result.data[i].HolidayTotal;
                }
                quoteCostEstService.setCurrentLabourEst(existRows);
                console.log(existRows);
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