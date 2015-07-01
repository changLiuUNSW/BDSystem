(function() {
    'use strict';

    angular.module('app.telesale.controllers')
        .controller('callsheetContactController', controller);
    controller.$inject = ['$scope', '$filter', '$modal', 'apiService', 'logger', 'typeLibrary', '$stateParams', 'telesaleService'];

    function controller($scope, $filter, $modal, apiService, logger, typeLibrary, $stateParams, telesaleService) {
        var self = this,
            editing = [];

        if ($stateParams.Contact)
            self.data = $stateParams.Contact;

        if ($stateParams.Site)
            self.site = $stateParams.Site;

        self.editable = false;
        self.selected = selectedPerson;
        self.callLine = callLine();
        self.prepareEdit = prepareEdit;
        self.edit = edit;
        self.cancel = cancel;
        self.save = save;
        self.updateNote = updateNote;

        if (self.data)
            self.callLineParams = telesaleService.initNgParams(self.data.CallLines);

        $scope.$on('event:endingCall', function (e) {
            e.preventDefault();
            $scope.$emit('event:endingContact', self.data.Id);
        });

        function selectedPerson() {
            if (!self.site)
                return null;

            return $filter('find')(self.site.ContactPersons, function(obj) {
                return obj.selected;
            });
        };

        function prepareEdit(person) {
            if (!person)
                return;

            var exist = $filter('find')(editing, function (obj) {
                return obj.clone.Id === person.Id;
            });

            if (!exist) {
                editing.push({
                    clone : angular.copy(person),
                    model : person
                });
            }
        };

        function edit(person) {
            if (!person)
                return;

            self.editable = !self.editable;
            editing = [];
            prepareEdit(person);
        };

        function cancel() {
            if (editing.length <= 0)
                return;

            editing.forEach(function (obj) {
                angular.copy(obj.clone, obj.model);
            });
            
            self.editable = !self.editable;
        };

        function save(form) {
            if (!form.$valid)
                return;

            var modalOptions = {
                templateUrl: 'tpl/telesale/callsheet/modal.confirm.html',
                controller: 'modalCpChangeController',
                resolve: {
                    $changes: function () {
                        return editing;
                    }
                }
            }

            var update = [];

            $modal.open(modalOptions).result.then(function (result) {
                if (!result)
                    return;

                var exist;
                result.forEach(function (obj) {
                    exist = $filter('find')(editing, function (person) {
                        return person.model.Id === obj.id;
                    });

                    if (exist) {
                        update.push({
                            update: angular.copy(exist.model),
                            history: angular.copy(exist.clone),
                            reason: obj.option,
                            name: 'Jing' // for esting
                        });
                    }
                });

                if (update.length > 0) {
                    apiService.contactPerson.pendingUpdate(update, function (success) {
                        editing = [];
                        self.editable = !self.editable;
                    }, handleError);
                }
            });
        };

        function callLine() {
            return {
                select: select,
                add: add,
                remove: remove,
                isInStatus: isInStatus
            };

            function isInStatus(target, status) {
                if (!target || !status)
                    return false;

                return typeLibrary.callLineStatus[status] === (target.Status & typeLibrary.callLineStatus[status]);
            }

            function select(target) {
                if (!target)
                    return;

                var exist = $filter('find')(self.data.CallLines, function(obj) {
                    return obj.selected;
                });

                if (exist)
                    delete exist.selected;

                target.selected = !target.selected;
            };

            function add(contact, user) {
                if (!contact)
                    return;

                var modalOptions = {
                    templateUrl: 'tpl/telesale/callsheet/modal.callline.html',
                    controller: 'modalCallLineController',
                    resolve: {
                        $initial: function () {
                            return user.Initial;
                        }
                    }
                }

                $modal.open(modalOptions).result.then(function(cl) {

                    var param = { contactId: contact.Id }
                    apiService.contact.addCallLine(param, cl, function(success) {
                        contact.CallLines.push(success.data);
                        self.callLineParams.reload();
                    }, handleError);
                });
            }

            function remove() {
                var selected = $filter('find')(self.data.CallLines, function(obj) {
                    return obj.selected;
                });

                if (!selected)
                    return;

                var param = { contactId: self.data.Id };

                apiService.contact.removeCallLine(param, selected, function(success) {
                    self.data.CallLines.forEach(function(item, idx, array) {
                        if (item.Id === selected.Id) {
                            array.splice(idx, 1);
                        };
                    });
                    self.callLineParams.reload();
                }, handleError);
            }
        };
     
        function updateNote(id, note) {
            if (!id || !note)
                return;

            var param = {
                contactId: id
            };

            apiService.contact.updateNote(param, JSON.stringify(note), function(success) {
                logger.success("Update success");
            }, handleError);
        }

        function handleError(error) {
            logger.serverError(error);
        }
    }
})();