(function() {
    'use strict';
    angular.module('app.resource.helper').
        factory('mobileService', mobileService);
    mobileService.$inject = ['$window'];

    function mobileService($window) {
        var service = {
            isSmartDevice: isSmartDevice()
        }

        return service;

        function isSmartDevice () {
            var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
            // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
            return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
        }
    }
})();