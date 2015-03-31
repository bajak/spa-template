app.factory("alerts", ["$timeout", function($timeout) {
    var alerts = [];
    return {
        get: function() {
            return alerts;
        },
        add: function(type, msg, time) {
            var cIndex = alerts.length;
            $timeout(function() {
                alerts.splice(cIndex);
            }, time !== undefined ? time : 6500);
            alerts.push({type: type, msg: msg});
        },
        close: function(index) {
            alerts.splice(index, 1);
        }
    }
}]);
