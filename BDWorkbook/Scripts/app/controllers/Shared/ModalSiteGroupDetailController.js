(function() {
    'use strict';
    angular.module('app.shared.controllers')
        .controller('ModalSiteGroupDetailCtrl', modalSiteGroupDetailCtrl);
    modalSiteGroupDetailCtrl.$inject = ['$modalInstance', 'group','groupType', 'siteGroupService', 'logger'];

    function modalSiteGroupDetailCtrl($modalInstance, group,groupType, siteGroupService, logger) {
        var vm = this;
        vm.cancel = cancel;
        vm.group = group;
        vm.groupType = groupType;
        vm.save = save;
        vm.typeDrp = ['building', 'group'];
        vm.codeDrp = codeDrp;

        function cancel() {
            $modalInstance.dismiss();
        }

        function codeDrp(type) {
            switch (type) {
                case 'building':
                    return ['qt'];
                case 'group':
                    return ['qa'];
                default:
                    return null;

            }
        }


        function save() {
            siteGroupService.saveGroup(vm.group).then(function (result) {
                logger.success("Save successfully", "Success");
                $modalInstance.close(result.data);
            }, function(error) {
                logger.serverError(error);
            });
        }
    }

})();