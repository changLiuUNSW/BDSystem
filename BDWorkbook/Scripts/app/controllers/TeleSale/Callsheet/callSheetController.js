(function () {
    'use strict';

    angular.module('app.telesale.controllers')
        .controller('callSheetController', controller);
    controller.$inject = [
        '$scope',
        'apiService',
        'logger',
        'userInfo',
        '$state',
        '$rootScope',
        'callService',
        '$modal',
        'telesaleService',
        '$filter',
        'typeLibrary',
        'callParams',
        'leadService'];

    function controller(
        $scope,
        apiService,
        logger,
        userInfo,
        $state,
        $rootScope,
        callService,
        $modal,
        telesaleService,
        $filter,
        typeLibrary,
        callParams,
        leadService) {

        var self = this;

        //page variables
        self.userInfo = angular.copy(userInfo);
        self.hasSite = false;
        self.hasLeadPerson = false;
        self.hasContact = false;
        self.showDetail = false;
        self.loading = false;
        self.hasTelesale = !isInRole('bd');
        self.site = null;
        self.contact = null;
        self.calls = [];
        self.currentCall = null;

        //functions
        self.next = next;
        self.end = end;

        if (self.hasTelesale) {
            self.telesale = {};
        };

        $scope.$on('event:callFromGroup', function (e, data) {
            e.preventDefault();
            e.stopPropagation();
            addToCalls(data);
        });

        $scope.$on('event:auth-loginConfirmed', function () {
            if (self.userInfo.userName.toLowerCase() !== userInfo.userName.toLowerCase()) {
                $state.reload();
            }
        });

        function isInRole(role) {
            if (!self.userInfo.userName) {
                $rootScope.$broadcast('event:auth-loginRequired');
                return false;
            }

            return telesaleService.isInRole(role);
        }

        function next() {
            self.loading = true;

            var callbackHandler;
            if (self.hasTelesale) {
                callParams.type = 'Telesale';
                callParams.initial = self.telesale.initial;
                callbackHandler = telesaleCallHandler;
            } else {
                callParams.type = 'BD';
                callParams.initial = null;
                callbackHandler = bdCallHandler;
            }

            telesaleService.makeCall(callParams, callbackHandler, errorHandler);
        }

        function telesaleCallHandler(success) {
            if (!success)
                return;

            if (success.unfinished) {
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
                        callParams.lastCallId = success.data.OccupiedId;
                        next();
                    };
                });
            };

            initialisePageVariables(success.data);
        }

        function bdCallHandler(success) {
            if (!success)
                return;

            initialisePageVariables(success.data);
        }

        function errorHandler(error) {
            if (error.data && error.data.ExceptionMessage) {
                displayError(error.data.ExceptionMessage);
            } else {
                logger.serverError(error);
            }
        };

        function displayError(msg) {
            if (!msg)
                return;

            swal({
                title: msg,
                type: "warning",
                confirmButtonColor: "#7266ba"
            });
        }

        function initialisePageVariables(data) {
            if (!data)
                return;

            if (data.Contact) {
                self.hasContact = true;
                self.contact = data.Contact;
            }

            if (data.Site) {
                self.hasSite = true;
                self.showDetail = true;
                self.site = data.Site;
            }

            if (data.LeadPerson) {
                self.hasLeadPerson = true;
                self.leadPerson = data.LeadPerson;
            }

            //selectCleaningContact();
            self.loading = false;
            addToCalls(data);
            self.currentCall = data;
            $state.go('telesale.callSheet.detail', data);
        }

        function addToCalls(data) {
            if (!data || !data.Site)
                return;

            var id = data.Site.Id;
            var exist = false;
            for (var i = 0; i < self.calls.length; i++) {
                if (self.calls[i].Site.Id === id) {
                    exist = true;
                    self.calls[i] = data;
                    break;
                }
            }

            if (!exist) {
                //strip the key for url param
                self.calls.push(data);
            }
        }

        function end() {
            if (!self.site || !self.contact) {
                displayError("Nothing to end");
                return;
            }

            var actions;
            var scriptListener = $scope.$on('event:endingScript', function (e, d) {
                scriptListener();
                actions = d;

                if (!self.currentCall) {
                    displayError("Invalid call");
                    return;
                }

                if (actions.length <= 0) {
                    displayError("No actions to confirm");
                    return;
                }

                var param = { paramId: self.currentCall.Contact.Id };

                var data = {
                    managerId: self.currentCall.LeadPerson.Id,
                    occupiedId: self.currentCall.OccupiedId,
                    initial: self.telesale.initial,
                    actions: actions,
                    url: leadService.getLeadDetailUrl(0)
                };

                processEndCall(param, data);
            });

            $scope.$broadcast('event:endingCall');
        }

        function processEndCall(param, data) {
            swal({
                title: "Confirm?",
                type: "warning",
                showCancelButton: true,
                allowEscapeKey: true,
                closeOnConfirm: false,
                text: "The call is completed.",
                confirmButtonColor: "#7266ba"
            }, function () {
                apiService.contact.endCall(param, data, function (success) {
                    swal("Success!", "Call completed", "success");
                }, function (error) {
                    var errorMsg = error.data.ExceptionMessage ? error.data.ExceptionMessage : 'Error';
                    swal("Error", errorMsg, "error");
                });
            });
        }
    };
})();