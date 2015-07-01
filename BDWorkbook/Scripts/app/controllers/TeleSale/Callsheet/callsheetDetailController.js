(function() {
    'use strict';

    angular.module('app.telesale.controllers')
        .controller('callSheetDetailController', controller);
    controller.$inject = ['telesaleService'];

    function controller(telesaleService) {
        console.log(telesaleService);
    };
})();