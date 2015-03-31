"use strict";

var app = angular.module("localization", ["ngCookies"]);

app.provider("localize", function() {
    var viewCache = {};
    var languages = {};
    var defaultLang = {};
    var defaultUrl = {};
    var conditions = [];
    var cLanguage = "";
    this.addLanguage = function(language) {
        languages[language.toLowerCase()] = true;
    };
    this.setDefaultLanguage = function(language) {
        defaultLang = language.toLowerCase();
    };
    this.addDefaultUrl = function(language, url) {
        defaultUrl[language] = url;
    };
    this.addCondition = function(language, func) {
        conditions.push({language: language.toLowerCase(), func: func});
    };
    this.$get = ["$rootScope", "$cookies", "$location", "$window", "$route", function($rootScope, $cookies, $location, $window, $route) {
        var mapAttrProperties = function(template, propertyKey, propertyValue) {
            var attrRegEx = /\/\[(.+?)\/\]/g;
            var attrKeys = propertyKey.match(attrRegEx);
            var elementKey = propertyKey.replace(attrRegEx, "");
            if (attrKeys != null) {
                for (var i = 0; i < attrKeys.length; i++) {
                    attrKeys[i] = attrKeys[i].replace(new RegExp("/\\[", "g"), "").
                        replace(new RegExp("/\\]", "g"), "");
                    template.find(elementKey).attr(attrKeys[i], propertyValue);
                }
                return true;
            }
            return false;
        };
        var mapDataProperties = function(propertyKey, propertyValue) {
            var index = propertyKey.indexOf("$");
            if (index !== -1) {
                var funcTree = propertyKey.substr(index + 1)
                    .replace(" ", "")
                    .split(".");
                var lastObject = $rootScope;
                for (var i = 0; i < funcTree.length; i++) {
                    if (i == funcTree.length - 1)
                        lastObject[funcTree[i]] = propertyValue;
                    else if (lastObject[funcTree[i]] === undefined)
                        lastObject[funcTree[i]] = {};
                    lastObject = lastObject[funcTree[i]];
                }
                return true;
            }
            return false;
        };
        var isCrawler = function() {
            var userAgent = navigator.userAgent;
            return userAgent.indexOf("PhantomJS") > -1;
        };
        var readLangFile =  function(content) {
            var keyValuePair = {};
            var indexOfUnescaped = function(text, match, start) {
                var index = text.indexOf(match, start);
                if (index < 1)
                    return index;
                if (content.substring(index - 1, index) == "\\")
                    return indexOfUnescaped(content.substring(index + 1) , match, 0);
                else
                    return index;
            };
            var subMatch = function(content, match) {
                var indexStart = indexOfUnescaped(content, match, 0);
                var indexEnd = indexOfUnescaped(content, match, indexStart + 1);
                var value = content.substring(indexStart + 1, indexEnd);
                return { value: value, indexStart: indexStart, indexEnd: indexEnd };
            };
            var isNextChar = function(content, match) {
                return content.trim().indexOf(match) == 0;
            };
            while (true) {
                var keySub = subMatch(content, "\"");
                content = content.substring(keySub.indexEnd + 1);
                if (!isNextChar(content, ":")) {
                    return false;
                }
                var valueSub = subMatch(content, "\"");
                content = content.substring(valueSub.indexEnd + 1);
                keyValuePair[keySub.value] = valueSub.value;
                if (!isNextChar(content, ","))
                    break;
            }
            return keyValuePair;
        };
        var loadView = function(url, controller, template, cache, isDocument) {
            if (isDocument)
                cache = false;
            if (cache && viewCache[controller] !== undefined)
                return viewCache[controller];
            viewCache[controller] = template;
            var xhr = $.ajax( { url: url, type: "GET", cache: false, async: false });
            if (xhr.status != 200)
                return viewCache[controller];
            var jTemplate;
            if (isDocument) {
                jTemplate = $(viewCache[controller]);
            }
            else
                jTemplate = $("<div></div>").append($(viewCache[controller]));
            var response = xhr.responseText;
            if (!response) {
                console.error("File not found: \"" + url + "\"");
                return;
            }
            var data = readLangFile(response);
            if (!data) {
                console.error("File is invalid: \"" + url + "\"");
            }
            for (var propertyKey in data) {
                if (!data.hasOwnProperty(propertyKey))
                    continue;
                var propertyValue = data[propertyKey];
                if (!mapDataProperties(propertyKey, propertyValue)) {
                    if (!mapAttrProperties(jTemplate, propertyKey, propertyValue))
                        jTemplate.find(propertyKey).html(propertyValue);
                }
            }
            if (!isDocument)
                viewCache[controller] = jTemplate.html();
            return viewCache[controller]
        };
        var localizeView = function(url, language, controller, template, cache, isDocument) {
            for (var key in url) {
                if (!url.hasOwnProperty(key))
                    continue;
                if (language.toLowerCase() != key.toLocaleLowerCase())
                    continue;
                return loadView(url[key], controller, template, cache, isDocument);
            }
            return template;
        };
        var localizeTemplate = function(template) {
            var url = $(template).attr("localize-url");
            if (url === undefined)
                return template;
            return localizeView(JSON.parse(url), cLanguage, {}, template, false, false);
        };
        var isSupportedLanguage = function(language) {
            var isSupported = false;
            for (var key in languages) {
                if (!languages.hasOwnProperty(key))
                    continue;
                if (key != language.toLowerCase())
                    continue;
                isSupported = true;
                break;
            }
            return isSupported;
        };
        var execConditions = function(language) {
            for (var i = 0; i < conditions.length; i++) {
                if (conditions[i].language.toLocaleLowerCase() != language.toLowerCase())
                    continue;
                conditions[i].func();
            }
        };
        var changeLanguage = function(language, init) {
            if (language.length <= 3)
                language +=  "-" + language;
            if (!isSupportedLanguage(language))
                language = defaultLang;
            cLanguage = language;
            if (!init)
                $location.search("lang", language);
            $cookies.language = language;
            $("html").attr("lang", language.split("-")[1]);
            localizeView(defaultUrl, language, "document", $(document), false, true);
            execConditions(language);
        };
        var initLanguage = function() {
            var lang = $location.search()["lang"];
            if (lang === undefined)
                lang = $cookies.language;
            if (lang === undefined)
                lang = $window.navigator.userLanguage
                    || $window.navigator.language;
            changeLanguage(lang, true);
        };
        var insertHrefLang = function(url) {
            if (Object.keys(languages).length <= 1)
                return;
            var paramIndex = url.indexOf("lang=");
            if (paramIndex > 0) {
                var paramEnd = url.indexOf("&", paramIndex);
                if (paramEnd > 0)
                    url = url.substr(0, paramIndex) + url.substr(paramEnd);
                else
                    url = url.substr(0, paramIndex - 1);
            }
            var head = $("head");
            head.find(".localize-hreflang").remove();
            head.append("<link rel='alternate' hreflang='x-default' href='" + url + "' class='localize-hreflang' />");
            for (var propertyKey in languages) {
                if (!languages.hasOwnProperty(propertyKey))
                    continue;
                head.append("<link rel='alternate' hreflang='" + propertyKey +
                    "' href='" + url + (url.indexOf("?") > 0 ? "&" : "?") + "lang=" + propertyKey + "' class='localize-hreflang' />");
            }
        };
        var initBinding = function() {
            $rootScope.$on("$routeChangeSuccess", function(event, current, previous) {
                insertHrefLang($location.absUrl());
                if (current.localizeUrl === undefined)
                    return;
                current.locals.$template = localizeView(
                    current.localizeUrl,
                    cLanguage,
                    current.controller,
                    current.locals.$template,
                    false, false);
            });
            $rootScope.$watch(function(){ return $location.search() }, function(search){
                    if (search["lang"] === undefined)
                        return;
                    if (cLanguage.toLocaleLowerCase() != search["lang"].toLocaleLowerCase())
                        changeLanguage(search["lang"].toLocaleLowerCase());
                    $route.reload();
                }
            );
        };
        var getCurrentLanguage = function() {
            return cLanguage;
        };
        var initialize = function() {
            initBinding();
            initLanguage();
        };
        return {
            initialize : initialize,
            changeLanguage: changeLanguage,
            localizeTemplate: localizeTemplate,
            getCurrentLanguage: getCurrentLanguage
        };
    }];
});
