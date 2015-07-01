(function() {
    'use strict';
    angular.module('app.directives')
    .directive('uiScroll', uiScroll);
    uiScroll.$inject = ['$location', '$anchorScroll'];

    function uiScroll($location, $anchorScroll) {
        return {
            restrict: 'AC',
            link: function (scope, el, attr) {
                el.on('click', function (e) {
                    $location.hash(attr.uiScroll);
                    $anchorScroll();
                });
            }
        };
    }
})();