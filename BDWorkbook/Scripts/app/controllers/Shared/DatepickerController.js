(function() {
    'use strict';
    angular.module('app.shared.controllers')
        .controller('DatepickerCtrl', datepickerCtrl);
    datepickerCtrl.$inject = ['$scope'];

    function datepickerCtrl($scope) {
        // Disable weekend selection
        $scope.disabled = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };
        $scope.formats = ['dd-MMMM-yyyy', 'dd/MM/yyyy', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[1];
    }
})();