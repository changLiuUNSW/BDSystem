(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalSendToWPPreStepsCtrl', ModalSendToWPPreStepsCtrl);
    ModalSendToWPPreStepsCtrl.$inject
        = ['logger', '$state', 'quote', '$modalInstance'];

    function ModalSendToWPPreStepsCtrl(logger, $state, quote, $modalInstance) {
        var vm = this;
        vm.quote = quote;
        vm.cancel = cancel;
        vm.confirm = confirm;

        function cancel() {
            $modalInstance.dismiss();
        };
        function confirm() {
            
        };

    }

})();