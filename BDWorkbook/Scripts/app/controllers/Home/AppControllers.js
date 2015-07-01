(function() {
    'use strict';
    angular.module('app.Home.controllers')
        .controller('AppCtrl', appCtrl);
    appCtrl.$inject = ['$scope', '$sessionStorage', '$window', 'userInfo', 'userService', '$rootScope'];
    function appCtrl($scope, $sessionStorage, $window, userInfo, userService,$rootScope) {
        // add 'ie' classes to html
        var isIE = !!navigator.userAgent.match(/MSIE/i);
        isIE && angular.element($window.document.body).addClass('ie');
        $scope.isSmartDevice && angular.element($window.document.body).addClass('smart');
        //userInfo
        $scope.userinfo = userInfo;
        // config
        $rootScope.app = {
            name: 'BD System',
            version: '1.0.0',
            // for chart colors
            color: {
                primary: '#7266ba',
                info: '#23b7e5',
                success: '#27c24c',
                warning: '#fad733',
                danger: '#f05050',
                light: '#e8eff0',
                dark: '#3a3f51',
                black: '#1c2b36'
            },
            settings :{
            themeID: 4,
            navbarHeaderColor: 'bg-info',
            navbarCollapseColor: 'bg-white-only',
            asideColor: 'bg-black',
            headerFixed: true,
            asideFixed: false,
            asideFolded: false,
            application: false,
            hideSetting: false,
            hideHeader: false,
            hidefooter: false,
            hideAside: false
            }
        }; // save settings to local storage
        if (angular.isDefined($sessionStorage.settings)) {
            $rootScope.app.settings = $sessionStorage.settings;
        } else {
            $sessionStorage.settings = $rootScope.app.settings;
        }
        $scope.$watch('app.settings', function () { $sessionStorage.settings = $rootScope.app.settings;}, true);

        $scope.logOff = userService.logOff;

        $scope.login= function() {
            $rootScope.$broadcast('event:auth-loginRequired');
        }

        $scope.isSmartDevice = function() {
            // Adapted from http://www.detectmobilebrowsers.com
            var ua = $window['navigator']['userAgent'] || $window['navigator']['vendor'] || $window['opera'];
            // Checks for iOs, Android, Blackberry, Opera Mini, and Windows mobile devices
            return (/iPhone|iPod|iPad|Silk|Android|BlackBerry|Opera Mini|IEMobile/).test(ua);
        }();
    }
})();