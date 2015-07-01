(function () {
    'use strict';
    angular.module('app.Lead.controllers')
        .controller('leadDetailCtrl', leadDetailCtrl);
    leadDetailCtrl.$inject = ['leadService', 'logger', '$stateParams', '$modal','$timeout'];

    function leadDetailCtrl(leadService, logger, $stateParams, $modal, $timeout) {
        var id = $stateParams.id;
        var vm = this;
        vm.lead = undefined;
        vm.loading = undefined;
        vm.appointmentDate = undefined;
        vm.appointmentEditing = false;
        vm.tabs = ['Overview', 'History'];
        vm.tabs.selected = vm.tabs[0];
        vm.action = action;
        vm.changeQp = changeQp;
        vm.appointmentSave = appointmentSave;

        init();


        function action(status) {
            switch (status.toLowerCase()) {
                case 'new':
                    bdContactModal();
                    break;
                case 'tobecalled':
                    bdContactModal();
                    break;
                case 'callback':
                    bdContactModal();
                    break;
                case 'appointment':
                    siteVisitModal();
                    break;
                case 'visited':
                    siteVisitModal(true);
                    break;
                default:
                    logger.error('Can not find corresponding action for status: ' + status, 'Error');
                    break;
            }
        }

        function appointmentSave() {
            var requestObj = {
                "AppointmentDate": vm.appointmentDate,
                "Url": leadService.getLeadDetailUrl(vm.lead.Id)
            }
            return leadService.updateAppointment(vm.lead.Id, requestObj).then(
                function (result) {
                    logger.success("Lead has been updated successfully", "Success");
                    vm.lead = result.data;
                    vm.appointmentEditing = false;
                    refreshTabContent();
                },
                function(error) {
                    logger.serverError(error);
                }
            );
        }

       
        function changeQp() {
            var modalInstance = $modal.open({
                templateUrl: 'modal.lead.changeQp.html',
                controller: 'ModalLeadChangeQpCtrl',
                size: null,
                backdrop: 'static',
                controllerAs: 'vm',
                resolve: {
                    lead: function () {
                        return angular.copy(vm.lead);
                    }
                }
            });
            modalInstance.result.then(function (lead) {
                vm.lead = lead;
                refreshTabContent();
            });
        }

        function siteVisitModal(hideVisited) {
            var modalInstance = $modal.open({
                templateUrl: 'modal.lead.sitevisit.html',
                controller: 'ModalLeadSiteVisitCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    leadId: function () {
                        return vm.lead.Id;
                    },
                    hideVisited: function () {
                        return hideVisited;
                    }
                }
            });
            modalInstance.result.then(function (lead) {
                vm.lead = lead;
                refreshTabContent();
            });
        }


        function bdContactModal() {
            var modalInstance = $modal.open({
                templateUrl: 'modal.lead.BDcontact.html',
                controller: 'ModalLeadBDContactCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    leadId: function () {
                        return vm.lead.Id;
                    }
                }
            });
            modalInstance.result.then(function (lead) {
                vm.lead = lead;
                refreshTabContent();
            });
        }

        function refreshTabContent() {
            var backup = vm.tabs.selected;
            vm.tabs.selected = null;
            $timeout(function () {
                vm.tabs.selected = backup;
            }, 100);
        }

        function getLead(leadId) {
            vm.loading = true;
            return leadService.getLead({ id: leadId }).then(function (result) {
                vm.lead = result.data;
                vm.loading = false;
            }, function(error) {
                logger.serverError(error);
            });
        }

        function init() {
            getLead(id);
        }
    }
})();