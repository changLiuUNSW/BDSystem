(function () {
    'use strict';
    angular.module('app.Lead.controllers')
        .controller('ModalLeadSiteVisitCtrl', modalLeadSiteVisitCtrl);
    modalLeadSiteVisitCtrl.$inject = ['$modalInstance', 'leadService', 'logger', 'leadId', 'hideVisited'];

    function modalLeadSiteVisitCtrl($modalInstance, leadService, logger, leadId, hideVisited) {
        var vm = this;
        vm.cancel = cancel;
        vm.hideVisited = hideVisited;
        vm.action = undefined;
        vm.ok = ok;

        function ok(action) {
            switch (action) {
                case 'quote':
                    quote().catch(function (error) {
                        logger.serverError(error);
                    });
                    break;
                case 'cancel':
                    cancelLead().catch(function (error) {
                        logger.serverError(error);
                    });
                    break;
                case 'visited':
                    visited().catch(function (error) {
                        logger.serverError(error);
                    });
                    break;
                default:
                    logger.error("The action " + action + " is not implemented");
                    break;
            }
        }

        function quote() {
            return leadService.quote(leadId).then(
              function (result) {
                  logger.success("Quote has been created successfully", "Success");
                  $modalInstance.close(result.data);
              }
          );
        }

        function visited() {
            return leadService.visited(leadId).then(
               function (result) {
                   logger.success("Lead has been updated", "Success");
                   $modalInstance.close(result.data);
               }
           );
        }

        function cancelLead() {
            return leadService.cancel(leadId).then(
               function (result) {
                   logger.success("Lead has been cancelled successfully", "Success");
                   $modalInstance.close(result.data);
               }
           );
        }

      
        function cancel() {
            $modalInstance.dismiss();
        };
    }

})();