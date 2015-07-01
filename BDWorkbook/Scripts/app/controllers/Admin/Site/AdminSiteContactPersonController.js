(function() {
    'use strict';
    angular.module('app.Admin.site.controllers')
        .controller('AdminSiteContactPersonCtrl', adminSiteContactPersonCtrl);
    adminSiteContactPersonCtrl.$inject = ['$scope', '$modal'];

    function adminSiteContactPersonCtrl($scope, $modal) {
        $scope.contactPersonModal = contactPersonModal;


        function contactPersonModal(size) {
            var modalInstance = $modal.open({
                templateUrl: 'contactModalContent.html',
                controller: 'ModalContactCtrl',
                size: size,
                backdrop: 'static',
                resolve: {
                    site: function () {
                        return angular.copy($scope.model.site);
                    }
                }
            });
            modalInstance.result.then(function (site) {
                $scope.model.site = site;
                $scope.initSiteDetail();
            }, function (site) {
                if (site) $scope.model.site = site;
            });
        }
    }
})();