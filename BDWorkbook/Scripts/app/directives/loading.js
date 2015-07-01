(function () {
    /**
* Loading Directive
* @see http://tobiasahlin.com/spinkit/
*/
    'use strict';
    angular.module('app.directives')
    .directive('loading', loading);
    loading.$inject = [];

    function loading() {
        return {
            restrict: 'AE',
            replace: 'false',
            template: '<div class="loading"><div class="double-bounce1"></div><div class="double-bounce2"></div></div>'
        }
    }
})();