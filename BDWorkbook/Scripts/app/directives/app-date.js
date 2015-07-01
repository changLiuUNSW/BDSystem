(function () {
    'use strict';
    angular.module('app.directives')
    .directive('appDate', appDate);
    appDate.$inject = ['formatter'];

    function appDate(formatter) {
        return {
            restrict: 'A',
            require: 'ngModel',
            scope: {},
            link: function ($scope, $elems, $attrs, ngModel) {
                if (!ngModel.$formatters)
                    return;

                ngModel.$formatters.push(formatter.Date);
            }
        }
    }
})();