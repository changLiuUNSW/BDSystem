(function () {
    'use strict';
    angular.module('app.resource.data').
        factory('userService', userService);
    userService.$inject = ['$q', '$http', '$localStorage', 'userInfo', '$rootScope'];

    function userService($q, $http, $localStorage, userInfo, $rootScope) {
        var baseUrl = config.ServerAddress + 'token';
        var buildFormData = function (formData) {
            var dataString = '';
            for (var prop in formData) {
                if (formData.hasOwnProperty(prop)) {
                    dataString += (prop + '=' + formData[prop] + '&');
                }
            }
            return dataString.slice(0, dataString.length - 1);
        };
        var authentication = {
            isAuth: false,
            userName: "",
            group:""
        };

        var services = {
            login: login,
            authentication: authentication,
            logOff: logOff
//            getCurrentUser: getCurrentUser,
//            intersect: intersect,
//            isAdmin: isAdmin
    }
        return services;
        function login(userLogin) {
            var deffered = $q.defer();
            var formData = { username: userLogin.username, password: userLogin.password, grant_type: 'password' };
            if ($localStorage.authorizationData) $localStorage.authorizationData = null;
            $http.post(baseUrl, buildFormData(formData), { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (data) {
                var user= {
                    userName: data.userName,
                    group: data.group,
                    token: data.access_token,
                }
                $localStorage.authorizationData = user;
                deffered.resolve(user);
            }).error(function (msg, code) {
                deffered.reject(msg.error_description);
            });
            return deffered.promise;
        }

        function logOff() {
            $localStorage.authorizationData = null;
            userInfo.userName = null;
            userInfo.group = null;
            $rootScope.$broadcast('event:auth-loginRequired');
        }

//

//       function getCurrentUser (params) {
//            return $resource(baseUrl).get(params);
//       }
//
//        function intersect (a, b) {
//            var t;
//            if (b.length > a.length) t = b, b = a, a = t; // indexOf to loop over shorter
//            return a.filter(function (e) {
//                if (b.indexOf(e) !== -1) return true;
//            });
//        }
//
//        function isAdmin (usergroup) {
//            return intersect(usergroup, config.admin).length > 0;
//
//        }

       

    }
})();