
$(document).ready(function () {
    $ = jQuery;

    $('#menu-collapse').click(function () {

        if ($(this).children().hasClass("k-i-expand-w")) {
            $(".left-column").width(1069);
            $(".right-column").hide();
            $(this).children().removeClass("k-i-expand-w").addClass("k-i-expand");
        } else {
            $(".left-column").width(863);
            $(".right-column").show();
            $(this).children().removeClass("k-i-expand").addClass("k-i-expand-w");
        }
    });
});


$(document).ready(function () {
    //Document.ready function ends here

    //f-Dropdown
    $(".f-dropdown").kendoDropDownList();


    //f-Numeric Textbox
    $(".f-numerictextbox").kendoNumericTextBox();

    //f-datepicker			
    $(".f-datepicker").kendoDatePicker();

    //f-multiselect			
    var required = $(".f-multiselect").kendoMultiSelect().data("kendoMultiSelect");

    //f-file-upload
    $(".f-file-upload").kendoUpload({
        async: {
            saveUrl: "save",
            removeUrl: "remove",
            autoUpload: true
        }
    });

    //f-tabstrip
    $(".f-tabstrip").kendoTabStrip({
        animation: {
            close: {
                duration: 1000
            }
        }
    });

    //f-tooltip
    $(document).ready(function () {
        var tooltip = $(".f-tooltip").kendoTooltip({

            //width: 180,
            //height: 40,
            position: "right",
            animation: {
                open: {
                    effects: "fade:in",
                    duration: 100
                },
                close: {
                    effects: "fade:out",
                    duration: 500
                }
            }
        }).data("kendoTooltip");
    });


    //f-tooltip-top
    $(document).ready(function () {
        var tooltip = $(".f-tooltip-top").kendoTooltip({
            //width: 180,
            //height: 40,
            position: "top",
        }).data("kendoTooltip");
    });
});




//f-splitter
$(document).ready(function () {
    $ = jQuery;
    $('.f-button-icon-1').click(function () {
        var splitterwidth = $('.box-6').width();
        if (splitterwidth < 1000) {
            if ($('.box-5-content').is(":visible")) {
                $('.box-5-content').hide("fast");
                $(".box-5").width(40);
                $(".box-5").height(35);
                $('.box-5-title').hide("fast");
                $(".box-6").addClass("expand-box-6");
                $(".box-6-content").addClass("expand-box-6-content");
                $(this).children().removeClass("expand-icon").addClass("collapse-icon");
            }
        }
        else if (splitterwidth > 1000) {
            $('.box-5-content').show("fast");
            $(".box-5").width(303);
            $(".box-5").height(367);
            $('.box-5-title').show("fast");
            $(".box-6").removeClass("expand-box-6");
            $(".box-6-content").removeClass("expand-box-6-content");
            $(this).children().removeClass("collapse-icon").addClass("expand-icon");
        }
    });
});

//f-splitter
$(document).ready(function () {
    $ = jQuery;
    $('#gridExpand').click(function () {
        if ($('#gridExpand span').hasClass("expand-icon-1")) {
            $('#gridExpand span').removeClass("expand-icon-1");
            $('#gridExpand span').addClass("collapse-icon-1");

        }
        else {
            $('#gridExpand span').removeClass("collapse-icon-1");
            $('#gridExpand span').addClass("expand-icon-1");
        }
    });
});




$(document).ready(function () {
    $ = jQuery;
    $('.f-button-icon-2').click(function () {
        var splitterwidth = $('.box-7').width();
        if (splitterwidth < 1000) {
            if ($('.box-8-content').is(":visible")) {
                $('.box-8-content').hide("fast");
                $('.f-button-icon-3').hide("fast");
                $(".box-8").width(40);
                $(".box-8").height(35);
                $('.box-8-title').hide("fast");
                $(".box-7").addClass("expand-box-6");
                $(".box-7-content").addClass("expand-box-6-content");
                $(this).children().removeClass("expand-icon").addClass("collapse-icon");
            }
        }
        else if (splitterwidth > 1000) {
            $('.box-8-content').show("fast");
            $(".box-8").width(514);
            $(".box-8").height(423);
            $('.box-8-title').show("fast");
            $('.f-button-icon-3').show("fast");
            $(".box-7").removeClass("expand-box-6");
            $(".box-7-content").removeClass("expand-box-6-content");
            $(this).children().removeClass("collapse-icon").addClass("expand-icon");
        }
    });
});



$(document).ready(function () {
    $ = jQuery;
    $('.f-button-icon-3').click(function () {
        var splitterwidth = $('.box-8').width();
        if (splitterwidth < 1000) {
            if ($('.box-7-content').is(":visible")) {
                $('.box-7-content').hide("fast");
                $('.f-button-icon-2').hide("fast");
                $(".box-7").width(40);
                $(".box-7").height(35);
                $('.box-7-title').hide("fast");
                $(".box-8").addClass("expand-box-6");
                $(".box-8-content").addClass("expand-box-6-content");
                $(this).children().removeClass("expand-icon").addClass("collapse-icon");
            }
        }
        else if (splitterwidth > 1000) {
            $('.box-7-content').show("fast");
            $(".box-7").width(538);
            $(".box-7").height(423);
            $('.f-button-icon-2').show("fast");
            $('.box-7-title').show("fast");
            $(".box-8").removeClass("expand-box-6");
            $(".box-8-content").removeClass("expand-box-6-content");
            $(this).children().removeClass("collapse-icon").addClass("expand-icon");
        }
    });
});
function CreateToolTip() {
    $(".f-tooltip").kendoTooltip({

        //width: 180,
        //height: 40,
        position: "right",
        animation: {
            open: {
                effects: "fade:in",
                duration: 100
            },
            close: {
                effects: "fade:out",
                duration: 500
            }
        }
    }).data("kendoTooltip");
}

//Scroll to Script

$(document).ready(function () {
    $ = jQuery;
    $(".scroll-down").click(function () {
        $("html, body").animate({
            scrollTop: 875
        }, 'slow');
        $('.scroll-down').hide("fast");
        $('.scroll-top').show("fast");
    });

    $(".scroll-top").click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, "slow");
        $('.scroll-top').hide("fast");
        $('.scroll-down').show("fast");
    });

    $(".scroll-top-1").click(function () {
        $("html, body").animate({
            scrollTop: 0
        }, "slow");
        $('.scroll-top').hide("fast");
        $('.scroll-down').show("fast");
        $('.scroll-top-1').hide("fast");
    });

    $(window).scroll(function () {
        scroll_position = $(window).scrollTop();
        if (scroll_position > 800) {
            $('.scroll-top').show("fast");
            $('.scroll-top-1').show("fast");
            $('.scroll-down').hide("fast");
        }
        else {
            $('.scroll-down').show("fast");
            $('.scroll-top-1').hide("fast");
            $('.scroll-top').hide("fast");
        }
    });


});

function SSMessage(text, onExit) {
    var kendoWindow = $("<div id='divMessege'/>").kendoWindow({
        title: "Message ",
        resizable: false,
        position: { top: 10, left: 10 },
        modal: true,
        deactivate: function () {
            this.destroy();
            if (onExit)
                onExit();
        },
    });
    kendoWindow.data("kendoWindow")
            .content('<div style="width:400px; padding:10px;margin-top:20px;"><label id="sslblmsg" class="f-label"></label></div>').
            center().open();
    kendoWindow
 .find(".No")
 .click(function () {
     kendoWindow.data("kendoWindow").close();
 })
 .end();

    $("#sslblmsg").html(text);
}