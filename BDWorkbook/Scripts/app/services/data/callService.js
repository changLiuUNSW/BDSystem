(function () {
    'use strict';
    angular.module('app.resource.helper')
        .factory('callService', callService);

    callService.$inject = ['apiService', '$q'];

    function callService(apiService, $q) {

        return {
            queue: queue(),
            callProcess: callProcess()
        }

        function queue() {

            return {
                loadAll: loadAll,
                loadTelesale: loadTelesale,
                loadReport: loadReport,
                assign: assign,
                deAssign: deAssign
            };

            function loadAll(tsParam, rptParam) {
                return $q.all([loadTelesale(tsParam), loadReport(rptParam)]);
            }

            function loadTelesale(param) {
                return apiService.telesale.get(param).$promise;
            }

            function loadReport(param) {
                return apiService.contact.report(param).$promise;
            }

            function assign(param, data) {
                return apiService.telesale.assign(param, data).$promise;
            }

            function deAssign(param) {
                return apiService.telesale.deAssign(param).$promise;
            }
        }

        function callProcess() {
            var type = {
                bd: 'BD',
                telesale: 'Telesale',
            };

            return {
                telesale: callByTelesale,
                bd : callByBd
            }

            function callByTelesale(param, deferred) {
                var defer;

                if (deferred)
                    defer = deferred;
                else {
                    defer = $q.defer();
                }

                param.type = type.telesale;
                apiService.contact.nextCall(param, function(success) {
                    if (success.error) {
                        swal({
                            title: "Do you want to continue",
                            text: "Record shows you have an unconfirmed call from last time, click yes to continue from the last call or no to start a fresh one",
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonText: "Yes",
                            cancelButtonText: 'No',
                            closeOnConfirm: true,
                            closeOnCancel: false,
                            confirmButtonColor: "#7266ba"
                        }, function(isConfirm) {
                            if (!isConfirm) {
                                swal("Success", "Loading new contact now!", "success");
                                param.lastCallId = success.data.OccupiedId;
                                callByTelesale(param, defer);
                            } else {
                                defer.resolve(success);
                            }
                        });
                    } else {
                        defer.resolve(success);
                    }
                }, function (error) {
                    defer.reject(error);
                });

                return defer.promise;
            }

            function callByBd(param) {
                var defer = $q.defer();

                param.type = type.bd;
                apiService.contact.nextCall(param, function (success) {
                    defer.resolve(success);
                }, function (error) {
                    defer.reject(error);
                });

                return defer.promise;
            }
        }
    }
})();