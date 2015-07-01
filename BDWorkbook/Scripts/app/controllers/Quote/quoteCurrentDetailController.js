(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCurrentDetailCtrl', quoteCurrentDetailCtrl);
    quoteCurrentDetailCtrl.$inject = ['$stateParams', 'quoteService', 'logger', '$modal', 'typeLibrary'];
    function quoteCurrentDetailCtrl($stateParams, quoteService, logger, $modal, typeLibrary) {
        /* jshint validthis: true */
        var vm = this;
        vm.quote = undefined;
        vm.questionType = typeLibrary.quoteQuestionType;
        vm.answerType = typeLibrary.quoteAnswerType;
        vm.quoteStatusType = typeLibrary.quoteStatusType;
        vm.tabs = ['Contact', 'Dead', 'Adjustment'];
        vm.tabs.selected = vm.tabs[0];
        vm.downloadQuote = downloadQuote;
        vm.contactModal = contactModal;
        vm.deadModal = deadModal;
        vm.adjustModal = adjustModal;
        vm.getDeadStatus = getDeadStatus;
        vm.getAdjustStatus = getAdjustStatus;
        vm.updateRating = updateRating;
        init();
        function init() {
            loadQuote();
        }


        function downloadQuote(quote) {
            quoteService.download(quote.Id);
        }

        function contactModal() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.contact.html',
                controller: 'ModalQuoteContactCtrl',
                controllerAs: 'vm',
                size: null,
                resolve:{
                    quote: function () {
                        return angular.copy(vm.quote);
                    }
                }
            });
            modalInstance.result.then(function (result) {
                loadQuote();
            });
        }

        function deadModal() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.dead.html',
                controller: 'ModalQuoteDeadCtrl',
                controllerAs: 'vm',
                size: null,
                resolve: {
                    quote: function () {
                        return angular.copy(vm.quote);
                    }
                }
            });
            modalInstance.result.then(function (result) {
                loadQuote();
            });
        }

        function adjustModal() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.adjust.html',
                controller: 'ModalQuoteAdjustCtrl',
                controllerAs: 'vm',
                size: null,
                resolve: {
                    quote: function () {
                        return angular.copy(vm.quote);
                    }
                }
            });
            modalInstance.result.then(function (result) {
                loadQuote();
            });
        }

       

        function getDeadStatus() {
            if (vm.quote.Status.Id === vm.quoteStatusType.Dead) {
                 return true;
            }
            for (var index in vm.quote.QuestionResults) {
                if (vm.quote.QuestionResults[index].Question.Type === vm.questionType.noDead) {
                 return false;
              }
            }
            return null;
        }

        function getAdjustStatus() {
            if (vm.quote.Status.Id === vm.quoteStatusType.WPIssues) {
                return true;
            }
            for (var index in vm.quote.QuestionResults) {
                if (vm.quote.QuestionResults[index].Question.Type === vm.questionType.noAdjust) {
                    return false;
                }
            }
            return null;
        }

        function updateRating(rate) {
            return quoteService.updateRate($stateParams.id, rate).then(function (result) {
                vm.quote = result.data;
                vm.rateEditing = false;
            }, function (error) {
                logger.serverError(error);
            });
        }

        function loadQuote() {
            vm.loading = true;
            return quoteService.getQuote({ id: $stateParams.id }).then(function (result) {
                vm.quote = result.data;
                console.log(vm.quote);
                vm.loading = false;
            }, function (error) {
                logger.serverError(error);
            });
        }
    }
})();