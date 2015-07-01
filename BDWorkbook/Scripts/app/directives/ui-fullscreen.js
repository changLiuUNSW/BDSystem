(function () {
    'use strict';
    angular.module('app.directives')
    .directive('uiFullscreen', uiFullscreen);
    uiFullscreen.$inject = [];

    function uiFullscreen() {
        return {
            restrict: 'AC',
            template: '<i class="fa fa-expand fa-fw text"></i><i class="fa fa-compress fa-fw text-active"></i>',
            link: function (scope, el, attr) {
                el.addClass('hide');
                if (screenfull.enabled) {
                    el.removeClass('hide');
                }
                el.on('click', function () {
                    var target;
                    attr.target && (target = $(attr.target)[0]);
                    el.toggleClass('active');
                    screenfull.toggle(target);
                });
            }
        };
    }
})();