(function() {
    'use strict';
    angular.module('app.Admin.site.controllers')
        .controller('AdminSiteSummaryCtrl', adminSiteSummaryCtrl);
    adminSiteSummaryCtrl.$inject = ['summaryService', '$q', '$filter', 'logger', 'siteGroupService', 'ngTableParams', '$modal'];

    function adminSiteSummaryCtrl(summaryService, $q, $filter, logger, siteGroupService, ngTableParams, $modal) {
        var vm = this;
        var groupList = [];
        var dashboardListConfig = [
            {
                type: 'cleaning',
                color: 'info',
                linkable: true
            },
            {
                type: 'security',
                color: 'primary',
                linkable: true
            },
            {
                type: 'maintenance',
                color: 'warning',
                linkable: true
            },
            {
                type: 'group',
                color: 'success',
                linkable: false
            }
        ];
        vm.dashboardList = [];
        vm.groupList = [];
        vm.tableParams = undefined;
        vm.addGroup = addGroup;
        vm.groupDetail = groupDetail;
        init(); 
        function init() {
            vm.loading = true;
            $q.all([loadingCount(), loadingGroupList()]).then(function (result) {
                vm.loading = false;
                initNgTable();
            }).catch(function(error) {
                logger.serverError(error);
            });
        }

        function addGroup() {
            var modalInstance = $modal.open({
                templateUrl: 'ModalGroupDetail.html',
                controller: 'ModalSiteGroupCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    modalConfig: function () {
                        return {
                            title: "Add Group",
                            type: null,
                            groupId: null,
                            editable: true
                        }
                    }
                }
            });
            modalInstance.result.then(function (updateGroup) {
                init();
            }, function (deletedGroup) {
               init();
            });
        }

        function groupDetail(group) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalGroupDetail.html',
                controller: 'ModalSiteGroupCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    modalConfig: function () {
                        return {
                            title: group.Type,
                            type: group.Type,
                            groupId: group.Id,
                            editable: true
                        }
                    }
                }
            });
            modalInstance.result.then(function (updateGroup) {
                init();
            }, function(deletedGroup) {
                if (deletedGroup) init();
            });
        }


        function initNgTable() {
            vm.tableParams = new ngTableParams({
                page: 1,            // show first page
                count: 10           // count per page
            }, {
                total: groupList.length, // length of data
                getData: function ($defer, params) {
                    // use build-in angular filter
                    var orderedData = params.sorting() ?
                            $filter('orderBy')(groupList, params.orderBy()) :
                            groupList;
                    var paramsFilters = params.filter();
                    for (var i in paramsFilters) {
                        if (paramsFilters[i].length == 0) delete paramsFilters[i];
                    }
                    orderedData = params.filter() ?
                        $filter('filter')(orderedData, params.filter()) :
                        orderedData;
                    params.total(orderedData.length); // set total for recalc pagination
                    $defer.resolve(vm.groupList = orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });
        }

        function loadingGroupList() {
            return siteGroupService.getAllGroup().then(function (result) {
                groupList = result.data;
                return groupList;
            });
        }

        function loadingCount() {
            return summaryService.getSiteCount().then(function(result) {
                var countList = result.data;
                vm.dashboardList = countList.map(function(obj) {
                    var config = $filter('filter')(dashboardListConfig, { type: obj.Type })[0];
                    return angular.extend(obj, config);
                });
                return vm.dashboardList;
            });
        }
    }
})();