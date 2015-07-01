(function () {
    'use strict';
    angular.module('app.directives')
    .directive('setNgAnimate', setNgAnimate);
    setNgAnimate.$inject = ['$animate'];

    function setNgAnimate($animate) {
        return {
            link: function ($scope, $element, $attrs) {
                $scope.$watch(function () {
                    return $scope.$eval($attrs.setNgAnimate, $scope);
                }, function (valnew, valold) {
                    $animate.enabled(!!valnew, $element);
                });
            }
        };
    }
})();