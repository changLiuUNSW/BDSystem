(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteProgressGroupCtrl', quoteProgressGroupCtrl);
    quoteProgressGroupCtrl.$inject = ['logger','searchService', '$stateParams'];

    function quoteProgressGroupCtrl(logger, searchService, $stateParams) {
        /* jshint validthis: true */
        var vm = this;
        vm.status = $stateParams.status;
        vm.paging = {
            currentPage: 1,
            pageSize: 10,
            totalItems: 0
        };
        vm.quotes = [];
        vm.keyword = undefined;
        vm.loadingQuoteList = loadingQuoteList;
        vm.setListPageSize = setListPageSize;
        vm.listPageChanges = listPageChanges;
        init();


        function setListPageSize(newPageSize) {
            vm.paging.pageSize = newPageSize;
            vm.paging.currentPage = 1;
            loadingQuoteList();
        }

        function listPageChanges(newPage) {
            vm.paging.currentPage = newPage;
            loadingQuoteList();
        }


        function loadingQuoteList() {
            var requestObj = {
                pageSize: vm.paging.pageSize,
                currentPage: vm.paging.currentPage,
                status: vm.status,
                keyword: vm.keyword
            };
            return searchService.searchQuote(requestObj).then(function (result) {
                vm.paging.totalItems = result.data.Total;
                vm.quotes = result.data.List;
            });
        }

        function init() {
            loadingQuoteList().catch(function (error) {
                logger.serverError(error);
            });
        }
     
    }
})();