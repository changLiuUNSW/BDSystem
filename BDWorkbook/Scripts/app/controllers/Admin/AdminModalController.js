(function() {
    'use strict';

    angular.module("app.Admin.modal.controllers")
        .controller('adminLeadPersonListController', adminLeadPersonListController);

    adminLeadPersonListController.$inject = ['$scope', 'apiService', '$modalInstance', 'logger'];
    function adminLeadPersonListController($scope, apiService, $modalInstance, logger) {
        $scope.persons = [];
        $scope.data = {};
        $scope.cancel = function() {
            $modalInstance.dismiss();
        };
        $scope.ok = function (form) {
            if (form.$invalid) {
                logger.error("Must select a person to confirm");
                return;
            }

            $modalInstance.close($scope.data.selected.Id);
        }
        apiService.leadPersonal.get().$promise.then(function (response) {
            $scope.persons = response.data;
        });
    };
})();