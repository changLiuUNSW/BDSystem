describe('quoteProgressCtrl: test spec', function() {
    var quoteProgressCtrl,
    logger,
    quoteService,
    $controller,
    $q,
    $scope;


    var initController = function () {
        quoteProgressCtrl = $controller('quoteProgressCtrl', {
            $scope:$scope,
            quoteService: quoteService
        });
    }


    beforeEach(module('app.quote.controllers', 'app.resource.helper'));
    beforeEach(inject(function($injector) {
        $controller = $injector.get('$controller');
        $scope = $injector.get('$rootScope').$new();
        logger = $injector.get('logger');
        $q = $injector.get('$q');
        quoteService = $q.defer();
        quoteService.getAllStatus= function() {
            return quoteService.promise;
        }
        initController();
    }));


    it('status list should be empty initially', function () {
        expect(quoteProgressCtrl.statusList).toEqual([]);
    });

    it('error should be loggered when loading status list failed', function() {
        spyOn(logger, 'serverError');
        quoteService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });

    it('status list should not be empty if loading successfully', function() {
        var result = {
            data:[1,2]
        }
        quoteService.resolve(result);
        $scope.$apply();
        expect(quoteProgressCtrl.statusList).toBe(result.data);
    });

})