describe('quoteProgressGroupCtrl: test spec', function() {
    var quoteProgressGroupCtrl,
        searchService,
        $scope,
        logger,
        $q,
        $controller;

    var initController = function (status) {
        var $stateParams = {
            status: status
        };
        quoteProgressGroupCtrl = $controller('quoteProgressGroupCtrl', {
            $scope: $scope,
            searchService: searchService,
            $stateParams: $stateParams
    });
    };
    beforeEach(module('app.quote.controllers', 'app.resource.helper'));
    beforeEach(inject(function($injector) {
        $controller = $injector.get('$controller');
        $scope = $injector.get('$rootScope').$new();
        logger = $injector.get('logger');
        $q = $injector.get('$q');
        searchService = $q.defer();
        searchService.searchQuote = function() {
           return searchService.promise;
        };
      
    }));

    it('quote list should be empty initially', function () {
        initController();
        $scope.$apply();
        expect(quoteProgressGroupCtrl.quotes).toEqual([]);
    });

    it('quote list and paging total count should not be empty if loading successfully', function () {
        initController();
        var result = {
            data: {
                Total: 2,
                List: [1, 2]
            }
        };
        searchService.resolve(result);
        $scope.$apply();
        expect(quoteProgressGroupCtrl.quotes).toBe(result.data.List);
        expect(quoteProgressGroupCtrl.paging.totalItems).toBe(result.data.Total);
    });

    it('error should be loggered when loading quote list failed', function () {
        initController();
        spyOn(logger, 'serverError');
        searchService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });



    it('page size should be update and current page should be 1 when setListPageSize is called', function () {
        initController();
        spyOn(quoteProgressGroupCtrl, 'setListPageSize').and.callThrough();
        quoteProgressGroupCtrl.setListPageSize(3);
        $scope.$apply();
        expect(quoteProgressGroupCtrl.setListPageSize).toHaveBeenCalledWith(3);
        expect(quoteProgressGroupCtrl.paging.pageSize).toBe(3);
        expect(quoteProgressGroupCtrl.paging.currentPage).toEqual(1);
    });


    it('currentPage should be update when listPageChanges is called', function () {
        initController();
        spyOn(quoteProgressGroupCtrl, 'listPageChanges').and.callThrough();
        quoteProgressGroupCtrl.listPageChanges(3);
        $scope.$apply();
        expect(quoteProgressGroupCtrl.listPageChanges).toHaveBeenCalledWith(3);
        expect(quoteProgressGroupCtrl.paging.currentPage).toEqual(3);
    });
})