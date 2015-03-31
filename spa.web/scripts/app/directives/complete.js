app.directive('complete', ["helper", function(helper) {
    return {
        restrict: 'A',
        replace: false,
        transclude: false,
        link: function(scope, element, attrs) {
            var to;
            var listener = scope.$watch(function() {
                clearTimeout(to);
                to = setTimeout(function () {
                    listener();
                    $(element).append("<div id='complete' style='hidden'><div/>");
                    helper.isComplete = true;
                }, 50);
            });
        }
    };
}]);
