(function () {
    'use strict';
    angular.module('app.resource.helper')
        .factory('quoteWorkFlowHelper', quoteWorkFlowHelper);
    quoteWorkFlowHelper.$inject = ['logger', '$state', '$modal', '$filter', 'quoteService'];

    function quoteWorkFlowHelper(logger, $state, $modal, $filter, quoteService) {
        var extensions = {
            'doc': true,
            'docx': true,
            'pdf': true
        };
        var services = {
            nextStep: nextStep,
            bdReview: bdReview,
            bdgmReview: bdgmReview,
            checkAllDone: checkAllDone,
            enableCostAdd: enableCostAdd,
            enableSendBtn: enableSendBtn,
            resolveIssue: resolveIssue,
            validate: validate,
            fileChanged: fileChanged
        }
        return services;

        function validate(file) {
            if (file.name) {
                var ext = file.name.substr(file.name.lastIndexOf('.') + 1);
                if (extensions[ext]) return true;
            }
            return false;
        }
        function fileChanged($files, $event, $rejectedFiles) {
            if ($rejectedFiles && $rejectedFiles.length)
                logger.error('Please check your file type (Allowed: doc, docx, pdf) or size (maximum: 20 MB). ', 'Error');
        }

        function nextStep(quote, context) {
            var status = quote.Status.Name||'';
            switch (status.toLowerCase()) {
                case 'new':
                    $state.go('quote.detail.progress.estimation', { id: quote.Id });
                    finishCostAndClickSend(quote);
                    break;
                case 'estimation':
                    $state.go('quote.detail.progress.estimation', { id: quote.Id });
                    finishCostAndClickSend(quote);
                    break;
                case 'wpreview':
                    quoteWpReviewModal(quote, context);
                    break;
                case 'qpreview':
                    qpReviewModal(quote,context);
                    break;
                case 'prefinalreview':
                    prefinalreviewModal(quote,context);
                    break;
                case 'finalreview':
                    finalReviewModal(quote, context);
                    break;
                case 'print':
                    printModal(quote, context);
                    break;
                case 'presenttoclient':
                    priceCheckModal(quote, context);
                    break;
                case 'wpissues':
                    $state.go('quote.detail.progress.issues', { id: quote.Id });
                    logger.info("Please resolve the outstanding issue under 'Issues' section.", 'Note');
                    break;
                case 'qpissues':
                    $state.go('quote.detail.progress.issues', { id: quote.Id });
                    logger.info("Please resolve the outstanding issue under 'Issues' section.", 'Note');
                    break;
                default:
                    logger.error('Can not find corresponding action for status: ' + status, 'Error');
                    break;
            }
        }

        function priceCheckModal(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.priceCheck.html',
                controller: 'ModalQuotePriceCheckCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    quote: function () {
                        return angular.copy(quote);
                    }
                }

            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function printModal(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.print.html',
                controller: 'ModalQuotePrintCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    quote: function () {
                        return angular.copy(quote);
                    }
                }

            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function bdReview(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.review.html',
                controller: 'ModalQuoteReviewCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    modalConfig: function () {
                        return {
                            quote: angular.copy(quote),
                            bdReview:true,
                        }
                    }
                }

            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function bdgmReview(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.review.html',
                controller: 'ModalQuoteReviewCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    modalConfig: function () {
                        return {
                            quote: angular.copy(quote),
                            bdgmReview: true,
                        }
                    }
                }

            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }


        function finishCostAndClickSend(quote) {
            if (checkCost(quote) === true) {
                logger.info("Please click 'Send' button under 'Estiomation' section.", 'Note');
            }
        }


        function checkCost(quote) {
            var result = checkAllDone(quote);
            if (result === null) {
                logger.info('No cost estimation found, please create cost estimations', 'Note');
            } else if(result===false) {
                    logger.info('Please finalize all the cost estimations', 'Note');
            }
            return result;
        }

        function prefinalreviewModal(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.prereview.html',
                controller: 'ModalQuotePreReviewCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    quote: function () {
                        return angular.copy(quote);
                    }
                }
            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function finalReviewModal(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.review.html',
                controller: 'ModalQuoteReviewCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    modalConfig: function () {
                        return {
                            quote: angular.copy(quote),
                            finalReview: true
                        }
                    }
                }
            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function qpReviewModal(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.review.html',
                controller: 'ModalQuoteReviewCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    modalConfig: function () {
                        return {
                            quote: angular.copy(quote),
                            qpReview: true
                        }
                    }
                }

            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function quoteWpReviewModal(quote, context) {
            var modalInstance = $modal.open({
                templateUrl: 'tpl/Quote/Modals/modal.quote.review.html',
                controller: 'ModalQuoteReviewCtrl',
                size: null,
                controllerAs: 'vm',
                resolve: {
                    modalConfig: function() {
                        return {
                            quote: angular.copy(quote),
                            uploadReq:true
                        }
                    }
                }

            });
            modalInstance.result.then(function (result) {
                context.loadQuote();
            });
        }

        function checkAllDone(quote) {
            if (angular.isArray(quote.QuoteCost) && quote.QuoteCost.length > 0) {
                var alldone = true;
                for (var i = 0; i < quote.QuoteCost.length; i++) {
                    if (quote.QuoteCost[i].Status === 0) {
                        alldone = false;
                        break;
                    }
                }
                return alldone;
            }
            return null;
        }

     

        function enableSendBtn(quote) {
            var status = quote.Status.Name || '';
            switch (status.toLowerCase()) {
                case 'new':
                    return true;
                case 'estimation':
                    return true;
                default:
                    return false;
            }
        }

        function enableCostAdd(quote) {
            var status = quote.Status.Name || '';
            switch (status.toLowerCase()) {
                case 'new':
                    return true;
                case 'estimation':
                    return true;
                case 'qpissues':
                    return true;
                default:
                    return false;   
            }
        }

        function resolveIssue(issue, context) {
            var quoteFinalize = true;
            var quote = context.quote;
            var reqeustObj = {
                "IssueId": issue.Id,
                "Url": quoteService.getQuoteDetailUrl(quote.Id),
                "Comment": issue.SolverComments
            }  
            if (enableCostAdd(quote)) {
                quoteFinalize = checkCost(quote) ;
            }

            if (quoteFinalize) {
                if (issue.UploadRequired) {
                    for (var i = 0; i < issue.files.length; i++) {
                        var file = issue.files[i];
                        quoteService.resolveUpload(reqeustObj, file)
                            .success(function (result) {
                                context.loadQuote();
                                logger.success('Issue has been resolved successfully.', 'Success');
                            }).error(function (error) {
                                logger.serverError(error);
                            });
                    }
                } else {
                    quoteService.resolve(reqeustObj).then(function (result) {
                        context.loadQuote();
                        logger.success('Issue has been resolved successfully.', "Success");
                    }, function (error) {
                        logger.serverError(error);
                    });
                }
               
            }
        }
    }
})();