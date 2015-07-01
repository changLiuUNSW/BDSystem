(function () {
    'use strict';
    angular.module('app.directives')
    .directive('googleMap', googleMap);
    googleMap.$inject = ['mapService'];

    function googleMap(mapService) {
        return {
            restrict: 'A',
            scope: {},
            link: function ($scope, $elem, $attrs) {
                var map = mapService.init($elem[0], "areaMap");
                map.then(function () {
                });
            }
        }
    }
})();