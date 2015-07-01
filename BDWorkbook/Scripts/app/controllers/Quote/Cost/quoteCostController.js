(function() {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostCtrl', quoteCostCtrl);
    function quoteCostCtrl() {
        var vm = this;
        vm.groups = [
            { title: 'Basic Details', sub: 'Quad Area, Industry Type and Days PW', state: 'quote.cost.detail' },
            { title: 'Site Location', sub: 'Address of site that is being quoted', state: 'quote.cost.loaction' },
            { title: 'Areas to clean and exclude', sub: 'Cleaning areas to clean and exclude', state: 'quote.cost.area' },
            { title: 'Costing Infomation', sub: 'Regular weeks, invoiced weeks, schools and other sites', state: 'quote.cost.info' },
            { title: 'Costing Additional', sub: 'Incontract work,supplies and Equipment', state: 'quote.cost.addition' },
            { title: 'Costing Quad Labour', sub: 'Cost Estimation for Quad Labour', state: 'quote.cost.quadlabour' },
            { title: 'Extra chanrge', sub: 'Extra charge Periodicals and Supplies', state: 'quote.cost.extra' },
            { title: 'Result', sub: 'Cost Estimation Result', state: 'quote.cost.result' }
        ];
    }

})();