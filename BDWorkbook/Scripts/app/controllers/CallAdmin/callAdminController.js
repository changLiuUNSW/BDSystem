(function() {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('callAdminController', controller);

    controller.$inject = ['googleApiLoader'];

    function controller(googleApiLoader) {
        var self = this;
        self.data = [
            ['Weekday', 'Contacted', 'Lead'],
            ['Monday', 15, 1],
            ['Tuesday', 5, 0],
            ['Wednesday', 25, 5],
            ['Thursday', 10, 1],
            ['Friday', 10, 2]
        ];

        self.options = {
            hAxis: {
                title: '',
            }
        };

        self.type = 'bar';
    };
})();