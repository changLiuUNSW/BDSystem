(function() {
    'use strict';
    angular.module('app.shared.controllers')
        .controller('ModalSiteGroupCtrl', modalSiteGroupCtrl);
    modalSiteGroupCtrl.$inject = ['$scope', 'modalConfig', '$modalInstance', '$modal', 'siteGroupService', 'siteService', '$filter', 'logger', 'ngTableParams', 'SweetAlert', 'searchService'];
    function modalSiteGroupCtrl($scope, modalConfig, $modalInstance, $modal, siteGroupService, siteService, $filter, logger, ngTableParams, sweetAlert, searchService) {
        var siteList = [];
        $scope.title = modalConfig.title;
        $scope.editable = modalConfig.editable ? modalConfig.editable : false;
        $scope.groupActions = ['Edit', 'Delete'];
        $scope.siteActions = ['Delete'];
        $scope.group = {};
        $scope.checkboxes = {};
        $scope.group.siteList = [];
        $scope.isEdited = false;
        $scope.inputfields = {
            siteToAdd: undefined
        };
        $scope.group.selected = undefined;
        $scope.initNgTable = initNgTable;
        $scope.loadGroupList = loadGroupList;
        $scope.init = init;
        $scope.selectAction = selectAction;
        $scope.save = save;
        $scope.searchSiteByKey = searchSiteByKey;
        $scope.cancel = cancel;
        $scope.groupSelect = groupSelect;
        $scope.addGroup = addGroup;
        $scope.addSite = addSite;
        $scope.applyAction = applyAction;
        $scope.init(modalConfig.groupId);

        function initNgTable () {
            $scope.tableParams = new ngTableParams({
                page: 1,            // show first page
                count: 10           // count per page
            }, {
                total: siteList.length, // length of data
                getData: function ($defer, params) {
                    // use build-in angular filter
                    var orderedData = params.sorting() ?
                            $filter('orderBy')(siteList, params.orderBy()) :
                            siteList;
                    var paramsFilters = params.filter();
                    for (var i in paramsFilters) {
                        if (paramsFilters[i].length == 0) delete paramsFilters[i];
                    }
                    orderedData = params.filter() ?
                        $filter('filter')(orderedData, params.filter()) :
                        orderedData;
                    params.total(orderedData.length); // set total for recalc pagination
                    $defer.resolve($scope.group.siteList = orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });


            $scope.checkboxes = { 'checked': false, items: {} };

            // watch for check all checkbox
            $scope.$watch('checkboxes.checked', function checkboxChecked(value) {
                angular.forEach($scope.group.siteList, function (item) {
                    if (angular.isDefined(item.Id)) {
                        $scope.checkboxes.items[item.Id] = value;
                    }
                });
            });
        }

       function loadGroupList(groupId) {
            return siteGroupService.searchGroup({ type: modalConfig.type }).then(function (result) {
                $scope.group.list = result.data;
                if (groupId) $scope.group.selected = $filter('filter')($scope.group.list, { Id: groupId })[0];
                return groupId;
            });
        }

       function getGroupDetail(id) {
            $scope.loading = true;
           return siteGroupService.getGroup({ id: id }).then(function(result) {
               siteList = result.data.Sites;
               $scope.initNgTable();
               $scope.loading = false;
           });
       };

       function init(groupId) {
           $scope.group.selected = undefined;
           var loading = $scope.loadGroupList(groupId);
           if (groupId) loading = loading.then(getGroupDetail(groupId));
           loading.catch(function (error) {
               logger.serverError(error);
           });
       }


       function selectAction(action) {
           var selectedGroup = $scope.group.selected;
           switch (action) {
               case 'Edit':
                   if (selectedGroup) {
                       editGroup(selectedGroup);
                   } else {
                       logger.error('Please select one group', 'Error');
                   }
                   break;
               case 'Delete':
                   if (selectedGroup) {
                       deleteGroup(selectedGroup);
                   } else {
                       logger.error('Please select one group', 'Error');
                   }
                   break;
               default:
                   logger.error('The action you select is not implemented', 'Error');
                   break;
           }


       }

       function deleteGroup(selectedGroup) {
           sweetAlert.confirm('Please Confirm', 'Delete this group ?').then(function() {
               siteGroupService.removeGroup(selectedGroup.Id).then(function (result) {
                   logger.success('Group ' + selectedGroup.Id + ' is deleted', 'Success');
                   if ($scope.editable) $modalInstance.dismiss(selectedGroup);
                   init(null);
                   $scope.isEdited = true;
               }, function (error) {
                   logger.serverError(error);
               });
           });
         
       }

        function editGroup(selectedGroup) {
            var modalInstance = $modal.open({
                templateUrl: 'siteGroupDetail.html',
                controller: 'ModalSiteGroupDetailCtrl',
                controllerAs: 'vm',
                size: null,
                backdrop: 'static',
                resolve: {
                    group: function () {
                        return angular.copy(selectedGroup);
                    },
                    groupType: function() {
                        return selectedGroup.Type;
                    }
                }
            });

            modalInstance.result.then(function (updateGroup) {
                angular.extend(selectedGroup, updateGroup);
                $scope.isEdited = true;
            });
        }

        function addGroup() {
            var modalInstance = $modal.open({
                templateUrl: 'siteGroupDetail.html',
                controller: 'ModalSiteGroupDetailCtrl',
                controllerAs: 'vm',
                size: null,
                backdrop: 'static',
                resolve: {
                    group: function () {
                        return { Type: modalConfig.type };
                    },
                    groupType: function () {
                        return modalConfig.type;
                    }
                }
            });
            modalInstance.result.then(function (addedGroup) {
                init(addedGroup.Id);
            });
        }

        function addSite() {
            var selectedGroup = $scope.group.selected;
            var siteToAdd = $scope.inputfields.siteToAdd;
            if (siteToAdd)
            {
                var requestObj = {
                    siteId: siteToAdd.Id,
                    orignGroupId: null,
                    newGroupId: selectedGroup.Id
                }
                siteService.updateGroup(requestObj).then(function (result) {
                    init(selectedGroup.Id);
                    $scope.inputfields.siteToAdd = undefined;
                    $scope.isEdited = true;
                    logger.success("Site has been Added", "Success");
                }, function (error) {
                    logger.serverError(error);
                });
            }
        }

        function applyAction(selected) {
            var selectedItems = [];
            angular.forEach($scope.checkboxes.items, function (v, i) {
                if (v) selectedItems.push(i);
            });
            if (!selected) {
                logger.error("Please select the action", "Error");
                return;
            }
            if (selectedItems.length === 0) {
                logger.error("Please select an item", "Error");
                return;
            }
            if (selected === 'Delete') {
                sweetAlert.confirm('Please Confirm', 'Delete selected items?').then(function() {
                    var selectedGroup = $scope.group.selected;
                    siteGroupService.removeSites(selectedGroup.Id, { 'siteIds': selectedItems }).then(function (result) {
                        init(selectedGroup.Id);
                        $scope.isEdited = true;
                        logger.success('Sites are deleted successfully', 'Success');
                    }, function (error) {
                        logger.serverError(error);
                    });
                });
            }
        }

        function searchSiteByKey(key) {
            return searchService.searchKey({ key: key, take: 5 }).then(function (result) {
                return result.data;
            }, function (error) {
                logger.serverError(error);
            });
        }

        function cancel() {
            $modalInstance.dismiss();
        };

        function save() {
            $modalInstance.close($scope.group.selected);
        }

        function groupSelect(model) {
            if (model && model.Id) getGroupDetail(model.Id);
        }
    }
})();