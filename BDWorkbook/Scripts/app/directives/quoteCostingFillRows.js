(function () {
    'use strict';
    angular.module('app.directives')
    .directive('quoteCostingfillrows', quoteCostingfillrows);

    function quoteCostingfillrows() {
        return {
            templateUrl: 'tpl/Quote/Estimation/Shared/fillRows.html'
        };
    }
})();
