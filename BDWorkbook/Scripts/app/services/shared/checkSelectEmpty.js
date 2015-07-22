(function() {
    'use strict';
    angular.module('app.resource.helper')
        .factory('checkSelectEmpty', checkSelectEmpty);
    checkSelectEmpty.$inject = ['logger'];
    function checkSelectEmpty(logger) {
        var service = {
            check: check
        };
        return service;
        function check(array, msg,msgType) {
            if (array.length <= 0) {
                logger.error(msg, msgType);
                return false;
            } else {
                return true;
            }
        }
    }
})();