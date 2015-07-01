(function() {
    'use strict';

    angular.module('app.directives').directive('googleChart', chartDirective);
    chartDirective.$inject = ['googleApiLoader'];
    function chartDirective(googleApiLoader) {
        return {
            restirct: 'A',
            scope: {
                chartType: '=',
                chartData: '=',
                chartOptions: '='
            },
            link: function ($scope, $elems, $attrs) {
                var loadChart = function () {
                    google.load('visualization', '1.1', { 'packages': [$scope.chartType], 'callback': drawChart });
                }

                var drawChart = function() {
                    var data = google.visualization.arrayToDataTable($scope.chartData);

                    switch($scope.chartType) {
                        case 'bar':
                            var chart = new google.charts.Bar($elems[0]);
                            chart.draw(data, google.charts.Bar.convertOptions($scope.chartOptions));
                            break;
                        default:
                            break;
                    }
                }

                googleApiLoader.asynLoad(loadChart);
            }
        }
    };
})()