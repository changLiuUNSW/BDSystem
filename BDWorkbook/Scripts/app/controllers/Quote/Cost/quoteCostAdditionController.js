(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostAdditionCtrl', quoteCostAdditionCtrl);
    quoteCostAdditionCtrl.$inject = [];
    function quoteCostAdditionCtrl() {
        var vm = this;
        vm.equipments = [
        { Description: 'Polivac', Code: 'pod1', UnitsFromSubby: 1, UnitsFromQuad: 2, Price: 1221, TotalCost: 333, IsLargeEquipment: true },
         { Description: 'Polivac', Code: 'pod1', UnitsFromSubby: 1, UnitsFromQuad: 2, Price: 1221, TotalCost: 333, IsLargeEquipment: true }
        ];
    }

})();