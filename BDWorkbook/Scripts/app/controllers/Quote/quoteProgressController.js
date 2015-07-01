(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteProgressCtrl', quoteProgressCtrl);
    quoteProgressCtrl.$inject = ['quoteService', 'logger'];

    function quoteProgressCtrl(quoteService, logger) {
        /* jshint validthis: true */
        var vm = this;
        vm.statusList = [];

        init();

        function loadingStatusList() {
            return quoteService.getAllStatus().then(function(result) {
                vm.statusList = result.data;
            });
        }
        function init() {
            loadingStatusList().catch(function(error) {
                logger.serverError(error);
            });
        }
    }
})();