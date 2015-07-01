(function () {
    'use strict';
    angular.module('app.telesale.controllers')
    .controller('modalCpChangeController', controller);
    controller.$inject = ['$scope', '$modalInstance', '$changes', 'utility'];

    function controller($scope, $modalInstance, $changes, $utiltliy) {
        init();

        $scope.confirm = confirm;
        $scope.cancel = cancel;

        function init() {
            var changes = getChanges();

            var viewModel = [];
            $scope.options = [
                {
                    value: 1,
                    desc: "Info change"
                }, {
                    value: 2,
                    desc: 'New manager'
                }
            ];

            changes.forEach(function(item) {
                var keys = Object.keys(item.change);

                var obj = [];

                viewModel.push({
                    id : item.obj.clone.Id,
                    firstname: item.obj.clone.Firstname,
                    lastname: item.obj.clone.lastname,
                    changes: obj
                });

                keys.forEach(function (key) {
                    obj.push({
                        key: key,
                        from: item.obj.clone[key],
                        to: item.obj.model[key]
                    });
                });
            });

            $scope.viewModel = viewModel;
        }

        function getChanges() {
            var changes = [];

            if (angular.isUndefined($changes) ||
                $changes.constructor !== Array ||
                $changes.length <= 0) {
                return changes;
            };

            $changes.forEach(function (obj) {
                var diff = $utiltliy.diff(obj.clone, obj.model, []);
                if (diff && Object.keys(diff).length > 0) {
                    changes.push({
                        change: diff,
                        obj : obj
                    });
                }
            });

            return changes;
        }

        function confirm(form) {
            if (!form.$valid)
                return;

            $modalInstance.close($scope.viewModel);
        }

        function cancel() {
            $modalInstance.dismiss();
        }
    }
})();