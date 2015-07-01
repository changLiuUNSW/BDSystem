(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostController_v2', controller);
    controller.$inject = ['apiService', '$q'];
    function controller(apiService, $q) {
        var self = this;
        self.equipments = [];
        self.data = undefined;

        var equipmentPromise = apiService.quoteCost.equipments().$promise;
        var costPromise = apiService.quoteCost.get({id:1}).$promise;

        $q.all([equipmentPromise, costPromise]).then(function(success) {
            self.equipments = success[0].data;
            self.data = success[1].data;
        });
    }

})();