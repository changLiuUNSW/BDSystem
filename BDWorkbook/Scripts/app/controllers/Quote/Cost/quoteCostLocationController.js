(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostLocationCtrl', quoteCostLocationCtrl);
    quoteCostLocationCtrl.$inject = ['$scope'];
    function quoteCostLocationCtrl($scope) {
        console.log('abc');
    }

})();