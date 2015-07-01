(function() {
    'use strict';
    // Declare app level module which depends on filters, and services
    angular.module('app', [
            'ngAnimate',
            'ngCookies',
            'ngStorage',
            'ngSanitize',
            'ui.router',
            'ui.bootstrap',
            'ui.jq',
            'ui.select',
            'ui.validate',

            'app.shared',
            'app.Home',
            'app.DashBoard',
            'app.telesale',
            'app.callAdmin',
            'app.quote',
            'app.Admin',
            'app.Lead',


            'app.resource',
            'app.filters',
            'app.directives',


            'ngFileUpload',
            'angular.filter',
            'chieffancypants.loadingBar',
            'xeditable',
            'oitozero.ngSweetAlert'
    ]);
})();

