(function () {
    'use strict';
    angular.module('app.resource.helper')
        .factory('cleaningHelper', cleaningHelper);
    cleaningHelper.$inject = ['$resource'];

    function cleaningHelper() {
        var services = {
            select: {
                trueOrFalseSelect: [
                    { value: false, text: "No" },
                    { value: true, text: "Yes" }
                ],
                callCycle: [
                    { value: 0, text: "0" },
                    { value: 3, text: "3 months" },
                    { value: 12, text: "12 months" }
                ]
            }
        }
        return services;
    }
})();
