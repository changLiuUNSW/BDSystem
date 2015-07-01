(function() {
    'use strict';
    angular.module('app.Admin.site.controllers')
        .controller('AdminSiteDetailCtrl', adminSiteDetailCtrl);
    adminSiteDetailCtrl.$inject = ['$scope', 'cleaningHelper'];

    function adminSiteDetailCtrl($scope, cleaningHelper) {
        $scope.cleaningHelper = cleaningHelper;
    }
})();