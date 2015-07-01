(function() {
        'use strict';
        angular.module('app')
            .run(
                [
                    '$rootScope', '$state', '$stateParams', 'userInfo', 'logger', 'userService', '$localStorage',
                    function($rootScope, $state, $stateParams, userInfo, logger, userService, $localStorage) {
                        if ($localStorage.authorizationData) angular.extend(userInfo, $localStorage.authorizationData);
                        $rootScope.$state = $state;
                        $rootScope.$stateParams = $stateParams;
                        //                $rootScope.$on("$stateChangeStart", function (event, toState, toParams, fromState, fromParams) {
                        //                    if ('access' in toState && !userService.intersect(userInfo.groups, toState.access).length) {
                        //                        event.preventDefault();
                        //                        if (fromState.url === '^') {
                        //                            $state.go('job.list');
                        //                        } else {
                        //                            logger.error('Seems like you tried accessing a route you do not access to...', 'error');
                        //                        }
                        //                    }
                        //                });

                        //capture the stat change event in order to apply the default app.settings across pages
                        //                    $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState) {
                        //                        //initial page load where the state event will fire after ng-init
                        //                        if (!fromState.name || fromState.url === '^')
                        //                            return;
                        //
                        //                        //internal views changes
                        //                        if (toState.name === fromState.name)
                        //                            return;
                        //
                        //                        $rootScope.app.settings.headerFixed = true,
                        //                        $rootScope.app.settings.asideFixed = false,
                        //                        $rootScope.app.settings.asideFolded = false,
                        //                        $rootScope.app.settings.application = false,
                        //                        $rootScope.app.settings.hideSetting = false,
                        //                        $rootScope.app.settings.hideHeader = false,
                        //                        $rootScope.app.settings.hidefooter = false,
                        //                        $rootScope.app.settings.hideAside = false;
                        //                    });
                    }
                ]
            );
})();