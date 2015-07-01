(function() {
    'use strict';
    angular.module('app.Home.controllers').
        controller('SigninFormController', signinFormController);
    signinFormController.$inject = ['$scope', '$http', 'userInfo', 'authService', 'userService'];

    function signinFormController($scope, $http, userInfo, authService, userService) {
        $scope.user = {};
        $scope.authError = null;
        $scope.login = login;
        function login() {
            $scope.authError = null;
            userService.login({ username: $scope.user.username, password: $scope.user.password })
                .then(function (user) {
                    userInfo.userName = user.userName;
                    userInfo.group = user.group;
                    authService.loginConfirmed();
                }, function (error) {
                    $scope.authError = error;
                });
        };
    }
})();