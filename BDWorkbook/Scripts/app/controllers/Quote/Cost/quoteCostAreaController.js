(function () {
    'use strict';
    angular.module('app.quote.controllers')
        .controller('quoteCostAreaCtrl', quoteCostAreaCtrl);
    quoteCostAreaCtrl.$inject = ['editableOptions', 'editableThemes'];
    function quoteCostAreaCtrl(editableOptions, editableThemes) {
        var vm = this;
        editableThemes.bs3.inputClass = 'input-sm';
        editableThemes.bs3.buttonsClass = 'btn-sm';
        editableOptions.theme = 'bs3';

        // editable table
        vm.areaToClean = [
          { id: 1, name: 'awesome user1' },
          { id: 2, name: 'awesome user2'},
          { id: 3, name: 'awesome user3'}
        ];

      

        vm.checkName = function (data) {
            if (!data) {
                return "Area name cannot be empty";
            }
        };

    
        // remove area
        vm.removeArea = function (index,list) {
            list.splice(index, 1);
        };

        // add user
        vm.addArea = function (name,list) {
            var inserted = {
                id: null,
                name: name
            };
            list.push(inserted);
        };
    }

})();


