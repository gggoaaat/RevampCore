$(function () {

    var timeout = 20 * 60000;
    //alert('start')

    $(document).bind("idle.idleTimer", function () {
        // function you want to fire when the user goes idle


        $.timeoutDialog({ timeout: 1, countdown: 60, logout_redirect_url: '/loggedout', restart_on_yes: true });
    });

    $(document).bind("active.idleTimer", function () {
        // function you want to fire when the user becomes active again
    });

    $.idleTimer(timeout);
});