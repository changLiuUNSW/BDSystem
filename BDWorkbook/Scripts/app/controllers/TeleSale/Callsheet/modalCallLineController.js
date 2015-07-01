(function () {
    'use strict';
    angular.module('app.telesale.controllers')
    .controller('modalCallLineController', callLineController);
    callLineController.$inject = ['$scope', '$initial', 'typeLibrary', 'logger', '$modalInstance'];

    function callLineController($scope, $initial, typeLibrary, logger, $modalInstance) {
        //reserved for scope btn actions
        $scope.statusValue = typeLibrary.callLineStatus;

        $scope.actions = {
            submit: function (form) {
                if (!form.$valid)
                    return;

                $modalInstance.close($scope.callLine);
            },

            cancel: function () {
                $modalInstance.dismiss();
            },

            changeEntertainment: function () {
                if (!$scope.callLine.Entertainment) {
                    delete $scope.callLine.Entertainment_Event;
                }
            }
        };

        //reserved for call line
        $scope.callLine = { Initial: $initial };
    }
})();