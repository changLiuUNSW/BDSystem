(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuotePrintCtrl', modalQuotePrintCtrl);
    modalQuotePrintCtrl.$inject = ['$modalInstance', 'quoteService', 'logger', 'quote'];

    function modalQuotePrintCtrl($modalInstance, quoteService, logger, quote) {
        /* jshint validthis: true */
        var vm = this;
        vm.completeDate = undefined;
        
        vm.cancel = cancel;
        vm.ok = ok;
        function cancel() {
            $modalInstance.dismiss();
        }

        function ok() {
            var requestObj = {
                QuoteId: quote.Id,
                Url: quoteService.getQuoteDetailUrl(quote.Id),
                Date: vm.completeDate
            };
            quoteService.print(requestObj).then(function (result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }
    }
})();