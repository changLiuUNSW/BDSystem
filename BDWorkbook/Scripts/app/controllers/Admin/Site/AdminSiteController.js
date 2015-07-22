(function() {
    'use strict';
    angular.module('app.Admin.site.controllers')
        .controller('AdminSiteCtrl', adminSiteCtrl);
    adminSiteCtrl.$inject = [
        '$scope',
        '$stateParams',
        'siteService',
        'saleBoxService',
        'logger',
        '$filter',
        'utility',
        'leadService',
        '$modal'];

    function adminSiteCtrl(
        $scope,
        $stateParams,
        siteService,
        saleBoxService,
        logger,
        $filter,
        utility,
        leadService,
        $modal) {
        $scope.siteId = $stateParams.id;
        $scope.loading = true;
        $scope.bizTypes = ['Cleaning', 'Security', 'Maintenance'];
        $scope.groups = [
            {
                name: 'Call',
                iconCss: 'fa fa-fw fa-phone'
            },
            {
                name: 'Contract',
                iconCss: 'fa fa-fw fa-file-text'
            },
            {
                name: 'Quote',
                iconCss: 'fa fa-fw  fa-bar-chart-o'
            }
        ];

        //default 'cleaning'
        $scope.bizTypes.selected = $stateParams.bizType ? $stateParams.bizType.toLowerCase() : 'cleaning';
        //default 'Call'
        $scope.groups.selected = $stateParams.group ? $stateParams.group.toLowerCase() : 'call';
        $scope.model = {
            'site': undefined,
            'contacts': undefined,
            'cleaningContract': undefined,
            'securityContract': undefined
    }

        $scope.getSite = getSite;
        $scope.initSiteDetail = initSiteDetail;
        $scope.setBizType = setBizType;
        $scope.setGroup = setGroup;
        $scope.init = init;
        $scope.updateSite = updateSite;
        $scope.newLead = newLead;
        $scope.isBd = isBd;
        $scope.siteBeforeChange = undefined;
        $scope.init();

        function getSite(id) {
            $scope.loading = true;
            siteService.getSite({ id: id }).then(function (result) {
                saveSiteBeforeChange(result.data);
                $scope.model.site = result.data;
                $scope.initSiteDetail();
                $scope.loading = false;
            }, function (error) {
                logger.serverError(error);
            });
        };

        function saveSiteBeforeChange(site) {
            if (!site)
                return;

            $scope.siteBeforeChange = angular.copy(site);
        }

       function initSiteDetail() {
           var site = $scope.model.site;
            //Building
            $scope.model.building = $filter('filter')(site.Groups, { Type: 'building' })[0];
            //Group
            $scope.model.group = $filter('filter')(site.Groups, { Type: 'group' })[0];
            $scope.$watch(function () {
                return $scope.bizTypes.selected;
            }, function (newValue) {
                if (!newValue)
                    return;

                getSiteDetail(newValue);
            });
       };

        function setBizType(type) {
            $scope.bizTypes.selected = type.toLowerCase();
        };

        function setGroup(group) {
            $scope.groups.selected = group.toLowerCase();
        };

        function getSiteDetail(type) {
            var site = $scope.model.site;
            $scope.model.contacts = $filter('filter')(site.Contacts, { BusinessType: { Type: type.toLowerCase() } });
        }

        function init () {
            $scope.getSite($scope.siteId);
        };

        function updateSite(form) {
            if (form.$invalid)
                return;

            var diffs = utility.diff($scope.siteBeforeChange, $scope.model.site, ['Id', 'SiteId']);
            if (Object.getOwnPropertyNames(diffs).length <= 0) {
                logger.error("No changes detected");
                return;
            }

            siteService.updateSiteFromAdmin(diffs).then(function (response) {
                logger.success("Information saved");
                $scope.siteBeforeChange = angular.copy($scope.model.site);
            }, function(error) {
                logger.serverError(error);
            });
        }

        function isBd() {
            return utility.isInRole("BD");
        }

        function newLead(contactId) {
            var param = { ContactId: contactId };

            if (utility.isInRole("BD")) {
                var modalOption = {
                    templateUrl: 'tpl/admin/modal/modal.leadPersonList.html',
                    controller: 'adminLeadPersonListController',
                }

                $modal.open(modalOption).result.then(function (id) {
                    param.LeadPersonId = id;
                    saveLead(param);
                });
            } else {
                saveLead(param);
            };
        };

        function saveLead(param) {
            leadService.newLead(param).then(function () {
                logger.success("Lead created");
            }, function (errorResponse) {
                logger.serverError(errorResponse);
            });
        }
    }
})();