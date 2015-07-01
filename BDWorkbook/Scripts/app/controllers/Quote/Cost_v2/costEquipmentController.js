(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostEquipmentController_v2', controller);
    controller.$inject = [];
    function controller() {
        var self = this;
        self.typed = undefined;
        self.addTo = addTo; 

        function addTo(cost) {
            if (!cost)
                return;

            if (!cost.EquipmentSupplies)
                cost.EquipmentSupplies = [];

            cost.EquipmentSupplies.push(self.typed);
        };
    };
})();