(function() {
    'use strict';
    angular.module('app.Admin.contactPerson.controllers')
        .controller('contactPersonCtrl', contactPersonCtrl);
    contactPersonCtrl.$inject = ['$scope', 'contactPersonService', '$stateParams', 'logger', 'contactPersonHelper', 'SweetAlert', '$state'];

    function contactPersonCtrl($scope, contactPersonService, $stateParams, logger, contactPersonHelper, sweetAlert, $state) {
        $scope.contactPersonId = $stateParams.id;
        $scope.contactPersonHelper = contactPersonHelper;
        $scope.contactPerson = undefined;
        $scope.getContactPerson = getContactPerson;
        $scope.updateProfile = updateProfile;
        $scope.deletePerson = deletePerson;
        $scope.currentPage = 0;
        $scope.pageSize = 5;
        $scope.filters = {
            reason: null
        }
        $scope.historyList = [];
        $scope.selectReason = selectReason;
        $scope.labelClass = labelClass;
        $scope.numberOfPages = numberOfPages;
        $scope.$watch('search', searchEvent);
        $scope.getHistoryList = getHistoryList;

 
        init();
        

        function getContactPerson () {
            $scope.loading = true;
            contactPersonService.getContactPerson({ id: $scope.contactPersonId }).then(function (result) {
                $scope.contactPerson = result.data;
                $scope.loading = false;
            }, function (error) {
                logger.serverError(error);
            });
        };
        function init() {
            $scope.getContactPerson();
            $scope.getHistoryList();

        };

        function updateProfile(person) {
            contactPersonService.saveContactPerson(person).then(function (result) {
                logger.success("Update successfully", "Success");

            }, function (error) {
                logger.serverError(error);
            });
        };

        function deletePerson(person) {
            sweetAlert.confirm('Please Confirm', 'Delete this contact person ?').then(function () {
                contactPersonService.removeContactPerson(person.Id).then(function (result) {
                    var id = result.data;
                    logger.success('Contact person ' + id + ' deleted', 'Success');
                    $state.go('Admin.search', { group: 'person' });
                }, function (error) {
                    logger.serverError(error);
                });
            });
        };

        function selectReason(reason) {
            $scope.filters.reason = reason;
            $scope.getHistoryList();
        };


        function labelClass(label) {
            return {
                'b-l-info': angular.lowercase(label) === 'new manager',
                'b-l-primary': angular.lowercase(label) === 'name correction'
            };
        };

        function numberOfPages(length) {
            return Math.ceil(length / $scope.pageSize);
        };

        function searchEvent() {
            $scope.currentPage = 0;
        };

        function getHistoryList() {
            var requestObj = {
                personId: $scope.personId,
                reason: $scope.filters.reason
            };
            $scope.loadingHistory = true;
            contactPersonService.getPersonHistory(requestObj).then(function (result) {
                $scope.loadingHistory = false;
                $scope.historyList = result.data;
            }, function (error) {
                logger.serverError(error);
            });
        }
    }
})();