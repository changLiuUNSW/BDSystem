(function () {
    'use strict';
    angular.module('app.resource.data')
        .factory('quoteCostEstService', quoteCostEstService);
    quoteCostEstService.$inject = ['$filter', '$resource','calculateFix','formatter'];
    function quoteCostEstService($filter, $resource, calculateFix, formatter) {
        var baseUrl = config.ServerAddress + config.apiprefix;
        var specList = ['CLIENTS', 'Customize Industry Spec'];
        var workWithInList = ['with the first 4 weeks of service', 'with the first 2 weeks of service'];


        

        var equAndToiTypes = {
            equs: [{
                name: 'Polishers/Gas Buffers',
                prefix: 'pol',
            }, {
                name: 'Vacuum',
                prefix: 'vac'
            }, {
                name: 'Shampoo Machine',
                prefix: 'sha'
            }, {
                name: 'Sweeper',
                prefix: 'swe'
            }, {
                name: 'Auto Scrubber',
                prefix: 'scr'
            }, {
                name: 'Water Presuure',
                prefix: 'wp'
            }, {
                name: 'Miscellaneous',
                prefix: 'misc'
            }],
            tois:[
            {
                name: 'Toilet Rolls',
                prefix: 'tr',
            }, {
                name: 'Toilet Tissue (special dispeners required)',
                prefix: 'tt'
            }, {
                name: 'Facial Tissues',
                prefix: 'ft'
            }, {
                name: 'Hand Towels',
                prefix: 'ht'
            }, {
                name: 'Soap',
                prefix: 's'
            }, {
                name: 'Hand Sanitisers',
                prefix: 'hs'
            }, {
                name: 'Toilet crystals',
                prefix: 'tc'
            }, {
                name: 'Dispensers',
                prefix: 'd'
            }, {
                name: 'Dishwashing Liquid',
                prefix: 'dl'
            }, {
                name: 'First Aid',
                prefix: 'fa'
            }, {
                name: 'Other',
                prefix: 'ot'
            }, {
                name: 'Bin Liners',
                prefix: 'b'
            }]
        };

     
        var currentEquipments = [];
        var currentSupplies = [];
        
        var editEquipments = [];
        var editToils = [];

        var currentPeriods = [];
        var editingPeriods = [];

        var currentIncludeAreas = [];
        var editingIncludeAreas = [];

        var currentExclAreas = [];
        var editingExclAreas = [];

        var currentLabours = [];
        var editingLabours = [];

        var currentLabourPeriods = [];
        var editingLabourPeriods = [];

        var editingOps = [];
        var currentOps = [];

        var currentOss = [];
        var editingOss = [];

        var regCleanWeeks = undefined;
        var adminCleanWeeks = undefined;
        var typeSchool = undefined;


        var service = {
            specList: specList,
            workWithInList: workWithInList,
            getPublicLiability: getPublicLiability,
            getStandrdRegions: getStandrdRegions,
            searchQuoteSource:searchQuoteSource,
            searchIndustryType: searchIndustryType,



            getSchoolOrNot: getSchoolOrNot,
            setSchoolOrNot:setSchoolOrNot,
            getMachineTypes: getMachineTypes,
            getToiletryTypes: getToiletryTypes,
            getEquipmentsByPrefix: getEquipmentsByPrefix,
            calculateEquipmentsCost: calculateEquipmentsCost,
            calculateToiletriesCost: calculateToiletriesCost,
            calculateLabourCost:calculateLabourCost,
            getToiletriesByPrefix: getToiletriesByPrefix,
            setCurrentEquipments: setCurrentEquipments,
            getCurrentEquipments: getCurrentEquipments,
            setEditingEqus: setEditingEqus,
            getEditingEqus: getEditingEqus,
            getEditingTois: getEditingTois,
            setEditingTois: setEditingTois,
            setCurrentToils: setCurrentToils,
            getCurrentToils: getCurrentToils,
            getCurrentAreasInclude: getCurrentAreasInclude,
            setCurrentAreasInclude:setCurrentAreasInclude,
            getEditingAreasInclude: getEditingAreasInclude,
            setEditingAreasInclude: setEditingAreasInclude,
            getCurrentAreasExcl: getCurrentAreasExcl,
            setCurrentAreasExcl:setCurrentAreasExcl,
            getEditingAreasExcl: getEditingAreasExcl,
            setEditingAreasExcl: setEditingAreasExcl,
            getCurrentPeriodicals: getCurrentPeriodicals,
            setCurrentPeriodicals:setCurrentPeriodicals,
            getEditingPerds: getEditingPerds,
            setEditingPerds: setEditingPerds,
            getCurrentLabourEst: getCurrentLabourEst,
            setCurrentLabourEst:setCurrentLabourEst,
            getEditingLabourEst: getEditingLabourEst,
            setEditingLabourEst:setEditingLabourEst,
            getCurrentLabourPeriodcials:getCurrentLabourPeriodcials,
            setCurrentLabourPeriodcials:setCurrentLabourPeriodcials,
            getEditingLabourPeriodcials:getEditingLabourPeriodcials,
            setEditingLabourPeriodcials:setEditingLabourPeriodcials,
            calculateLabourPeriodicalsCost: calculateLabourPeriodicalsCost,

            getEditingOptionalPerds: getEditingOptionalPerds,
            setEditingOptionalPerds:setEditingOptionalPerds,
            getCurrentOptionalPerds:getCurrentOptionalPerds,
            setCurrentOptionalPerds: setCurrentOptionalPerds,

            getCurrentOptionalSupps:getCurrentOptionalSupps,
            setCurrentOptionalSupps:setCurrentOptionalSupps,
            getEditingOptionalSupps:getEditingOptionalSupps,
            setEditingOptionalSupps:setEditingOptionalSupps,

            getRegCleanWeeks: getRegCleanWeeks,
            getAdminCleanWeeks: getAdminCleanWeeks,
            getAllMachineTypes: getAllMachineTypes,
            getAllWorkType: getAllWorkType
        };
        return service;



        function getPublicLiability() {
            return $resource(baseUrl + 'cost/liability').get().$promise;
        }
        function getStandrdRegions() {
            return $resource(baseUrl + 'cost/standardregion').get().$promise;
        }
        function searchQuoteSource(keyword) {
            return $resource(baseUrl + 'cost/qoutesource').get({ keyword: keyword }).$promise;
        }
        function searchIndustryType(keyword,top) {
            return $resource(baseUrl + 'cost/industrytype').get({ keyword: keyword, top: top }).$promise;
        }










        function getSchoolOrNot() {
            return typeSchool;
        };
        function setSchoolOrNot(boolean) {
            typeSchool = boolean;
        };
        function getMachineTypes() {
            return equAndToiTypes.equs;
        };
        function getToiletryTypes() {
            return equAndToiTypes.tois;
        };
    

        function setCurrentEquipments(equis) {
            currentEquipments = [];
            angular.copy(equis, currentEquipments);
        };
        function getCurrentEquipments() {
            return currentEquipments;
        };
        function setEditingEqus(equis) {
            editEquipments = [];
            editEquipments = equis;
        };
        function getEditingEqus() {
            return editEquipments;
        };
        function getEditingTois() {
            return editToils;
        };
        function setEditingTois(tois) {
            editToils = [];
            editToils = tois;
        };
        function getCurrentToils() {
            return currentSupplies;
        };
        function setCurrentToils(supps) {
            currentSupplies = [];
            angular.copy(supps, currentSupplies);
        };
        function getCurrentPeriodicals() {
            return currentPeriods;
        };
        function setCurrentPeriodicals(periods) {
            currentPeriods = [];
            angular.copy(periods, currentPeriods);
        };
        function getEditingPerds() {
            return editingPeriods;
        };
        function setEditingPerds(periods) {
            editingPeriods = [];
            editingPeriods = periods;
        };
        function getCurrentAreasInclude() {
            return currentIncludeAreas;
        };
        function setCurrentAreasInclude(areas) {
            currentIncludeAreas = [];
            angular.copy(areas, currentIncludeAreas);
        };
        function getEditingAreasInclude() {
            return editingIncludeAreas;
        };
        function setEditingAreasInclude(areas) {
            editingIncludeAreas = [];
            editingIncludeAreas = areas;
        };
        function getCurrentAreasExcl() {
            return currentExclAreas;
        };
        function setCurrentAreasExcl(areas) {
            currentExclAreas = [];
            angular.copy(areas, currentExclAreas);
        };
        function getEditingAreasExcl() {
            return editingExclAreas;
        };
        function setEditingAreasExcl(areas) {
            editingExclAreas = [];
            editingExclAreas = areas;
        };
        function getCurrentQuote() {
            return currentQuote;
        };
        function getRegCleanWeeks() {
            return regCleanWeeks;
        };
        function getAdminCleanWeeks() {
            return adminCleanWeeks;
        };

        function getCurrentLabourEst() {
            console.log(currentLabours);
            return currentLabours;
        };
        function setCurrentLabourEst(labours) {
            currentLabours = [];
            angular.copy(labours, currentLabours);
            
        };
        function getEditingLabourEst() {
            return editingLabours;
        };
        function setEditingLabourEst(labours) {
            editingLabours = [];
            editingLabours = labours;
        };
        function getCurrentLabourPeriodcials() {
            return currentLabourPeriods;
        };
        function setCurrentLabourPeriodcials(periods) {
            currentLabourPeriods = [];
            angular.copy(periods, currentLabourPeriods);
        };
        function getEditingLabourPeriodcials() {
            return editingLabourPeriods;
        };
        function setEditingLabourPeriodcials(periods) {
            editingLabourPeriods = [];
            editingLabourPeriods = periods;
        };

        function getCurrentOptionalPerds() {
            return currentOps;
        };
        function setCurrentOptionalPerds(periods) {
            currentOps = [];
            angular.copy(periods, currentOps);
        };
        function getEditingOptionalPerds() {
            return editingOps;
        };
        function setEditingOptionalPerds(periods) {
            editingOps = [];
            editingOps = periods;
        };

        function getCurrentOptionalSupps() {
            return currentOss;
        };
        function setCurrentOptionalSupps(supplies) {
            currentOss = [];
            angular.copy(supplies, currentOss);
        };
        function getEditingOptionalSupps() {
            return editingOss;
        };
        function setEditingOptionalSupps(supplies) {
            editingOss = [];
            editingOss = supplies;
        };

        function getAllMachineTypes() {
            return $resource(baseUrl + 'cost/equipment').get();
        };
        function getEquipmentsByPrefix(prefix) {
            return $resource(baseUrl + 'cost/equipment').get({ prefix: prefix }).$promise;
        };
        function calculateEquipmentsCost(equipments) {
            return $resource(baseUrl + "quote/calculator/equipment").save(equipments).$promise;
        };
        function calculateToiletriesCost(toiletries) {
            return $resource(baseUrl + "QuoteEstimation").save(toiletries).$promise;
        }
        function getToiletriesByPrefix(prefix) {
            return $resource(baseUrl + 'cost/toiletry').get({ prefix: prefix }).$promise;
        };
        function getAllWorkType() {
            return $resource(baseUrl + 'Cost/rate/labour').get().$promise;
        };
        function calculateLabourCost(labours) {
            return $resource(baseUrl + "quote/calculator/labourWage").save(labours).$promise;
        };
        function calculateLabourPeriodicalsCost(cost) {
            return $resource(baseUrl + "quote/calculator/labourPeriodical").save(cost).$promise;
        };
    }

})();