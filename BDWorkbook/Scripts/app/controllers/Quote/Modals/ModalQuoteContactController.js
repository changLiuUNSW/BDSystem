(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuoteContactCtrl', modalQuoteContactCtrl);
    modalQuoteContactCtrl.$inject = ['$modalInstance', 'quote', 'quoteService', 'logger'];

    function modalQuoteContactCtrl($modalInstance, quote, quoteService, logger) {
        /* jshint validthis:true */
        var vm = this;
        vm.lastContactDate = undefined;
        vm.action = undefined;
        vm.questions = undefined;
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
                    contact();
                    break;
                case 'no':
                    notContact();
                    break;
                default:
                    logger.error('cannot find corresponding action for '+action);
                    break;
            }
            
        }

        function notContact() {
            var reqeustObj = [];
            angular.forEach(vm.questions, function (v, k) {
                var obj = {
                    QuestionId: v.Id,
                    AnswerId: v.result.Id,
                    Additional: v.result.additional
                };
                reqeustObj.push(obj);
            });

            return quoteService.notContact(quote.Id, reqeustObj).then(function (result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function (error) {
                logger.serverError(error);
            });
        }


        function contact() {
            var requestObj = {
                QuoteId: quote.Id,
                Date:vm.lastContactDate
            };
            quoteService.contact(requestObj).then(function (result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function(error) {
                logger.serverError(error);
            });
        }


        function loadingQuestion() {
            vm.loading = true;
            return quoteService.getNotCalledQuestions().then(function (result) {
                vm.questions = result.data;
                vm.loading = false;
            });
        }
    }
})();