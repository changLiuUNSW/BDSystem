(function () {
    'use strict';
    /* Filters */
    // need load the moment.js to use this filter. 
    angular.module('app.filters')
        .filter('timestamp', timestamp);

    function timestamp() {
        return function (date) {
            return new Date(date).getTime();
        };
    }
})();