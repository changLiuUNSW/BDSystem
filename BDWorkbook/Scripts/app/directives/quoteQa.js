(function() {
    'use strict';
    angular.module('app.directives')
    .directive('quoteQa', quoteQuestionAndAnswer);

    function quoteQuestionAndAnswer() {
        return {
            templateUrl: 'tpl/Shared/quoteAnswers.html',
            replace:true,
            restrict: 'A', //Only matches attribute name
            scope: {
                quoteQa: '=',
                qaType:'='
            },

        };
    }
})();
