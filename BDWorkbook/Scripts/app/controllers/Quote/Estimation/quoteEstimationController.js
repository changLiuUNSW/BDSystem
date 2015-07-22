(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteEstimationCtrl', quoteEstimationCtrl);
    quoteEstimationCtrl.$inject =
        ['quoteCostEstService','quoteCostService', '$stateParams', 'logger', '$q', '$modal', '$scope', 'calculateFix', 'checkSelectEmpty'];
    function quoteEstimationCtrl
        (quoteCostEstService,quoteCostService, $stateParams, logger, $q, $modal, $scope, calculateFix, checkSelectEmpty) {
        var vm = this;
        vm.cost = undefined;
        vm.loading = undefined;
        vm.currentProcess = undefined;
        vm.processBtns = [
            {
            text: 'Quote Details',
            code: 'QD',
        }, {
            text: 'Costings Additional',
            code: 'CA',
            initial: function () {
                vm.costAddTab = 'equ';
            }
           
        }, {
            text: 'Areas to clean & exclude',
            code: 'ACE',
            initial: function () {
                vm.areasTab = 'include';
            }
           
        }, {
            text: 'Quad Labour',
            code: 'QL',
            initial: function () {
                vm.quadTab = 'est';
            }
        }, {
            text: 'Optional Services / Supplies',
            code: 'OSS',
            initial: function () {
                vm.optionalTab = 'period';
            }
        }];
        vm.selectQs = selectQs;
        vm.selectIndustryType = selectIndustryType;
        vm.processSelected = processSelected;
        vm.searchQuoteSource = searchQuoteSource;
        vm.searchIndustryType = searchIndustryType;
        vm.industryList = undefined;
        vm.workWithInList = quoteCostEstService.workWithInList;
        vm.specList = quoteCostEstService.specList;
        vm.liabilityList = undefined;
        vm.regionList = undefined;
        vm.quoteSourceList = undefined;
        vm.selectLiability = selectLiability;
        vm.selectRegion = selectRegion;
        vm.selectDays = selectDays;
        vm.checkedFreq = checkedFreq;

        function processSelected(process) {
            vm.currentProcess = process.code;
            if (process.initial)process.initial();
        }
        
      
        function selectQs(qs) {
            if (qs) {
                vm.cost.QuoteSourceId = qs.Id;
            } else {
                vm.cost.QuoteSourceId = null;
            }
           
        }
        
        function selectIndustryType(type) {
            if (type) {
                vm.cost.IndustryTypeId = type.Id;
            } else {
                vm.cost.IndustryTypeId = null;
            }
            
        }
        

        function searchQuoteSource(keyword) {
            quoteCostEstService.searchQuoteSource(keyword).then(function (result) {
                vm.quoteSourceList = result.data;
            }, function (error) {
                logger.serverError(error);
            });
        }

        
        function searchIndustryType(keyword) {
            quoteCostEstService.searchIndustryType(keyword).then(function (result) {
                vm.industryList = result.data;
            }, function (error) {
                logger.serverError(error);
            });
        };

      
        function selectLiability(liability) {
            if (liability) {
                vm.cost.PublicLiabilityId = liability.Id;
            } else {
                vm.cost.PublicLiabilityId = null;
            }
        }

        function selectRegion(region) {
            if (region) {
                vm.cost.StandardRegionId = region.Id;
            } else {
                vm.cost.StandardRegionId = null;
            }
        }

        function selectDays() {
            var count=0;
            var days = vm.cost.DayOfClean;
            for (var day in days) {
                if (days[day]) count++;
            }
            vm.cost.DaysCleanPerWeek = count;
            resetFreq();
        }

        

        function checkedFreq(current) {
            if (current == 'fortnightly' && vm.cost.Fortnightly) {
                vm.cost.Monthly = undefined;
            }
            if (current == 'monthly' && vm.cost.Monthly) {
                vm.cost.Fortnightly = undefined;
            }
            resetDays();
        }

        function resetFreq() {
            vm.cost.Fortnightly = undefined;
            vm.cost.Monthly = undefined;
        }

        function resetDays() {
            var days = vm.cost.DayOfClean;
            for (var day in days) {
                days[day]=undefined;
            }
            vm.cost.DaysCleanPerWeek = 0;
        }


        vm.costAddTab = undefined;
        vm.leapYearAlloc = true;
        vm.picnicDay = true;
        vm.addedEquipments = undefined;
        vm.addedToiletries = undefined;
        
        vm.selectedOM = undefined;
      

        vm.selectFeq = undefined;

        vm.chooseOM = chooseOM;
        function chooseOM() {
            var om = vm.selectedOM;
            vm.currentOmText = om.text;
            vm.currentOmType = om.type;
            vm.cost.GmCode = om.gm.code;
            vm.cost.GmText = om.gm.text;
            vm.cost.GGmCode = om.ggm.code;
            vm.cost.GGmText = om.ggm.text;
        };

        vm.equiHandle = equiHandle;
        function equiHandle(handleType) {
            switch (handleType) {
                case 'new':
                    openEditorModal('tpl/Quote/Estimation/Modals/modal.quote.estimation.equipments.html', 'modalQuoteCostEstCostingEquipmentCtrl', 'list', 'equipment');
                    break;
                case 'edit':
                    if (vm.checkedEquipments.length <= 0) {
                        logger.error('Select at least one equipment, please.', 'ERROR!');
                        break;
                    }
                    quoteCostEstService.setEditingEqus(vm.checkedEquipments);
                    openEditorModal('tpl/Quote/Estimation/Modals/modal.quote.estimation.equipments.html', 'modalQuoteCostEstCostingEquipmentCtrl', 'editor', 'equipment');
                    vm.checkedEquipments = [];
                    break;
                case 'del':
                    if (vm.checkedEquipments.length <= 0) {
                        logger.error('Select at least one equipment, please.', 'ERROR!');
                        break;
                    }
                    for (var i = 0; i < vm.checkedEquipments.length; i++) {
                        var idx = vm.addedEquipments.indexOf(vm.checkedEquipments[i]);
                        vm.addedEquipments.splice(idx, 1);
                        quoteCostEstService.setCurrentEquipments(vm.addedEquipments);
                    }
                    vm.checkedEquipments = [];
                    break;
                default: break;
            }
        };






        vm.checkedEquipments = [];
        vm.checkedToiletries = [];
        vm.selPeriodicals = [];
        vm.selCleanAreas = [];
        vm.selExclAreas = [];
        vm.checkedLaboursPeriodicals = [];

        vm.checkedLabours = [];
        vm.checkMachine = checkMachine;
        vm.checkToi = checkToi;
        vm.checkPeriodical = checkPeriodical;
        vm.checkCleanAreas = checkCleanAreas;
        vm.checkExclAreas = checkExclAreas;
        vm.checkedLaboursPeriodicals = checkLaboursPeriodicals;
        vm.addedPeriodicals = [];
        vm.cleanAreas = [];
        vm.toiletriesHandle = toiletriesHandle;
        vm.periodicalsHandle = periodicalsHandle;
        vm.addClearArea = addClearArea;
        vm.editClearArea = editClearArea;
    

       

 
        vm.calculatePeriodicalsCost = calPeriodicalsCost;
        vm.showLabourCost = true;
        vm.allowancesShow = false;
        vm.showPeriodicalsCost = true;
        vm.oncostsShow = false;
        vm.othercostsShow = false;
        vm.labours = [];
        vm.selectLabours =[];
        vm.labourEstInfoAdd = labourEstInfoAdd;
        vm.labourEstInfoEdit = labourEstInfoEdit;
        vm.labourPdAdd = labourPdAdd;
        vm.addedLabourPeriodicals = [];
        vm.addedOptionalPds = [];
        vm.addedOptionalSupps = [];
        vm.optionalPdHandle = optionalPdHandle;
        vm.optionalSupHandle = optionalSupHandle;
        vm.quadTab = undefined;
        vm.optionalTab = undefined;
        vm.areasTab = undefined;
        vm.quadSummaryCost = [];
        vm.labourWdaysTotal = 0;
        vm.labourSatTotal = 0;
        vm.labourSunTotal = 0;
        vm.labourHoliTotal = 0;
        vm.addExcludeArea = addExcludeArea;
        vm.editExcludeArea = editExcludeArea;
        vm.checkLabours = checkLabours;
        vm.labourEstInfoDel = labourEstInfoDel;
        vm.exculdeAreas = [];
        vm.fixedDateRangeChanged = fixedDateRangeChanged;
        vm.fixedDateRangeFrom = undefined;
        vm.fixedDateRangeTo = undefined;
        vm.optionalPdAdd = optionalPdAdd;
        vm.optionalSupAdd = optionalSupAdd;
        vm.saveDraft = saveDraft;
    

        init();
        //-----------
        function init() {
            vm.loading = true;
            loadCost().then(
                 $q.all([quoteCostEstService.getPublicLiability(), quoteCostEstService.getStandrdRegions()]).then(function (results) {
                     vm.liabilityList = results[0].data;
                     vm.regionList = results[1].data;
                     vm.loading = false;
                     // default click detail page.
                     vm.processSelected(vm.processBtns[0]);
                     initFakeData();
                 }, function (error) {
                     logger.serverError(error);
                 })
                );
           
        };

        function loadCost() {
            return quoteCostService.getCost({ id: $stateParams.id }).then(function (result) {
                vm.cost = result.data;
                console.log(vm.cost);
            },function(error) {
                logger.serverError(error);
            });
        }
        //-----------
        function initFakeData() {

            vm.estSchool = true;
          
            vm.feqTypes = [
            {
                name:'Fnly',value:'fnly'
            }, {
                name:'Mthly',value:'mthly'
            }];
            vm.arraySpec = [{
                text: 'CLIENTS'
            }, {
                text: 'Customise Industry Spec'
            }];

            vm.quadAreaOM = [
            {
                om: 'ADEM',
                area: 'N2RP',
                text: 'Anna Demelo',
                gm: {
                    code: 'GCAM',
                    text: 'Grant Cameron'
                },
                ggm: {
                    code: 'DCUT',
                    text: 'Dobrilla Cutler'
                }
            }, {
                om: 'AMOO',
                area: 'QLGU',
                text: 'Anita Moore',
                gm: {
                    code: 'SHAL',
                    text: 'Suzie Halley'
                },
                ggm: {
                    code: 'SHAL',
                    text: 'Suzie Halley'
                }
            }, {
                om: 'CASS',
                area: 'V1FN',
                text: 'Chris Assigal',
                gm: {
                    code: 'MDRO',
                    text: 'Mark Droscher'
                },
                ggm: {
                    code: 'DTOL',
                    text: 'Dee Tolson'
                }
            }];
            
           

            vm.quadSummaryCost = {
                labourCost: {
                    pw: '128',
                    pmth: '512',
                    pa:'2048',
                },
                allowance: {
                    tiolet: {
                        rate: '12.09',
                        no: '',
                        pw: '12.09',
                        pmth: '128.21',
                        pa:'1256.23'
                    },
                    leading10: {
                        rate: '15.01',
                        no: '2',
                        pw: '13.02',
                        pmth: '156.15',
                        pa:'256.36'
                    }
                }
            };
        };
        
       
        function toiletriesHandle(handleType) {
            if (handleType == 'new') {
                var modalInstance = $modal.open({
                    templateUrl: 'tpl/Quote/Estimation/Modals/modal.quote.estimation.toiletries.html',
                    controller: 'modalQuoteCostEstCostingToiletriesCtrl',
                    controllerAs: 'vm',
                    size: 'xxl',
                    resolve: {
                        handle: function () {
                            return handleType;
                        }
                    }
                });
                modalInstance.result.then(function (result) {
                    vm.addedToiletries = quoteCostEstService.getCurrentToils();
                });
            } else if (handleType == 'edit') {
                if (vm.checkedToiletries.length <= 0) {
                    logger.error('Select at least one toiletries, please.', 'ERROR!');
                } else {
                    var beforeEditing = [];
                    var modalInstance = $modal.open({
                        templateUrl: 'tpl/Quote/Estimation/Modals/modal.quote.estimation.toiletries.html',
                        controller: 'modalQuoteCostEstCostingToiletriesCtrl',
                        controllerAs: 'vm',
                        size: 'xxl',
                        resolve: {
                            handle: function() {
                                return handleType;
                            }
                        }
                    });
                    modalInstance.opened.then(function() {
                        angular.copy(quoteCostEstService.getCurrentToils(), beforeEditing);
                    });
                    modalInstance.result.then(function(result) {
                        vm.addedToiletries = quoteCostEstService.getCurrentToils();
                    }, function(reason) {
                        quoteCostEstService.setCurrentToils(beforeEditing);
                        vm.addedToiletries = quoteCostEstService.getCurrentToils();
                    });
                }
            } 
        };
        function periodicalsHandle(handleType) {
            switch (handleType) {
                case 'new':
                    openEditorModal('tpl/Quote/Estimation/Modals/modal.quote.estimation.periodicals.html', 'modalQuoteCostEstCostingPeriodicalsCtrl', 'fillRows', 'periodical');
                    break;
                case 'edit':
                    if (vm.selPeriodicals.length <= 0) {
                        logger.error('Select at least one periodicals, please.', 'ERROR!');
                        break;
                    }
                    quoteCostEstService.setEditingPerds(vm.selPeriodicals);
                    openEditorModal('tpl/Quote/Estimation/Modals/modal.quote.estimation.periodicals.html', 'modalQuoteCostEstCostingPeriodicalsCtrl', 'editor', 'periodical');
                    vm.selPeriodicals = [];
                    break;
                case 'del':
                    if (vm.selPeriodicals.length <= 0) {
                        logger.error('Select at least one periodicals, please.', 'ERROR!');
                        break;
                    }
                    for (var i = 0; i < vm.selPeriodicals.length; i++) {
                        var idx = vm.addedPeriodicals.indexOf(vm.selPeriodicals[i]);
                        vm.addedPeriodicals.splice(idx, 1);
                    }
                    quoteCostEstService.setCurrentPeriodicals(vm.addedPeriodicals);
                    vm.selPeriodicals = [];
                    break;
                default: break;
            }
        };
        function optionalPdHandle(handleType) {
            switch (handleType) {
                case 'new':
                    var row = [
                        {
                            descritpion: '',
                            cost: '',
                            freqmarginPa: '',
                            clientPrice: ''
                        }
                    ];
                    vm.addedOptionalPds.push(row);
                    break;
                case 'del':
                    break;
                default: break;
            }
        };
        function optionalSupHandle(handle) {
            switch (handle) {
                case 'new':
                    var row = [
                        {
                            descritpion: '',
                            code: '',
                            cost: '',
                            unit: ''
                        }
                    ];
                    vm.addedOptionalSupps.push(row);
                    break;
                case 'del':
                    break;
                default: break;
            }
        };
        
        function labourEstInfoAdd() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.labour.estimation.html',
                controller: 'modalQuoteLabourEstCtrl',
                controllerAs: 'vm',
                size: 'xxl',
                resolve: {
                    handle: function () {
                        return 'new';
                    }
                }
            });
            modalInstance.result.then(function (result) {
                vm.labours = quoteCostEstService.getCurrentLabourEst();
                vm.labourWdaysTotal = 0;
                vm.labourSatTotal = 0;
                vm.labourSunTotal = 0;
                vm.labourHoliTotal = 0;
                for (var i = 0; i < vm.labours.length; i++) {
                    vm.labourWdaysTotal = calculateFix.add(vm.labours[i].WeekdayTotal, vm.labourWdaysTotal);
                    vm.labourSatTotal = calculateFix.add(vm.labours[i].SaturdayTotal, vm.labourSatTotal);
                    vm.labourSunTotal = calculateFix.add(vm.labours[i].SundayTotal, vm.labourSunTotal);
                    vm.labourHoliTotal = calculateFix.add(vm.labours[i].HolidayTotal, vm.labourHoliTotal);
                }
            });

        };
        function labourEstInfoEdit() {
            if (vm.checkedLabours.length <= 0) {
                logger.error('Select at least one labour, please.', 'ERROR!');
            } else {
                quoteCostEstService.setEditingLabourEst(vm.checkedLabours);
                var beforeEditing = [];
                var modalInstance = $modal.open({
                    templateUrl: 'tpl/Quote/Estimation/Modals/modal.labour.estimation.html',
                    controller: 'modalQuoteLabourEstCtrl',
                    controllerAs: 'vm',
                    size: 'xxl',
                    resolve: {
                        handle: function() {
                            return 'editor';
                        }
                    }
                });
                modalInstance.opened.then(function() {
                    angular.copy(quoteCostEstService.getCurrentLabourEst(), beforeEditing);
                });
                modalInstance.result.then(function(result) {
                    vm.labours = quoteCostEstService.getCurrentLabourEst();
                    vm.labourWdaysTotal = 0;
                    vm.labourSatTotal = 0;
                    vm.labourSunTotal = 0;
                    vm.labourHoliTotal = 0;
                    for (var i = 0; i < vm.labours.length; i++) {
                        vm.labourWdaysTotal = calculateFix.add(vm.labours[i].WeekdayTotal, vm.labourWdaysTotal);
                        vm.labourSatTotal = calculateFix.add(vm.labours[i].SaturdayTotal, vm.labourSatTotal);
                        vm.labourSunTotal = calculateFix.add(vm.labours[i].SundayTotal, vm.labourSunTotal);
                        vm.labourHoliTotal = calculateFix.add(vm.labours[i].HolidayTotal, vm.labourHoliTotal);
                    }
                }, function(reason) {
                    quoteCostEstService.setCurrentLabourEst(beforeEditing);
                    vm.labours = quoteCostEstService.getCurrentLabourEst();
                });
                vm.checkedLabours = [];
            }

        };
        function labourEstInfoDel() {
            if (vm.checkedLabours.length <= 0) {
                logger.error('Select at least one labour, please.', 'ERROR!');
            }
            for (var i = 0; i < vm.checkedLabours.length; i++) {
                var idx = vm.labours.indexOf(vm.checkedLabours[i]);
                vm.labours.splice(idx, 1);
            }
            quoteCostEstService.setCurrentPeriodicals(vm.labours);
            vm.labourWdaysTotal = 0;
            vm.labourSatTotal = 0;
            vm.labourSunTotal = 0;
            vm.labourHoliTotal = 0;
            for (var i = 0; i < vm.labours.length; i++) {
                vm.labourWdaysTotal = calculateFix.add(vm.labours[i].WeekdayTotal, vm.labourWdaysTotal);
                vm.labourSatTotal = calculateFix.add(vm.labours[i].SaturdayTotal, vm.labourSatTotal);
                vm.labourSunTotal = calculateFix.add(vm.labours[i].SundayTotal, vm.labourSunTotal);
                vm.labourHoliTotal = calculateFix.add(vm.labours[i].HolidayTotal, vm.labourHoliTotal);
            }
            vm.checkedLabours = [];
        };
        function labourPdAdd() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.labour.periodicals.html',
                controller: 'modalQuoteLabourPeriodicalsCtrl',
                controllerAs: 'vm',
                size: 'xxl',
                resolve: {
                    handle: function () {
                        return 'new';
                    }
                }
            });
            modalInstance.result.then(function (result) {
                vm.addedLabourPeriodicals = quoteCostEstService.getCurrentLabourPeriodcials();
                console.log(vm.addedLabourPeriodicals);
            });
        }

        
        

        
        function checkLaboursPeriodicals(c) {
            var idx = vm.checkedLaboursPeriodicals.indexOf(c);
            if (idx > -1) {
                vm.checkedLaboursPeriodicals.splice(idx, 1);
            } else {
                vm.checkedLaboursPeriodicals.push(c);
            }
        }

        function checkMachine(c) {
            var idx = vm.checkedEquipments.indexOf(c);
            if (idx > -1) {
                vm.checkedEquipments.splice(idx, 1);
            } else {
                vm.checkedEquipments.push(c);
            }
        };
        function checkToi(c) {
            var idx = vm.checkedToiletries.indexOf(c);
            if (idx > -1) {
                vm.checkedToiletries.splice(idx, 1);
            } else {
                vm.checkedToiletries.push(c);
            }
        }
        
        function checkPeriodical(c) {
            var idx = vm.selPeriodicals.indexOf(c);
            if (idx > -1) {
                vm.selPeriodicals.splice(idx, 1);
            } else {
                vm.selPeriodicals.push(c);
            }
        };
        function checkCleanAreas(c) {
            var idx = vm.selCleanAreas.indexOf(c);
            if (idx > -1) {
                vm.selCleanAreas.splice(idx, 1);
            } else {
                vm.selCleanAreas.push(c);
            }
        };

        function checkExclAreas(c) {
            var idx = vm.selExclAreas.indexOf(c);
            if (idx > -1) {
                vm.selExclAreas.splice(idx, 1);
            } else {
                vm.selExclAreas.push(c);
            }
        };
        function checkLabours(c) {
            var idx = vm.checkedLabours.indexOf(c);
            if (idx > -1) {
                vm.checkedLabours.splice(idx, 1);
            } else {
                vm.checkedLabours.push(c);
            }
        };
        function openEditorModal(url, control, type, modaleType) {
            var beforeEditing = [];
            var modalInstance = $modal.open({
                templateUrl: url,
                controller: control,
                controllerAs: 'vm',
                size: 'xxl',
                resolve: {
                    handle: function () {
                        return type;
                    }
                }
            });
            modalInstance.opened.then(function () {
                if (modaleType=='equipment')
                    angular.copy(quoteCostEstService.getCurrentEquipments(),beforeEditing);
                else if(modaleType=='supply')
                    angular.copy(quoteCostEstService.getCurrentToils(),beforeEditing);
                else if(modaleType=='periodical')
                    angular.copy(quoteCostEstService.getCurrentPeriodicals(),beforeEditing);
            });
            modalInstance.result.then(function (result) {
                console.log(result);
                if (modaleType=='equipment')
                    vm.addedEquipments = quoteCostEstService.getCurrentEquipments();
                else if(modaleType=='supply')
                    vm.addedToiletries = quoteCostEstService.getCurrentToils();
                else if(modaleType=='periodical')
                    vm.addedPeriodicals = quoteCostEstService.getCurrentPeriodicals();
            },function(reason) {
                if (modaleType == 'equipment') {
                    quoteCostEstService.setCurrentEquipments(beforeEditing);
                    vm.addedEquipments = quoteCostEstService.getCurrentEquipments();
                }
                else if (modaleType == 'supply') {
                    quoteCostEstService.setCurrentToils(beforeEditing);
                    vm.addedToiletries = quoteCostEstService.getCurrentToils();
                }
                else if (modaleType == 'periodical') {
                    quoteCostEstService.setCurrentPeriodicals(beforeEditing);
                    vm.addedToiletries = quoteCostEstService.getCurrentPeriodicals();
                }
            });
        };
        function calPeriodicalsCost(item) {
            var eachCost = item.costPerTime;
            var freq = item.freqPa;
            item.costPa = calculateFix.mul(eachCost, freq).toFixed(2);
        };
        function addClearArea() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.areas.include.html',
                controller: 'modalQuoteCostEstAreasIncludeCtrl',
                controllerAs: 'vm',
                size: 'lg',
                resolve: {
                    handle: function () {
                        return 'new';
                    }
                }
            });
            modalInstance.result.then(function (result) {
                vm.cleanAreas = quoteCostEstService.getCurrentAreasInclude();
            });
        };
        function editClearArea() {
            var beforeEditing = [];
            quoteCostEstService.setEditingAreasInclude(vm.selCleanAreas);
            vm.selCleanAreas = [];
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.areas.include.html',
                controller: 'modalQuoteCostEstAreasIncludeCtrl',
                controllerAs: 'vm',
                size: 'lg',
                resolve: {
                    handle: function () {
                        return 'editor';
                    }
                }
            });
            modalInstance.opened.then(function () {
               angular.copy(quoteCostEstService.getCurrentAreasInclude(), beforeEditing);
            });
            modalInstance.result.then(function (result) {
                vm.cleanAreas = quoteCostEstService.getCurrentAreasInclude();
            }, function (reason) {
                quoteCostEstService.setCurrentAreasInclude(beforeEditing);
                vm.cleanAreas = quoteCostEstService.getCurrentAreasInclude();
            });
        };
        function addExcludeArea() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.areas.exclude.html',
                controller: 'modalQuoteCostEstAreasExclCtrl',
                controllerAs: 'vm',
                size: 'lg',
                resolve: {
                    handle: function () {
                        return 'new';
                    }
                }
            });
            modalInstance.result.then(function (result) {
                vm.exculdeAreas = quoteCostEstService.getCurrentAreasExcl();
            });
        };
        function editExcludeArea() {
            var beforeEditing = [];
            quoteCostEstService.setEditingAreasExcl(vm.selExclAreas);
            vm.selExclAreas = [];
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.areas.exclude.html',
                controller: 'modalQuoteCostEstAreasExclCtrl',
                controllerAs: 'vm',
                size: 'lg',
                resolve: {
                    handle: function () {
                        return 'editor';
                    }
                }
            });
            modalInstance.opened.then(function () {
                angular.copy(quoteCostEstService.getCurrentAreasExcl(), beforeEditing);
            });
            modalInstance.result.then(function (result) {
                vm.exculdeAreas = quoteCostEstService.getCurrentAreasExcl();
            }, function (reason) {
                quoteCostEstService.setCurrentAreasInclude(beforeEditing);
                vm.exculdeAreas = quoteCostEstService.getCurrentAreasExcl();
            });
        }
        
        function fixedDateRangeChanged() {
            console.log();
        };
        function optionalPdAdd() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.optional.periodicals.html',
                controller: 'modalQuoteCostEstOptionalPeriodicalsCtrl',
                controllerAs: 'vm',
                size: 'lg',
                resolve: {
                    handle: function () {
                        return 'new';
                    }
                }
            });
            modalInstance.result.then(function (result) {
                vm.addedOptionalPds = quoteCostEstService.getCurrentOptionalPerds();
            });
        };

        function optionalSupAdd() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Estimation/Modals/modal.optional.supplies.html',
                controller: 'modalQuoteCostEstOptionalSuppliesCtrl',
                controllerAs: 'vm',
                size: 'lg',
                resolve: {
                    handle: function () {
                        return 'new';
                    }
                }
            });
            modalInstance.result.then(function (result) {
                vm.addedOptionalPds = quoteCostEstService.getCurrentOptionalSupps();
            });
        };
        function saveDraft() {
            console.log(vm.cost);
        }
    };

})();
