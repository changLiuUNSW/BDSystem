describe('quoteCurrentListCtrl: test spec', function() {
    var quoteCurrentListCtrl,
        $controller,
        logger,
        searchService,
        quoteService,
        typeLibrary,
        $q,
        $scope;


    var initController= function() {
        quoteCurrentListCtrl = $controller('quoteCurrentListCtrl', {
            $scope: $scope,
            searchService: searchService,
            quoteService:quoteService
    });
    }
    beforeEach(module('app.quote.controllers', 'app.resource.helper'));
    beforeEach(inject(function($injector) {
        $controller = $injector.get('$controller');
        $scope = $injector.get('$rootScope').$new();
        logger = $injector.get('logger');
        typeLibrary = $injector.get('typeLibrary');
        $q = $injector.get('$q');
        quoteService = $q.defer();
        searchService = $q.defer();
        quoteService.getOverDueList= function() {
            return quoteService.promise;
        }
        searchService.searchCurrentQuote = function (requestObj) {
            return searchService.promise;

        };
        searchService.currentQuoteConfig = {sortableColumns:[],overdueList:[]};
    }));

    it('bizTypesList should be equal to typeLibrary.businessType initially', function () {
        initController();
        expect(quoteCurrentListCtrl.bizTypesList).toBe(typeLibrary.businessType);
    });

    it('overdueList should be equal to searchService.currentQuoteConfig.overdueList initially', function () {
        initController();
        expect(quoteCurrentListCtrl.overdueList).toBe(undefined);
    });

    it('columnsList should be equal to searchService.currentQuoteConfig.sortableColumns initially', function () {
        initController();
        expect(quoteCurrentListCtrl.columnsList).toBe(searchService.currentQuoteConfig.sortableColumns);
    });

    it('page size should be update and current page should be 1 when setListPageSize is called', function () {
        initController();
        spyOn(quoteCurrentListCtrl, 'setListPageSize').and.callThrough();
        quoteCurrentListCtrl.setListPageSize(3);
        $scope.$apply();
        expect(quoteCurrentListCtrl.setListPageSize).toHaveBeenCalledWith(3);
        expect(quoteCurrentListCtrl.paging.pageSize).toBe(3);
        expect(quoteCurrentListCtrl.paging.currentPage).toEqual(1);
    });

    it('currentPage should be update when listPageChanges is called', function () {
        initController();
        spyOn(quoteCurrentListCtrl, 'listPageChanges').and.callThrough();
        spyOn(searchService, 'searchCurrentQuote').and.callThrough();
        quoteCurrentListCtrl.listPageChanges(3);
        $scope.$apply();
        expect(quoteCurrentListCtrl.listPageChanges).toHaveBeenCalledWith(3);
        expect(searchService.searchCurrentQuote).toHaveBeenCalled();
        expect(quoteCurrentListCtrl.paging.currentPage).toEqual(3);
    });

    it('filter.order should be update when orderClick is called', function () {
        initController();
        spyOn(quoteCurrentListCtrl, 'orderClick').and.callThrough();
        spyOn(searchService, 'searchCurrentQuote').and.callThrough();
        quoteCurrentListCtrl.orderClick('desc');
        $scope.$apply();
        expect(quoteCurrentListCtrl.orderClick).toHaveBeenCalledWith('desc');
        expect(searchService.searchCurrentQuote).toHaveBeenCalled();
        expect(quoteCurrentListCtrl.filter.order).toEqual('desc');
    });

    it('paging.totalItems and quote list should be updated when search quote successfully', function() {
        initController();
        var result = {
            data: {
                Total: 2,
                List: [1, 2]
            }
        };
        searchService.resolve(result);
        $scope.$apply();
        expect(quoteCurrentListCtrl.quotes).toBe(result.data.List);
        expect(quoteCurrentListCtrl.paging.totalItems).toBe(result.data.Total);
    });

    it('error should be loggered when loading quote list failed', function () {
        initController();
        spyOn(logger, 'serverError');
        searchService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });

    it('error should be loggered when loading overdue list failed', function () {
        initController();
        spyOn(logger, 'serverError');
        quoteService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });

    it('when overdueClcik is called, filter.overdue value should be same with the parameter', function() {
        initController();
        spyOn(quoteCurrentListCtrl, 'overdueClick').and.callThrough();
        quoteCurrentListCtrl.overdueClick('test');
        expect(quoteCurrentListCtrl.filter.overdue).toEqual('test');
    });

    it('when loading overdue list success,vm.overduelist should be equal with result.data', function () {
        initController();
        quoteService.resolve({
            data: []
        });
        $scope.$apply();
        expect(quoteCurrentListCtrl.overdueList).toEqual([]);
    });



    it('searchCurrentQuote should be called initially', function () {
        spyOn(searchService, 'searchCurrentQuote').and.callThrough();
        initController();
        expect(searchService.searchCurrentQuote).toHaveBeenCalled();
    });


    it('when filter.overdue=ContactCheckOverDue, requestobj ContactCheckOverDue =true', function () {
        initController();
        spyOn(quoteCurrentListCtrl, 'orderClick').and.callThrough();
        spyOn(searchService, 'searchCurrentQuote').and.callThrough();
        quoteCurrentListCtrl.filter.overdue = 'ContactCheckOverDue';
        quoteCurrentListCtrl.orderClick('desc');
        $scope.$apply();
        expect(quoteCurrentListCtrl.orderClick).toHaveBeenCalledWith('desc');
        expect(quoteCurrentListCtrl.filter.order).toEqual('desc');
        var callArg = searchService.searchCurrentQuote.calls.argsFor(0)[0];
        expect(callArg.ContactCheckOverDue).toEqual(true);
    });
    it('when filter.overdue=DeadCheckOverDue, requestobj DeadCheckOverDue =true', function () {
        initController();
        spyOn(quoteCurrentListCtrl, 'orderClick').and.callThrough();
        spyOn(searchService, 'searchCurrentQuote').and.callThrough();
        quoteCurrentListCtrl.filter.overdue = 'DeadCheckOverDue';
        quoteCurrentListCtrl.orderClick('desc');
        $scope.$apply();
        expect(quoteCurrentListCtrl.orderClick).toHaveBeenCalledWith('desc');
        expect(quoteCurrentListCtrl.filter.order).toEqual('desc');
        var callArg = searchService.searchCurrentQuote.calls.argsFor(0)[0];
        expect(callArg.DeadCheckOverDue).toEqual(true);
    });

    it('when filter.overdue=AjustCheckOverDue, requestobj AjustCheckOverDue =true', function () {
        initController();
        spyOn(quoteCurrentListCtrl, 'orderClick').and.callThrough();
        spyOn(searchService, 'searchCurrentQuote').and.callThrough();
        quoteCurrentListCtrl.filter.overdue = 'AjustCheckOverDue';
        quoteCurrentListCtrl.orderClick('desc');
        $scope.$apply();
        expect(quoteCurrentListCtrl.orderClick).toHaveBeenCalledWith('desc');
        expect(quoteCurrentListCtrl.filter.order).toEqual('desc');
        var callArg = searchService.searchCurrentQuote.calls.argsFor(0)[0];
        expect(callArg.AjustCheckOverDue).toEqual(true);
    });

})