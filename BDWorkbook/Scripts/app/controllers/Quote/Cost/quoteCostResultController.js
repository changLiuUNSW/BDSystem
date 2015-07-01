(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostResultCtrl', quoteCostResultCtrl);
    quoteCostResultCtrl.$inject = ['$scope'];
    function quoteCostResultCtrl($scope) {
        console.log('abc');
    }
})();