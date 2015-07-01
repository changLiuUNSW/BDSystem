(function () {
    'use strict';
    angular.module('app.directives')
    .directive('authIntercepter', authIntercepter);
    authIntercepter.$inject = [];
    function authIntercepter() {
        return {
            restrict: 'C',
            link: function (scope, elem, attrs) {
                //once Angular is started, remove class:
                elem.removeClass('waiting-for-angular');
                var login = elem.find('#login-holder');
                var main = elem.find('#app');
                login.hide();
                scope.$on('event:auth-loginRequired', function () {
                    login.slideDown('slow', function () {
                       
                        scope.$applyAsync(function() {
                            elem.find('.modal').hide();
                            elem.find('.modal-backdrop').hide();
                            main.hide();
                        });
                    });
                });
                scope.$on('event:auth-loginConfirmed', function () {
                    scope.$applyAsync(function () {
                        main.show();
                        elem.find('.modal-backdrop').show();
                        elem.find('.modal').show();
                        login.slideUp();
                    });
                 
                });
            }
        }
    }
})();