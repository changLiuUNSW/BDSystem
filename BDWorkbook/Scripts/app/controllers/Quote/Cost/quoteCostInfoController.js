(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostInfoCtrl', quoteCostInfoCtrl);
    quoteCostInfoCtrl.$inject = ['$scope'];
    function quoteCostInfoCtrl($scope) {
        console.log('abc');
    }

})();