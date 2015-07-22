(function() {
    'use strict';

    angular.module('app.telesale.controllers')
        .controller('callsheetSiteController', siteController);
    siteController.$inject = [
        '$scope',
        'apiService',
        '$modal',
        'logger',
        'typeLibrary',
        'telesaleService',
        '$stateParams',
        'callParams',
        '$filter'
    ];

    function siteController(
        $scope,
        apiService,
        $modal,
        logger,
        typeLibrary,
        telesaleService,
        $stateParams,
        callParams,
        $filter) {

        var self = this;
        if ($stateParams.Site)
            self.data = $stateParams.Site;

        if ($stateParams.Contact)
            self.contact = $stateParams.Contact;

        self.newContactPerson = newContactPerson;
        self.assignContactPerson = assignContactPerson;
        self.selectContactPerson = selectContactPerson;
        self.expandGroup = onGroupClicked;
        self.matchContact = matchContact;
        initCleaningContactPerson();

        if (self.data)
            self.contactPersonParams = telesaleService.initNgParams(self.data.ContactPersons);

        self.popover = {
            group: {
                url: 'tpl/telesale/callsheet/popover.group.html',
                title: ''
            },

            search: {
                url: 'tpl/telesale/callsheet/popover.search.html',
                title: ''
            }
        };

        $scope.$on('event:endingCall', function(e) {
            e.preventDefault();
            $scope.$emit('event:endingSite', self.data.Id);
        });

        function initCleaningContactPerson() {
            if (!self.contact || !self.data)
                return;

            var cleaningContact,
                cleaningContactPerson;

            cleaningContact = $filter('find')(self.data.Contacts, function (obj) {
                return obj.BusinessTypeId === typeLibrary.businessType.cleaning;
            });

            if (cleaningContact) {
                cleaningContactPerson = $filter('find')(self.data.ContactPersons, function (obj) {
                    return obj.Id === cleaningContact.ContactPersonId;
                });

                if (cleaningContactPerson && !cleaningContactPerson.selected)
                    cleaningContactPerson.selected = !cleaningContact.selected;
            }
        }

        function selectContactPerson(person) {
            if (!person)
                return;

            self.data.ContactPersons.forEach(function (p) {
                console.log(p);
                console.log(person);
                if (p.Id === person.Id) {
                    p.selected = !p.selected;
                } else {
                    p.selected = false;
                }
            });
        }

        function newContactPerson() {
            if (!self.data)
                return;

            var options = {
                templateUrl: 'tpl/Shared/modal.contactPerson.html',
                controller: 'ModalContactPersonCtrl',
                resolve: {
                    siteId: function () {
                        return self.data.Id;
                    },
                    person: function () {
                        return undefined;
                    }
                }
            }

            $modal.open(options).result.then(function (result) {
                self.data.ContactPersons.push(result);
            });
        };

        function assignContactPerson(person, type) {
            if (!person)
                return;

            if (person.Id == 0 || !self.contact || !self.data)
                return;

            var targetContact = findContact(type);
            var param = {
                contactId: self.contact.Id,
                paramId: person.Id,
                targetContactId: targetContact != null ? targetContact.Id : 0,
                type : type
            };

            apiService.contact.assignContactPerson(
                param,
                null,
                function (success) {
                    if (!targetContact)
                        self.data.Contacts.push(success.contact);
                    else
                        targetContact.ContactPersonId = success.contact.ContactPersonId;
                }, handleError);
        };

        function matchContact(person, type) {
            if (!person || !type)
                return false;

            var next = findContact(type);
            return next ? next.ContactPersonId === person.Id : false;
        }

        function findContact(type) {
            if (!self.data || !type)
                return null;

            var i, next;
            for (i = 0; i < self.data.Contacts.length; i++) {
                next = self.data.Contacts[i];
                if (next.BusinessTypeId === typeLibrary.businessType[type]) {
                    return next;
                }
            }
            return null;
        }

        function onGroupClicked(group) {
            if (!group)
                return;

            var options = {
                templateUrl: 'tpl/telesale/callsheet/modal.group.html',
                controller: 'modalGroupController',
                controllerAs: 'modal',
                resolve: {
                    $group: function () {
                        return group;
                    }
                },
                size: 'lg'
            }

            $modal.open(options).result.then(function (siteId) {
                $scope.$emit('event:callFromGroup', siteId);
            });
        }

        function handleError(error) {
            logger.serverError(error);
        }
    };
})();