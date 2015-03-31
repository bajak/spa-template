app.provider('userService', function() {
    this.$get = ["$http", "helper", function($http, helper) {
        return {
            create: function(username, password, success, error) {
                return $http({
                    url:  helper.getBaseUrl() + '/Api/User/Create',
                    method: "POST",
                    data: { username: username, password: password }
                }).success(success).error(error);
            },
            login: function(username, password, success, error) {
                return $http({
                    url:  helper.getBaseUrl() + '/Api/User/Login',
                    method: "POST",
                    data: { username: username, password: password }
                }).success(success).error(error);
            },
            check: function(success, error) {
                return $http({
                    url:  helper.getBaseUrl() + '/Api/User/Check',
                    method: "GET"
                }).success(success).error(error);
            },
            logout: function(success, error) {
                return $http({
                    url:  helper.getBaseUrl() + '/Api/User/Logout',
                    method: "POST"
                }).success(success).error(error);
            }
        }
    }];
});
