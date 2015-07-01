(function() {
    'use strict';

    angular.module('app.telesale.controllers').controller('modalActionController', controller);
    controller.$inject = ['$scope', 'inputs', '$modalInstance'];

    function controller($scope, inputs, $modalInstance) {
        var self = this;
        self.inputs = inputs;
        self.cancel = cancel;
        self.confirm = confirm;
        self.formatName = formatName;
        self.isDate = false;
        self.dtOptions = {
            minDate: new Date(),
            dtOpen: function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                self.dtOptions.dtOpened = !self.dtOptions.dtOpened;
            }
        };

        inputs.forEach(function (input) {
            if (input.name.toLowerCase().indexOf('callbackdate') > -1) {
                self.isDate = true;
            };
        });

        function cancel() {
            $modalInstance.dismiss();
        };

        function confirm() {
            $modalInstance.close(inputs);
        }

        function formatName(name) {

            var nameArray = name.match(/[A-Z]{1,1}[a-z]*/g);

            if (!nameArray) {
                return name;
            }
            else {
                var nm = '';

                nameArray.forEach(function(str) {
                    nm = nm + str + ' ';
                });

                return nm;
            }
        };
    };
})();