(function() {
    'use strict';
    angular.module('app.shared.controllers')
        .controller('ModalContactCtrl', modalContactCtrl);
    modalContactCtrl.$inject = ['$scope', '$modalInstance', 'logger', 'site', 'contactPersonService', '$modal', 'SweetAlert', '$filter'];
    function modalContactCtrl($scope, $modalInstance, logger, site, contactPersonService, $modal, sweetAlert, $filter){
        $scope.site = site;
        $scope.personUpdated = false;
        $scope.actions = [
                { text: "Edit" },
                { text: "Delete" }
        ];
        $scope.checked = checked;
        $scope.disableCheckbox = disableCheckbox;
        $scope.toggleSelection = toggleSelection;
        $scope.cancel = cancel;
        $scope.edit = edit;
        $scope.delete = deletePerson;
        $scope.actionSelect = actionSelect;
        $scope.add = addPerson;
        $scope.save = savePerson;

        function checked(person, contact) {
            return contact.ContactPersonId === person.Id;
        };
        function disableCheckbox(person, contact) {
            if (contact.ContactPersonId) return contact.ContactPersonId !== person.Id;
            return false;
        };
        function toggleSelection(person, contact) {
            if (contact.ContactPersonId) {
                contact.ContactPersonId = null;
            } else {
                contact.ContactPersonId = person.Id;
            }
        };
        function cancel() {
            if ($scope.personUpdated) $modalInstance.dismiss($scope.site);
            $modalInstance.dismiss();
        };

     
        //TODO: ng-repeat will create isolate scope. When roll back for modal dismiss, we always revert it in site level. 
        function edit(person) {
            var backup = undefined;
            var modalInstance = $modal.open({
                templateUrl: 'ModalContent.html',
                controller: 'ModalContactPersonCtrl',
                size: null,
                backdrop: 'static',
                resolve: {
                    siteId: function () {
                        return $scope.site.Id;
                    },
                    person: function () {
                        backup = angular.copy($scope.site);
                        return person;
                    }
                }
            });

            modalInstance.result.then(function () {
                $scope.personUpdated = true;
            },
            //dismiss, rollback
            function (reason) {
                $scope.site = backup;
            });
        };

        function deletePerson(person) {
            sweetAlert.confirm('Please Confirm', 'Delete this contact person ?').then(function() {
                contactPersonService.removeContactPerson(person.Id).then(function (result) {
                    var id = person.Id;
                    var contactPersons = $scope.site.ContactPersons;
                    var contacts = $scope.site.Contacts;

                    //Remove deleted person from contact Person list
                    for (var i = contactPersons.length - 1; i >= 0; i--) {
                        if (contactPersons[i].Id === id) {
                            contactPersons.splice(i, 1);
                            break;
                        }
                    }
                    //set contactPerson Id to null for corresponding contact
                    for (var k = contacts.length - 1; k >= 0; k--) {
                        if (contacts[k].ContactPersonId === id) {
                            contacts[k].ContactPersonId = null;
                            break;
                        }
                    }

                    $scope.personUpdated = true;
                    logger.success('Contact person ' + person.Id + ' deleted', 'Success');
                }, function (error) {
                    logger.serverError(error);
                });
            });
        }

        function actionSelect(person) {
            switch ($scope.actions.selected) {
                case "Edit":
                    $scope.edit(person);
                    $scope.actions.selected = undefined;
                    break;
                case "Delete":
                    $scope.delete(person);
                    $scope.actions.selected = undefined;
                    break;
            }
        };
        function addPerson(size) {
            var modalInstance = $modal.open({
                templateUrl: 'ModalContent.html',
                controller: 'ModalContactPersonCtrl',
                size: size,
                resolve: {
                    siteId: function () {
                        return $scope.site.Id;
                    },
                    person: function () {
                        return null;
                    }
                }
            });
            modalInstance.result.then(function (person) {
                $scope.site.ContactPersons.push(person);
                $scope.personUpdated = true;
            }, function () {
            });
        };
        function savePerson() {
            contactPersonService.updateContact($scope.site.Contacts).then(function (result) {
                logger.success("Update success", "Success");
                //update contact person information for contact
                angular.forEach($scope.site.Contacts, function (value, key) {
                    if (value.ContactPersonId === null) {
                        value.ContactPerson = null;
                    } else {
                        var person = $filter('filter')($scope.site.ContactPersons, { Id: value.ContactPersonId })[0];
                        value.ContactPerson = angular.copy(person);
                    }
                });
                $modalInstance.close($scope.site);
            }, function (error) {
                logger.serverError(error);
            });
        };
    }
})();