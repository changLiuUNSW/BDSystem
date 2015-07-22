(function () {
    'use strict';
    /**
     * Config for the router
     */
    angular.module('app')
       .config(
            [
                '$stateProvider', '$urlRouterProvider',
                function ($stateProvider, $urlRouterProvider) {
                    $urlRouterProvider.when('', '/dashboard');
                    $urlRouterProvider
                        .otherwise('/access/404');
                    $stateProvider
                        //Lead
                        .state('lead', {
                            abstract: true,
                            url: '/lead',
                            template: '<ui-view/>'
                        })
                        .state('lead.list', {
                            abstract: true,
                            url: '/list',
                            templateUrl: '/tpl/Lead/lead.list.html',
                            controller: 'leadListCtrl',
                            controllerAs: 'vm'
                        })
                        .state('lead.list.group', {
                            url: '/group/{status}',
                            templateUrl: '/tpl/Lead/lead.list.group.html',
                            controller: 'leadListGroupCtrl',
                            controllerAs: 'vm'

                        })
                        .state('lead.detail', {
                            url: '/detail/{id:[0-9]{1,10}}',
                            templateUrl: '/tpl/Lead/lead.detail.html',
                            controller: 'leadDetailCtrl',
                            controllerAs: 'vm'
                        })

                        //Quote
                        .state('quote', {
                            abstract: true,
                            url: '/quote',
                            template: '<ui-view/>',
                        })
                        .state('quote.progress', {
                            abstract: true,
                            url: '/progress',
                            templateUrl: '/tpl/Quote/Progress/quote.progress.html',
                            controller: 'quoteProgressCtrl',
                            controllerAs: 'vm'
                        })
                        .state('quote.progress.group', {
                            url: '/group/{status}',
                            templateUrl: '/tpl/Quote/Progress/quote.progress.group.html',
                            controller: 'quoteProgressGroupCtrl',
                            controllerAs: 'vm'
                        })
                        .state('quote.current', {
                            abstract: true,
                            url: '/current',
                            template: '<ui-view/>'
                        })
                        .state('quote.current.list', {
                            url: '/list',
                            controller: 'quoteCurrentListCtrl',
                            controllerAs: 'vm',
                            templateUrl: '/tpl/Quote/Current/quote.current.list.html'
                        })

                        //quote Detail
                        .state('quote.detail', {
                            abstract: true,
                            url: '/detail',
                            template: '<ui-view/>'
                        })
                        .state('quote.detail.progress', {
                            abstract: true,
                            url: '/progress/{id:[0-9]{1,10}}',
                            controller: 'quoteProgressDetailCtrl',
                            controllerAs: 'vm',
                            templateUrl: '/tpl/Quote/Progress/quote.detail.html'
                        })
                        .state('quote.detail.progress.overview', {
                            url: '/overview',
                            templateUrl: '/tpl/Quote/Progress/quote.detail.overview.html',
                        })
                        .state('quote.detail.progress.estimation', {
                            url: '/estimation',
                            templateUrl: '/tpl/Quote/Progress/quote.detail.estimation.html'
                        })
                        .state('quote.detail.progress.issues', {
                            url: '/issues',
                            templateUrl: '/tpl/Quote/Progress/quote.detail.issues.html'
                        })
                        
                        .state('quote.detail.progress.history', {
                            url: '/history',
                            templateUrl: '/tpl/Quote/Progress/quote.detail.history.html',
                        })
                        //Quote detail current
                        .state('quote.detail.current', {
                            url: '/current/{id:[0-9]{1,10}}',
                            controller: 'quoteCurrentDetailCtrl',
                            controllerAs: 'vm',
                            templateUrl: '/tpl/Quote/Current/quote.current.detail.html'
                        })
                        
                        //Estimation
                        .state('quote.estimation', {
                            url: '/estimation/{id:[0-9]{1,10}}',
                            controller: 'quoteEstimationCtrl',
                            controllerAs: 'vm',
                            templateUrl: '/tpl/Quote/Estimation/estimation.html'
                        })
                      
                        //Qute Cost
                        .state('quote.cost', {
                            url: '/cost',
                            abstract: true,
                            controller: 'quoteCostCtrl',
                            controllerAs: 'cost',
                            templateUrl: '/tpl/Quote/Cost/cost.html'
                        })
                        .state('quote.cost.detail', {
                            url: '/detail',
                            controller: 'quoteCostDetailCtrl',
                            controllerAs: 'costDetail',
                            templateUrl: '/tpl/Quote/Cost/cost.detail.html'
                        })
                        .state('quote.cost.loaction', {
                            url: '/location',
                            controller: 'quoteCostLocationCtrl',
                            controllerAs: 'costLocation',
                            templateUrl: '/tpl/Quote/Cost/cost.location.html'
                        })
                        .state('quote.cost.area', {
                            url: '/area',
                            controller: 'quoteCostAreaCtrl',
                            controllerAs: 'costArea',
                            templateUrl: '/tpl/Quote/Cost/cost.area.html'
                        })
                        .state('quote.cost.info', {
                            url: '/info',
                            controller: 'quoteCostInfoCtrl',
                            templateUrl: '/tpl/Quote/Cost/cost.info.html'
                        })
                        .state('quote.cost.addition', {
                            url: '/addition',
                            controller: 'quoteCostAdditionCtrl',
                            controllerAs: 'costAddition',
                            templateUrl: 'tpl/Quote/Cost/cost.addition.html'
                        })
                        .state('quote.cost.quadlabour', {
                            url: '/quadlabour',
                            controller: 'quoteCostQuadLabourCtrl',
                            templateUrl: '/tpl/Quote/Cost/cost.quadlabour.html'
                        })
                        .state('quote.cost.extra', {
                            url: '/extra',
                            controller: 'quoteCostExtraCtrl',
                            templateUrl: '/tpl/Quote/Cost/cost.extra.html'
                        })
                        .state('quote.cost.result', {
                            url: '/result',
                            controller: 'quoteCostResultCtrl',
                            templateUrl: '/tpl/Quote/Cost/cost.result.html'
                        })
                        //DashBoard
                        .state('dashboard', {
                            url: '/dashboard',
                            templateUrl: 'tpl/DashBoard/index.html',
                            controller: 'DashBoardCtrl'
                        })
                        //TeleSale
                        .state('telesale', {
                            abstract: true,
                            url: '/telesale',
                            template: '<ui-view/>',
                        })
                        .state('telesale.callSheet', {
                            url: '/callsheet',
                            views: {
                                '': {
                                    templateUrl: '/tpl/telesale/callsheet/callsheet.html',
                                    controller: 'callSheetController',
                                    controllerAs: 'callsheet',
                                },
                                'header@telesale.callSheet': {
                                    templateUrl: '/tpl/telesale/callsheet/callsheet.header.html',
                                    controller: 'callSheetHeaderController',
                                    controllerAs: 'header'
                                },
                                'detail@telesale.callSheet': {
                                    templateUrl: '/tpl/telesale/callsheet/callsheet.detail.html'
                                }
                            }
                        })
                        .state('telesale.callSheet.detail', {
                            url: '/{key}',
                            params: {
                                Site: null,
                                Contact: null,
                                LeadPerson: null,
                                Script: null,
                                ScriptActions: null,
                            },
                            views: {
                                'site': {
                                    templateUrl: '/tpl/telesale/callsheet/callsheet.detail.site.html',
                                    controller: 'callsheetSiteController',
                                    controllerAs: 'site',
                                },
                                'contact': {
                                    templateUrl: '/tpl/telesale/callsheet/callsheet.detail.contact.html',
                                    controller: 'callsheetContactController',
                                    controllerAs: 'contact',
                                },
                                'script': {
                                    templateUrl: '/tpl/telesale/callsheet/callsheet.script.html',
                                    controller: 'callsheetScriptController',
                                    controllerAs: 'script'
                                }
                            }
                        })
                        .state('telesale.queue', {
                            url: '/callqueue',
                            views: {
                                '': {
                                    templateUrl: '/tpl/telesale/queue/queue.html',
                                    controller: 'queueController',
                                    controllerAs: 'queue'
                                },
                                'aside@telesale.queue': {
                                    templateUrl: '/tpl/telesale/queue/queue.aside.html',
                                    controller: 'queueAsideController',
                                    controllerAs: 'aside'
                                },
                                'info@telesale.queue': {
                                    templateUrl: '/tpl/telesale/queue/queue.info.html',
                                    controller: 'queueInfoController',
                                    controllerAs: 'info'
                                }
                            }
                        })
                        .state('ts', {
                            url: '/call',
                            abstract: true,
                            templateUrl: '/tpl/calladmin/dash.html'
                        })
                        .state('ts.dash', {
                            url: '/dash',
                            views: {
                                '' : {
                                    templateUrl: '/tpl/calladmin/dash.content.html',
                                    controller: 'callAdminController',
                                    controllerAs: 'admin'
                                }
                            }
                        })
                        .state('ts.allocation', {
                            url: '/allocation',
                            views: {
                                '': {
                                    templateUrl: '/tpl/CallAdmin/allocation/dash/allocation.html',
                                    controller: 'allocationController',
                                },
                                'aside@ts.allocation': {
                                    templateUrl: '/tpl/CallAdmin/allocation/allocation.aside.html'
                                },
                                'table@ts.allocation': {
                                    templateUrl: '/tpl/CallAdmin/allocation/allocation.table.html'
                                }
                            }
                        })
                        .state('ts.report', {
                            url: '/report',
                            views: {
                                '': {
                                    templateUrl: '/tpl/callAdmin/report/dash/report.html',
                                    controller: 'reportController',
                                    controllerAs: 'report'
                                },
                                'statistic@ts.report': {
                                    templateUrl: '/tpl/calladmin/report/report.statistic.html',
                                    controller: 'reportStatisticController',
                                    controllerAs: 'statistic'
                                },
                                'table@ts.report': {
                                    templateUrl: '/tpl/calladmin/report/report.statistic.table.html'
                                }
                            }
                        })
                        .state('ts.leadpriority', {
                            url: '/leadpriority',
                            views: {
                                '': {
                                    templateUrl: '/tpl/calladmin/leadpriority/dash/leadpriority.html',
                                    controller: 'leadPriorityController',
                                    controllerAs: 'leadPriority'
                                },
                                'aside@ts.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.aside.html'
                                },
                                'content@ts.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.content.html'
                                },
                                'stats@ts.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.stats.html'
                                },
                                'shift@ts.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.shift.html'
                                }
                            }
                        })
                        //CallAdmin
                        .state('calladmin', {
                            abstract: true,
                            url: '/calladmin',
                            template: '<ui-view/>',
                        })
                        .state('calladmin.allocation', {
                            url: '/allocation',
                            views: {
                                '': {
                                    templateUrl: '/tpl/CallAdmin/allocation/allocation.html',
                                    controller: 'allocationController'
                                },
                                'aside@calladmin.allocation': {
                                    templateUrl: '/tpl/CallAdmin/allocation/allocation.aside.html'
                                },
                                'table@calladmin.allocation': {
                                    templateUrl: '/tpl/CallAdmin/allocation/allocation.table.html'
                                }
                            }
                        })
                        .state('calladmin.leadpriority', {
                            url: '/leadpriority',
                            views: {
                                '': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.html',
                                    controller: 'leadPriorityController',
                                    controllerAs: 'leadPriority'
                                },
                                'aside@calladmin.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.aside.html'
                                },
                                'content@calladmin.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.content.html'
                                },
                                'stats@calladmin.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.stats.html'
                                },
                                'shift@calladmin.leadpriority': {
                                    templateUrl: '/tpl/calladmin/leadpriority/leadpriority.shift.html'
                                }
                            }
                        })
                        .state('calladmin.area', {
                            url: '/area',
                            views: {
                                '': {
                                    templateUrl: '/tpl/CallAdmin/Area/area.html',
                                    controller: 'areaController'
                                },
                                'postcode@calladmin.area': {
                                    templateUrl: '/tpl/CallAdmin/Area/area.postcode.html'
                                }
                            }
                        })
                        .state('calladmin.report', {
                            url: '/report',
                            views: {
                                '': {
                                    templateUrl: '/tpl/callAdmin/report/report.html',
                                    controller: 'reportController',
                                    controllerAs: 'report'
                                },
                                'statistic@calladmin.report': {
                                    templateUrl: '/tpl/calladmin/report/report.statistic.html',
                                    controller: 'reportStatisticController',
                                    controllerAs: 'statistic'
                                },
                                'table@calladmin.report': {
                                    templateUrl: '/tpl/calladmin/report/report.statistic.table.html'
                                }
                            }
                        })
                        //Admin
                        .state('Admin', {
                            abstract: true,
                            url: '/Admin',
                            template: '<ui-view/>',
                        })

                        //Admin- search
                        .state('Admin.search', {
                            url: '/search?group&bizType',
                            templateUrl: '/tpl/Admin/search.html',
                            controller: 'AdminSearchCtrl'
                        })

                        //Admin -site
                        .state('Admin.site', {
                            abstract: true,
                            url: '/site',
                            template: '<ui-view/>'
                        })
                        //site summary
                        .state('Admin.site.summary', {
                            url: '/summary',
                            templateUrl: 'tpl/Admin/Site/siteSummary.html',
                            controller: 'AdminSiteSummaryCtrl',
                            controllerAs: 'vm'
                        })

                        //site detail
                        .state('Admin.site.detail', {
                            url: '/detail/{id:[0-9]{1,10}}?group&bizType',
                            views: {
                                '': {
                                    templateUrl: 'tpl/Admin/Site/site.html',
                                    controller: 'AdminSiteCtrl'
                                },
                                'siteInfo@Admin.site.detail': {
                                    templateUrl: 'tpl/Admin/Site/site.siteInfo.html',
                                    controller: 'AdminSiteInfoCtrl'
                                },
                                'contactPerson@Admin.site.detail': {
                                    templateUrl: 'tpl/Admin/Site/site.contactPerson.html',
                                    controller: 'AdminSiteContactPersonCtrl'
                                },
                                'detail@Admin.site.detail': {
                                    templateUrl: 'tpl/Admin/Site/site.detail.html',
                                    controller: 'AdminSiteDetailCtrl'
                                }
                            }
                        })


                        //Admin -contactPerson
                        .state('Admin.contactPerson', {
                            abstract: true,
                            url: '/contactperson',
                            template: '<ui-view/>',
                        })
                        .state('Admin.contactPerson.summary', {
                            url: '/summary',
                            templateUrl: '/tpl/Admin/ContactPerson/personSummary.html',
                            controller: 'contactPersonSummaryCtrl'
                        })
                        //Admin-contactPerson.Detail
                        .state('Admin.contactPerson.detail', {
                            url: '/detail/{id:[0-9]{1,10}}',
                            templateUrl: '/tpl/Admin/ContactPerson/personDetail.html',
                            controller: 'contactPersonCtrl',

                        })
                        //Access
                        .state('access', {
                            url: '/access',
                            template: '<div ui-view class="fade-in-right-big smooth"></div>'
                        })
                       
                        .state('access.404', {
                            url: '/404',
                            templateUrl: 'tpl/Access/page_404.html'
                        });
                }
            ]
        );
})();