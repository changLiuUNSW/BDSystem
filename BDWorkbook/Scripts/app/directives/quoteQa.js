(function() {
    'use strict';
    angular.module('app.directives')
        .directive('quoteQa', quoteQuestionAndAnswer);

    function quoteQuestionAndAnswer() {
        return {
            templateUrl: 'tpl/Shared/quoteAnswers.html',
            restrict: 'A', //Only matches attribute name
            controller: quoteQaController,
            controllerAs: 'vm',
            scope: {
                quoteQa: '=',
                qaType: '='
            },
        };
    }

    quoteQaController.$inject = ['$scope', 'typeLibrary', 'quoteService', 'logger'];

    function quoteQaController($scope, typeLibrary, quoteService, logger) {
        var vm = this;
        vm.answerType = typeLibrary.quoteAnswerType;
        vm.paging = {
            currentPage: 1,
            pageSize: 5,
            totalItems: 0
        };
        vm.setListPageSize = setListPageSize;
        vm.listPageChanges = listPageChanges;
        vm.resultList = [];
        init();
        function init() {
            loadingQuestionResults();
        }

        function loadingQuestionResults() {
            return quoteService.getQuestionResults({ id: $scope.quoteQa, type: $scope.qaType }).then(function (result) {
                vm.resultList = result.data;
                vm.paging.totalItems = vm.resultList.length;
                }, function(error) {
                    logger.serverError(error);
                }
            );
        }

        function setListPageSize(newPageSize) {
            vm.paging.pageSize = newPageSize;
            vm.paging.currentPage = 1;
           
        };

        function listPageChanges(newPage) {
            vm.paging.currentPage = newPage;
      
        };
    }
})();