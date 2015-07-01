(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCurrentListCtrl', quoteCurrentListCtrl);
    quoteCurrentListCtrl.$inject = ['logger', 'searchService','quoteService', 'typeLibrary','$q'];

    function quoteCurrentListCtrl(logger, searchService,quoteService, typeLibrary,$q) {
        /* jshint validthis: true */
        var vm = this;
        vm.paging = {
            currentPage: 1,
            pageSize: 10,
            totalItems: 0
        };

        vm.quotes = [];
        vm.filter = {
            bizTypeSelect: undefined,
            keyword: undefined,
            order: 'asc',
            sortColumn: undefined,
            overdue: undefined
        };
        vm.overdueList = undefined;
        vm.bizTypesList = typeLibrary.businessType;
        vm.quoteStatusType = typeLibrary.quoteStatusType;
        vm.columnsList = searchService.currentQuoteConfig.sortableColumns;
        vm.loadingQuoteList = loadingQuoteList;
        vm.overdueClick = overdueClick;
        vm.setListPageSize = setListPageSize;
        vm.listPageChanges = listPageChanges;
        vm.orderClick = orderClick;
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

        function orderClick(order) {
            vm.filter.order = order;
            loadingQuoteList();
        }

        function overdueClick(value) {
            vm.filter.overdue = value;
            loadingQuoteList();
        }


        function loadingOverdueList() {
            return quoteService.getOverDueList().then(function (result) {
                vm.overdueList = result.data;
            }, function(error) {
                logger.serverError(error);
            });
        }

        function loadingQuoteList() {
            var requestObj = {
                PageSize: vm.paging.pageSize,
                CurrentPage: vm.paging.currentPage,
                Order: vm.filter.order,
                SortColumn: vm.filter.sortColumn,
                Keyword: vm.filter.keyword,
                QuoteType: vm.filter.bizTypeSelect,
                ContactCheckOverDue: false,
                DeadCheckOverDue: false,
                AjustCheckOverDue: false
            };
            if (vm.filter.overdue) {
                var propName = vm.filter.overdue;
                if (requestObj.hasOwnProperty(propName)) {
                    requestObj[propName] = true;
                }
            }
            return searchService.searchCurrentQuote(requestObj).then(function(result) {
                vm.paging.totalItems = result.data.Total;
                vm.quotes = result.data.List;
            }, function(error) {
                logger.serverError(error);
            });
        }


        function init() {
            $q.all(loadingQuoteList(), loadingOverdueList());
        }
    }
})();