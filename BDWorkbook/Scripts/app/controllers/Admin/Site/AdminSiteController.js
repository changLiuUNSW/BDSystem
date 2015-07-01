(function() {
    'use strict';
    angular.module('app.Admin.site.controllers')
        .controller('AdminSiteCtrl', adminSiteCtrl);
    adminSiteCtrl.$inject = ['$scope', '$stateParams', 'siteService', 'saleBoxService', 'logger', '$filter'];

    function adminSiteCtrl($scope, $stateParams, siteService, saleBoxService, logger, $filter) {
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
            'cleaningDetail': {
                calls: undefined,
                contract: undefined,
                quote: undefined
            },
            'securityDetail': {
                calls: undefined,
                contract: undefined,
                quote: undefined
            },
            'maintenanceDetail': {
                calls: undefined,
                contract: undefined,
                quote: undefined
            }
        }

        $scope.getSite = getSite;
        $scope.initSiteDetail = initSiteDetail;
        $scope.setBizType = setBizType;
        $scope.setGroup = setGroup;
        $scope.init = init;
        $scope.updateSite = updateSite;
        $scope.init();

        function getSite(id) {
            $scope.loading = true;
            siteService.getSite({ id: id }).then(function (result) {
                $scope.model.site = result.data;
                $scope.initSiteDetail();
                $scope.loading = false;
            }, function (error) {
                logger.serverError(error);
            });
        };

       function initSiteDetail() {
           var site = $scope.model.site;
           console.log(site);
            //Building
            $scope.model.building = $filter('filter')(site.Groups, { Type: 'building' })[0];
            //Group
            $scope.model.group = $filter('filter')(site.Groups, { Type: 'group' })[0];

            //Cleaning Detail
            $scope.model.cleaningDetail.contract = site.CleaningContract;
            $scope.model.cleaningDetail.calls = $filter('filter')(site.Contacts, { BusinessType: { Type: "cleaning" } });
            $scope.model.cleaningDetail.quote = null;
            
            //Security Detail
            $scope.model.securityDetail.contract = site.SecurityContract;
            $scope.model.securityDetail.calls = $filter('filter')(site.Contacts, { BusinessType: { Type: "security" } });
            $scope.model.securityDetail.quote = null;
            //Maintenance Detail
            $scope.model.maintenanceDetail.contract = null;
            $scope.model.maintenanceDetail.calls = $filter('filter')(site.Contacts, { BusinessType: { Type: "maintenance" } });
            $scope.model.maintenanceDetail.quote = null;
        };
        //TODO: Not implement for security and maintenance
       function setBizType(type) {
            if (type.toLowerCase() === 'security' || type.toLowerCase() === 'maintenance') {
                logger.error('Not Implement', 'Error');
                return;
            }
            $scope.bizTypes.selected = type.toLowerCase();
        };
       function setGroup(group) {
            $scope.groups.selected = group.toLowerCase();
        };

        function init () {
            $scope.getSite($scope.siteId);
        };


        function updateSite () {
            console.log($scope.model.site);
        }
    }
})();