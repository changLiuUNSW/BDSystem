(function() {
    'use strict';
    angular.module('app.shared.controllers')
        .controller('confirmationController', confirmationController);
    confirmationController.$inject = ['$scope', '$modalInstance', 'title', 'message'];
    function confirmationController($scope, $modalInstance, title, message) {
        $scope.title = typeof title == "undefined" ? "" : title;
        $scope.message = typeof message == "undefined" ? "Confirm to proceed" : message;

        $(document).on('keypress', function (e) {
            if (e.keyCode == 13) {
                $scope.confirm();
            }
        });

        $scope.cancel = function () {
            $(document).off('keypress');
            $modalInstance.dismiss();
        };

        $scope.confirm = function () {
            $(document).off('keypress');
            $modalInstance.close();
        };
    }
})();