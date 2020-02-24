function IrisEllipse_Add() {
    if ($('#iris-modal-ellipse').length == 0) {
        var html = '';
        html += '<div id="iris-modal-ellipse" class="modal-backdrop in" style="display: none; z-index: 999999;">' +
                    '<div id="circleG">' +
                        '<div id="circleG_1" class="circleG"></div>' +
                        '<div id="circleG_2" class="circleG"></div>' +
                        '<div id="circleG_3" class="circleG"></div>' +
                    '</div>' +
                '</div>';
        $('body').append(html);
    }
}

function IrisEllipse_Hide() {
    if ($('#iris-modal-ellipse').length > 0) {
        $('#iris-modal-ellipse').hide();
        $('#iris-modal-ellipse').css('opacity', 0);
    }
    //TODO:Check if this is needed.
    //else {
    //    iGlobal.ellipse.hide();
    //}
}

function IrisEllipse_Show() {
    if ($('#iris-modal-ellipse').length > 0 && ($('#iris-modal-ellipse').css('display') == 'none')) {
        $('#iris-modal-ellipse').css('opacity', 0);
        $('#iris-modal-ellipse').show();
        IrisEllipse_Unfade($('#iris-modal-ellipse'));
    }
    else {
        IrisEllipse_Add();
        $('#iris-modal-ellipse').css('opacity', 0);
        $('#iris-modal-ellipse').show();
        IrisEllipse_Unfade($('#iris-modal-ellipse'));
    }
}

function IrisEllipse_Unfade(element) {
    var op = 0.1;
    var timer = setInterval(function () {
        if (op < .5) {
            flag = true;
            element.css('opacity', op);
            element.css('filter', 'alpha(opacity=' + op * 100 + ')');
            op += op * 0.1;

            if (op >= .5) {
                clearInterval(timer);
            }
        }
    }, 100);
}

var commonDynamic = {
    functions: {}
}









//Required mGlobal setup:
//mGlobal.ajaxData = {
//  handles: {
//      list: {
//          forms: {
//              <formType1>: [ <form1>, <form2> ]
//          },
//          <type1>: {
//              <form1>: [ <handle1-a>, <handle1-b>, <handle1-c> ],
//              <form2>: [ <handle2-a>, <handle2-b>, <handle2-c> ]
//          }
//      },
//      <formType1>: {
//          <handle1-a>: {
//              fn: function (model) {
//                  .
//                  .
//                  model.options.callBack = function (model) {
//                      commonDynamic.functions.ajaxData.handles.ajaxCall(model);
//                  }
//                  .
//                  .
//              },
//              callBack: function (model) {}, /*OPTIONAL*/
//              response: {},
//              isLoaded: false
//          },
//          .
//          .
//          .
//      }
//  }
//};
//
//Function Calls: /*REQUIRES model.formType */
//Load/Reset ajaxDynamic data for specified handle                  >> commonDynamic.functions.ajaxData.handles.ajaxCall()  /*ADDITIONAL { reset, handle } */
//Load/Reset ajaxDynamic data for multiple forms/handles            >> commonDynamic.functions.ajaxData.handles.load()      /*ADDITIONAL { reset, exclude[], handles[]/forms[] } */
//Check if ajaxData has been loaded for formTypes/form(s)/handle(s) >> commonDynamic.functions.ajaxData.handles.check()     /*ADDITIONAL { reset, exclude[], handles[]/forms[] } */
//Check if ajaxData loaded, if not then load ajaxDynamic response   >> commonDynamic.functions.ajaxData.handles.wait()      /*ADDITIONAL { fn(), autohide, reset }*/
