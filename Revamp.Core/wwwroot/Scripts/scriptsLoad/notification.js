var number = $('#header_notification_bar .badge').text();
//number = parseInt(number);
//number = number + 3;

$('#header_notification_bar').click(function () {
    $('#header_notification_bar .badge').hide();
    $('#header_notification_bar .badge').text(number);
    $('#header_notification_bar .dropdown-menu li:first').html("<div class='notification-bar-top'><div class='notification-bar-settings'><a href='/Profile#tab_4-4'>Settings</a></div><div>You have 0 new notifications</div> </div> ");
    $.getJSON('/notification/notificationchecked', function () { }); //Saves each time a users clicks on the notification icon
});

$('#1001_1001_1001_1004_UserName, #1001_1001_1001_1005_Password').on('mousedown', function () {
    $('.error').empty();
});

//$('#header_notification_bar .scroller').scroll(function () {

$('#header_notification_bar .scroller').bind('slimscroll', function (e, pos) {
    console.log("Reached " + pos);

    if (pos == "bottom") {
        var last = $('#header_notification_bar .scroller li:last').data("id"); //id of last activity
        var items;
        $.getJSON("/notification/UpdateNotificationsList/?id=" + last, function (data) {
            items = data;
            for (var i = 0; i < items.length; i++) {
                var notificationDate = new Date(parseInt(items[i].dt_created.substr(6)));

                var text = '<li data-id="' + items[i].activity_id + '"><a href="/forms/dashboard/' + items[i].applications_id + '">' +
                '<span class="label label-sm label-icon label-' + items[i].variant_name + '">' +
                '<i class="fa fa-' + items[i].symbol_name + '"></i></span>' +
                '<strong>' + items[i].desc_text + '</strong> <span class="time">' + notificationDate.toLocaleString() + ' </span></a> </li>';
                $("#header_notification_bar .dropdown-menu-list").append(text);

            }
        });
    }

    //next 10 items after the current ten in the list

});