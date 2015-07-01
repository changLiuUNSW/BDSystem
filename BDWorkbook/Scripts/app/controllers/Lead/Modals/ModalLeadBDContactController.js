(function() {
    'use strict';
    angular.module('app.Lead.controllers')
        .controller('ModalLeadBDContactCtrl', modalLeadBdContactCtrl);
    modalLeadBdContactCtrl.$inject = ['$modalInstance', 'leadService', 'logger', 'leadId'];

    function modalLeadBdContactCtrl($modalInstance, leadService, logger, leadId) {
        var vm = this;
        vm.cancel = cancel;
        vm.action = undefined;
        vm.appointmentDate = undefined;
        vm.callbackDate = undefined;
        vm.ok = ok;

        function ok(action) {
            switch (action) {
                case 'appointment':
                    appointment().catch(function(error) {
                        logger.serverError(error);
                    });
                    break;
                case 'callback':
                    callback().catch(function (error) {
                        logger.serverError(error);
                    });
                    break;
                case 'contacted':
                    contactNotSuccess().catch(function (error) {
                        logger.serverError(error);
                    });
                    break;
                case 'cancel':
                    cancelLead().catch(function(error) {
                        logger.serverError(error);
                    });
                    break;
                default:
                    logger.error("The action "+action+" is not implemented");
                    break;
            }
        }

        function cancelLead() {
            return leadService.cancel(leadId).then(
               function (result) {
                   logger.success("Lead has been cancelled successfully", "Success");
                   $modalInstance.close(result.data);
               }
           );
        }

        function contactNotSuccess() {
            return leadService.contactNotSuccess(leadId).then(
                function(result) {
                    logger.success("Lead has been updated successfully", "Success");
                    $modalInstance.close(result.data);
                }
            );
        }

        function appointment() {
            var requestObj = {
                "AppointmentDate": vm.appointmentDate,
                "Url": leadService.getLeadDetailUrl(leadId)
        }
            return leadService.updateAppointment(leadId, requestObj).then(
                function (result) {
                    logger.success("Lead has been updated successfully", "Success");
                    $modalInstance.close(result.data);
                }
            );
        }

        function callback() {
            return leadService.updateCallback(leadId, vm.callbackDate).then(
               function (result) {
                   logger.success("Lead has been updated successfully", "Success");
                   $modalInstance.close(result.data);
               }
           );
        }


        function cancel() {
            $modalInstance.dismiss();
        };
    }

})();