(function () {
    'use strict';
    angular.module('app.directives')
    .directive('ngDroppable', ngDroppable);
    ngDroppable.$inject = [];

    function ngDroppable() {
        return {
            restrict: 'A',
            scope: {
                droppableFn: '='
            },
            link: function ($scope, $elem, $attrs) {
                if (typeof $scope.droppableFn == "undefined" || typeof $scope.droppableFn != "function")
                    return;

                var elem = $elem[0];

                elem.addEventListener('dragover', function (e) {
                    e.preventDefault();
                });
                elem.addEventListener('drop', function (e) {
                    e.preventDefault();
                    e.dataTransfer.dropEffect = "move";
                    $scope.droppableFn(e.dataTransfer.getData('data'));
                });
            }
        }
    }
})();