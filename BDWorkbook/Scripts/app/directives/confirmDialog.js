/**
 * @directive
 * @name app.directives.confirmClick
 *
 * @description
 * Pop up confirmation dialog
 *
 * @options
 * @confirm Options  
 *      confirmShow : evaluate whether to display confirmation
 *      confirmTitle : dialog title message
 *      confirmMessage: dialog display message
 *      confirmCallback: dialog call back function
 */
angular.module('app.directives')
    .directive('confirmClick', ['$modal',function($modal) {
    return {
        restrict: 'A',
        scope: {
            confirmShow: '=',
            confirmMessage: '@' ,
            confirmTitle: '@',
            confirmCallback: '='
        },
        link: function($scope, $elem, $attrs) {
            $elem.bind('click', function () {
                if (typeof $scope.confirmShow != "undefined" && $scope.confirmShow() != true)
                    return;

                var modalInstance = $modal.open({
                    templateUrl: 'tpl/Shared/confirmationModal.html',
                    controller: 'confirmationController',
                    scope: $scope,
                    resolve: {
                        title : function() {
                            return $scope.confirmTitle;
                        },

                        message : function() {
                            return $scope.confirmMessage;
                        }
                    }
                });

                modalInstance.result.then(function () {
                    $scope.confirmCallback();
                });
            });
        }
    };
}]);
