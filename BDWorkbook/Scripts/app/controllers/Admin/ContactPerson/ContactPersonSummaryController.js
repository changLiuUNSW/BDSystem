(function() {
    'use strict';
    angular.module('app.Admin.contactPerson.controllers')
    .controller('contactPersonSummaryCtrl', contactPersonSummaryCtrl);
    contactPersonSummaryCtrl.$inject = ['$scope', 'summaryService', '$stateParams', 'logger', '$filter', 'ngTableParams', 'contactPersonService', '$q', 'SweetAlert'];
    function contactPersonSummaryCtrl($scope, summaryService, $stateParams, logger, $filter, ngTableParams, contactPersonService, $q, sweetAlert) {
        $scope.actionList = [
        { value: 1, text: 'Delete selected' },
        { value: 2, text: 'Change to New Manager' }
        ];
       $scope.dashboardList = [];
       var colorList = [
       {
           type: 'cleaning',
           color: 'info',
       },
       {
           type: 'security',
           color: 'primary',
       },
       {
           type: 'maintenance',
           color: 'warning',
       },
       {
           type: 'other',
           color: 'success',
       }
        ];
      var historyList = [];
        $scope.tableParams = new ngTableParams({
            page: 1,            // show first page
            count: 10           // count per page
        }, {
            total: historyList.length, // length of data
            getData: function ($defer, params) {
                // use build-in angular filter
                var orderedData = params.sorting() ?
                        $filter('orderBy')(historyList, params.orderBy()) :
                        historyList;

                params.total(orderedData.length); // set total for recalc pagination
                $defer.resolve($scope.historyList = orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        });
        $scope.checkboxes = { 'checked': false, items: {} };
        $scope.$watch('checkboxes.checked', checkboxCheckedEvent);
        $scope.$watch('checkboxes.items', checkboxesItemsEvent, true);
        $scope.applyAction = applyAction;
        $scope.loadingCountAndHistory = loadingCountAndHistory;
        $scope.init = init;

        $scope.init();


        // watch for check all checkbox
        function checkboxCheckedEvent(value) {
            angular.forEach($scope.historyList, function (item) {
                if (angular.isDefined(item.Id)) {
                    $scope.checkboxes.items[item.Id] = value;
                }
            });
        }

        // watch for data checkboxes
       function checkboxesItemsEvent(values) {
            if (!$scope.historyList) {
                return;
            }
            var checked = 0, unchecked = 0,
                total = $scope.historyList.length;
            angular.forEach($scope.historyList, function (item) {
                checked += ($scope.checkboxes.items[item.Id]) || 0;
                unchecked += (!$scope.checkboxes.items[item.Id]) || 0;
            });
            if ((unchecked == 0) || (checked == 0)) {
                $scope.checkboxes.checked = (checked == total);
            }
            // grayed checkbox
            angular.element(document.getElementById("select_all")).prop("indeterminate", (checked != 0 && unchecked != 0));
        };

       function applyAction(selected) {
           var selectedItems = [];
           angular.forEach($scope.checkboxes.items, function (v, i) {
               if (v) selectedItems.push(i);
           });
            if (!selected) {
                logger.error("Please select the action", "Error");
                return;
            }
            if (selectedItems.length===0) {
                logger.error("Please select item", "Error");
                return;
            }
            //Delete selected
            if (selected.value === 1) {
                sweetAlert.confirm('Please Confirm', 'Delete selected items?').then(function () {
                    contactPersonService.removeContactPersonHistory({ ids: selectedItems }).then(function (result) {
                        logger.success("Update successfully", "Success");
                        $scope.loadingCountAndHistory();
                    }, function (error) {
                        logger.error(error.data.Message, "Query failed");
                    });
                });
            }
            //Change new manager
            if (selected.value === 2) {
                sweetAlert.confirm('Please Confirm', 'Change selected items to new manager?').then(function () {
                    contactPersonService.changeToNewManager(selectedItems).then(function (result) {
                        logger.success("Update successfully", "Success");
                        $scope.loadingCountAndHistory();
                    }, function (error) {
                        logger.serverError(error);
                    });
                });
            }
        }

       function loadingCountAndHistory () {
            $scope.loading = true;
            $q.all([contactPersonService.getAllPersonHistory(), summaryService.getcontactPersonCount()]).then(function (results) {
                var countList = results[1].data;
                historyList = results[0].data;
                $scope.dashboardList = countList.map(function (obj) {
                    var config = $filter('filter')(colorList, { type: obj.Type })[0];
                    return angular.extend(obj, config);
                });
                $scope.loading = false;
            }, function (error) {
                logger.serverError(error);
            });
        }

       function init() {
            $scope.loadingCountAndHistory();
        }
    }
})();