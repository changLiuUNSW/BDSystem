(function () {
    'use strict';
    angular.module('app.Lead.controllers')
        .controller('ModalLeadChangeQpCtrl', modalLeadChangeQpCtrl);
    modalLeadChangeQpCtrl.$inject = ['$modalInstance', 'lead', 'leadService', 'logger', '$filter', 'SweetAlert'];

    function modalLeadChangeQpCtrl($modalInstance, lead, leadService, logger, $filter, sweetAlert) {
        var vm = this;
        vm.cancel = cancel;
        vm.loading = undefined;
        vm.qpList = undefined;
        vm.selectedQp = undefined;
        vm.ok = ok;
        vm.isSame =isSame;
        init();

        function loadingQp(zone) {
            vm.loading = true;
            return leadService.getAllQpByZone({zone:zone}).then(function(result) {
                vm.qpList = result.data;
                vm.loading = false;
                return vm.qpList;
            }, function (error) {
                logger.serverError(error);
            });
        }

     

        function isSame() {
            if (vm.selectedQp) {
                return vm.selectedQp.Initial === lead.LeadPersonal.Initial;
            }
            return false;
        }


        function ok() {
            sweetAlert.confirm('Please Confirm', 'Change QP will reset the status to NEW. Do you want to process?').then(function() {
                var requestObj = {
                    QpId: vm.selectedQp.Id,
                    Url: leadService.getLeadDetailUrl(lead.Id)
                }
                return leadService.updateQp(lead.Id, requestObj).then(function (result) {
                    logger.success("QP has been updated successfully", "Success");
                    $modalInstance.close(result.data);
                }, function (error) {
                    logger.serverError(error);
                });
            });
        }

     

        function cancel() {
            $modalInstance.dismiss();
        };

        function initSelectedQp(list) {
            vm.selectedQp = $filter('filter')(list, { Initial: lead.LeadPersonal.Initial })[0];
        }

        function init() {
            loadingQp(lead.SalesBox.Zone)
                .then(initSelectedQp);
        }
    }

})();