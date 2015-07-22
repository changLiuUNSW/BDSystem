(function () {
    'use strict';
    angular.module('app.resource.helper')
        .factory('utility', ['userInfo', '$filter', utility]);

    function utility(userInfo, $filter) {

        var user = userInfo;

        return {
            diff: findDiff,
            isInRole: isInRole
            }

        function isInRole(role) {
            if (!user.group || !user.userName)
                return false;

            var groups = user.group.split(',');

            var found = $filter('find')(groups, function(obj) {
                return obj.toLowerCase() == role.toLowerCase();
            });

            return found != undefined;
        }

        //find changed value from the model
        function findDiff(original, edited, keyNames) {

            var diff = {},
                arrayDiff = {};

            if (typeof original == "undefined" && typeof edited == "undefined")
                return diff;

            if (typeof original == "undefined" && typeof edited != "undefined")
                return edited;

            if (typeof original != "undefined" && typeof edited == "undefined")
                return diff;

            if (original == null && edited == null)
                return diff;

            if (original == null && edited != null) {
                return edited;
            }

            if (original != null && edited == null) {
                return diff;
            }

            if (original.constructor !== edited.constructor)
                return diff;
            
            if (original.constructor !== Array) {
                for (var name in original) {
                    if (name in edited) {
                        var property = original[name];
                        if (!property || property.constructor !== Array) {
                            if (property && typeof property == "object" && Object.keys(property).length > 0) {

                                var objectDiff = findDiff(property, edited[name], keyNames);
                                if (Object.keys(objectDiff).length > 0) {
                                    diff[name] = objectDiff;
                                    if (keyNames) {
                                        keyNames.forEach(function (id) {
                                            if (original.hasOwnProperty(id) && edited.hasOwnProperty(id))
                                                diff[id] = edited[id];
                                        });
                                    }
                                }
                            } else if (property !== edited[name]) {
                                diff[name] = edited[name];

                                if (keyNames) {
                                    keyNames.forEach(function (id) {
                                        if (original.hasOwnProperty(id) && edited.hasOwnProperty(id))
                                            diff[id] = edited[id];
                                    });
                                }
                            }
                        } else {
                            for (var i = 0; i < property.length; i++) {
                                var difference = findDiff(property[i], edited[name][i], keyNames);
                                if (Object.keys(difference).length > 0) {
                                    if (typeof diff[name] == "undefined")
                                        diff[name] = new Array();

                                    if (keyNames) {
                                        keyNames.forEach(function (id) {
                                            if (original.hasOwnProperty(id) && edited.hasOwnProperty(id))
                                                diff[id] = edited[id];
                                        });
                                    }

                                    diff[name].push(difference);
                                }
                            }
                        }
                    }
                }
            } else {
                var diffs = [];
                for (var idx = 0; idx < edited.length; idx++) {
                    arrayDiff = findDiff(original[idx], edited[idx], keyNames);
                    if (Object.keys(arrayDiff).length > 0) {
                        diffs.push(arrayDiff);
                    }
                }

                return diffs;
            }

            return diff;
        };
    }
})();