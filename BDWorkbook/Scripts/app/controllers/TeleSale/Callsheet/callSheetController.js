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
        'leadService',
        'utility'];

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
        leadService,
        utility) {

        var self = this;

        //page variables
        self.userInfo = angular.copy(userInfo);
        self.isTelesale = !isInRole('bd');
        self.loading = false;
        self.site = null;
        self.contact = null;
        self.leadPerson = null;
        self.calls = [];

        //functions
        self.next = next;
        self.end = end;
        self.closeTabe = closeCall;
        self.activateCall = activateCall;

        if (self.isTelesale) {
            self.telesale = {};
        };

        $scope.$on('event:callFromGroup', function (e, siteId) {
            e.preventDefault();
            e.stopPropagation();
            var existingCall = $filter('find')(self.calls, function(call) {
                return call.Site.Id === siteId;
            });

            if (existingCall) {
                initPageVariables(existingCall);
            }
            else {
                callParams.SiteId = siteId;
                telesaleService.makeCall(callParams, function (success) {
                    initPageVariables(success.data);
                }, function(fail) {
                });
            }
        });

        $scope.$on('event:auth-loginConfirmed', function () {
            $state.go('telesale.callSheet', {}, { reload : true });
        });

        function isInRole(role) {
            if (!self.userInfo.userName) {
                $rootScope.$broadcast('event:auth-loginRequired');
                return false;
            }

            return utility.isInRole(role);
        }

        function next() {
            self.loading = true;
            self.calls = [];
            self.site = null;
            self.contact = null;
            self.leadPerson = null;
            var callbackHandler;
            if (self.isTelesale) {
                callParams.Initial = self.telesale.initial;
                callbackHandler = telesaleCallHandler;
            } else {
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
                        callParams.LastCallId = success.data.OccupiedId;
                        next();
                    };
                });
            };

            init(success.data);
        }

        function bdCallHandler(success) {
            if (!success)
                return;

            init(success.data);
        }

        function init(data) {
            data.canDelete = false;
            initPageVariables(data);

            activateCall(self.calls[0]);
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

        function initPageVariables(data) {
            if (!data)
                return;

            //self.contact = data.Contact;
            //self.site = data.Site;
            //self.leadPerson = data.LeadPerson;
            self.loading = false;

            addToCalls(data);
            if (data.Contact && data.Contact.Code == "PMS") {
                var groups = data.Site.Groups;
                groups.forEach(function(group) {
                    group.Sites.forEach(function (site) {
                        if (site.Key.substring(0, 2) === 'MT') {
                            addToCalls({
                                Site: site,
                                Contact: null,
                                LeadPerson: data.LeadPerson,
                                Script: data.Script,
                                ScriptActions: angular.copy(data.ScriptActions),
                                canDelete: false,
                            });
                        };
                    });
                });
            }

            data.active = true;
        }

        function addToCalls(data) {
            if (!data || !data.Site)
                return;

            var isInvalid = false;
            for (var i = 0; i < self.calls.length; i++) {
                if (self.calls[i].Site.Id === data.Site.Id) {
                    isInvalid = true;
                    self.calls[i] = data;
                    break;
                }
            }

            if (!isInvalid) {
                self.calls.push(data);
            }
        }

        function activateCall(call) {
            if (!call)
                return;

            $state.go('telesale.callSheet.detail', call);
        }

        function closeCall(index) {
            if (self.calls.length == 1) {
                displayError("Can not close the last one");
                return;
            }

            self.calls.splice(index, 1);
        }

        function end() {
            var call = getActivedCall();
            if (!call)
                displayError("Invalid call to end");

            var actions;
            var scriptListener = $scope.$on('event:endingScript', function (e, d) {
                scriptListener();
                actions = d;

                var defaultActions = getActivedDefaultActions(call);
                if (actions.length <= 0 && defaultActions.length <= 0) {
                    displayError("No actions to confirm");
                    return;
                }

                var data = {
                    SiteId: call.Site.Id,
                    ContactId: call.Contact ? call.Contact.Id : null,
                    LeadPersonId: call.LeadPerson ? call.LeadPerson.Id : null,
                    OccupiedId: call.OccupiedId,
                    Initial: self.isTelesale ? self.telesale.initial : null,
                    Actions: {
                        actions : actions.concat(defaultActions)
                    },
                    RedirectUrl: leadService.getLeadDetailUrl(0)
                };

                processEndCall(data);
            });

            $scope.$broadcast('event:endingCall');
        }

        function getActivedDefaultActions(call) {
            var actions = [];

            if (!call || !call.ScriptActions)
                return actions;

            call.ScriptActions.forEach(function(action) {
                if (action.active)
                    actions.push(action);
            });

            return actions;
        };

        function getActivedCall() {
            return $filter('find')(self.calls, function(call) {
                return call.active;
            });
        }

        function processEndCall(data) {
            swal({
                title: "Confirm?",
                type: "warning",
                showCancelButton: true,
                allowEscapeKey: true,
                closeOnConfirm: false,
                text: "The call is completed.",
                confirmButtonColor: "#7266ba"
            }, function () {
                apiService.contact.endCall(data, function (success) {
                    swal("Success!", "Call completed", "success");
                }, function (error) {
                    var errorMsg = error.data.ExceptionMessage ? error.data.ExceptionMessage : 'Error';
                    swal("Error", errorMsg, "error");
                });
            });
        }
    };
})();