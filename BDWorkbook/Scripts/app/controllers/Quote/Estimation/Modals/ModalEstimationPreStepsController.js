(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalEstimationPreStepsCtrl', modalEstimationPreStepsCtrl);
    modalEstimationPreStepsCtrl.$inject
        = [ 'quoteCostService','saleBoxService', 'logger', '$state', 'quote', '$modalInstance'];

    function modalEstimationPreStepsCtrl(quoteCostService, saleBoxService, logger, $state, quote, $modalInstance) {
        var vm = this;
        vm.quote = quote;
        vm.cost = {
            QuoteId: quote.Id,
            Address: {}
        };
        vm.postCodeAndState = postCodeAndState;
        vm.getPostCodeAndState = getPostCodeAndState;
        vm.addrSelect = addrSelect;
        vm.createCost = createCost;
        vm.resetWeeks = resetWeeks;
        vm.saving = undefined;
        function getPostCodeAndState(state, postCode) {
            return saleBoxService.getSalesbox ({ postCode: postCode, state: state, take: 5 })
                .then(function (result) {
                    return result.data;
                }, function (error) {
                    logger.serverError(error);
                });
        }

        function postCodeAndState(item) {
            vm.cost.State = item.State;
        }

        function resetWeeks() {
            vm.cost.RegCleanWeeks = null;
            vm.cost.AdminCleanWeeks = null;
        }

        function createCost(cost) {
            console.log(cost);
            quoteCostService.createCost(cost).then(function(result) {
                $state.go('quote.estimation', {id:result.data.Id});
                $modalInstance.dismiss();
            }, function(error) {
                logger.serverError(error);
            });

        }

        function addrSelect(item) {
            if (item) {
                vm.cost.Company = item.Name;
                vm.cost.Address.Unit = item.Unit;
                vm.cost.Address.Number = item.Number;
                vm.cost.Address.Street = item.Street;
                vm.cost.Address.Suburb = item.Suburb;
                vm.cost.State = item.State;
                vm.cost.Postcode = item.Postcode;
            } else {
                vm.cost.Company = null;
                vm.cost.Address.Unit = null;
                vm.cost.Address.Number = null;
                vm.cost.Address.Street = null;
                vm.cost.Address.Suburb = null;
                vm.cost.State = null;
                vm.cost.Postcode = null;
            }
        }
    }

})();