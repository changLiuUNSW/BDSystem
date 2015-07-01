(function () {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('reportController', reportController);
    reportController.$inject = [];

    function reportController() {
        var self = this;
        self.loading = false;
    }
})();