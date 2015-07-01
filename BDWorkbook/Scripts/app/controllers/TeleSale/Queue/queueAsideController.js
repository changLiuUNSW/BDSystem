(function() {
    'use strict';
    angular.module('app.telesale.controllers')
        .controller('queueAsideController', asideController);

    asideController.$inject = ['$scope'];
    function asideController($scope) {
        var self = this;
        self.select = select;
        self.isSelected = match;
        self.selected = null;

        function match(telesale) {
            if (!telesale || !$scope.$parent.queue.selected)
                return false;

            return telesale.Id === $scope.$parent.queue.selected.Id;
        }

        function select(telesale) {
            if (!telesale)
                return;

            $scope.$parent.queue.selected = telesale;
        }
    }
})();