commonDynamic.functions.tools = (function () {
    return {
        objectifyForm: function (formArray) {//serialize data function

            var returnArray = {};
            for (var i = 0; i < formArray.length; i++) {
                returnArray[formArray[i]['name']] = formArray[i]['value'];
            }
            return returnArray;
        },
        stringFormat: function (str, col) {
            col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

            return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
                if (m == "{{") { return "{"; }
                if (m == "}}") { return "}"; }
                return col[n];
            });
        },
        firstToUpper: function (str) {
            str = str.toLowerCase().replace(/\b[a-z]/g, function (letter) {
                return letter.toUpperCase();
            });

            return str;
        },
        safeDateOnlyFormat: function (dt) {
            var retVal = null;

            if (dt != null && dt != '') {
                retVal = moment(dt).format('MM/DD/YYYY');
            }

            return retVal;
        },
        calculateDate: function () {
            var oneDay = 24 * 60 * 60 * 1000;

            var _start = function (endDate, numDays) {
                var startDate = new Date(endDate);
                startDate.setDate(startDate.getDate() - parseInt(numDays));

                var dd = startDate.getDate();
                var mm = startDate.getMonth() + 1;
                var y = startDate.getFullYear();

                return mm + '/' + dd + '/' + y;
            }

            var _end = function (startDate, numDays) {
                var endDate = new Date(startDate);
                endDate.setDate(endDate.getDate() + parseInt(numDays));

                var dd = endDate.getDate();
                var mm = endDate.getMonth() + 1;
                var y = endDate.getFullYear();

                return mm + '/' + dd + '/' + y;
            }

            var _span = function (startDate, endDate) {
                if ($.type(startDate) == 'string') {
                    startDate = new Date(startDate);
                }
                if ($.type(endDate) == 'string') {
                    endDate = new Date(endDate);
                }

                return Math.round(Math.abs((startDate.getTime() - endDate.getTime()) / (oneDay)));
            }

            return {
                startDate: function (endDate, numDays) {
                    return _start(endDate, numDays);
                },
                endDate: function (startDate, numDays) {
                    return _end(startDate, numDays);
                },
                span: function (startDate, endDate) {
                    return _span(startDate, endDate);
                }
            };
        },
        numberCheck: function () {

        },
        isNotNullEmptyOrUndefined: function (thisObject) {
            if (thisObject != undefined && thisObject != '') {
                return true;
            }
            else {
                return false;
            }
        },
        getSetLocalStorage: function (thisModel) {
            var _case = thisModel.GetorSet.toLowerCase() != undefined ? thisModel.GetorSet.toLowerCase() : '';

            switch (_case) {
                default:
                case 'get':
                    return localStorage.getItem(thisModel.property);
                case 'set':
                    localStorage.setItem(thisModel.property, thisModel.value);
                    break;
            }

        },
        getUrlVars: function () {
            var vars = {};
            var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi,
                function (m, key, value) {
                    vars[key] = value;
                });
            return vars;
        },
        getUrlParam: function (parameter, defaultvalue) {
            var urlparameter = defaultvalue;
            if (window.location.href.indexOf(parameter) > -1) {
                urlparameter = commonDynamic.functions.tools.getUrlVars()[parameter];
            }
            if (typeof urlparameter == "string" ) {
                if (urlparameter.toLowerCase() == 'true') {
                    return true;
                }
                if (urlparameter.toLowerCase() == 'false') {
                    return false;
                }
            }
            
            return urlparameter;
        }
    }
})();
