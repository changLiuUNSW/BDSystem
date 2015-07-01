(function() {
    'use strict';
    angular.module('app.directives')
    .directive('quoteQuestion', quoteQuestion);

    function quoteQuestion() {
        return {
            restrict: 'A',
            scope: {
                quoteQuestion: "="
            },
            templateUrl: 'tpl/Shared/quoteQuestion.html'
        };
    }
})();
