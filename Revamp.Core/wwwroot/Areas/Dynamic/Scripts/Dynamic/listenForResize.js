var handleOnResize1 = function () {

    $('.listen-for-resize').css('height', '300px;')

    function handleWindowSize() {

        if ($('.listen-for-resize').position() != undefined && $('.listen-for-resize').position().top != undefined) {
            var howfarfromtop = $('.listen-for-resize').position().top;

            var currentWrapHeight = +$('.page-content').css('min-height').replace('px', '') - 55;

            $('.listen-for-resize').css('height', (currentWrapHeight - howfarfromtop) + 'px');
        }
    }

    var resize;
    if (Metronic.isIE8()) {
        var currheight;
        $(window).resize(function () {
            if (currheight == document.documentElement.clientHeight) {
                return; //quite event since only body resized not window.
            }
            if (resize) {
                clearTimeout(resize);
            }
            resize = setTimeout(function () {
                console.log('hi!')
            }, 50); // wait 50ms until window resize finishes.
            currheight = document.documentElement.clientHeight; // store last body client height
        });
    } else {
        $(window).resize(_.debounce(function () {
            if (resize) {
                clearTimeout(resize);
            }
            resize = setTimeout(function () {
                handleWindowSize();
                //console.log('hi!')
            }, 50); // wait 50ms until window resize finishes.
        }, 500));
    }

    function init() {
        setTimeout(function () {
            handleWindowSize();
            //console.log('hi!')
        }, 500);
    }

    $(document).ready(function () {
        init();
    })

    return {
        init: function () { init(); }
    }
}();