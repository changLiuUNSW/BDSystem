//telesale filters
angular.module('app.filters')
    //filter priority list by name or initial
    .filter('address', [function () {
        return function (site) {
            var result = '';
            if (site.Unit) {
                result += site.Unit + ' ';
            }
            if (site.Number) {
                result += site.Number+' ';
            }
            if (site.Street) {
                result += site.Street + ' ';
            }
            if (site.Suburb) {
                result += site.Suburb + ' ';
            }
            if (site.State) {
                result += site.State + ' ';
            }
            if (site.Postcode) {
                result += site.Postcode;
            }
            return result;
        };
    }]);