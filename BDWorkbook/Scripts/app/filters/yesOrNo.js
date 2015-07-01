(function () {
    'use strict';

    angular.module('app.filters')
        .filter('yesOrNo', yesOrNo);

    function yesOrNo() {
        return function (input) {
            if (input !== undefined && input !== null) return input === true ? 'Yes' : 'No';
            return 'N/A';
        };
    }
})();