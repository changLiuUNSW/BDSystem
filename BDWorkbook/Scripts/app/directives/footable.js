(function() {
    'use strict';
    angular.module('app.directives')
        .directive('footableSync', footableSync);

    function footableSync() {
        var directive= {
            restrict: 'EA',
            link:link
        }
        return directive;

        function link(scope, element) {
            var footableTable = element.parents('table');
            if (!scope.$last) {
                return false;
            }
            scope.$evalAsync(function () {

                if (!footableTable.hasClass('footable-loaded')) {
                    footableTable.footable();
                }
                footableTable.trigger('footable_initialized');
                footableTable.trigger('footable_resize');
                footableTable.data('footable').redraw();
            });
        }
    }
})();