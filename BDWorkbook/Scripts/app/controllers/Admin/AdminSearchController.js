(function() {
    'use strict';
    angular.module('app.Admin.Search.controllers')
    .controller('AdminSearchCtrl', adminSearchCtrl);
    adminSearchCtrl.$inject = ['$scope', '$stateParams', 'logger', 'searchService', 'ngTableParams', '$state', '$filter'];
    function adminSearchCtrl($scope, $stateParams, logger, searchService, ngTableParams, $state, $filter) {
        var predefine = [];
        //default biz type set to cleaning
        var businessType = $stateParams.bizType ? $stateParams.bizType.toLowerCase() : 'cleaning';
        $scope.dropdownHeaders = [];
        $scope.showHeaders = [];
        $scope.dropdownSettings = {
            dynamicTitle: false,
            scrollableHeight: '250px',
            scrollable: true
        };
        $scope.panelHeader = {
            name: 'Loading..',
            colorClass: ''
        };
        $scope.paging = {
            currentPage: 1,
            pageSize: 15,
            totalItems: 0
        };
        //TODO:Bug with ng table if we add loading animation
        //$scope.loading = false;
        $scope.sorter = {
            sortColumn: null,
            order: null
        };

        //default jump to 'address table'
        $scope.table = $stateParams.group ? $stateParams.group.toLowerCase() : 'site';
        if (searchService.config.hasOwnProperty($scope.table)) {
            predefine = searchService.config[$scope.table];
            $scope.panelHeader = {
                name: $scope.table,
                colorClass: 'bg-info'
            };
        } else {
            logger.error("Cannot find this path", "Path Error");
            $state.go('access.404');
        }
        //Initialize show/hide columns and filters
        $scope.columns = angular.copy(predefine);
        angular.forEach($scope.columns, function (v, k) {
            var filter = {};
            filter[k] = v.type;
            if (!('hidden' in v && v['hidden'])) {
                $scope.dropdownHeaders.push({ id: k, label: v.field });
                if (v['visible']) $scope.showHeaders.push({ id: k });
            }
            angular.extend(v, { term: null, filter: filter });
        });
        $scope.bizTypeColumn = $filter('filter')($scope.columns, { field: 'BusinessType' })[0];
        $scope.onItemSelect = onItemSelect;
        $scope.url = url;
        $scope.setDataListPageSize = setDataListPageSize;
        $scope.dataListPageChanges = dataListPageChanges;
        $scope.selectType = selectType;
        $scope.loadDataList = loadDataList;
        $scope.setDateValue = setDateValue;
        $scope.tableParams = new ngTableParams(
            {
                counts: []
            },
            {
                getData: function ($defer, params) {
                    //Filters
                    angular.forEach($scope.columns, function (v, k) {
                        v.term = null;
                    });
                    angular.forEach(params.filter(), function (v, k) {

                        $scope.columns[k].term = v;
                    });
                    //Sorter
                    angular.forEach(params.sorting(), function (v, k) {
                        $scope.sorter.sortColumn = k;
                        $scope.sorter.order = v;
                    });
                    $scope.loadDataList();
                    $defer.resolve();
                }
            }
        );



        function onItemSelect(item) {
            angular.forEach($scope.columns, function (v, k) {
                v['visible'] = false;
            });
            angular.forEach(item, function (v, k) {
                $scope.columns[v.id]['visible'] = true;
            });
        };


        function url(item) {
            var hrefUrl;
            switch ($scope.table) {
                case 'site':
                    hrefUrl = $state.href('Admin.site.detail', { id: item.SiteId, bizType: businessType });
                    break;
                    //TODO: Some contacts do not have contactPersonId
                //Get pre business type
                case 'person':
                    hrefUrl = $state.href('Admin.contactPerson.detail', { id: item.ContactPersonId});
                    break;
                default:
                    hrefUrl = $state.href('access.404');
                    break;
            }
            return hrefUrl;
        }

 
       function setDataListPageSize (newPageSize) {
            $scope.paging.pageSize = newPageSize;
            $scope.paging.currentPage = 1;
            $scope.loadDataList();
        };

       function dataListPageChanges(newPage) {
            $scope.paging.currentPage = newPage;
            $scope.loadDataList();
        };
        function selectType (type) {
            businessType = type;
            $scope.loadDataList();
        };
        function loadDataList () {
            $scope.bizTypeColumn.term = businessType;
            var requestObj = {
                currentPage: $scope.paging.currentPage,
                pageSize: $scope.paging.pageSize,
                searchFields: $scope.columns,
                order: $scope.sorter.order,
                sortColumn: $scope.sorter.sortColumn,
                type: $scope.table
            };
            //TODO: hard code!! If the table is person and type is other, we get data from searchPersonNoContact
            if ($scope.table === 'person') {
                searchService.searchPerson(requestObj).then(function (result) {
                    $scope.dataList = result.data['List'];
                    $scope.paging.totalItems = result.data['Total'];
                }, function (error) {
                    logger.serverError(error);
                });
            } else {
                searchService.searchSite(requestObj).then(function (result) {
                    $scope.dataList = result.data['List'];
                    $scope.paging.totalItems = result.data['Total'];
                }, function (error) {
                    logger.serverError(error);
                });
            }
        };

        //TODO: temp solution to fix infinate loops for ng-table filters
        function setDateValue (value, model) {
            if (value == null) {
                $scope.tableParams.filter()[model] = null;
            } else {
                var start = $filter('date')(value.startDate._d, "dd/MM/yyyy");
                var end = $filter('date')(value.endDate._d, "dd/MM/yyyy");
                $scope.tableParams.filter()[model] = start + '-' + end;
            }
        };
    }
})();