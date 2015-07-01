(function() {
    'use strict';
    angular.module('app.Admin.site.controllers')
    .controller('AdminSiteInfoCtrl', adminSiteInfoCtrl);
    adminSiteInfoCtrl.$inject = ['$scope', 'saleBoxService', 'logger', '$modal', 'siteService','$filter'];

    function adminSiteInfoCtrl($scope, saleBoxService, logger, $modal, siteService, $filter) {
        $scope.getPostCodeAndState = getPostCodeAndState;
        $scope.editBuilding = editBuilding;
        $scope.editGroup = editGroup;
        $scope.postCodeAndState = postCodeAndState;

        function getPostCodeAndState (state, postCode) {
            return saleBoxService.getSalesbox({ postCode: postCode, state: state, take: 5 }).then(function (result) {
                return result.data;
            }, function (error) {
                logger.serverError(error);
            });
        }

        function editGroup(size) {
            var group = $scope.model.group;
            var groupId = group ? group.Id : null;
            var modalInstance = $modal.open({
                templateUrl: 'ModalSiteGroup.html',
                controller: 'ModalSiteGroupCtrl',
                size: size,
                backdrop: 'static',
                resolve: {
                    modalConfig: function () {
                        return {
                            title: 'group',
                            type: 'group',
                            groupId: groupId,
                            editable: false
                        }
                    }
                }
            });
            modalInstance.result.then(function (data) {
                var newGroupId = data ? data.Id : null;
                if (groupId !== newGroupId) {
                    var requestObj = {
                        siteId: $scope.model.site.Id,
                        orignGroupId: groupId,
                        newGroupId: newGroupId
                    }
                    siteService.updateGroup(requestObj).then(function (result) {
                        var siteGroups = $scope.model.site.Groups;
                        //Update view Model
                        for (var i = 0; i < siteGroups.length; i++) {
                            if (siteGroups[i].Id === groupId) {
                                siteGroups.splice(i, 1);
                                break;
                            }
                        }
                        if (newGroupId) siteGroups.push(data);
                        $scope.model.group = $filter('filter')(siteGroups, { Type: 'group' })[0];
                        logger.success("Group has been updated", "Success");
                    }, function (error) {
                        logger.serverError(error);
                    });
                } else {
                    if (data) angular.extend($scope.model.group, data);
                }
            });
        }


        function editBuilding(size) {
            var building = $scope.model.building;
            var groupId = building ? building.Id : null;
            var modalInstance = $modal.open({
                templateUrl: 'ModalSiteGroup.html',
                controller: 'ModalSiteGroupCtrl',
                size: size,
                backdrop: 'static',
                resolve: {
                    modalConfig: function () {
                        return {
                            title: 'building',
                            type: 'building',
                            groupId: groupId,
                            editable: false
                        }
                    }
                }
            });
            modalInstance.result.then(function (data) {
                var newGroupId = data ? data.Id : null;
                if (groupId !== newGroupId) {
                    var requestObj = {
                        siteId: $scope.model.site.Id,
                        orignGroupId: groupId,
                        newGroupId: newGroupId
                    }
                    siteService.updateGroup(requestObj).then(function(result) {
                        var siteGroups = $scope.model.site.Groups;
                        //Update view Model
                        for (var i = 0; i < siteGroups.length; i++) {
                            if (siteGroups[i].Id === groupId) {
                                siteGroups.splice(i, 1);
                                break;
                            }
                        }
                        if (newGroupId) siteGroups.push(data);
                        $scope.model.building = $filter('filter')(siteGroups, { Type: 'building' })[0];
                        logger.success("Building has been updated", "Success");
                    }, function(error) {
                        logger.serverError(error);
                    });
                } else {
                    if(data)angular.extend($scope.model.building,data);
                }
            });
        }

        function postCodeAndState(item) {
            $scope.model.site.State = item.State;
            $scope.model.site.GmZone = item.Zone;
            $scope.model.site.Region = item.Region;
        }
    }
})();