(function () {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('leadPriorityController', controller);
    controller.$inject = ['apiService', 'logger', '$q', '$filter',  "utility", 'formatter', '$modal'];

    function controller(apiService, logger, $q, $filter, utility, formatter, $modal) {
        var self = this;

        self.persons = undefined;
        self.groups = undefined;
        self.stats = undefined;
        self.shift = undefined;
        self.selected = undefined;
        self.selectedStats = undefined;
        self.editable = false;
        self.adding = false;

        self.edit = edit;
        self.getGroup = getGroup;
        self.select = select;
        self.cancel = cancel;
        self.save = save;
        self.add = add;
        self.editShift = editShift;

        self.dtOptions = {
            minDate: new Date(),
            dateOptions: {
                formatYear: 'yyyy'
            },
            dtOpen: function($event) {
                $event.preventDefault();
                $event.stopPropagation();
                self.dtOptions.dtOpened = !self.dtOptions.dtOpened;
            }
        };

        var param = { stats: true },
            person = apiService.leadPersonal.get(param).$promise,
            shift = apiService.leadPersonal.getShift().$promise,
            clones = [];

        $q.all([person, shift]).then(function (success) {
            var data = success[0].data;
            self.persons = data.persons;
            self.groups = data.groups;
            self.stats = data.stats;
            self.shift = success[1].data[0];
        });

        function edit(bool) {
            self.editable = bool;
            clones = [];

            if (self.selected)
                clone(self.selected);
        }

        function getGroup(id) {
            return $filter('find')(self.groups, function(group) {
                return (id === group.Id);
            });
        }

        function select(p) {
            if (self.adding) {
                swal({ title: "Error!", text: "Complete the new person first!", type: "error", confirmButtonText: "Ok" });
                return;
            }

            self.selected = p;
            self.selectedStats = $filter('find')(self.stats, function (obj) {
                return obj.PersonId === p.Id;
            });

            if (self.editable)
                clone(p);
        }

        function clone(p) {
            if (!p || !clones)
                return;

            for (var i = 0; i < clones.length; i++) {
                if (clones[i].Id === p.Id)
                    return;
            }

            clones.push(angular.copy(p));
        }

        function cancel() {

            if (self.adding) {
                delete self.selected;
                self.adding = !self.adding;
                self.editable = !self.editable;
                return;
            }

            if (!clones || clones.length <= 0)
                return;

            if (!self.persons || self.persons.length <= 0)
                return;

            clones.forEach(function (c) {
                self.persons.forEach(function(p) {
                    if (p.Id === c.Id) {
                        angular.copy(c, p);
                    }
                });
            });

            self.editable = false;
        }

        function add() {
            var p = {
                Id: 0
            };

            self.selected = p;
            self.editable = true;
            self.adding = true;
        }

        function save(form) {
            if (!form.$valid)
                return;

            if (self.adding) {
                var added = angular.copy(self.selected);
                added.LeadsOnHoldDate = formatter.Date(added.LeadsOnHoldDate);
                delete added.Leads;
                delete added.LeadGroup;
                apiService.leadPersonal.save(added, function(success) {
                    self.editable = false;
                    self.adding = false;
                    self.selected = success.data;
                    self.persons.push(success.data);
                }, function(fail) {
                    logger.serverError(fail);
                });

                return;
            }

            if (!clones || clones.length <= 0)
                return;

            var list = [];

            clones.forEach(function(c) {
                var model = $filter('find')(self.persons, function(obj) {
                    return obj.Id === c.Id;
                });

                if (model) {
                    var diff = utility.diff(c, model, []);
                    if (Object.keys(diff).length > 0) {
                        var copy = angular.copy(model);
                        copy.LeadsOnHoldDate = formatter.Date(copy.LeadsOnHoldDate);
                        delete copy.Leads;
                        delete copy.LeadGroup;
                        list.push(copy);
                    }
                }
            });

            if (list.length <= 0) {
                self.editable = false;
                return;
            }

            apiService.leadPersonal.batchUpdate(list, function (success) {
                self.editable = false;
                list = [];
            }, function(fail) {
                logger.serverError(fail);
            });
        }

        function editShift() {

            var url = '/tpl/CallAdmin/LeadPriority/shiftEstimation_modal.html';
            var ctrl = 'shiftEstimationController';

            var cloned = angular.copy(self.shift);

            var modalOptions = {
                templateUrl: url,
                controller: ctrl,
                resolve: {
                    $shift: function() {
                        return cloned;
                    }
                }
            }

            var modal = $modal.open(modalOptions);
            modal.result.then(function(result) {
                var diff = utility.diff(self.shift, result, []);
                if (Object.keys(diff).length > 0) {
                    apiService.leadPersonal.updateShift(result, function(success) {
                        self.shift = success.data;
                    }, function(fail) {
                        logger.serverError(fail);
                    });
                }
            });
        }
    };
})();