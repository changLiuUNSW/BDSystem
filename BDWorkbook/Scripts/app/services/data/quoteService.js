(function() {
    'use strict';
    angular.module('app.resource.data')
        .factory('quoteService', quoteService);
    quoteService.$inject = ['$resource', '$location', '$state', 'Upload', 'typeLibrary'];
    function quoteService($resource, $location, $state, $upload, typeLibrary) {
        var currentDomain = config.clientAddress;
        var questionType = typeLibrary.quoteQuestionType;
        var baseUrl = config.ServerAddress + config.apiprefix + "quote";
        var service = {
            getQuoteDetailUrl: getQuoteDetailUrl,
            getQuoteIssueUrl:getQuoteIssueUrl,
            getQuote: getQuote,
            getAllStatus: getAllStatus,
            getOverDueList:getOverDueList,
            sendToWp: sendToWp,
            bdreview: bdreview,
            bdgmreview: bdgmreview,
            qpreview: qpreview,
            getToCurrentQuestions: getToCurrentQuestions,
            getNotCalledQuestions: getNotCalledQuestions,
            getNoClientEmailQuestions:getNoClientEmailQuestions,
            getQuoteDeadQuestions: getQuoteDeadQuestions,
            getQuoteNoDeadQuestions: getQuoteNoDeadQuestions,
            getAdjustQuestions: getAdjustQuestions,
            getQuestionResults:getQuestionResults,
            preFinalreview:preFinalreview,
            finalreview: finalreview,
            print:print,
            cancelQuote:cancelQuote,
            reviewFailed: reviewFailed,
            priceCheckFailed: priceCheckFailed,
            resolve: resolve,
            resolveUpload:resolveUpload,
            upload: upload,
            reUpload:reUpload,
            download: download,
            downloadPricePage:downloadPricePage,
            finalize: finalize,
            notContact: notContact,
            notSendEmail: notSendEmail,
            sendEmail:sendEmail,
            contact: contact,
            dead: dead,
            notDead: notDead,
            notAdjust: notAdjust,
            adjust:adjust,
            updateRate: updateRate
        };
        return service;


        function updateRate(id,rate) {
            return $resource(baseUrl + '/rate/' + id).save(rate).$promise;
        }

        function notAdjust(id, questionModels) {
            return $resource(baseUrl + '/notadjust/' + id).save(questionModels).$promise;
        }


        function notDead(id, questionModels) {
            return $resource(baseUrl + '/notdead/' + id).save(questionModels).$promise;
        }

        function dead(id, questionModels) {
            return $resource(baseUrl + '/dead/' + id).save(questionModels).$promise;
        }

        function notContact(id, questionModels) {
            return $resource(baseUrl + '/notcontact/' + id).save(questionModels).$promise;
        }

        function notSendEmail(id, questionModels) {
            return $resource(baseUrl + '/notsendemail/' + id).save(questionModels).$promise;
        }

        function sendEmail(id) {
            return $resource(baseUrl + '/sendemail/' + id).save().$promise;
        }

        function finalize(id, questionModels) {
            return $resource(baseUrl + '/finalize/' + id).save(questionModels).$promise;
        }

        function getAdjustQuestions() {
            return $resource(baseUrl + '/question').get({ questionType: questionType.noAdjust }).$promise;
        }

        function getQuoteDeadQuestions() {
            return $resource(baseUrl + '/question').get({ questionType: questionType.dead }).$promise;
        }

        function getQuoteNoDeadQuestions() {
            return $resource(baseUrl + '/question').get({ questionType: questionType.noDead }).$promise;
        }

        function getNotCalledQuestions() {
            return $resource(baseUrl + '/question').get({ questionType: questionType.notCalled }).$promise;
        }

        function getNoClientEmailQuestions() {
            return $resource(baseUrl + '/question').get({ questionType: questionType.noEmail }).$promise;
        }

        function getToCurrentQuestions() {
            return $resource(baseUrl + '/question').get({ questionType: questionType.toCurrent }).$promise;
        }


        function getQuoteIssueUrl(id) {
            return currentDomain + '/' + $state.href('quote.detail.progress.issues', { id: id }).$promise;
        }
        function getQuoteDetailUrl(id) {
            return currentDomain + '/' + $state.href('quote.detail.progress.overview', { id: id }).$promise;
        }

        function bdreview(params) {
            return $resource(baseUrl + '/bdreview').save(params).$promise;
        }

   

        function bdgmreview(params) {
            return $resource(baseUrl + '/bdgmreview').save(params).$promise;
        }

        function qpreview(params) {
            return $resource(baseUrl + '/qpreview').save(params).$promise;
        }

        function preFinalreview(params) {
            return $resource(baseUrl + '/prefinalreview').save(params).$promise;
        }

        function finalreview(params) {
            return $resource(baseUrl + '/finalreview').save(params).$promise;
        }

        function print(params) {
            return $resource(baseUrl + '/print').save(params).$promise;
        }

        function sendToWp(params) {
            return $resource(baseUrl + '/send').save(params).$promise;
        }

        function getQuote(params) {
            return $resource(baseUrl + '/:id').get(params).$promise;
        }

        function reviewFailed(params) {
            return $resource(baseUrl + '/reviewfailed').save(params).$promise;
        }

        function priceCheckFailed(params) {
            return $resource(baseUrl + '/pricecheckfailed').save(params).$promise;
        }

        function adjust(params) {
            return $resource(baseUrl + '/adjust').save(params).$promise;
        }

        function resolve(params) {
            return $resource(baseUrl + '/resolve').save(params).$promise;
        }

        function getAllStatus() {
            return $resource(baseUrl + '/status').get().$promise;
        }

        function getOverDueList() {
            return $resource(baseUrl + '/overdue').get().$promise;
        }

        function contact(params) {
            return $resource(baseUrl + '/contact').save(params).$promise;
        }

        function cancelQuote(params) {
            return $resource(baseUrl + '/cancel').save(params).$promise;
        }

        function getQuestionResults(params) {
            return $resource(baseUrl + '/result/:id').get(params).$promise;
        }

        function resolveUpload(resolveModel, file) {
            return $upload.upload({
                url: baseUrl + '/resolveupload',
                data: resolveModel,
                file: file
            });
        }
      

        function upload(model, file) {
            return $upload.upload({
                url: baseUrl + '/upload',
                data: model,
                file: file
            });
        }

        function reUpload(model, file) {
            return $upload.upload({
                url: baseUrl + '/reupload',
                data:model,
                file: file
            });
        }

        function downloadPricePage(id) {
            window.open(baseUrl + '/pricepage/' + id, '_blank', '');
        }

        function download(id) {
            window.open(baseUrl + '/download/'+id, '_blank', '');
        }
    }
})();