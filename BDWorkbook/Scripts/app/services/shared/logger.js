(function() {
    'use strict';
    angular.module('app.resource.helper')
        .factory('logger', loggerService);
    loggerService.$inject = ['$log'];

    function loggerService($log) {

        // This logger wraps the toastr logger and also logs to console using ng $log
        // toastr.js is library by John Papa that shows messages in pop up toast.
        // https://github.com/CodeSeven/toastr

        toastr.options.timeOut = 2500; // 2 second toast timeout
        toastr.options.positionClass = 'toast-top-full-width';

        var logger = {
            serverError: serverError,
            error: error,
            info: info,
            log: log, // straight to console; bypass toast
            success: success,
            warning: warning
        };

        return logger;

        function error(message, title) {
            toastr.error(message, title);
            $log.error("Error: " + message);
        }

        function info(message, title) {
            toastr.info(message, title);
            $log.info("Info: " + message);
        }

        function log(message) {
            $log.log(message);
        }

        function success(message, title) {
            toastr.success(message, title);
            $log.info("Success: " + message);
        }

        function serverError(errorMsg) {
            if (errorMsg.data) {

                var stateError = getStateError(errorMsg.data.ModelState);
                if (stateError) {
                    toastr.error(stateError);
                    $log.error(stateError);
                    return;
                }

                var exceptionMsg = errorMsg.data.ExceptionMessage || '';
                var message = errorMsg.data.Message + '<br>' + exceptionMsg;
                toastr.error(message, "Server Error");
                $log.error("Server Error: " + message);
            } else if (errorMsg.status && errorMsg.status === 404) {
                toastr.error("Not found", "Server Error");
                $log.error("Server Error: Not found");
            } else {
                console.log(errorMsg);
                toastr.error("Unexpected error", "Server Error");
                $log.error("Server Error: Unexpected error");
            }
        }

        function getStateError(state) {
            if (!state)
                return undefined;

            var names = Object.getOwnPropertyNames(state);
            for (var i = 0; i < names.length; i++) {
                var propName = names[i];
                if (state[propName] && state[propName].length > 0) {
                    return state[propName][0];
                }
            }

            return undefined;
        }

        function warning(message, title) {
            toastr.warning(message, title);
            $log.warn("Warning: " + message);
        }
    }
})();