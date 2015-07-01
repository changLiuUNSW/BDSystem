'use strict';

angular.module('app.Admin', [
  'app.Admin.Search.controllers',
  'app.Admin.site.controllers',
  'app.Admin.contactPerson.controllers'
]);

angular.module('app.Admin.Search.controllers',['ngTable', 'ngTableResizableColumns', 'angularjs-dropdown-multiselect', 'ngBootstrap']);
angular.module('app.Admin.site.controllers',[]);
angular.module('app.Admin.contactPerson.controllers',[]);
