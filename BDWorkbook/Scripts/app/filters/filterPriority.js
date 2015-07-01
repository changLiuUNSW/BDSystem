//telesale filters
angular.module('app.filters')
    //filter priority list by name or initial
    .filter('filterPriority', [function () {
        return function (list, str) {

            var regExp = new RegExp(str, 'i');

            if (!str)
                return list;

            var newList = [];

            list.forEach(function (value) {

                var name = value.QPLeadDetail.Name;
                var initial = value.QPLeadDetail.Initial;

                if (name.search(regExp) > -1 || initial.search(regExp) > -1) {
                    newList.push(value);
                }
            });
            return newList;
        };
    }]);