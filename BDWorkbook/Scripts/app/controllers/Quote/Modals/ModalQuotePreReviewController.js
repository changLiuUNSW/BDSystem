(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuotePreReviewCtrl', modalQuotePreReviewCtrl);
    modalQuotePreReviewCtrl.$inject =
        ['$modalInstance', 'quoteService', 'logger', 'quote', 'quoteWorkFlowHelper', 'SweetAlert'];

    function modalQuotePreReviewCtrl($modalInstance, quoteService, logger, quote, quoteWorkFlowHelper, sweetAlert) {
        /* jshint validthis: true */
        var vm = this;
        vm.cancel = cancel;
        vm.files = undefined;
        vm.ok = ok;
        vm.fileChanged = quoteWorkFlowHelper.fileChanged;
        vm.validate = quoteWorkFlowHelper.validate;
        function ok(action) {
            var requestObj = {
                QuoteId: quote.Id,
                Url: quoteService.getQuoteDetailUrl(quote.Id)
            };
            switch (action) {
            case 'yes':
                sweetAlert.confirm('Please Confirm',
                    'The quote will be sent to be final reivewed. Confirm?').then(function () {
                    reviewSuccess(requestObj);
                });
                break;
            case 'no':
                uploadFile(vm.files, requestObj);
                break;
            default:
                logger.error('Cannot find corresponding action for ' + action);
                break;
            }
        }


        function reviewSuccess(requestObj) {
            quoteService.preFinalreview(requestObj).then(function (result) {
                logger.success('Quote has been updated successfully.', 'Success');
                $modalInstance.close(result.data);
            }, function(error) {
                logger.serverError(error);
            });
        }

        function uploadFile(requestObj,files) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                quoteService.reUpload(requestObj,file)
                    .success(function(result) {
                        logger.success('Quote has been updated successfully.', 'Success');
                        $modalInstance.close(result.data);
                    }).error(function(error) {
                        logger.serverError(error);
                    });
            }
        }

        function cancel() {
            $modalInstance.dismiss();
        }
    }

})();