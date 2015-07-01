(function() {
    'use strict';
    angular.module('app.telesale.controllers')
        .controller('queueController', queueController);
    queueController.$inject = ['logger', 'callService', '$filter'];

    function queueController(logger, callService, $filter) {

        var self = this;
        self.loading = true;
        self.events = [];
        self.telesales = null;
        self.summaries = null;
        self.updateTelesale = updateTelesale;

        var param = { type: 'assignable' };

        callService.queue.loadAll(null, param).then(function (success) {
            onAsyncComplete(function () {
                self.telesales = success[0].data;
                self.summaries = buildTable(success[1].data);;
            });
        }, handleError);

        function handleError(error) {
            logger.serverError(error);
        }

        function onAsyncComplete(taskAfterComplete) {
            self.loading = false;
            taskAfterComplete();
        }

        function buildTable(summaries) {
            if (!summaries)
                return [];

            var tableArray = [];

            summaries.forEach(function(summary) {

                var exist = $filter('find')(tableArray, function(obj) {
                    return obj.Group === summary.Code;
                });

                var row = {
                    Inhouse: summary.Inhouse,
                    NonInhouse: summary.NonInhouse,
                    Size: summary.Size,
                    LeadPerson: summary.LeadPerson,
                    Total: summary.Total,
                    Area: summary.Area
                };

                if (!exist) {
                    tableArray.push({
                        Group: summary.Code,
                        Rows: [row]
                    });
                } else {
                    exist.Rows.push(row);
                }
            });

            return tableArray;
        }
        
        function updateTelesale(telesale) {
            if (!telesale)
                return;

            if (!self.telesales || 
                self.telesales.constructor !== Array || 
                self.telesales.length <= 0)
                return;

            for (var i = 0; i < self.telesales.length; i++) {
                var target = self.telesales[i];
                if (!target)
                    continue;

                if (target.Id === telesale.Id) {
                    self.telesales[i].Assignments = telesale.Assignments;
                    break;
                }
            };
        }
    }
})();