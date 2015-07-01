(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuoteReviewCtrl', modalQuoteReviewCtrl);
    modalQuoteReviewCtrl.$inject =
        ['$scope', '$modalInstance', 'quoteService', 'logger', 'modalConfig', 'quoteWorkFlowHelper'];

    function modalQuoteReviewCtrl($scope, $modalInstance, quoteService, logger,modalConfig,quoteWorkFlowHelper) {
        /* jshint validthis: true */
        var vm = this;
        vm.cancel = cancel;
        vm.files = undefined;
        vm.uploadReq = modalConfig.uploadReq;
        vm.description = undefined;
        vm.ok = ok;
        vm.fileChanged = quoteWorkFlowHelper.fileChanged;
        vm.validate = quoteWorkFlowHelper.validate;
        function ok(action) {
            switch (action) {
                case 'yes':
                    reviewSuccess();
                break;
                case 'no':
                    reviewFailed(modalConfig.quote.Id, vm.description);
                    break;
            default:
                logger.error('Cannot find corresponding action for ' + action);
                break;
            }
        }


        function reviewSuccess() {
            var requestObj = {
                QuoteId: modalConfig.quote.Id,
                Url: quoteService.getQuoteDetailUrl(modalConfig.quote.Id)
            };
            if (modalConfig.uploadReq) {
                uploadFile(vm.files, requestObj);
            } else if (modalConfig.bdReview) {
                quoteService.bdreview(requestObj).then(function (result) {
                    logger.success('Quote has been updated successfully', 'Success');
                    $modalInstance.close(result.data);
                }, function (error) {
                    logger.serverError(error);
                });
            } else if (modalConfig.bdgmReview) {
                quoteService.bdgmreview(requestObj).then(function (result) {
                    logger.success('Issue has been updated successfully', 'Success');
                    $modalInstance.close(result.data);
                }, function (error) {
                    logger.serverError(error);
                });
            } else if (modalConfig.qpReview) {
                quoteService.qpreview(requestObj).then(function (result) {
                    logger.success('Quote has been updated successfully', 'Success');
                    $modalInstance.close(result.data);
                }, function (error) {
                    logger.serverError(error);
                });
            } else if (modalConfig.finalReview) {
                quoteService.finalreview(requestObj).then(function (result) {
                    logger.success('Quote has been updated successfully', 'Success');
                    $modalInstance.close(result.data);
                }, function (error) {
                    logger.serverError(error);
                });
            } else {
                logger.error('not implement');
            }
        }

        function reviewFailed(id, description) {
            var reqeustObj = {
                QuoteId: id,
                Url: quoteService.getQuoteIssueUrl(id),
                IssueDetail: description
            };
            return quoteService.reviewFailed(reqeustObj).then(function (result) {
                logger.success('Issue has been raised successfully', 'Success');
                $modalInstance.close(result.data);
           }, function(error) {
                logger.serverError(error);
            });
        }
        function uploadFile(files, requestObj) {
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                quoteService.upload(requestObj, file)
                    .success(function (result) {
                        logger.success('Quote has been saved successfully.', 'Success');
                        $modalInstance.close(result.data);
                }).error(function (error) {
                        logger.serverError(error);
                    });
            }


        }

        function cancel() {
            $modalInstance.dismiss();
        }
    }

})();