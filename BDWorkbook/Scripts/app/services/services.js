(function() {
    'use strict';

    /* Services */
    angular.module('app.resource', [
      'app.resource.data',
      'app.resource.helper',
      'app.resource.map',
      'app.resource.auth'
    ]);

    angular.module('app.resource.data', ['ngResource']);
    angular.module('app.resource.helper', []);
    angular.module('app.resource.map', []);
    angular.module('app.resource.auth', []);
})();


