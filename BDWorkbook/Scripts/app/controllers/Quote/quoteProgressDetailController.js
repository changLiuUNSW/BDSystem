(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteProgressDetailCtrl', quoteProgressDetailCtrl);
    quoteProgressDetailCtrl.$inject = ['quoteService',
        'quoteCostService',
        '$state',
        'quoteWorkFlowHelper',
        '$stateParams',
        'logger', '$modal', 'SweetAlert', '$filter', 'typeLibrary'];
    function quoteProgressDetailCtrl(quoteService,
        quoteCostService,
        $state,
        quoteWorkFlowHelper, $stateParams, logger, $modal, sweetAlert, $filter, typeLibrary) {
        /* jshint validthis: true */
        var vm = this;
        vm.quote = undefined;
        vm.loadQuote = loadQuote;
        //Page size is shared by all list ui such as cost estimation list,issue list
        vm.pageSize = 5;
        //TODO: this Page NO is for cost estimation only, need to be removed and initialize from ng-init
        vm.currentPage = 0;
        vm.bulkActions = ['Delete', 'Finalize'];
        vm.selectedBulkAction = undefined;
        vm.costActions = ['Edit', 'Delete', 'Finalize'];
        vm.businessType = typeLibrary.businessType;
        vm.numberOfPages = numberOfPages;
        vm.resetPage = resetPage;
        vm.enableCostAdd = quoteWorkFlowHelper.enableCostAdd;
        vm.nextStep = quoteWorkFlowHelper.nextStep;
        vm.checkAllDone = quoteWorkFlowHelper.checkAllDone;
        vm.resolveIssue = quoteWorkFlowHelper.resolveIssue;
        vm.enableSendBtn = quoteWorkFlowHelper.enableSendBtn;
        vm.bdgmReview = quoteWorkFlowHelper.bdgmReview;
        vm.bdReview = quoteWorkFlowHelper.bdReview;
        vm.fileChanged = quoteWorkFlowHelper.fileChanged;
        vm.validate = quoteWorkFlowHelper.validate;
        vm.cancelQuote = cancelQuote;
        vm.uploadCost = uploadCost;
        vm.costAction = costAction;
        vm.bulkAction = bulkAction;
        vm.downloadQuote = downloadQuote;
        vm.downloadPricePage = downloadPricePage;
        vm.downloadCost = downloadCost;
        vm.sendToWp = sendToWp;
        vm.sendToWpNoInfo = sendToWpNoInfo;
        vm.addCost = addCost;
        vm.navigate = navigate;
        init();
        
        function navigate(cost) {
            var type = cost.CostType;
            var hrefUrl;
            switch (type) {
                //Upload
//                case 1:
//                    hrefUrl = $state.href('quote.progress.group', { id: cost.Id });
//                    break;
               //System
                case 2:
                    hrefUrl = $state.href('quote.estimation', { id: cost.Id });
                    break;
                default:
                    hrefUrl = $state.href('access.404');
                    break;
            }
            return hrefUrl;
        }


        function cancelQuote(quote) {
            sweetAlert.confirm('Please Confirm', 'Cancel this quote ?').then(function() {
                quoteService.cancelQuote(quote.Id).then(function (result) {
                    loadQuote();
                    logger.success('quote has been cancelled successfully.', 'Success');
                }, function (error) {
                    logger.serverError(error);
                });
            });
        }

        function resetPage() {
            vm.currentPage = 0;
        }

        function bulkAction() {
            if (!vm.selectedBulkAction) {
                return;
            }
            var costs = selectedCosts();
            if (costs.length < 1) {
                vm.selectedBulkAction = null;
                logger.info('you do not select any cost estimations', 'Note');
                return;
            }
            var costIds = $filter('map')(costs, 'Id');
            switch (vm.selectedBulkAction) {
            case 'Delete':
                vm.selectedBulkAction = null;
                deleteCostConfirm(costIds);
                break;
            case 'Finalize':
                vm.selectedBulkAction = null;
                finalizeConfirm(costIds);
                break;
            default:
                logger.error('cannot find corresponding action', 'Error');
                break;
            }
        }


        function selectedCosts() {
            return $filter('filter')(vm.quote.QuoteCost, { selected: true });
        }

        function deleteCostConfirm(ids) {
            sweetAlert.confirm('Please Confirm', 'Delete the selected cost estimations ?').then(function() {
                quoteCostService.removeCosts(ids).then(function (result) {
                    logger.success('cost estimations has been deleted successfully.', 'Success');
                    loadQuote();
                }, function (error) {
                    logger.serverError(error);
                });
            });
        }

        function finalizeConfirm(ids) {
            sweetAlert.confirm('Please Confirm',
                'You cannot do any further actions after you finalize. Do you still want to process this action? ')
                .then(function () {
                quoteCostService.finalize(ids).then(function (result) {
                    logger.success('cost estimations has been finalized successfully.', 'Success');
                    loadQuote();

                }, function (error) {
                    logger.serverError(error);
                });
            });
        }

        function numberOfPages(length) {
            return Math.ceil(length / vm.pageSize);
        }

        function init() {
            loadQuote();
        }

     
        function addCost() {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/quote/estimation/modals/modal.estimation.preSteps.html',
                controller: 'ModalEstimationPreStepsCtrl',
                size: null,
                controllerAs: 'vm',
                resolve:{
                    quote: function () {
                        return vm.quote;
                    }
                }
            });
//            modalInstance.result.then(function () {
//            });
        }

        function sendToWp() {
           // logger.error('send to WP for cleaning type is not implemented yet', 'error');
            var modalInstance = $modal.open({
                templateUrl: 'tpl/quote/modals/modal.quote.preSendToWp.html',
                controller: 'ModalSendToWPPreStepsCtrl',
                size: 'lg',
                controllerAs: 'vm',
                resolve: {
                    quote: function () {
                        return vm.quote;
                    }
                }
            });
            modalInstance.result.then(function () {
            });
        }

        function sendToWpNoInfo() {
            sweetAlert.confirm('Please Confirm',
                'You cannot do any further actions after you send to WP. Do you still want to process this action ?')
                .then(function () {
                var requestObj = {
                    QuoteId: vm.quote.Id,
                    Url: quoteService.getQuoteDetailUrl(vm.quote.Id)
                };
                quoteService.sendToWp(requestObj).then(function (result) {
                    loadQuote();
                    logger.success('Cost estimations have been sent to WP successfully.', 'Success');
                }, function (error) {
                    logger.serverError(error);
                });
            });
        }

        function downloadQuote(quote) {
            quoteService.download(quote.Id);
        }

        function downloadPricePage(quote) {
            quoteService.downloadPricePage(quote.Id);
        }

        function downloadCost(quote) {
            if (quote.State.toUpperCase() === 'NZ') {
                quoteCostService.download('nz');
            } else {
                if (quote.BusinessType.Id !== vm.businessType.cleaning) {
                    quoteCostService.download(quote.BusinessType.Type);
                }
            }
        }

        function costAction(cost) {
            if (!cost.action) {
                return;
            }
            switch (cost.action) {
            case 'Edit':
                cost.action = null;
                editCost(cost);
                break;
            case 'Delete':
                cost.action = null;
                deleteCostConfirm([cost.Id]);
                break;
            case 'Finalize':
                cost.action = null;
                finalizeConfirm([cost.Id]);
                break;
            default:
                logger.error('cannot find corresponding action for ' + cost.action, 'Error');
                cost.action = null;
                break;
            }
        }

        function uploadCost(cost) {
            var modalInstance = $modal.open({
                templateUrl: 'modal.quote.cost.upload.html',
                controller: 'ModalQuoteCostUploadCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    quoteId: function() {
                        return vm.quote.Id;
                    },
                    cost: function() {
                        if (cost) {
                            return angular.copy(cost);
                        }
                        return null;
                    }
                }
            });
            modalInstance.result.then(function() {
                loadQuote();
            });
        }

        function editCost(cost) {
            switch (cost.CostType) {
                //Type is Upload
            case 1:
                uploadCost(cost);
                break;
            //Type is sytem calcuator
            case 2:
                logger.error('not implement', 'error');
                break;

            }
        }


        function loadQuote() {
            vm.loading = true;
            return quoteService.getQuote({ id: $stateParams.id }).then(function (result) {
                vm.quote = result.data;
                resetPage();
                console.log(vm.quote);
                vm.loading = false;
            }, function(error) {
                logger.serverError(error);
            });
        }
    }
})();