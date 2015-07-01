(function () {
    'use strict';
    //client side pagination offset
    angular.module('app.filters')
        .filter('offset', offset);

    function offset() {
        return function (input, start) {
            if (!input || input.constructor != Array)
                return [];

            start = parseInt(start, 10);
            return input.slice(start);
        };
    }
})();