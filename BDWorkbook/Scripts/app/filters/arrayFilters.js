(function() {
    'use strict';
    //simple array filter to get first matched item
    angular.module('app.filters')
        .filter('find', find)
        .filter('reverse', reverse);

    function reverse() {
        return function (array) {
            if (!array || array.constructor != Array || array.length <= 0)
                return undefined;

            return array.reverse();
        }
    }

    function find() {
        return function(array, compare) {
            if (!array || array.constructor !== Array || array.length <= 0)
                return undefined;

            if (!compare)
                return undefined;

            var i;

            for (i = 0; i < array.length; i++) {
                var obj = array[i];
                if (compare(obj))
                    return obj;
            }

            return undefined;
        }
    }
})();