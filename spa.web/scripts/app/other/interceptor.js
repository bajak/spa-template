app.factory('httpInterceptor', ["$q", "$rootScope", "alerts", function ($q, $rootScope, alerts) {
    return {
        // On request success
        request: function (config) {
            return config || $q.when(config);
        },
        // On request failure
        requestError: function (rejection) {
            alerts.add("error", "Request Failure: " + rejection.status + "<br>" + rejection.data, 5000);
            return $q.reject(rejection);
        },
        // On response success
        response: function (response) {
//            alerts.add("Success", "Response Succeed !");
            return response || $q.when(response);
        },
        // On response failure
        responseError: function (rejection) {
            if (rejection.status != 0) {
                var errorMessage = rejection.data;
                if (rejection.data.ExceptionMessage)
                    errorMessage = rejection.data.ExceptionMessage;
                alerts.add("error", "Response Failure: " + rejection.status + "<br>" + errorMessage, 5000);
            }
            return $q.reject(rejection);
        }
    };
}]);

