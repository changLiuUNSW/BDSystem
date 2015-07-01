(function() {
    'use strict';

    angular.module('app.resource.helper')
        .factory('googleApiLoader', loader);

    loader.$inject = [];

    function loader() {
        return {
            asynLoad: asynLoad
        };

        function asynLoad(callback) {
            if (typeof window.googleMapsInitialized == "undefined" || !google) {
                var asynUrl = 'https://www.google.com/jsapi?callback=';
                window.googleApiInitialized = callback;
                load(asynUrl, 'googleApiInitialized');
            }
        }

        function load (url, callbackName) {
            var script = document.createElement('script');
            script.src = url + callbackName;
            document.body.appendChild(script);
        };
    }
})();