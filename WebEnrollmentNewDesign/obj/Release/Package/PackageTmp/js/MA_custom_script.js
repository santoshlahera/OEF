
$(document).ready(function () {
    $ = jQuery;
    //Document.ready function ends here
    $(".f-tabstrip").kendoTabStrip({
        animation: {
            close: {
                duration: 1000
            }
        }
    });
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

    //f-file-upload-basic
    $(".f-file-upload-basic").kendoUpload();


    //f-tabstrip




    //Document.ready function ends here
});


//slide button script
$(document).ready(function () {
    $('.slider-frame').toggle(function () {
        $('.slider-button').addClass('on').html('ON');
    }, function () {
        $('.slider-button').removeClass('on').html('OFF');
    });
});



//Header Menu 
$(document).ready(function () {
    var flag = 0;

    $('.header-menu').click(function () {
        if (flag == 0) {
            $('.header-menu-dd-bg').slideDown('Fast');
            flag = 1;
        }
        else {
            $('.header-menu-dd-bg').slideUp('Fast');
            flag = 0;
        }
    });
});


//f-splitter
$(document).ready(function () {
    $ = jQuery;
    $('.f-button-icon-2').click(function () {
        var splitterwidth = $('.box-1').width();
        if (splitterwidth < 470) {
            if ($('.splitter-container-1').is(":visible")) {
                $('.splitter-container-1').hide("fast");
                $(".box-1").width(94);
                $('.f-button-icon-1').hide("fast");
                $(".box-2").addClass("expand-box-2");
                $(".payment-content").addClass("expand-payment-content");
                $('.box-1-label').hide("fast");
                $(this).children().removeClass("k-i-maximize").addClass("k-i-minimize");
            } else {
                $('.splitter-container-1').show("fast");
                $(".box-1").width(466);
                $('.f-button-icon-1').show("fast");
                $(".box-2").removeClass("expand-box-2");
                $(".payment-content").removeClass("expand-payment-content");
                $('.box-1-label').show("fast");
                $(this).children().removeClass("k-i-minimize").addClass("k-i-maximize");
            }
        }
    });


    $('.f-button-icon-1').click(function () {
        var splitterwidth = $('.box-1').width();
        if (splitterwidth > 450) {
            if ($('.splitter-container-2').is(":visible")) {
                $('.splitter-container-2').hide("fast");
                $(".box-2").width(94);
                $('.f-button-icon-2').hide("fast");
                $(".box-1").addClass("expand-box-2");
                $('.box-2-label').hide("fast");
                $(this).children().removeClass("k-i-maximize").addClass("k-i-minimize");
            } else {
                $('.splitter-container-2').show("fast");
                $(".box-2").width(597);
                $(".box-1").removeClass("expand-box-2");
                $('.f-button-icon-2').show("fast");
                $('.box-2-label').show("fast");
                $(this).children().removeClass("k-i-minimize").addClass("k-i-maximize");
            }
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

    $(".f-tooltip-1").kendoTooltip({

        width: 119,
        height: 20,
        position: "top",
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

function ValidateEmail(ct) {

    var val = ct.value;
    if (val != "") {
        var re = /^([a-zA-Z0-9_\.])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        if (!re.test(val)) {
            ct.style.border = "2px Red Solid";
            return false;
        }
        ct.style.border = "";
        return true;
    }
    return true;
}