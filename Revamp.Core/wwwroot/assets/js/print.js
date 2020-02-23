


function iframeLoaded(iframe, printwindow) {
    
    //var iFrameID = iframe;// document.getElementById(iframe);
    if (iframe) {
        // here you can make the height, I delete it first, then I make it again
        iframe.height = "";

        iframe.height = iframe.contentWindow.document.body.scrollHeight + "px";
        
        
        $(iframe.contentWindow.document.body).find(".portlet-title .actions").css("display", "none");
        $(iframe.contentWindow.document.body).css("background-color", "#FFFFFFF");

        //////if (printwindow == 1) {
        //////    window.print();
        //////    window.close();
        //////    //return true;
        //////}
    }
}
function printdiv() {
    this.window.print();
}

function loadEntryData() {
    //alert( $('.entry').length);
    
    $('.entry').each(function (index, element) {

        $.get($(element).attr("data"), function () {

        }).done(function (data) {

            $(element).html(data);
            if (index <= $('.entry').length) {
                $('.wait').hide();
                $('.showprint').show();
            }

            //$('.entry').find(':input').attr('disabled', 'disabled');
            $(".entry").css({ "opacity": "1", "pointer-events": "none" });

            //if(docbody=="")
            //{
            //    mywindow.document.write("<div id='infodiv' style='background:#FFFFFF'><center><h1>Print Preview</h1></center><a class='btn green' style='cursor:pointer' onclick='printdiv();'><i class='fa fa-print'></i> Print</a><br><br></div>");
            //}
            //docbody=docbody + data;                            
            //mywindow.document.write(data );
            //if(i!=$('.printcheck:checked').length)
            //    mywindow.document.write("<div class='pageBreak'></div>");

            //i++;
        })
        
    });

}

