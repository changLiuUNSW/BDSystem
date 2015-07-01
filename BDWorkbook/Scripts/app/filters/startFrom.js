(function () {
    'use strict';
    //We already have a limitTo filter built-in to angular,
    //let's make a startFrom filter
    angular.module('app.filters')
        .filter('startFrom', startFrom);

    function startFrom() {
        return function (input, start) {
            if (input) {
                start = +start; //parse to int
                return input.slice(start);
            }
            return [];
        }
    }
})();