(function () {
    'use strict';
    angular.module('app.Lead.controllers')
        .controller('leadListGroupCtrl', leadListGroupCtrl);
    leadListGroupCtrl.$inject = ['leadService', 'logger', '$stateParams', 'searchService'];

    function leadListGroupCtrl(leadService, logger, $stateParams, searchService) {
        var vm = this;
        vm.status = $stateParams.status;
        vm.paging = {
            currentPage: 1,
            pageSize: 10,
            totalItems: 0
        };
        vm.Leads = [];
        vm.keyword = undefined;
        vm.setListPageSize = setListPageSize;
        vm.listPageChanges = listPageChanges;
        vm.loadLeadList = loadLeadList;
        init();

       

        function setListPageSize(newPageSize) {
            vm.paging.pageSize = newPageSize;
            vm.paging.currentPage = 1;
            loadLeadList();
        };

        function listPageChanges(newPage) {
            vm.paging.currentPage = newPage;
            loadLeadList();
        };

     

        function loadLeadList() {
            var requestObj = {
                pageSize: vm.paging.pageSize,
                currentPage: vm.paging.currentPage,
                status: vm.status,
                keyword: vm.keyword
            }

            return searchService.searchLead(requestObj).then(function (result) {
                vm.paging.totalItems = result.data.Total;
                vm.Leads = result.data.List;
         
            });
        }


        function init() {
            loadLeadList()
            .catch(function (error) {
                logger.serverError(error);
            });
        }
    }
})();