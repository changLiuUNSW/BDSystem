(function() {
    'use strict';
    angular.module('app.directives')
    .directive('costSummary', costSummary);
    function costSummary() {
        return {
            template: function($ele,$attr) {
                var currentCost = $attr.costSummary;
                var summary = {
                    pw: '',
                    pmth: '',
                    pa: '',
                };
                var param = '';
                var text = '';
                switch (currentCost) {
                case 'showPeriodicalsCost':
                    param = 'vm.showPeriodicalsCost';
                    text = 'Periodicals performed by Quad labour (excl on costs)';
                    return showCollapse(param, text, summary);
                case 'showLabourCost':
                    param = 'vm.showLabourCost';
                    text = 'Labour Cost';
                    summary.pw = 'vm.quadSummaryCost.labourCost.pw';
                    summary.pmth = 'vm.quadSummaryCost.labourCost.pmth';
                    summary.pa = 'vm.quadSummaryCost.labourCost.pa';
                    return showCollapse(param, text, summary);
                case 'showSubTotalQAP':
                    text = 'Sub total - Quad labour + allowances + Periodicals';
                    return showTotalBar(text, summary);
                case 'showSubTotalQA':
                    text = 'Sub total - Quad labour + allowances';
                    return showTotalBar(text, summary);
                case 'showSubTotalAll':
                    text = 'Sub Total - Quad labour + allowances + Periodicals + all on costs';
                    return showTotalBar(text, summary);
                }
            }
        };
    };
    function showCollapse(param, text, summary) {
        return '<div class="b-info panel panel-defaul" is-open="' + param + '">' +
                            '<div class="panel-heading">' +
                                '<h4 class="panel-title">' +
                                '<a href="" class="accordion-toggle" ng-click="' + param + '=!' + param + '">' +
                                '<span class="text-info font-bold text-md">' +
                                text +
                                '</span><i class="pull-right fa fa-angle-right" ng-class="{\'fa-angle-down\':' + param + ', \'fa-angle-right\': !' + param + '}"></i>' +
                                '</a>' +
                                '</h4>' +
                            '</div>' +
                            '<div class="panel-collapse collapse" collapse="!' + param + '" style="height: 0px;">' +
                                '<div class="panel-body text-left">' +
                                '<div class="col-md-4 col-xs-4"><label class="font-bold">PW:</label><span class="text-info m-l-md">{{' + summary.pw + '}}</span></div>' +
                                '<div class="col-md-4 col-xs-4"><label class="font-bold">PMth:</label><span class="text-info m-l-md" data-ng-bind="' + summary.pmth + '">$312.17</span></div>' +
                                '<div class="col-md-4 col-xs-4"><label class="font-bold">PA:</label><span class="text-info m-l-md" data-ng-bind="' + summary.pa + '">$3,746.08</span></div>' +
                                '</div>' +
                            '</div>' +
                        '</div>';
    }
    function showTotalBar(text, summary) {
        return '<div class="row text-md">' +
            '<div class="col-md-6 font-bold">' + text + '</div>' +
            '<div class="col-md-2 font-bold">PW: <span class="text-info m-l-md" data-ng-bind="' + summary.pw + '"></span></div>' +
            '<div class="col-md-2 font-bold">PMth: <span class="text-info m-l-md" data-ng-bind="' + summary.pmth + '"></span></div>' +
            '<div class="col-md-2 font-bold">PA: <span class="text-info m-l-md" data-ng-bind="' + summary.pa + '"></span></div>' +
        '</div>';
    }
})();
