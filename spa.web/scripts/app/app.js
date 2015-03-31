var app = angular.module("app", ["localization", "ngRoute", "ngSanitize", "ngAnimate", "ngCookies", "ngResource", "angularLocalStorage"]);

angular.element(document).ready(function() {
    var html = document.getElementsByTagName('html')[0];
    angular.bootstrap(html, ['app']);
});
