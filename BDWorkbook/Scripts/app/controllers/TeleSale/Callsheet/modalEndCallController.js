(function() {
    'strict';

    angular.module('app.telesale.controllers').controller('modalEndCallController', controller);
    controller.$inject = ['$actions', '$modalInstance', 'typeLibrary'];

    function controller($actions, $modalInstance, typeLibrary) {
        var self = this;

        var clones = angular.copy($actions);
        self.actions = $actions;
        self.confirm = confirm;
        self.cancel = cancel;
        self.needInput = needInput;

        self.dtOptions = {
            minDate: new Date(),
            dtOpen: function ($event) {
                $event.preventDefault();
                $event.stopPropagation();
                self.dtOptions.dtOpened = !self.dtOptions.dtOpened;
            }
        };

        function needInput(action) {
            switch (action.Type) {
                case typeLibrary.ScriptActions.DaToCheck:
                case typeLibrary.ScriptActions.ExternalManagement:
                    return 'text';

                case typeLibrary.ScriptActions.UpdateCallbackDate:
                    return 'date';

                default:
                    return null;
            }
        };
         
        function confirm() {
            $modalInstance.close(self.actions);
        }

        function cancel() {
            clones.forEach(function(clone) {
                for (var i = 0; i < self.actions.length; i++) {
                    if (self.actions[i].Type === clone.Type) {
                        angular.copy(clone, self.actions[i]);
                        break;
                    }
                }
            });

            $modalInstance.dismiss();
        }
     };
})()