app.provider("helper", function() {
    var mouseX = 0;
    var mouseY = 0;
    var isComplete = false;
    $(document).mousemove(function(event) {
        mouseX = event.pageX;
        mouseY = event.pageY;
    });
    this.$get = function() {
        return {
            getMousePosition: function() {
                return { mouseX: mouseX, mouseY: mouseY };
            },
            getGuid: function() {
                function _p8(s) {
                    var p = (Math.random().toString(16)+"000000000").substr(2,8);
                    return s ? "-" + p.substr(0,4) + "-" + p.substr(4,4) : p ;
                }
                return _p8() + _p8(true) + _p8(true) + _p8();
            },
            getBaseUrl: function() {
                return location.protocol + "//" + location.hostname
                    + (location.port && ":" + location.port);
            },
            pointerEventsSupport: function() {
                var element = document.createElement('x'),
                    documentElement = document.documentElement,
                    getComputedStyle = window.getComputedStyle,
                    supports;
                if(!('pointerEvents' in element.style)){
                    return false;
                }
                element.style.pointerEvents = 'auto';
                element.style.pointerEvents = 'x';
                documentElement.appendChild(element);
                supports = getComputedStyle &&
                    getComputedStyle(element, '').pointerEvents === 'auto';
                documentElement.removeChild(element);
                return !!supports;
            },
            isComplete: isComplete
        };
    }
});
