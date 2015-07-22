(function() {
    'use strict';

    angular.module('app.telesale.controllers')
       .controller('callsheetScriptController', controller);
    controller.$inject = ['$scope', 'apiService', '$modal', 'formatter', '$stateParams'];

    function controller($scope, apiService, $modal, formatter, $stateParms) {
        var self = this;

        if ($stateParms.Script) {
            self.data = $stateParms.Script;
            self.root = self.data.Root;
            self.current = self.data.Root;
        }

        if ($stateParms.ScriptActions) {
            self.defaultActions = $stateParms.ScriptActions;
        }

        self.contactName = null;
        self.actions = []; //total actions been selected from the scripts
        self.inputs = []; //input for the current script
        self.histories = []; //script history
        self.next = next; 
        self.inputs = getInputsFromScript;
        self.selectHistory = onHistoryClicked;
        self.selectActions = onActionClicked;
        self.formatter = formatter;

        $scope.$on('event:endingCall', function (e) {
            e.preventDefault();
            $scope.$emit('event:endingScript', getActiveActions());
        });

        function next(isTrue) {
            if (!self.current)
                return;

            var nextPage,
                history;

            nextPage = (isTrue) ? self.current.Right : self.current.Left;
            history = {
                id: self.histories.length,
                isTrue: isTrue,
                script: self.current,
                time: Date.now()
            };

            self.histories.push(history);
            self.current = nextPage;
            updateScriptQuestion(self.current.Value);

            if (!self.current.Value.End) {
                self.inputs = getInputsFromScript(self.current);
            }
            else {
                self.inputs = [];
            }
        };

        //this function captures the input from user ( mainly contact name )
        //and replace the name into the rest of the scripts if neccessary
        function updateScriptQuestion(script) {
            if (!script || !script.Question)
                return;

            if (!self.histories || self.histories.length <= 0)
                return;

            var latest = self.histories[self.histories.length - 1];

            if (latest.script &&
                latest.script.Value &&
                latest.script.Value.Actions) {
                latest.script.Value.Actions.forEach(function(action) {
                    //5 update contact name
                    //22 update tenant
                    if (action.Type === 5 || action.Type === 22) {
                        if (action.Firstname || action.Lastname)
                            self.contactName = '(' + action.Firstname + ' ' + action.Lastname + ')';
                        else
                            self.contactName = '(the person / no name input)';
                    };
                });
            };

            if (script.Question.search(/\@\w*/i) != -1)
                script.Question = script.Question.replace(/\@\w*/i, self.contactName);
            else if (script.Question.search(/\([\w+\s?\/]*\)/i) != -1)
                script.Question = script.Question.replace(/\([\w+\s?\/]*\)/i, self.contactName);
        }

        function getInputsFromScript(script) {
            var inputs = [];

            if (!script)
                return inputs;

            if (script.Value && script.Value.Actions) {
                script.Value.Actions.forEach(function(action) {
                    var props = Object.getOwnPropertyNames(action);
                    if (props && props.length > 0) {
                        var input = {
                            action: action,
                            props: []
                        };

                        inputs.push(input);
                        props.forEach(function (prop) {
                            if (prop === 'Description' ||
                                prop === 'Type' ||
                                prop === 'strike' ||
                                prop === 'TenantId' ||
                                prop === 'UpdateInMonth' ||
                                prop.indexOf('#') === 0 ||
                                prop.indexOf('$') === 0)
                                return;

                            input.props.push(prop);
                        });
                    }
                });
            }

            return inputs;
        }

        function onHistoryClicked(history) {
            if (!history)
                return;

            self.histories.splice(history.id, self.histories.length - history.id);
            self.current = history.script;
            self.inputs = getInputsFromScript(self.current);
        };

        function onActionClicked() {
            var modalOptions = {
                templateUrl: 'tpl/telesale/callsheet/modal.endCall.html',
                resolve: {
                    $actions: function() {
                        return self.defaultActions;
                    }
                },
                controller: 'modalEndCallController',
                controllerAs: 'endModal'
            };

            $modal.open(modalOptions);
        }

        //there are two places where we need to look for actions
        //1. those in the histories that has been selected as positive respoonse
        //2. those in the last page of the script because they are not put into the history
        function getActiveActions() {
            var actions = [];
            if (!self.histories || self.histories.length <= 0)
                return actions;

            self.histories.forEach(function(history) {
                if (!history.isTrue)
                    return;

                if (history.script &&
                    history.script.Value &&
                    history.script.Value.Actions) {
                    actions = actions.concat(history.script.Value.Actions);
                }
            });

            if (self.current &&
                self.current.Value &&
                self.current.Value.Actions &&
                self.current.Value.End) {
                actions = actions.concat(self.current.Value.Actions);
            }

            return actions;
        }

        function displayError(txt) {
            swal({
                type: 'error',
                title: txt,
                confirmButtonColor: '#f05050'
            });
        };
    };
})();