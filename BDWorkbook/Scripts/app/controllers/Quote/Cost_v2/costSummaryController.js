(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostSummaryController_v2', controller);
    controller.$inject = [];
    function controller() {
        var self = this;

        function cost(t) {
            this.weekly = 5000,
            this.monthly = this.weekly * 4,
            this.yearly = this.weekly *52;

            this.title = t;
        }

        self.categories = [
        {
            title: 'Regular Cleaning',
            prices: [
                new cost("Subcontractor cost")
            ]
        },
        {
            title: 'Other cleaning/supplies',
            prices: [
                new cost("Incontractor work"),
                new cost("Incontractor supplies"),
                new cost("Incontractor equipment")
                ]
            },
            {
                title: 'Quad Labour costing',
                prices: [
                    new cost("Labour"),
                    new cost("Periodicals"),
                    new cost("On Costs"),
                    new cost("Mats/Uniforms/Phone ect"),
                    new cost("Total")
                ]
            }
        ];
    }
})();