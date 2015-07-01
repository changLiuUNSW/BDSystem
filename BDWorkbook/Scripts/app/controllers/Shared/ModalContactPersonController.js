(function () {
    'use strict';
    angular.module('app.shared.controllers')
        .controller('ModalContactPersonCtrl', modalContactPersonCtrl);
    modalContactPersonCtrl.$inject = ['$scope', 'siteId', 'person', 'contactPersonService', 'logger', '$modalInstance', 'contactPersonHelper'];

    function modalContactPersonCtrl($scope, siteId, person, contactPersonService, logger, $modalInstance, contactPersonHelper) {
        $scope.siteId = siteId;
        $scope.contactPersonHelper = contactPersonHelper;
        $scope.contactPerson = person || {SiteId: siteId};
        $scope.cancel = cancel;
        $scope.save = save;

        function cancel() {
            $modalInstance.dismiss('cancel');
        };

        function save() {
            contactPersonService.saveContactPerson($scope.contactPerson).then(function (result) {
                logger.success("Save successfully", "Success");
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        };
    }
})();