(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuoteEmailCtrl', modalQuoteEmailCtrl);
    modalQuoteEmailCtrl.$inject = ['$modalInstance', 'quote', 'quoteService', 'logger'];

    function modalQuoteEmailCtrl($modalInstance, quote, quoteService, logger) {
        /* jshint validthis:true */
        var vm = this;
        vm.action = undefined;
        vm.questions = undefined;
        vm.quote = quote;
        vm.cancel = cancel;
        vm.ok = ok;
        init();

        function cancel() {
            $modalInstance.dismiss();
        }

        function init() {
            loadingQuestion().catch(function(error) {
                logger.serverError(error);
            });
        }

        function ok(action) {
            switch (action.toLowerCase()) {
                case 'yes':
                    allowEmailSend();
                    break;
                case 'no':
                    notSendEmail();
                    break;
                default:
                    logger.error('cannot find corresponding action for '+action);
                    break;
            }
            
        }

        function notSendEmail() {
            var reqeustObj = [];
            angular.forEach(vm.questions, function (v, k) {
                var obj = {
                    QuestionId: v.Id,
                    AnswerId: v.result.Id,
                    Additional: v.result.additional
                };
                reqeustObj.push(obj);
            });

            return quoteService.notSendEmail(quote.Id, reqeustObj).then(function (result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }


        function allowEmailSend() {
            return quoteService.sendEmail(quote.Id).then(function (result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }
        function loadingQuestion() {
            vm.loading = true;
            return quoteService.getNoClientEmailQuestions().then(function (result) {
                vm.questions = result.data;
                vm.loading = false;
            });
        }
    }
})();