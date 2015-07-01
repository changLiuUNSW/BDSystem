(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostExtraCtrl', quoteCostExtraCtrl);
    quoteCostExtraCtrl.$inject = ['$scope'];
    function quoteCostExtraCtrl($scope) {
        console.log('abc');
    }

})();