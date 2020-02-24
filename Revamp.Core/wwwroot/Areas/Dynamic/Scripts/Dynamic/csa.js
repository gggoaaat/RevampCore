var csa = (function (m) {

    var readyInterval = [];

    var readyModules = {
        templatesDownloaded: false
    }

    var commandList = [];

    var isReady = {
        getTemplates: false
    };

    function checkIfReady(m) {
        
        var thisInterval = setInterval(function () {
            var isItReady = true;

            for (thisObject in isReady) {
                if (typeof isReady[thisObject] == "boolean" && !isReady[thisObject]) {
                    isItReady = false;
                    break;
                }
            }

            if (isItReady) {
                for (var i = 0; i < readyInterval.length; i++) {
                    clearInterval(readyInterval[i]);
                }
                
                run(commandList);
            }
        }, 1000);

        readyInterval.push(thisInterval);

        return false;
    }

    function run(m) {
        if (commandList.length > 0) {
            for (var i = 0; i < commandList.length; i++) {
                if (commandList[i] != undefined && typeof commandList[i] == "function") {
                    commandList[i]();
                }
            }
        }
    }

    function go(m) {
        commandList.push(m);

        $(document).ready(function () {
            checkIfReady(m);
        });
    }

    function flipOn(m) {
        isReady = true;
    }

    return {
        flipOn: function (m) { flipOn(m) },
        ready: function (m) { go(m) }
    }
})();
