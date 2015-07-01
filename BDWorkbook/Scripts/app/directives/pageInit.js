(function () {
    'use strict';
    angular.module('app.directives')
    .directive('pageInit', pageInit);
    pageInit.$inject = ['$rootScope'];

    function pageInit($rootScope) {
        return {
            restrict: 'A',
            scope: {
                pageInit:'='
            },
            link: function ($scope, $elems, $attrs, ngModel) {
                var defaultSetting = {
                    headerFixed: true,
                    asideFixed: false,
                    asideFolded: false,
                    application: false,
                    hideSetting: false,
                    hideHeader: false,
                    hidefooter: false,
                    hideAside: false
                };
                angular.extend(defaultSetting, $scope.pageInit);
                angular.extend($rootScope.app.settings, defaultSetting);
            }
        }
    }
})();