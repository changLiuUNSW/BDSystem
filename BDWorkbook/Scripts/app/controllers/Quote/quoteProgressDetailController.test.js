describe('quoteProgressDetailCtrl: test spec', function () {
    var quoteProgressGroupCtrl,
        quoteService,
        quoteCostService,
        quoteWorkFlowHelper = {
            enableCostAdd: jasmine.createSpy('enableCostAdd'),
            nextStep: jasmine.createSpy('nextStep'),
            checkAllDone: jasmine.createSpy('checkAllDone'),
            resolveIssue: jasmine.createSpy('resolveIssue'),
            enableSendBtn: jasmine.createSpy('enableSendBtn'),
            bdgmReview: jasmine.createSpy('bdgmReview'),
            bdReview: jasmine.createSpy('bdReview'),
            fileChanged: jasmine.createSpy('fileChanged'),
            validate: jasmine.createSpy('validate'),
    },
    $scope,
    $modal,
    sweetAlert,
    logger,
    $q,
    $controller;

    var initController = function (id) {
        var $stateParams = {
            id: id
        };
        quoteProgressGroupCtrl = $controller('quoteProgressDetailCtrl', {
            $scope: $scope,
            $stateParams: $stateParams,
            quoteService: quoteService,
            quoteCostService: quoteCostService,
            quoteWorkFlowHelper:quoteWorkFlowHelper,
            $modal: $modal,
            SweetAlert: sweetAlert
        });
    };
    beforeEach(module('app.quote.controllers', 'app.resource.helper'));

    beforeEach(inject(function ($injector) {
        $controller = $injector.get('$controller');
        $scope = $injector.get('$rootScope').$new();
        logger = $injector.get('logger');
        $q = $injector.get('$q');
        sweetAlert = $q.defer();
        $modal = $q.defer();
        quoteService = $q.defer();
        quoteCostService = $q.defer();
        sweetAlert.confirm= function(obj1,obj2) {
            return sweetAlert.promise;
        }
        quoteCostService.removeCosts = function () {
            return quoteCostService.promise;

        };
        quoteCostService.finalize = function () {
            return quoteCostService.promise;
        };

        quoteService.getQuote = function () {
            return quoteService.promise;
        };

        quoteService.cancelQuote = function () {
            return quoteService.promise;
        };
        $modal.open = function (object) {
            return {
                result: $modal.promise
            }
        };
        
    }));

    it('Initial params: quote=undefined, pageSize=5, currentpage=0,bulkActions = ["Delete", "Finalize"], costActions = ["Edit", "Delete", "Finalize"]', function () {
        initController();
        expect(quoteProgressGroupCtrl.quote).toEqual(undefined);
        expect(quoteProgressGroupCtrl.pageSize).toEqual(5);
        expect(quoteProgressGroupCtrl.currentPage).toEqual(0);
        expect(quoteProgressGroupCtrl.bulkActions).toEqual(['Delete', 'Finalize']);
        expect(quoteProgressGroupCtrl.costActions).toEqual(['Edit', 'Delete', 'Finalize']);
    });

    it('loadQuote should be called initially', function () {
        spyOn(quoteService, 'getQuote').and.callThrough();
        initController(1);
        expect(quoteService.getQuote).toHaveBeenCalledWith({ id: 1 });

    });

    it('quote equal with result.data when get quote successfully', function () {
        initController();
        var result = {
            data: {
                id: 1
            }
        };
        quoteService.resolve(result);
        $scope.$apply();
        expect(quoteProgressGroupCtrl.quote).toBe(result.data);
        expect(quoteProgressGroupCtrl.loading).toEqual(false);
    });

    it('error should be loggered when get quote  failed', function () {
        initController();
        spyOn(logger, 'serverError');
        quoteService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });

    it('when cancelquote is called, sweet alert confirm should display', function () {
        initController();
        spyOn(quoteProgressGroupCtrl, 'cancelQuote').and.callThrough();
        spyOn(sweetAlert, 'confirm').and.callThrough();
        quoteProgressGroupCtrl.cancelQuote();
        $scope.$apply();
        expect(sweetAlert.confirm).toHaveBeenCalledWith('Please Confirm', 'Cancel this quote ?');
    });

    it('when cancel quote and confirmed , cancel quote function should be called with quote Id', function () {
        initController();
        spyOn(quoteProgressGroupCtrl, 'cancelQuote').and.callThrough();
        spyOn(sweetAlert, 'confirm').and.callThrough();
        spyOn(quoteService, 'cancelQuote').and.callThrough();
        quoteProgressGroupCtrl.cancelQuote({Id:1});
        sweetAlert.resolve();
        $scope.$apply();
        expect(quoteService.cancelQuote).toHaveBeenCalledWith(1);
    });


    it('when cancel quote successfully, reload quote and logger success', function () {
        initController();
        spyOn(quoteProgressGroupCtrl, 'cancelQuote').and.callThrough();
        spyOn(logger, 'success');
        spyOn(sweetAlert, 'confirm').and.callThrough();
        spyOn(quoteService, 'cancelQuote').and.callThrough();
        spyOn(quoteService, 'getQuote').and.callThrough();
        quoteProgressGroupCtrl.cancelQuote({ Id: 1 });
        sweetAlert.resolve();
        quoteService.resolve({data: {} });
        $scope.$apply();
        expect(quoteService.getQuote).toHaveBeenCalled();
        expect(logger.success).toHaveBeenCalled();
    });


    it('when cancel quote failed, error should be logged', function () {
        initController();
        spyOn(quoteProgressGroupCtrl, 'cancelQuote').and.callThrough();
        spyOn(logger, 'serverError');
        spyOn(sweetAlert, 'confirm').and.callThrough();
        spyOn(quoteService, 'cancelQuote').and.callThrough();
        quoteProgressGroupCtrl.cancelQuote({ Id: 1 });
        sweetAlert.resolve();
        quoteService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });

//
//    it('BulkAction: if nothing select')

})