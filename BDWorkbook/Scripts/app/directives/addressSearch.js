(function() {
    'use strict';
    angular.module('app.directives')
        .directive('addressSearch', addressSearch);

    function addressSearch() {
        return {
            templateUrl: 'tpl/Shared/addressSearch.html',
            restrict: 'E',
            scope: {
                selectEvent: '&'
            },
            controller: addressSearchController,
            controllerAs: 'vm',
        };
    }

    addressSearchController.$inject = ['searchService', 'logger','$scope'];

    function addressSearchController(searchService, logger, $scope) {
        /* jshint validthis: true */
        var vm = this;
        vm.addrList = [];
        vm.searchAddress = searchAddress;
        vm.selected = undefined;
        vm.onSelect = function (item,model) {
            var func = $scope.selectEvent();
            func(model);
        }

        function searchAddress(keyword) {
            searchService.searchAddress({ keyword: keyword, take: 5 }).then(function(result) {
                vm.addrList = result.data;
            }, function(error) {
                logger.serverError(error);
            });
        }
    }
})();