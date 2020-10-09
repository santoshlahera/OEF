/* Frontier - Mobile Website July 2016 - Author: Prodigy */

$(window).load(function () {
    (function () {
        var SwipeCarrousel = {
            init: function (elements) {
                $(elements).each(function () {
                    SwipeCarrousel.addSwipeSupport.call(this);
                });
            },
            addSwipeSupport: function () {
                $(this)
                .swiperight(function () { $(this).carousel('prev') })
                .swipeleft(function () { $(this).carousel('next') });
            }
        };
        SwipeCarrousel.init($('.f-m-carousel'));
    })();

});



$(document).ready(function () {
    $ = jQuery;


    //carousel script
    $('.f-m-carousel').carousel({
        interval: false
    });




    //Scroll Top button script
    $(".f-m-scrolltop-btn").click(function () {
        $("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });

    //Banner Zipcode show script
    $(".f-m-banner-b-link").click(function () {
        $(".f-m-banner-form").slideDown();
        $(this).hide();
    });
    $(".zipcode").click(function () {
        $(".f-m-banner-form").slideDown();
        $(this).hide();
    });

    //Textbox label - script
    $(".form-element-left").click(function () {
        $(this).addClass("form-element-left-active");
        $(this).find('.form-control').focus();
    });


    $('.form-element-left .form-control').focusin(function () {
        $(this).parent().parent().addClass("form-element-left-active");
        //$(this).find('.form-control').focus();

    });

    $('.form-element-left .form-control').focusout(function () {
        if ($(this).val().trim().length == 0) {
            $(this).parent().parent().removeClass("form-element-left-active");

        }

    });




    //Header show/hide in scroll - script
    var lastScrollTop = 0;
    $(window).scroll(function (event) {
        var st = $(this).scrollTop();
        if (st > lastScrollTop) {
            var scroll_position = $(document).scrollTop().valueOf();
            if (scroll_position >= 400) {
                $(".f-m-scrolltop-btn").show('slow');
                //$(".f-m-header").slideUp();
               // $(".f-m-header-right").slideUp();
               // $(".f-m-header-left").slideUp();

            }
        } else {
            $(".f-m-header").slideDown();
            $(".f-m-header-right").slideDown();
            $(".f-m-header-left").slideDown();

        }

        var scroll_position_final = $(document).scrollTop().valueOf();
        if (scroll_position_final <= 3) { $(".f-m-scrolltop-btn").hide('slow'); }
        lastScrollTop = st;
    });


});

//Tooltip Script
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
});

//Popover Script
$(function () {
    $('[data-toggle="popover"]').popover()
});



//This script is based on the javascript code of Roman Feldblum (web.developer@programmer.net) 
// Original script : http://javascript.internet.com/forms/format-phone-number.html 
//Original script is revised by Eralper Yilmaz (http://www.eralper.com) 
//Revised script : http://www.kodyaz.com 
//Format : "(123) 456-7890"
//source : http://www.kodyaz.com/articles/javascript-phone-format-phone-number-format.aspx
var zChar = new Array(' ', '(', ')', '-', '.');
var maxphonelength = 14;
var phonevalue1;
var phonevalue2;
var cursorposition;

function ParseForNumber1(object) {
    phonevalue1 = ParseChar(object.value, zChar);
}

function ParseForNumber2(object) {
    phonevalue2 = ParseChar(object.value, zChar);
}

function backspacerUP(object, e) {
    if (e) {
        e = e
    } else {
        e = window.event
    }
    if (e.which) {
        var keycode = e.which
    } else {
        var keycode = e.keyCode
    }

    ParseForNumber1(object)

    if (keycode >= 48) {
        ValidatePhone(object)
    }
}

function backspacerDOWN(object, e) {
    if (e) {
        e = e
    } else {
        e = window.event
    }
    if (e.which) {
        var keycode = e.which
    } else {
        var keycode = e.keyCode
    }
    ParseForNumber2(object)
}

function GetCursorPosition() {

    var t1 = phonevalue1;
    var t2 = phonevalue2;
    var bool = false
    for (i = 0; i < t1.length; i++) {
        if (t1.substring(i, 1) != t2.substring(i, 1)) {
            if (!bool) {
                cursorposition = i
                window.status = cursorposition
                bool = true
            }
        }
    }
}

function ValidatePhone(object) {

    var p = phonevalue1

    p = p.replace(/[^\d]*/gi, "")

    if (p.length < 3) {
        object.value = p
    } else if (p.length == 3) {
        pp = p;
        d4 = p.indexOf('(')
        d5 = p.indexOf(')')
        if (d4 == -1) {
            pp = "(" + pp;
        }
        if (d5 == -1) {
            pp = pp + ")";
        }
        object.value = pp;
    } else if (p.length > 3 && p.length < 7) {
        p = "(" + p;
        l30 = p.length;
        p30 = p.substring(0, 4);
        p30 = p30 + ") "

        p31 = p.substring(4, l30);
        pp = p30 + p31;

        object.value = pp;

    } else if (p.length >= 7) {
        p = "(" + p;
        l30 = p.length;
        p30 = p.substring(0, 4);
        p30 = p30 + ") "

        p31 = p.substring(4, l30);
        pp = p30 + p31;

        l40 = pp.length;
        p40 = pp.substring(0, 9);
        p40 = p40 + "-"

        p41 = pp.substring(9, l40);
        ppp = p40 + p41;

        object.value = ppp.substring(0, maxphonelength);
    }

    GetCursorPosition()

    if (cursorposition >= 0) {
        if (cursorposition == 0) {
            cursorposition = 2
        } else if (cursorposition <= 2) {
            cursorposition = cursorposition + 1
        } else if (cursorposition <= 4) {
            cursorposition = cursorposition + 3
        } else if (cursorposition == 5) {
            cursorposition = cursorposition + 3
        } else if (cursorposition == 6) {
            cursorposition = cursorposition + 3
        } else if (cursorposition == 7) {
            cursorposition = cursorposition + 4
        } else if (cursorposition == 8) {
            cursorposition = cursorposition + 4
            e1 = object.value.indexOf(')')
            e2 = object.value.indexOf('-')
            if (e1 > -1 && e2 > -1) {
                if (e2 - e1 == 4) {
                    cursorposition = cursorposition - 1
                }
            }
        } else if (cursorposition == 9) {
            cursorposition = cursorposition + 4
        } else if (cursorposition < 11) {
            cursorposition = cursorposition + 3
        } else if (cursorposition == 11) {
            cursorposition = cursorposition + 1
        } else if (cursorposition == 12) {
            cursorposition = cursorposition + 1
        } else if (cursorposition >= 13) {
            cursorposition = cursorposition
        }
      
        //var txtRange = object.createTextRange();
        //txtRange.moveStart("character", cursorposition);
        //txtRange.moveEnd("character", cursorposition - object.value.length);
        //txtRange.select();
    }

}

function ParseChar(sStr, sChar) {

    if (sChar.length == null) {
        zChar = new Array(sChar);
    }
    else zChar = sChar;

    for (i = 0; i < zChar.length; i++) {
        sNewStr = "";

        var iStart = 0;
        var iEnd = sStr.indexOf(sChar[i]);

        while (iEnd != -1) {
            sNewStr += sStr.substring(iStart, iEnd);
            iStart = iEnd + 1;
            iEnd = sStr.indexOf(sChar[i], iStart);
        }
        sNewStr += sStr.substring(sStr.lastIndexOf(sChar[i]) + 1, sStr.length);

        sStr = sNewStr;
    }

    return sNewStr;
}

function movetoNext(current, nextFieldID) {
    if (current.value.length == current.maxLength) {
        document.getElementById(nextFieldID).focus();

    }
    else if (current.value.length > current.maxLength) {
        alert(current.value.substring(0, str.length - 1));
        current.value = current.value.substring(0, str.length - 1);
    }
}



















