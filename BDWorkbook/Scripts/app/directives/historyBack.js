(function () {
    'use strict';
    angular.module('app.directives')
    .directive('historyBack', historyBack);
    historyBack.$inject = ['$window'];
    function historyBack($window) {
        return {
            restrict: 'AE',
            scope: {},
            replace:true,
            template:'<a tooltip="Back" class="btn btn-sm btn-default w-xxs w-auto-xs"><i class="fa fa-long-arrow-left"></i></a>',
            link: link
        }
        function link($scope, $elems, $attrs) {
            $elems.bind('click', function () {
                $window.history.back();
            });
        }
    }
})();