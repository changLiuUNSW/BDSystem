(function () {
    'use strict';
    angular.module('app.directives')
    .directive('labelColor', labelColor);
    labelColor.$inject = [];

    function labelColor() {
        var directive = {
            restrict: 'EA',
            scope: {
                labelColor:'='
            },
            link: link
        }
        return directive;

        function link(scope, $el, attrs) {
            var type = scope.labelColor;
            var colorClass;
            switch (angular.lowercase(type)) {
                case 'cleaning':
                    colorClass= 'b-l-info';
                    break;
                case 'security':
                    colorClass = 'b-l-primary';
                    break;
                case 'maintenance':
                    colorClass = 'b-l-success';
                    break;
                default:
                    colorClass = 'b-l-light';
                    break;
            }
            $el.addClass(colorClass);
        }
    }
})();