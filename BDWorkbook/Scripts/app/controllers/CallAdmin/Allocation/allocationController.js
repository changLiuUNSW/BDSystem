(function() {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('allocationController', allocationController);
    allocationController.$inject = ['$scope', 'apiService', '$q', 'logger'];

    function allocationController($scope, apiService, $q, logger) {

        var dataModule = function() {

            return {
                load: load,
                save: save,
                remove: remove
            };
            
            function save(allocation) {
                return apiService.allocation.save(allocation).$promise;
            };

            function remove(allocation) {
                var param = { id: allocation.Id };
                return apiService.allocation.remove(param).$promise;
            }

            function load() {
                $scope.data = {};
                $scope.data.loading = true;
                var zoneAllocation = apiService.salesbox.zoneAllocation().$promise;
                var personal = apiService.leadPersonal.get().$promise;

                return $q.all([zoneAllocation, personal]);
            }
        }();

        var main = function(data) {
            data.load().then(function (success) {
                $scope.data.sizes = success[0].data.sizes;
                $scope.data.zones = success[0].data.zones;
                $scope.data.persons = success[1].data;
                init();
            }, function(error) {
                logger.error(error.data.ExceptionMessage, error.data.Message);
            });

            function init() {
                $scope.data.loading = false;
                initEdit();
                initQuery();
                initPaging();
            }
            
            function initPaging() {
                $scope.paging = {};
                $scope.paging.itemsPerPage = 10;
                $scope.paging.totalItems = $scope.data.zones.length;

                $scope.$watch('paging.currentPage', function (page) {
                    if (!page)
                        return;

                    $scope.paging.startIdx = (page-1) * $scope.paging.itemsPerPage;
                });
            }

            function initQuery() {
                $scope.query = {};
            }
            
            function initEdit() {
                $scope.edit = {}

                $scope.edit.selectPerson = selectPerson;
                $scope.edit.hideAside = hideAside;
                $scope.edit.getAllocation = getAllocation;
                $scope.edit.save = save;
                $scope.edit.remove = remove;

                function remove(zone, size, $event) {
                    $event.stopPropagation();

                    var allocation = getAllocation(zone, size);

                    if (!allocation)
                        return;

                    dataModule.remove(allocation).then(function (success) {
                        removeAllocation(zone, size);
                    });
                };

                function save(zone, size) {
                    if (!$scope.edit.select)
                        return;


                    if (getAllocation(zone, size)) {
                        logger.error("Zone has been assigned to another person", "Error");
                        return;
                    }

                    var param = {
                        Zone: zone.Zone,
                        Size: size,
                        Initial: $scope.edit.select.Initial,
                        LeadPersonalId: $scope.edit.select.Id
                    };

                    dataModule.save(param).then(function(success) {
                        logger.success("Allocated " + param.Initial + " to " + param.Zone + " " + param.Size);
                        zone.Allocations.push(success.data);
                    }, function(error) {
                        logger.error(error.data.ExceptionMessage, error.data.Message);
                    });
                }

                function getAllocation(zone, size) {
                    var i, allocation;
                    for (i = 0; i < zone.Allocations.length; i++) {
                        allocation = zone.Allocations[i];
                        if (allocation.Size == size)
                            return allocation;
                    }

                    return null;
                }

                function removeAllocation(zone, size) {
                    var i, allocation;
                    for (i = 0; i < zone.Allocations.length; i++) {
                        allocation = zone.Allocations[i];
                        if (allocation.Size == size) {
                            zone.Allocations.splice(i, 1);
                            return;
                        }
                    }
                }

                function selectPerson(person) {
                    if (person.style) {
                        delete person.style;
                        delete $scope.edit.style;
                        delete $scope.edit.select;
                    } else {
                        person.style = { 'background': '#f5f5f5' };
                        $scope.edit.style = { 'cursor': 'pointer' };
                        if ($scope.edit.select) {
                            delete $scope.edit.select.style;
                        }

                        $scope.edit.select = person;
                    }
                }

                function hideAside() {
                    $scope.edit.aside = !$scope.edit.aside;
                }
            }
        }(dataModule || {});
    }
})();