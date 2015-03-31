app.directive("alert_", function() {
    return {
        restrict: "A",
        templateUrl: "./Templates/alert.html",
        transclude: true,
        replace: true,
        scope: {
            model: "=",
            close: "&"
        }
    }
});
