(function() {
    'use strict';
    angular.module('app.resource.helper')
        .service('typeLibrary', typeLibrary);
    typeLibrary.$inject = [];

    function typeLibrary() {

        this.quoteAnswerType = {
            None: 0,
            Date: 1,
            Text: 2
        };
        this.quoteQuestionType = {
            toCurrent: 1,
            notCalled: 2,
            dead: 3,
            noDead: 4,
            noAdjust: 5,
            noEmail:6
        };

        this.businessType = {
            cleaning: 1,
            security: 2,
            maintenance: 3,
            others: 4
        };
        this.quoteStatusType =
        {
            New:1,
            Estimation: 2,
            WPReview:3,
            QPReview:4,
            BDReview:5,
            PreFinalReview:6,
            FinalReview:7,
            Print:8,
            PresentToClient:9,
            WPIssues:10,
            QPIssues: 11,
            Current:12,
            Cancel:13,
            Dead:14
        };

        this.callLineStatus = {
            attempted: 1,
            contacted: 1 << 1,
            notAnswered: 1 << 2
        };

        this.ScriptActions = {
            DaToCheck: 19,
            ExternalManagement: 12,
            SendInfo: 22,
            UpdateCallbackDate: 20
        };
    }
})();