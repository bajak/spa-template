app.controller("IndexCtrl", ["$scope", "$rootScope", "localize", "alerts", function($scope, $rootScope, localize, alerts) {
    $scope.changeLanguage = localize.changeLanguage;
    $scope.index = {};
    $scope.index = $rootScope.index;

    $scope.user = {};
    $scope.user.isAuthenticated = false;
    $scope.authenticated = function() {
        if ($scope.user.isAuthenticated)
            return "authenticated";
        else
            return "not-authenticated";
    };

    $scope.alerts = {};
    $scope.alerts.items = alerts.get();
    $scope.alerts.close = alerts.close;
}]);
