(function() {
    'use strict';
    angular.module('app.resource.helper')
        .factory('contactPersonHelper', contactPersonHelper);
    contactPersonHelper.$inject = ['$resource'];

    function contactPersonHelper() {
        var services= {
            nameCheck: nameCheck
        }
        return services;
        function nameCheck (fristName,lastName) {
            return !(fristName || lastName);
        }
    }
})();