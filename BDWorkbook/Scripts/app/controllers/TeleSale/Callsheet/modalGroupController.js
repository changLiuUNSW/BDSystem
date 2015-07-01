(function() {
    'strict';

    angular.module('app.telesale.controllers').controller('modalGroupController', controller);
    controller.$inject = ['$scope', '$modalInstance', '$group', 'ngTableParams', 'typeLibrary'];

    function controller($scope, $modalInstance, $group, ngTableParams, typeLibrary) {
        var self = this;
        self.group = $group;
        self.getStyle = getStyle;
        self.makeCall = makeCall;

        self.params = new ngTableParams({
            page: 1,
            count: 5
        }, {
            counts: [],
            getData: function($defer, params) {
                params.total(self.group.Sites.length);
                $defer.resolve(self.group.Sites.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            }
        });

        function getStyle(contact) {
            switch(contact.BusinessTypeId) {
                case typeLibrary.businessType.cleaning:
                    return 'label-primary';
                case typeLibrary.businessType.security:
                    return 'label-success';
                case typeLibrary.businessType.maintenance:
                    return 'label-warning';
            }


            return 'label'
        };

        function makeCall(id) {
            swal({
                title: "Confirm",
                text: "Switch to a new record",
                type: "warning",
                showCancelButton: true,
                confirmButtonText: "Yes",
                cancelButtonText: 'No',
                confirmButtonColor: "#7266ba"
            }, function (isConfirm) {
                if (isConfirm) {
                    $modalInstance.close(id);
                }
            });
        }
    }
})();