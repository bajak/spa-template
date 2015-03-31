app.controller("UserCtrl", ["$scope", "userService", function($scope, userService) {
    $scope.model = {};
    $scope.model.error = 0;
    var setAuthenticated = function(user, model, isAuthenticated) {
        model.error = 0;
        user.isAuthenticated = isAuthenticated;
    };
    var setErrorCode = function(status, data, model) {
        if (status == 401) {
            var responseCode = parseInt(data);
            if (responseCode != Number.NaN) {
                console.log(data);
                console.log(responseCode);
                model.error = responseCode;
            }
        }
    };
    $scope.model.create = function() {
        userService.create($scope.model.username, $scope.model.password,
            function() {
                setAuthenticated($scope.user, $scope.model, true);
            },
            function(data, status, headers, config) {
                setErrorCode(status, data, $scope.model);
            });
    };
    $scope.model.login = function() {
        userService.login($scope.model.username, $scope.model.password,
            function() {
                setAuthenticated($scope.user, $scope.model, true);
            },
            function(data, status, headers, config) {
                setErrorCode(status, data, $scope.model);
            });
    };
    $scope.model.logout = function() {
        userService.logout(
            function() {
                setAuthenticated($scope.user, $scope.model, false);
            }, function() {});
    };
}]);
