(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuoteAdjustCtrl', modalQuoteAdjustCtrl);
    modalQuoteAdjustCtrl.$inject = ['$modalInstance', 'quote', 'quoteService', 'logger'];

    function modalQuoteAdjustCtrl($modalInstance, quote, quoteService, logger) {
        /* jshint validthis:true */
        var vm = this;
        vm.quote = quote;
        vm.action = undefined;
        vm.newPa = undefined;
        vm.print = {};
        vm.questions = undefined;
        vm.cancel = cancel;
        vm.ok = ok;
        init();

        function cancel() {
            $modalInstance.dismiss();
        }

        function init() {
            loadingQuestion();
        }

        function ok(action) {
            switch (action.toLowerCase()) {
            case 'yes':
                adjust();
                break;
            case 'no':
                notAdjust();
                break;
            default:
                logger.error('cannot find corresponding action for ' + action);
                break;
            }

        }

        function notAdjust() {
            var reqeustObj = [];
            angular.forEach(vm.questions, function (v, k) {
                var obj = {
                    QuestionId: v.Id,
                    AnswerId: v.result.Id,
                    Additional: v.result.additional
                };
                reqeustObj.push(obj);
            });

            return quoteService.notAdjust(quote.Id, reqeustObj).then(function(result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function(error) {
                logger.serverError(error);
            });
        }


        function adjust() {
            var reqeustObj = {
                NewPa: vm.newPa,
                QuoteId: vm.quote.Id,
                Url: quoteService.getQuoteIssueUrl(vm.quote.Id),
                pricePageOnly: vm.print.pricePageOnly,
                isPdf: vm.print.isPdf,
                IssueDetail: '(Price PA changed from ' +
                    vm.quote.TotalPA +
                    ' to ' + vm.newPa +
                    ') Description: ' +
                    vm.description
            };
            return quoteService.adjust(reqeustObj).then(function (result) {
                logger.success('Issue has been raised successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }


        function loadingQuestion() {
            vm.loading = true;
            quoteService.getAdjustQuestions().then(function (result) {
                vm.questions =result.data;
                vm.loading = false;
            }, function(error) {
                logger.serverError(error);
            });
        }
    }
})();