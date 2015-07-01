(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuoteDeadCtrl', modalQuoteDeadCtrl);
    modalQuoteDeadCtrl.$inject =
        ['$modalInstance', 'quote', 'quoteService', 'logger', '$q'];

    function modalQuoteDeadCtrl($modalInstance, quote, quoteService, logger, $q) {
        /* jshint validthis: true */
        var vm = this;
        vm.action = undefined;
        vm.deadQuestions = undefined;
        vm.notDeadQuestions = undefined;
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
                dead();
                break;
            case 'no':
                notDead();
                break;
            default:
                logger.error('cannot find corresponding action for ' + action);
                break;
            }

        }

        function notDead() {
            var reqeustObj = [];
            angular.forEach(vm.notDeadQuestions, function(v, k) {
                var obj = {
                    QuestionId: v.Id,
                    AnswerId: v.result.Id,
                    Additional: v.result.additional
                };
                reqeustObj.push(obj);
            });

            return quoteService.notDead(quote.Id, reqeustObj).then(function(result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function(error) {
                logger.serverError(error);
            });
        }


        function dead() {
            var reqeustObj = [];
            angular.forEach(vm.deadQuestions, function(v, k) {
                var obj = {
                    QuestionId: v.Id,
                    AnswerId: v.result.Id,
                    Additional: v.result.additional
                };
                reqeustObj.push(obj);
            });
            quoteService.dead(quote.Id, reqeustObj).then(function(result) {
                logger.success('Quote has been updated successfully', 'Success');
                $modalInstance.close(result.data);
            }, function(error) {
                logger.serverError(error);
            });
        }


        function loadingQuestion() {
            vm.loading = true;
            $q.all([
                quoteService.getQuoteDeadQuestions(),
                quoteService.getQuoteNoDeadQuestions()
            ]).then(function(results) {
                vm.deadQuestions = results[0].data;
                vm.notDeadQuestions = results[1].data;
                vm.loading = false;
            }, function(error) {
                logger.serverError(error);
            });
        }
    }
})();