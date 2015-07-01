(function() {
    'use strict';
    angular.module('app.telesale.controllers')
        .controller('queueInfoController', infoController);

    infoController.$inject = ['$scope', 'callService', 'logger'];

    function infoController($scope, callService, logger) {
        var self = this;
        self.tabs = [];
        self.initTab = initTab;
        self.isAssignTo = isAssignTo;
        self.isAssigned = isAssigned;
        self.assign = assign;
        self.deAssign = deAssign;

        function assign(code, row, telesale) {
            if (!row || !code || !telesale)
                return;

            var param = { id: telesale.Id };
            var data = { Area: row.Area, QpCode: code, Size: row.Size };

            callService.queue.assign(param, JSON.stringify(data)).then(function (success) {
                $scope.$parent.queue.updateTelesale(success.data);
                logger.success("Assign complete");
            }, handleError);
        }



        function deAssign(telesaleId, qpCode, row) {
            if (!telesaleId || !qpCode || !row)
                return;

            var param = {
                id: telesaleId,
                Id: row.Id,
                code: qpCode,
                area: row.Area,
            };

            callService.queue.deAssign(param).then(function (success) {
                $scope.$parent.queue.updateTelesale(success.data);
                logger.success("Remove complete");
            }, handleError);
        }

        //check whether particular area/code is assigned to one of the telesale
        //return the name of the telesale been assigned
        function isAssigned(code, row, telesales) {
            if (!telesales ||
                telesales.constructor !== Array ||
                telesales.length <= 0)
                return null;

            for (var i = 0; i < telesales.length; i++) {
                var telesale = telesales[i];

                if (isAssignTo(code, row, telesale)) {
                    return telesale.Name;
                }
            }

            return null;
        };

        //checks whether particular telesale is assigned to the area/code
        function isAssignTo(code, row, telesale) {
            if (!telesale || !row || !code)
                return false;

            if (!telesale.Assignments ||
                telesale.Assignments.constructor !== Array ||
                telesale.Assignments.length <= 0)
                return false;

            for (var i = 0; i < telesale.Assignments.length; i++) {
                var assignment = telesale.Assignments[i];
                
                if (!assignment)
                    continue;

                if (assignment.QpCode == code &&
                    assignment.Area == row.Area &&
                    assignment.Size == row.Size
                    ) {
                    return true;
                };
            }

            return false;
        }

        function initTab($index) {
            self.tabs[$index] = new tab($index);
        }

        function tab(idx) {
            var pagingSelf = this;
            this.startIdx = null;
            this.currentPage = null;
            this.itemsPerPage = 5;

            $scope.$watch('info.tabs[' + idx +'].currentPage', function (pageNum) {
                if (!pageNum)
                    return;

                pagingSelf.startIdx = (pageNum - 1) * pagingSelf.itemsPerPage;
            });
        };

        function handleError(error) {
            logger.serverError(error);
        }
    }
})();