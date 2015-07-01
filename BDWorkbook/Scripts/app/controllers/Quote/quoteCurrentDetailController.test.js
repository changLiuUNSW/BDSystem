describe('quoteCurrentDetailCtrl: test spec', function () {
    var quoteCurrentDetailCtrl,
        $controller,
        typeLibrary,
        quoteService,
        $modal,
        $q,
        logger,
        $scope;

    var initController = function (id) {
        quoteCurrentDetailCtrl = $controller('quoteCurrentDetailCtrl', {
            $scope: $scope,
            quoteService: quoteService,
            $stateParams: {
                id: id
            },
            $modal: $modal
        });
    }
    beforeEach(module('app.quote.controllers', 'app.resource.helper'));

    beforeEach(inject(function($injector) {
        $scope = $injector.get('$rootScope').$new();
        logger = $injector.get('logger');
        $controller = $injector.get('$controller');
        $q = $injector.get('$q');
        typeLibrary = $injector.get('typeLibrary');
        $modal = $q.defer();
        quoteService = $q.defer();
        quoteService.download = jasmine.createSpy('open');

        quoteService.getQuote = function (requestObj) {
            return quoteService.promise;

        };
        quoteService.updateRate = function (id, rate) {
            return quoteService.promise;
        }

        $modal.open = function(object) {
            return {
                result: $modal.promise
            }
        };
    }));


    it('quoteService getQuote should be called with id from $stateParams initially', function () {
        spyOn(quoteService, 'getQuote').and.callThrough();
        initController(1);
        expect(quoteService.getQuote).toHaveBeenCalledWith({ id: 1 });
    });

    it('quote list equal with result.data when get quote successfully', function () {
        initController();
        var result = {
            data: {
               id:1
            }
        };
        quoteService.resolve(result);
        $scope.$apply();
        expect(quoteCurrentDetailCtrl.quote).toBe(result.data);
        expect(quoteCurrentDetailCtrl.loading).toEqual(false);
    });


    it('error should be loggered when get quote  failed', function () {
        initController();
        spyOn(logger, 'serverError');
        quoteService.reject();
        $scope.$apply();
        expect(logger.serverError).toHaveBeenCalled();
    });

    it('quoteservice download should be called with quote when downloadQuote triggered', function() {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'downloadQuote').and.callThrough();
        var quote= {
            Id:1
        }
        quoteCurrentDetailCtrl.downloadQuote(quote);
        $scope.$apply();
        expect(quoteCurrentDetailCtrl.downloadQuote).toHaveBeenCalledWith(quote);
        expect(quoteService.download).toHaveBeenCalledWith(1);
    });

    it('contact modal should be open with copy of quote entity when contactModal is called', function () {
        initController();
        quoteCurrentDetailCtrl.quote = {
            Id:1
        };
        spyOn(quoteCurrentDetailCtrl, 'contactModal').and.callThrough();
        spyOn($modal, 'open').and.callThrough();
        quoteCurrentDetailCtrl.contactModal();
        var callArg = $modal.open.calls.argsFor(0)[0];
        expect($modal.open).toHaveBeenCalled();
        expect(callArg.resolve.quote()).toEqual(quoteCurrentDetailCtrl.quote);
    });

    it('when contact modal close  reload quote', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'contactModal').and.callThrough();
        spyOn(quoteService, 'getQuote').and.callThrough();
        spyOn($modal, 'open').and.callThrough();
        quoteCurrentDetailCtrl.contactModal();
        expect($modal.open).toHaveBeenCalled();
        $modal.resolve();
        $scope.$apply();
        expect(quoteService.getQuote).toHaveBeenCalled();
    });
   



    it('dead modal should be open with copy of quote entity when deadModal is called', function () {
        initController();
        quoteCurrentDetailCtrl.quote = {
            Id: 1
        };
        spyOn(quoteCurrentDetailCtrl, 'deadModal').and.callThrough();
        spyOn($modal, 'open').and.callThrough();
        quoteCurrentDetailCtrl.deadModal();
        var callArg = $modal.open.calls.argsFor(0)[0];
        expect($modal.open).toHaveBeenCalled();
        expect(callArg.resolve.quote()).toEqual(quoteCurrentDetailCtrl.quote);
    });

    it('when dead modal close  reload quote', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'deadModal').and.callThrough();
        spyOn(quoteService, 'getQuote').and.callThrough();
        spyOn($modal, 'open').and.callThrough();
        quoteCurrentDetailCtrl.deadModal();
        expect($modal.open).toHaveBeenCalled();
        $modal.resolve();
        $scope.$apply();
        expect(quoteService.getQuote).toHaveBeenCalled();
    });

    it('adjust modal should be open with copy of quote entity when adjustModal is called', function () {
        initController();
        quoteCurrentDetailCtrl.quote = {
            Id: 1
        };
        spyOn(quoteCurrentDetailCtrl, 'adjustModal').and.callThrough();
        spyOn($modal, 'open').and.callThrough();
        quoteCurrentDetailCtrl.adjustModal();
        var callArg = $modal.open.calls.argsFor(0)[0];
        expect($modal.open).toHaveBeenCalled();
        expect(callArg.resolve.quote()).toEqual(quoteCurrentDetailCtrl.quote);
    });

    it('when adjust modal close  reload quote', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'adjustModal').and.callThrough();
        spyOn(quoteService, 'getQuote').and.callThrough();
        spyOn($modal, 'open').and.callThrough();
        quoteCurrentDetailCtrl.adjustModal();
        expect($modal.open).toHaveBeenCalled();
        $modal.resolve();
        $scope.$apply();
        expect(quoteService.getQuote).toHaveBeenCalled();
    });

    it('Dead Status: when status is dead return true', function() {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'getDeadStatus').and.callThrough();
        quoteCurrentDetailCtrl.quote= {
            Status: {
                Id:quoteCurrentDetailCtrl.quoteStatusType.Dead
            }
        }
        quoteCurrentDetailCtrl.getDeadStatus();
        expect(quoteCurrentDetailCtrl.getDeadStatus).toHaveBeenCalled();
        expect(quoteCurrentDetailCtrl.getDeadStatus()).toEqual(true);
    });

    it('Dead Status: when status is not dead but if any one of questions type is no dead return false', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'getDeadStatus').and.callThrough();
        quoteCurrentDetailCtrl.quote = {
            Status: {
                Id: 1000
            },
            QuestionResults: [
                {
                    Question: {
                        Type: quoteCurrentDetailCtrl.questionType.noDead
                    }
                }
            ]
        }
        quoteCurrentDetailCtrl.getDeadStatus();
        expect(quoteCurrentDetailCtrl.getDeadStatus).toHaveBeenCalled();
        expect(quoteCurrentDetailCtrl.getDeadStatus()).toEqual(false);
    });

    it('Dead Status: when status is not dead and no questions type is no dead return null', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'getDeadStatus').and.callThrough();
        quoteCurrentDetailCtrl.quote = {
            Status: {
                Id: 1000
            },
            QuestionResults: [
                {
                    Question: {
                        Type: 3123
                    }
                }
            ]
        }
        quoteCurrentDetailCtrl.getDeadStatus();
        expect(quoteCurrentDetailCtrl.getDeadStatus).toHaveBeenCalled();
        expect(quoteCurrentDetailCtrl.getDeadStatus()).toEqual(null);
    });

    it('Adjust Status: when quote status is WPIssues  return true', function() {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'getAdjustStatus').and.callThrough();
        quoteCurrentDetailCtrl.quote = {
            Status: {
                Id: quoteCurrentDetailCtrl.quoteStatusType.WPIssues
            }
        }
        quoteCurrentDetailCtrl.getAdjustStatus();
        expect(quoteCurrentDetailCtrl.getAdjustStatus).toHaveBeenCalled();
        expect(quoteCurrentDetailCtrl.getAdjustStatus()).toEqual(true);
    });


    it('Adjust Status: when status is not WPIssues but if any one of questions type is noAdjust return false', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'getAdjustStatus').and.callThrough();
        quoteCurrentDetailCtrl.quote = {
            Status: {
                Id: 1000
            },
            QuestionResults: [
                {
                    Question: {
                        Type: quoteCurrentDetailCtrl.questionType.noAdjust
                    }
                }
            ]
        }
        quoteCurrentDetailCtrl.getAdjustStatus();
        expect(quoteCurrentDetailCtrl.getAdjustStatus).toHaveBeenCalled();
        expect(quoteCurrentDetailCtrl.getAdjustStatus()).toEqual(false);
    });

    it('Adjust Status: when status is not WPIssues and no questions type is noAdjust return null', function () {
        initController();
        spyOn(quoteCurrentDetailCtrl, 'getAdjustStatus').and.callThrough();
        quoteCurrentDetailCtrl.quote = {
            Status: {
                Id: 1000
            },
            QuestionResults: [
                {
                    Question: {
                        Type: 3123
                    }
                }
            ]
        }
        quoteCurrentDetailCtrl.getAdjustStatus();
        expect(quoteCurrentDetailCtrl.getAdjustStatus).toHaveBeenCalled();
        expect(quoteCurrentDetailCtrl.getAdjustStatus()).toEqual(null);
    });

    it('when update rating succeed, rating Editing flag should be false', function() {
        initController(1);
        spyOn(quoteCurrentDetailCtrl, 'updateRating').and.callThrough();
        spyOn(quoteService, 'updateRate').and.callThrough();
        quoteCurrentDetailCtrl.updateRating(50);
        quoteService.resolve({
            data:{}
        });
        $scope.$apply();
        expect(quoteCurrentDetailCtrl.updateRating).toHaveBeenCalledWith(50);
        expect(quoteService.updateRate).toHaveBeenCalledWith(1, 50);
        expect(quoteCurrentDetailCtrl.rateEditing).toEqual(false);
    });

    it('when update rating failed, error will be logged', function () {
        initController(1);
        spyOn(quoteCurrentDetailCtrl, 'updateRating').and.callThrough();
        spyOn(quoteService, 'updateRate').and.callThrough();
        spyOn(logger, 'serverError');
        quoteCurrentDetailCtrl.updateRating(50);
        quoteService.reject();
        $scope.$apply();
        expect(quoteCurrentDetailCtrl.updateRating).toHaveBeenCalledWith(50);
        expect(quoteService.updateRate).toHaveBeenCalledWith(1, 50);
        expect(logger.serverError).toHaveBeenCalled();
    });
})