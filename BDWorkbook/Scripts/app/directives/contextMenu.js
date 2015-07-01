angular.module('app.directives')
//context menu for tables
    .directive('contextMenu', [
        function() {
            return {
                restrict: 'A',
                scope: { options: '=' },
                link: function($scope, $elem, $attr) {
                    
                    //menu should be hidden at the beginning
                    //$elem.hide();

                    $(document).on('mousemove', function (e) {
                        $scope.x = e.pageX;
                        $scope.y = e.pageY;
                    });

                    //clean up global event when $scope is destroyed
                    $scope.$on('$destroy', function () {
                        $(document).off('mousemove');
                    });

                    $scope.$on('showContextMenu', function () {
                        $elem.hide();
                        $elem.css({
                            //display: 'block',
                            left: $scope.x,
                            top: $scope.y
                        });

                        $elem.show(200);
                        $(document).on('click', function () {
                            $(document).off('click');
                            $elem.hide();
                        });
                    });
                }
            }
        }
    ]);
