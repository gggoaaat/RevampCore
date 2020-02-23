



        $(".app-roles-link").on("click", function () {
            $("a").removeClass("current");
            $(this).addClass("current");
        });

$('#app-roles').on('shown.bs.modal', function (e) {
    $.ajaxSetup({ cache: false });
    $.getJSON("/security/sendroles/", function (data) {
        var roles = data;
        $("#app-option-roles").html("");
        for (var i = 0; roles.length > i; i++) {
            $("#app-option-roles").append("<option  value='" + roles[i].ViewRole.roles_id + "'>" + roles[i].ViewRole.role_name + "</option>");
        }
    }).done(function () {
        $('#app-roles .loumultiselect').multiSelect({
            afterSelect: function (values) {
                var ro = [];
                if ($("a.current").data("roles")  != undefined && $("a.current").data("roles")  != "") {
                    // alert($("#form-title").data("roles"))
                    ro = $("a.current").data("roles").toString().split(',');
                }

                var val = values.toString().split(',');
                //alert(values.toString());

                $(val).each(function (index, item) { ro.push(item); });
                var ro1=$.unique(ro.sort())
                $("a.current").data("roles", ro1);
            },
            afterDeselect: function (values) {
                //alert("Deselect value: "+values);
                if (values != null) {
                    var ro = [];
                    if ($("a.current").data("roles")  != undefined && $("a.current").data("roles") != "") {
                        ro = $("a.current").data("roles") ;
                    }
                    var index = ro.indexOf(values.toString());
                    if (index > -1) {
                        ro.splice(index, 1);
                    }
                }
            }
        });
        if ($("a.current").data("roles") != undefined) {
            //$.each($("a.current").data("roles"), function (i, e) {
            //    $("#app-option-roles option[value='" + e + "']").prop("selected", "true");

            //});
            var ro = [];
            $('#app-roles .loumultiselect').multiSelect('deselect_all');
            ro = $("a.current").data("roles");
            $('#app-roles .loumultiselect').multiSelect('select', ro);
        } else {
            $.getJSON("/security/sendapproles/" + $("a.current").attr("data-app-id"), function (data) {
                //var roles = data;
                //$.each(roles, function (i, e) {
                //    $("#app-option-roles option[value='" + e + "']").prop("selected", "true");

                //});

                var ro = [];
                $('#app-roles .loumultiselect').multiSelect('deselect_all');

                ro = data;
                $('#app-roles .loumultiselect').multiSelect('select', ro);
            })
        }
    });
});

$("#app-roles .modal-footer button.green").on("click", function () {
    if ($("a.current").data("roles") != undefined) {
        var approles = {
            roles: new Array,
            app_id: $("a.current").attr("data-app-id")
        }

        approles.roles = $("a.current").data("roles");

        $.ajax({
            type: "POST",
            url: "/security/ReceiveAppRoles",
            // The key needs to match your method's input parameter (case-sensitive).
            data: JSON.stringify(approles),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {

            },
            failure: function (errMsg) {
                alert(errMsg);
            }

        })
    }
});

$(".disable-app-link").on("click", function(e){
    $("a").removeClass("current");
    $(this).parents(".col-md-3:first").addClass("current-app");
    $(this).addClass("current");
});

$('#disable-app').on('shown.bs.modal', function (e) {
    $("#disable-app .modal-footer button.green").on("click", function () {
        $.ajax({
            type: "POST",
            url: $('a.current').data("app"),

        });

        $(".current-app").remove();
        $("#app-disabled").show();
        setTimeout("$('#app-disabled').remove()", 10000);
    });
});

$("#app-import .modal-footer button.green").on("click", function () {

    if(IsJsonString($("#app-json").val())){
        var json =  JSON.parse($("#app-json").val());

        json.parent_id = "";
        json.app_roles = ["SYSTEM ADMIN"];
        json.app_core = null;

        var response = $.ajax({
            type: "POST",
            url: "/applications/DoesAppNameExist?name=" + json.app_name,
            // The key needs to match your method's input parameter (case-sensitive).

            async: false,
            success: function (data) {

            },
            failure: function (errMsg) {
                alert(errMsg);
            }


        }).responseText;

        if (response == "True"){
            json.app_name = json.app_name + "_1";
        }

        for (var i = 0; json.stage.length > i; i++) {
            json.stage[i].stage_roles = ["SYSTEM ADMIN"];

            for (var g = 0; json.stage[i].grid_sections.length > g; g++) {
                json.stage[i].grid_sections[g].section_roles = ["SYSTEM ADMIN"];


            }
            for (var o = 0; json.stage[i].grid_items.length > o; o++) {
                json.stage[i].grid_items[o].section_roles = ["SYSTEM ADMIN"];
            }
        }

        $.ajax({
            type: "POST",
            url: "/stages/receivejson",
            data: JSON.stringify(json)
        });

        window.location.reload();

    }
});


var a=$(location).attr('href').toString().split("/")
var disptext="View By " + a[a.length-1].toString();

//get cores
$.getJSON("/cores/GetCores/", function (data) {
    var cores = data;
    $("#app-options-core").html("");
    $("#app-options-core").append("<li ><a href='/applications/'>All</a></li>");
    for (var i = 0; cores.length > i; i++) {
        var sel = "";
        //alert(cores[i].ViewCore.cores_id.toString() +"=="+a[a.length-2].toString());
        if(a[a.length-1].toString().trim().toLowerCase()=="core" && cores[i].ViewCore.cores_id.toString()==a[a.length-2].toString().trim())
        {
            //alert('a');
            //$('#sortfilter').text($('#sortfilter').text + ">> " +cores[i].ViewCore.core_name);
            disptext =disptext+ ">> " +cores[i].ViewCore.core_name;
        }
        $("#app-options-core").append("<li><a href='/applications/index/"+ cores[i].ViewCore.cores_id +"/Core'>"+ cores[i].ViewCore.core_name +"</a></li>");
    }

}).done(function(){
    if(a[a.length-1].toString().trim().toLowerCase()=="core")
        $('#sortfilter').text(disptext);
});

//get users
$.getJSON("/Identity/GetIdentities/", function (data) {
    var users = data;
    $("#app-options-user").html("");
    $("#app-options-user").append("<li><a href='/applications/'>All</a></li>");
    for (var i = 0; users.length > i; i++) {
        var sel = "";
        if(a[a.length-1].toString().toLowerCase()=="user" && users[i].toLowerCase()==a[a.length-2].toString().toLowerCase())
        {
            disptext =disptext+ ">> " +users[i];
        }
        $("#app-options-user").append("<li><a href='/applications/index/"+ users[i]  +"/User'>"+ users[i]  +"</a></li>");
    }
}).done(function(){
    if(a[a.length-1].toString().trim().toLowerCase()=="user")
        $('#sortfilter').text(disptext);
});
if(a[a.length-1].toString().toLowerCase()!="core" || a[a.length-1].toString().toLowerCase()!="user")
    $('#sortfilter').text('');


function IsJsonString(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}


$(".more").on("click", function () {
    if (!$(this).hasClass("open")) {
        $($(this).parent().prev()).find(".more-item").show();
        $(this).addClass("open");
        $(this).text("View Less...");
    } else {
        $($(this).parent().prev()).find(".more-item").hide();
        $(this).removeClass("open");

        $(this).text("View More...");
    }
});


