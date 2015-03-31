app.config(["$routeProvider", "$locationProvider", "$provide", function ($routeProvider, $locationProvider, $provide) {

    $routeProvider.when("/Start",
        {
            templateUrl: "./Views/start.html",
            controller: "StartCtrl",
            localizeUrl: {
                "en-EN": "./Content/Text/start-en-EN.lang",
                "de-DE": "./Content/Text/start-de-DE.lang"
            }
        }).when("/User",
        {
            templateUrl: "./Views/user.html",
            controller: "UserCtrl"
//            localizeUrl: {
//                "en-EN": "./Content/Text/user-en-EN.lang",
//                "de-DE": "./Content/Text/user-de-DE.lang"
//            }
        }).when("/About",
        {
            templateUrl: "./Views/about.html",
            controller: "AboutCtrl",
            localizeUrl: {
                "en-EN": "./Content/Text/about-en-EN.lang",
                "de-DE": "./Content/Text/about-de-DE.lang"
            }
        }).
        otherwise(
        {
            redirectTo: "/Start"
        });
        $provide.decorator('$sniffer', ["$delegate", function($delegate) {
            if(!window.history && !window.history.pushState)
                $delegate.history = false;
            return $delegate;
        }]);
        $locationProvider.hashPrefix('!');
        $locationProvider.html5Mode(true);
}]);
