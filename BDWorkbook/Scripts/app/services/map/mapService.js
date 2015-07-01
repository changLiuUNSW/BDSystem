(function () {
    'use strict';
    angular.module('app.resource.map')
        .factory('mapService', mapService);
    mapService.$inject = ['$q'];
    
    function mapService($q) {
        var maps = [],
           marks = [];

        //load google apis
        if (typeof google == "undefined" || !google) {
            var asyncUrl = 'https://maps.googleapis.com/maps/api/js?callback=',
                mapDefer = $q.defer();

            window.googleMapsInitialized = mapDefer.resolve;
            var asyncLoad = function (url, callbackName) {
                var script = document.createElement('script');
                script.src = url + callbackName;
                document.body.appendChild(script);
            };
            asyncLoad(asyncUrl, 'googleMapsInitialized');
        }

        var service = {
            getMap: getMap,
            init: init,
            setMark: setMark,
            removeMark: removeMark,
            geocode: geocode
        };

        return service;



        function getMap(date) {
            return $filter('date')(date, 'yyyy-MM-dd');
        }

        function init(elem, name) {
            if (!elem || !name)
                return mapDefer.promise;

            return mapDefer.promise.then(function() {
                var mapOptions = {
                    zoom: 10,
                    center: new google.maps.LatLng(-34.397, 150.644)
                };

                var map = new google.maps.Map(elem, mapOptions);
                map.name = name;
                maps.push(map);
            });
        }
        function setMark(latLng, map) {
            mapDefer.promise.then(function () {
                if (latLng['D'] === undefined || latLng['k'] === undefined)
                    return;

                if (typeof latLng['D'] != "number" || typeof latLng['k'] != "number")
                    return;

                var marker = new google.maps.Marker({
                    position: latLng,
                    map: map,
                });

                console.log(marker);
                marks.push(marker);
            });
        }
        function removeMark() {
            mapDefer.promise.then(function() {
                marks.forEach(function (mark) {
                    mark.setMap(null);
                });
            });
        }

        function geocode(request, fn) {
            mapDefer.promise.then(function() {
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode(
                    request,
                    function (result, status) {
                        fn(result, status);
                    });
            });
        }

    }
})();