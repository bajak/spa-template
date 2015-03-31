app.config(["localizeProvider", function (localizeProvider) {
    localizeProvider.setDefaultLanguage("en-EN");
    localizeProvider.addLanguage("en-EN");
    localizeProvider.addLanguage("de-DE");
    localizeProvider.addDefaultUrl("en-EN", "./Content/Text/index-en-EN.lang");
    localizeProvider.addDefaultUrl("de-DE", "./Content/Text/index-de-DE.lang");
}]);

app.run(["localize", function(localize) {
        localize.initialize();
    }
]);
