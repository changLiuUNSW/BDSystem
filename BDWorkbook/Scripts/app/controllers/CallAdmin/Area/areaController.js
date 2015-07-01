(function() {
    'use strict';
    angular.module('app.callAdmin.controllers')
        .controller('areaController', areaController);
    areaController.$inject = ['$scope', 'apiService', 'logger'];

    function areaController($scope, apiService, logger) {

        //data module
        var dataModule = function () {

            var load = function (page, size, query) {

                var param = {
                    page: page,
                    pageSize: size,
                    postcode: query
                };

                return apiService.salesbox.get(param).$promise;
            }

            var remove = function (salesbox) {
                var param = {
                    postcode : salesbox.Postcode,
                    state : salesbox.State
                };
                return apiService.salesbox.remove(param).$promise;
            }

            var functions = {
                load: load,
                remove: remove
            };

            return functions;
        }();

        //main module
        var main = function (data) {

            var functions = {
                init: function () {
                    initQuery();
                    initPaging();
                    initData();
                    initEdit();
                },
            };

            var initPaging = function() {
                $scope.paging = {};
                $scope.paging.itemsPerPage = 10;

                $scope.$watch('paging.currentPage', function (value) {
                    if (!value)
                        return;
                    loadPage();
                });

                $scope.paging.currentPage = 1;
            }

            //data initialize
            var initData = function () {
                $scope.data = {};
            }

            //main editing scope initialize
            var initEdit = function () {
                $scope.edit = {};
                $scope.edit.form = {};

                $scope.edit.remove = remove;

                function remove(salesbox) {
                    dataModule.remove(salesbox).then(function(success) {
                        logger.success("Record deleted");
                        loadPage();
                    }, function(error) {
                        logger.error(error.data.Message, "Error loading page");
                    });
                }
            }

            //query scope initialize
            var initQuery = function () {
                $scope.query = {};

                $scope.query.find = function (postcode) {
                    loadPage();
                }
            }

            function loadPage() {
                data.load($scope.paging.currentPage, $scope.paging.itemsPerPage, $scope.query.postcode).then(function (success) {
                    $scope.data.salesboxes = success.data.Data;
                    $scope.paging.totalItems = success.data.TotalCount;
                    $scope.paging.numPages = success.data.totalPages;
                }, function(error) {
                    logger.error(error.data.Message, "Error loading page");
                });
            }

            return functions;

        }(dataModule || {});

        main.init();
    }
})();