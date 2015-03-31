app.config(["$httpProvider", function ($httpProvider) {
    $httpProvider.interceptors.push('httpInterceptor');
    delete $httpProvider.defaults.headers.common["X-Requested-With"];
}]);
