(function () {
    /**
* Loading Directive
* @see http://tobiasahlin.com/spinkit/
*/
    'use strict';
    angular.module('app.directives')
    .directive('spinnerLoading', spinnerLoading);
    spinnerLoading.$inject = [];

    function spinnerLoading() {
        return {
            restrict: 'AE',
            replace: 'false',
            template: '<div class="spinner">' +
                '<div class="spinner-container container1">' +
                '<div class="circle1"></div>' +
                '<div class="circle2"></div>' +
                '<div class="circle3"></div>' +
                '<div class="circle4"></div>' +
                '</div>' +
                '<div class="spinner-container container2">' +
                '<div class="circle1"></div>' +
                '<div class="circle2"></div>' +
                '<div class="circle3"></div>' +
                '<div class="circle4"></div>' +
                '</div>' +
                '<div class="spinner-container container3">' +
                '<div class="circle1"></div>' +
                '<div class="circle2"></div>' +
                '<div class="circle3"></div>' +
                '<div class="circle4"></div>' +
                '</div></div>'
        };
    }
})();