(function() {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('reportStatisticController', reportStatisticController);

    reportStatisticController.$inject = ['$scope', 'apiService', 'logger', '$q', '$filter'];

    function reportStatisticController($scope, apiService, logger, $q, $filter) {

        var self = this;

        //table format
        //static columns : will be populated before other columns, no comparison
        //columns : will be used to compare to history value
        //keys : key for matching the history value
        //groups: apply style on groups
        //style: default row style
        self.reportType = {
            weekly: {
                id: 'weekly',
                //column format
                //header: display for column name
                //property: object property name
                columns: [
                    { header: 'Overdue', property: 'Overdue' },
                    { header: 'Overdue + ready', property: 'OverdueAndReady' },
                    { header: 'Total', property: 'Total' }
                ],
                staticColumns: [
                    { header: 'Name', property: 'Code' }
                ],

                keys: ['Code'],
                groups: [
                    {
                        name: 'BD/GM',
                        property: 'Code', //object property for comparing with values in codes
                        codes: ['DH', 'DHP', 'PF', 'DB', 'DBP', 'IS', 'ISP', 'JP', 'JPP'],
                        style: { 'background-color': '#FFF5EB' }
                    },
                    {
                        name: 'PM/Group',
                        property: 'Code',
                        codes: ['PMS', 'BMS', 'G', 'GRP', 'GOV', ' QA', 'QT', 'LPM'],
                        style: { 'background-color' : '#FFFFE6'}
                    }
                ],

                style: { name: 'OPR', 'background-color': '#E6F0FF' }
            },

            complete: {
                id: 'complete',
                columns: [
                    { header: 'Total', property: 'Total' },
                    { header: 'Da to check', property: 'DaToCheck' },
                    { header: 'Could not get name', property: 'NoName' },
                    { header: 'Recall ext management details', property: 'ReCall' },
                    { header: 'Inhouse', property: 'Inhouse' }
                ],
                staticColumns: [
                    { header: 'Name', property: 'Code' },
                    { header: 'Call Freq', property: 'CallFreq' },
                    { header: 'Size', property: 'Size' }
                ],
                keys: ['Code', 'CallFreq', 'Size']
            }
        };

        self.load = load;
        self.fromLastWeek = fromLastWeek;
        self.showArrowSign = showArrowSign;
        self.style = getStyle;
        self.data = null;
        self.history = null;
        self.page = 1;
        self.idx = 0;
        self.itemsPerPage = 15;
        //paging

        self.type = self.reportType.weekly;

        $scope.$watch('statistic.page', function (value) {
            self.idx = (value - 1) * self.itemsPerPage;
        });

        $scope.$watch('statistic.type', function (value) {
            if (!value)
                return;

            load({ type: value.id });
        });

        function load(param) {
            var context;
            var report = apiService.contact.report(param).$promise;
            var history = apiService.contact.reportHistory(param).$promise;

            if (!self.data) {
                context = $scope.$parent.report;
            } else {
                context = self;
            }

            context.loading = true;
            //reset
            self.data = null;
            self.history = null;

            $q.all([report, history]).then(function (success) {
                self.data = success[0].data;
                self.history = success[1].data;
                context.loading = false;
            }, handleError);
        }

        function fromLastWeek(row) {
            var lastweek = $filter('find')(self.history, function(history) {

                var match = true;

                for (var i = 0; i < self.type.keys.length; i++) {
                    var key = self.type.keys[i];
                    if (row[key] !== history[key]) {
                        match = false;
                        break;
                    }
                }

                return match;
            });

            return getDiff(row, lastweek, self.type.columns);
        }

        function getDiff(thisweek, lastweek, columnNames) {
            var diff = {};

            if (!thisweek || !lastweek)
                return diff;

            if (!columnNames || columnNames.constructor !== Array)
                return diff;

            columnNames.forEach(function (column) {
                var name = column.property;
                diff[name] = { 
                    value: Math.abs(thisweek[name] - lastweek[name]),
                    bool: thisweek[name] > lastweek[name]
                }
            });

            if (diff.value === 0) {
                delete diff.bool;
            }

            return diff;
        }

        function showArrowSign(lastWeekVal) {
            if (!lastWeekVal || !lastWeekVal.value)
                return false;

            if (lastWeekVal.valueOf === 0)
                return false;

            return true;
        }

        function getStyle(row) {
            if (!self.type || !self.type.groups)
                return undefined;

            var i;
            for (i = 0; i < self.type.groups.length; i++) {
                var group = self.type.groups[i];

                if ($filter('find')(group.codes, function (code) {
                    return row[group.property] == code;
                })) {
                    return group.style;
                }
            }

            return self.type.style;
        }

        function handleError(error) {
            logger.serverError(error);
        }
    }
})();