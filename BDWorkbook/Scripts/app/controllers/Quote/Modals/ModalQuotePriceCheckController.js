(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuotePriceCheckCtrl', modalQuotePriceCheckCtrl);
    modalQuotePriceCheckCtrl.$inject = ['$modalInstance', 'quoteService', 'logger', 'quote'];

    function modalQuotePriceCheckCtrl($modalInstance, quoteService, logger, quote) {
        /* jshint validthis: true */
        var vm = this;
        vm.newPa = undefined;
        vm.description = undefined;
        vm.questions = undefined;
        vm.print = {};
        vm.quote = quote;
        vm.cancel = cancel;
        vm.ok = ok;
        init();

        function cancel() {
            $modalInstance.dismiss();
        }

        function ok(action) {
            switch (action) {
                case 'yes':
                    reviewSuccess();
                    break;
                case 'no':
                    reviewFailed(quote.Id, vm.description, vm.newPa);
                    break;
                default:
                    logger.error('Cannot find corresponding action for ' + action);
                    break;
            }
        }


        function init() {
            vm.loading = true;
            return quoteService.getToCurrentQuestions().then(function (result) {
                vm.questions = result.data;
                vm.loading = false;
            });
        }

        function reviewSuccess() {
            var reqeustObj = [];
            angular.forEach(vm.questions, function(v, k) {
                var obj = {
                    QuestionId:v.Id,
                    AnswerId:v.result.Id
                };
                reqeustObj.push(obj);
            });
            return quoteService.finalize(quote.Id, reqeustObj).then(function (result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }

        function reviewFailed(quoteId, description,newPa) {
            var reqeustObj = {
                NewPa: newPa,
                QuoteId: quoteId,
                Url: quoteService.getQuoteIssueUrl(quoteId),
                pricePageOnly: vm.print.pricePageOnly,
                isPdf: vm.print.isPdf,
                IssueDetail: '(Price PA changed from ' +
                    vm.quote.TotalPA + ' to ' +
                    newPa + ') Description: ' +
                    description
            };
            return quoteService.priceCheckFailed(reqeustObj).then(function (result) {
                logger.success('Issue has been raised successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }

    }
})();