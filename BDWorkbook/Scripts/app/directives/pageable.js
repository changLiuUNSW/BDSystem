(function () {
    'use strict';
    angular.module('app.directives')
    .directive('pageable', pageable);
    pageable.$inject = [];

    function pageable() {
        return {
            restrict: 'EA',
            replace: true,

            scope: {
                pageSizeChanged: "&",
                currentPageChanged: "&",
                totalItems: "=",
                pageSize: "="
            },

            controller: [
                '$scope', function ($scope) {

                    $scope.pageSizeOptions = [{ id: 5, title: 5 }, { id: 10, title: 10 }, { id: 15, title: 15 }, { id: 25, title: 25 }, { id: 50, title: 50 }];

                    $scope.pagination = { tempPageSize: $scope.pageSize, currentPage: 1 };

                    $scope.updatePageSize = function () {
                        var func = $scope.pageSizeChanged();
                        func($scope.pagination.tempPageSize);
                    };

                    $scope.pageChanged = function () {
                        var func = $scope.currentPageChanged();
                        func($scope.pagination.currentPage);
                    };
                }
            ],
            template: '<div class="row">' + ' <div class="col-sm-4 hidden-xs">' +
                '<form class="form-inline" role="form">' +
                '<div class="form-group">' +
                '<label class="control-label hidden-sm" for="drpPageSize">Item per page:</label>' +
                '<select ng-model="pagination.tempPageSize" class="form-control input-sm" ng-options="item.id as item.title for item in pageSizeOptions" id="drpPageSize" name="drpPageSize"></select>' +
                '<button class="btn btn-sm btn-default m-l-xs" ng-click="updatePageSize()">' +
                '<span class="glyphicon glyphicon-refresh"></span>' +
                '</button>' +
                '</div>' +
                '</form>' +
                '</div>' +
                '<div class="col-sm-4 text-center hidden-xs"><small class="text-muted inline m-t-sm m-b-sm">Total: {{totalItems}}</small></div>'
                +
                '<div class="col-sm-4 text-right text-center-xs">' +
                '<pagination boundary-links="true" direction-links="false" max-size="5" ng-change="pageChanged()" items-per-page="pageSize" total-items="totalItems" ng-model="pagination.currentPage" class="pagination-sm m-t-none m-b-none" previous-text="&lsaquo;" next-text="&rsaquo;" first-text="&laquo;" last-text="&raquo;"></pagination>' +
                '</div>'
        }
    }
})();