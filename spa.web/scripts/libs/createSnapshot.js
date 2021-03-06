var page = require('webpage').create();
var system = require('system');

var lastReceived = new Date().getTime();
var requestCount = 0;
var responseCount = 0;
var requestIds = [];
var startTime = new Date().getTime();

page.onResourceReceived = function (response) {
    if (requestIds.indexOf(response.id) !== -1) {
        lastReceived = new Date().getTime();
        responseCount++;
        requestIds[requestIds.indexOf(response.id)] = null;
    }
};
page.onResourceRequested = function (request) {
    if (requestIds.indexOf(request.id) === -1) {
        requestIds.push(request.id);
        requestCount++;
    }
};

function checkLoaded() {
    return page.evaluate(function () {
        return document.all["complete"];
    }) != null;
}

page.open(system.args[1], function () { });

var checkComplete = function () {
    if ((new Date().getTime() - lastReceived > 300 
	&& requestCount === responseCount) 
	|| new Date().getTime() - startTime > 10000 
	|| checkLoaded()) {
        clearInterval(checkCompleteInterval);
        var result = page.content;
        console.log(result);
        phantom.exit();
    }
}
var checkCompleteInterval = setInterval(checkComplete, 300);
