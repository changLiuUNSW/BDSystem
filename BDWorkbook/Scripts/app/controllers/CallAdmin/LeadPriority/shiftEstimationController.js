(function () {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('shiftEstimationController', shiftEstimationController);
    shiftEstimationController.$inject = ['$scope','$modalInstance','$shift'];

    function shiftEstimationController($scope, $modalInstance, $shift) {
        $scope.shift = angular.copy($shift);

        $scope.actions = {
            cancel: function () {
                $modalInstance.dismiss();
            },

            confirm: function (form) {
                if (!form.$valid)
                    return;

                $modalInstance.close($scope.shift);
            }
        };
    }
})();