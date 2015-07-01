(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('ModalQuoteCostUploadCtrl', modalQuoteCostUploadCtrl);
    modalQuoteCostUploadCtrl.$inject =
        ['$scope', '$modalInstance', 'logger', 'quoteCostService', 'quoteId', 'cost', 'saleBoxService'];

    function modalQuoteCostUploadCtrl($scope, $modalInstance, logger, quoteCostService, quoteId,cost, saleBoxService) {
        var extensions = {
            'xls': true,
            'xlsx': true,
            'xlsm': true
        };
        /* jshint validthis: true */
        var vm = this;
        vm.files = undefined;
        vm.cancel = cancel;
        vm.uploadFile = uploadFile;
        vm.validate = validate;
        vm.fileChanged = fileChanged;
        vm.getPostCodeAndState = getPostCodeAndState;
        vm.postCodeAndState = postCodeAndState;

        vm.cost =cost || {
            QuoteId: quoteId
        };


        function fileChanged($files, $event, $rejectedFiles) {
            if ($rejectedFiles && $rejectedFiles.length)
            {
                logger.error(
                    'Please check your file type (Allowed: xls, xlsx,xlsm) or size (maximum: 2 MB). ',
                    'Error');
                }
        }

        function validate(file) {
            if (file.name) {
                var ext = file.name.substr(file.name.lastIndexOf('.') + 1);
                if (extensions[ext]) {
                    return true;
                }
            }
            return false;
        }

        function getPostCodeAndState(state, postCode) {
            return saleBoxService.getSalesbox
                ({ postCode: postCode, state: state, take: 5 })
                .then(function (result) {
                return result.data;
            }, function (error) {
                logger.serverError(error);
            });
        }

        function postCodeAndState(item) {
            vm.cost.State = item.State;
        }



        function uploadFile(files) {
            console.log(vm.cost);
            for (var i = 0; i < files.length; i++) {
                var file = files[i];
                quoteCostService.uploadCost(vm.cost, file)
                    .success(function (result) {
                        logger.success('cost estimations has been saved successfully.','Success');
                        $modalInstance.close(result.data);

                }).error(function(error) {
                    logger.serverError(error);
                });
            }


        }

        function cancel() {
            $modalInstance.dismiss();
        }
    }

})();