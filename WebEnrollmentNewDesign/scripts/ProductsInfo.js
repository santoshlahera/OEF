var Glbtabid = "";
var GlbpricePlanId = "";
var AllInfo = "";
var PayBy = "Credit";
var SocialSecurity = "SSN";
var ConfirmationMsg = "";
var PlessBill = "No";
var PlessComm = "No";
var GlobalTabName = "";
//var tbName = "DisplayAll";
var tbName = "Recommended";
var ltbName = "";
var gbZIPCode = "";
var gbZIPName = "";
var Produrl = "";
var BrandId = "";
var brandCode = "";
var errorMsg = "";
var flag = 0;
var pTDSP = "";
var firstLoad = "Y";
var TDSPCode = "";
var TDSPName = "";
var changethezip = "";
var TDSPZip = "";
$(document).ready(function () {
    Produrl = $("#hnProdUrl").val();
});

function changeURLforZip() {
    changethezip = "y";

    $('#changethezip').show();
    $('#ordernow').hide();

    $('#zippMsg').show();
    $('#ordernowMsg').hide();

    $('#closeBt').show();
    InsertMileStone(tbName + ":ChangeZipcode");
    if ($("#IsPartner").val() != "y") {
        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/ChangeZipCode");
    }
    ltbName = "Display All";
}

function changeURLforPromocode() {
    InsertMileStone(tbName + ":ChangePromoCode");
    if ($("#IsPartner").val() != "y") {
        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/Promocode");
    }
}

function show_hideLanguage_img() {
    var IschkdLang = document.getElementById("english").checked;
    if (IschkdLang == true) {
        $("#imgEnglish").show();
        $("#imgSpanish").hide();
    }
    else {
        $("#imgEnglish").hide();
        $("#imgSpanish").show();
    }
}

function show_hideCommunication_img() {

    var IschkdLang = document.getElementById("Yes").checked;
    if (IschkdLang == true) {
        $("#imgYes").show();
        $("#imgNo").hide();
        $("#divInvoice").hide();
        PlessComm = "Yes";

    }
    else {
        $("#rbPaperlessBilling_Yes").prop("checked", false);
        $("#imgYes").hide();
        $("#imgNo").show();
        $("#divInvoice").css("display", "block");
        PlessComm = "No";

    }

}
function GetPaymentMethod(pMethod) {

    PayBy = pMethod;
}
function GetSocialSecurityType(SSSecurity) {

    SocialSecurity = SSSecurity;
}
$(function () {
    $('#loadingmessage').show();
    //   $("#tabs").tabs();
    $("#tabs").show();
    $('#loadingmessage').hide();

});


$(function () {
    $('label').click(function () { $(this).prev('input').focus(); });
    $("#imgSpanish").hide();
    $("#imgNo").show();

    $("#imgMoving").hide();
    $("#imgOtrUserYes").hide();
    $("#imgBNo").hide();
    $("#txtAddress").keyup(function () {
        $("#serAddMsg").empty();
        $('#loadingAddress').show();
        var address = $("#txtAddress").val();
        var zip = $("#txtZipCode2").val();
        if ($("#txtAddress").val() == '') {
            $("#serAddMsg").empty();
            $("#btAddress").css("display", "none");
            $("#btContinue").css("display", "none");
        }
        var serialize1 = {
            serAdd: address,
            ZipCode: zip
        };
        $.ajax({
            type: 'POST',
            data: serialize1,
            url: Produrl + "/Home/GetServiceAddress",
            success: function (response) {
                $('#loadingAddress').hide();
                if (response != "") {
                    autocomplete(document.getElementById("txtAddress"), response);
                }
                else {
                    $("#serAddMsg").text("Invalid Address - Please Check the Address Entered");
                    $("#serAddMsg").css("color", "Red");
                    $("#btAddress").css("display", "none");
                    $("#btContinue").css("display", "none");
                }
            },
            failure: function (response) {
                alert("failer");
            },
            error: function (response) {
                alert("error");
            }
        });

    });
});

$(function () {
    $("#txtAddress2").keyup(function () {

        $("#btContinue2").css("display", "none");
        $("#serAddMsg2").empty();
        var address = $("#txtAddress2").val();

        var zip = $("#txtZipCode3").val();
        if ($("#txtAddress2").val() == '') {
            $("#btAddress2").css("display", "block");
        }
        else {
            $('#btAddress2').removeAttr('disabled');
        }
        $("#loadingAddress2").show();
        var serialize1 = {
            serAdd: address,
            ZipCode: zip
        };
        $.ajax({
            type: 'POST',
            data: serialize1,
            url: Produrl + "/Home/GetServiceAddress",
            success: function (response) {
                $("#loadingAddress2").hide();
                if (response != "") {
                    autocomplete2(document.getElementById("txtAddress2"), response);
                }
                else {

                }
            },
            failure: function (response) {
                alert("failer");
            },
            error: function (response) {
                alert("error");
            }
        });

    });

});


function autocomplete(inp, arr) {
    $("#btAddress2").css("display", "none");
    $("#btAddress").css("display", "none");
    // $("#txtAddress").text = "";
    var currentFocus;

    // inp.addEventListener("input", function (e) {
    // var a, b, i, val = this.value;
    var a, b, i, val = $(".autocomplete").val();

    closeAllLists();
    //if (!val) { return false; }
    currentFocus = -1;

    a = document.createElement("DIV");
    a.setAttribute("id", this.id + "autocomplete-list");
    a.setAttribute("class", "autocomplete-items");

    //  this.parentNode.appendChild(a);
    $(".autocomplete").append(a);

    for (i = 0; i < arr.length; i++) {

        if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {

            b = document.createElement("DIV");

            b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
            b.innerHTML += arr[i].substr(val.length);

            b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";

            b.addEventListener("click", function (e) {

                inp.value = this.getElementsByTagName("input")[0].value;
                //alert("sending value="+inp.value);ch

                CheckServiceAddress_Info(inp.value, "ordernow");
                closeAllLists();
            });
            a.appendChild(b);
        }
    }
    // });

    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {

            currentFocus++;

            addActive(x);
        } else if (e.keyCode == 38) { //up

            currentFocus--;

            addActive(x);
        } else if (e.keyCode == 13) {

            e.preventDefault();
            if (currentFocus > -1) {

                if (x) x[currentFocus].click();
            }
        }
    });




    function addActive(x) {

        if (!x) return false;

        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);

        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {

        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {

        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }

    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

function validatePhoneNumber() {
    var phone = document.getElementById("txtCell1").value;
    var phoneNumber = phone.replace(/[^0-9]/g, '');
    if (phoneNumber.length != 10) {
        $(".mobilnumberEror").show();
        $("#txtCell1").css('border-color', 'red')
        return false;
    } else {
        $(".mobilnumberEror").hide();
        $("#txtCell1").css('border-color', '')
        return true;
    }
}

//Service Address
function CheckServiceAddress_Info(selVal, reqFrom) {

    $("#btAddress").css("display", "none");
    $("#btContinue").css("display", "none");
    var checkreq = reqFrom;
    $('.loadingmessage2').show();
    var serialize1 = {
        serviceAdd: selVal,
    };
    $.ajax({

        type: 'POST',
        data: serialize1,
        url: Produrl + "/Home/CheckServiceAddress",
        success: function (response) {


            $("#btAddress").css("display", "none");
            $("#btContinue").css("display", "none");
            $("#btAddress2").css("display", "none");
            $("#btContinue2").css("display", "none");
            $('.loadingmessage2').hide();
            $("#serAddMsg").empty();
            $("#serAddMsg").append(response[1]);
            $("#serAddMsg").css("color", "red");
            $("#serAddMsg2").empty();
            $("#serAddMsg2").append(response[1]);
            $("#serAddMsg2").css("color", "red");
            //    alert(response);
            if (response[1] != "") {
                $("#serAddMsg").empty();
                $("#serAddMsg2").empty();
                $("#btAddress").css("display", "block");
                $("#btAddress2").css("display", "block");
                $("#btContinue").css("display", "none");
                $("#btContinue2").css("display", "none");
                $("#serAddMsg").append(response[1]);
                $("#serAddMsg2").append(response[1]);
                //$("#btAddress").show();
                $("#btAddress").css("display", "none");
                $("#btContinue").css("display", "none");
                $("#btAddress2").css("display", "none");
                $("#btContinue2").css("display", "none");
            }
            else if (response[2] == "Differenttdsp" && (checkreq == "ordernow" || changethezip == "y")) {
                $('#myDifferntTDSPMsg').modal({
                    backdrop: 'static',
                    keyboard: false
                })
                $('#myDifferntTDSPMsg').modal('show');
                $('#myAddressModal').css({ 'opacity': 0.5 });
            }
            if (response[0] == "Comm") {
                $("#btAddress").css("display", "none");
                $("#btContinue").css("display", "block");
                $("#btAddress2").css("display", "none");
                $("#btContinue2").css("display", "block");

            }

            //if (response[1] != "") {

            //if (response[1] != "" && response[0] != "Mismatch") {

            //    $("#serAddMsg").show();
            //    $("#serAddMsg2").show();
            //    //$("#btAddress").css("display", "none");
            //    //$("#btContinue").css("display", "none");
            //    //$("#btAddress2").css("display", "none");
            //    //$("#btContinue2").css("display", "none");

            //    $("#btAddress").css("display", "block");
            //    $("#btAddress2").css("display", "block");
            //    $("#btContinue").css("display", "none");
            //    $("#btContinue2").css("display", "none");
            //}
            if (response[1] == "" && response[0] != "Mismatch") {
                $("#serAddMsg").show();
                $("#serAddMsg2").show();
                //$("#btAddress").css("display", "none");
                //$("#btContinue").css("display", "none");
                //$("#btAddress2").css("display", "none");
                //$("#btContinue2").css("display", "none");

                $("#btAddress").css("display", "block");
                $("#btAddress2").css("display", "block");
                $("#btContinue").css("display", "none");
                $("#btContinue2").css("display", "none");
            }
            else {

                $("#serAddMsg").empty();
                $("#serAddMsg2").empty();
                $("#btAddress").css("display", "block");
                $("#btAddress2").css("display", "block");
                $("#btContinue").css("display", "none");
                $("#btContinue2").css("display", "none");
                $("#serAddMsg").append(response[1]);
                $("#serAddMsg2").append(response[1]);
                //$("#btAddress").show();
                $("#btAddress").css("display", "none");
                $("#btContinue").css("display", "none");
                $("#btAddress2").css("display", "none");
                $("#btContinue2").css("display", "none");
            }
        },
        failure: function (response) {
        },
        error: function (response) {
        }
    });
}

$(function () {
    $("#myAddressModal").on("hidden.bs.modal", function () {
        $("#txtAddress").val('');
        $("#serAddMsg").text('');
    });

    $("#myModal").on("hidden.bs.modal", function () {
        $("#txtPromocode").val('');
        $("#lbPromoCodemsg").text('');
    });
    $("#myAddressZipModal2").on("hidden.bs.modal", function () {
        $("#txtAddress2").val('');
        $("#serAddMsg2").text('');
    });
    //$('#myAddressZipModal2').on('shown.bs.modal', function () {
    //    //var currrentvale = document.getElementById("lbzipcode").textContent;
    //    document.getElementById("txtZipCode3").textContent = '23223';
    //    alert(document.getElementById("lbzipcode").textContent='45454');
    //})

});


function autocomplete2(inp, arr) {

    $("#btContinue2").css("display", "none");
    // $("#txtAddress").text = "";
    var currentFocus;

    // inp.addEventListener("input", function (e) {
    // var a, b, i, val = this.value;
    var a, b, i, val = $(".autocomplete2").val();

    closeAllLists();
    //if (!val) { return false; }
    currentFocus = -1;

    a = document.createElement("DIV");
    a.setAttribute("id", this.id + "autocomplete-list");
    a.setAttribute("class", "autocomplete-items");

    //  this.parentNode.appendChild(a);
    $(".autocomplete2").append(a);

    for (i = 0; i < arr.length; i++) {

        if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {

            b = document.createElement("DIV");

            b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
            b.innerHTML += arr[i].substr(val.length);

            b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";

            b.addEventListener("click", function (e) {

                inp.value = this.getElementsByTagName("input")[0].value;

                CheckServiceAddress_Info(inp.value, "");
                closeAllLists();
            });
            a.appendChild(b);
        }
    }
    // });

    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {

            currentFocus++;

            addActive(x);
        } else if (e.keyCode == 38) { //up

            currentFocus--;

            addActive(x);
        } else if (e.keyCode == 13) {

            e.preventDefault();
            if (currentFocus > -1) {

                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {

        if (!x) return false;

        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);

        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {

        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {

        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }

    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

function ValidateZipCode() {

    var tzip = document.getElementById("txtZipCode2");
    var taddr = document.getElementById("txtAddress");
    document.getElementById("txtAddress").value = "";
    var vzip = tzip.value;
    taddr.disabled = false;

    if (vzip == "" || vzip.length != 5) {

        taddr.disabled = true;
        document.getElementById("txtAddress").value = "";
        // lzip.innerHTML = "Invalid Zip Code";
        tzip.style.border = "2px Red Solid";
        $("#btAddress").css("display", "none");
        $("#btContinue").css("display", "none");
        $("#serAddMsg").empty();

        $("#serAddMsg").append("Enter Zip Code with minimum 5 numerics");
        $("#serAddMsg").css("color", "red");
        tzip.focus();

    } else {
        tzip.style.border = "2px Green Solid";
        $("#serAddMsg").empty();
        //  lzip.innerHTML = ""; laddr.innerHTML = "";
        taddr.disabled = false;
        taddr.focus();

    }
    if (vzip.length == 5 && taddr.innerHTML != "") {
        $("#btAddress").css("display", "block");
    }
}

function ValidateZipCode2() {
    var tzip = document.getElementById("txtZipCode3");
    var taddr = document.getElementById("txtAddress2");
    document.getElementById("txtAddress2").value = "";
    // $('#btAddress2').prop('disabled', true);
    var vzip = tzip.value;
    taddr.disabled = false;

    if (vzip == "" || vzip.length != 5) {

        taddr.disabled = true;
        document.getElementById("txtAddress2").value = "";
        // lzip.innerHTML = "Invalid Zip Code";
        tzip.style.border = "2px Red Solid";
        $("#btAddress").css("display", "none");
        $("#btContinue").css("display", "none");
        $("#serAddMsg2").empty();
        $("#serAddMsg2").append("Enter Zip Code with minimum 5 numerics");
        $("#serAddMsg2").css("color", "red");
        tzip.focus();
    } else {
        tzip.style.border = "2px Green Solid";
        $("#serAddMsg2").empty();
        //  lzip.innerHTML = ""; laddr.innerHTML = "";
        taddr.disabled = false;
        taddr.focus();

    }
    if (vzip.length == 5 && taddr.innerHTML != "") {
        $("#btAddress").css("display", "block");
    }
}

function ValidateZipCodeOrdernow() {
    var tzip = document.getElementById("txtZipCode2");
    var taddr = document.getElementById("txtAddress");
    document.getElementById("txtAddress").value = "";
    // $('#btAddress2').prop('disabled', true);
    var vzip = tzip.value;
    taddr.disabled = false;

    if (vzip == "" || vzip.length != 5) {

        taddr.disabled = true;
        document.getElementById("txtAddress").value = "";
        // lzip.innerHTML = "Invalid Zip Code";
        tzip.style.border = "2px Red Solid";
        $("#btAddress").css("display", "none");
        $("#serAddMsg").empty();
        $("#serAddMsg").append("Enter Zip Code with minimum 5 numerics");
        $("#serAddMsg").css("color", "red");
        tzip.focus();
    } else {
        tzip.style.border = "2px Green Solid";
        $("#serAddMsg").empty();
        //  lzip.innerHTML = ""; laddr.innerHTML = "";
        taddr.disabled = false;
        taddr.focus();

    }
    if (vzip.length == 5 && taddr.innerHTML != "") {
        $("#btAddress").css("display", "block");
    }
}
function showTabs(TabName) {

    var kwhC = "";
    GlobalTabName = TabName;

    var radios = document.getElementsByName('kwh_choice');

    for (var i = 0, length = radios.length; i < length; i++) {
        if (radios[i].checked) {


            kwhC = radios[i].value

            break;
        }
    }
    tbName = GlobalTabName;
    tbName = tbName.replace(" ", '');
    if ($("#IsPartner").val() != "y") {
        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName);
    }
    $('#loadingmessage').show();
    var serializeform = {
        tabId: TabName,
        kwh_Choice: kwhC
    };
    $.ajax({
        type: 'POST',
        data: serializeform,
        url: Produrl + "/Home/GetProductDetail",
        success: function (response) {
            $('#loadingmessage').hide();
            if (response) {

                if (response[0] != "") {
                    $("#signUpmsg").empty();
                    $("#signUpmsg").append(response[0]);
                }
                else {
                    $("#signUpmsg").empty();
                    //   $("#signUpmsg").append("Sign up for a new plan and get our Happiness Guarantee!");
                }
                if (response[1] != "") {
                    $("#chooseMsg").empty();
                    $("#chooseMsg").append(response[1]);
                }
                else {
                    $("#chooseMsg").empty();
                    //    $("#chooseMsg").append("Choose the electricity plan that’s right for you from a range of competitively priced plans designed to meet your needs. Plus, you’ll get our 60-Day Happiness Guarantee*.");
                }
                if (response[2] != "") {
                    $("#divInfo").empty();
                    $("#divInfo").append(response[2]);

                    $("#divInfo2").empty();
                    $("#divInfo2").append(response[2]);

                    $("#divInfoLanding").empty();
                    $("#divInfoLanding").append(response);
                }
                if (response[4] != "") {
                    $("#tabFooter").empty();
                    $("#tabFooter").append(response[4]);
                    sessionStorage.setItem("tabFooter", response[4]);
                }
                if (response[6] != "" && response[6] != null) {

                    var cPath = $("#hdnHdrImagePath").val() + response[6];
                    imageExists(imgSRC, function (exists) {
                        //Show the result
                        if (exists == false) {
                            $("#tabHeaderImage").hide();
                        }
                        else {
                            $("#tabHeaderImage").prop("src", cPath);
                        }

                    });

                    function imageExists(url, callback) {
                        var img = new Image();
                        img.onload = function () { callback(true); };
                        img.onerror = function () { callback(false); };
                        img.src = url;
                    }

                }

            } else {
                //  alert('failed');
            }
        },
    });

}
function showTabsDetails(pricePlanid, kwh_Choice) {
    $("#ProductDetailInfo").empty();
    InsertMileStone(tbName + ":ProductLearnMore:PPId" + pricePlanid);
    if (GlobalTabName == "") {
        // GlobalTabName = "Display All";
        // GlobalTabName = $("#hdnIdName").val();
    }
    var tabId = GlobalTabName;
    GlbpricePlanId = pricePlanid;
    $('#loadingmessage').show();
    if ($("#IsPartner").val() != "y") {
        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/ProductDetail");
    }
    var req = {
        pPId: pricePlanid,
        tabId: tabId,
        kwh_Choice: kwh_Choice
    };
    $.ajax({
        type: 'POST',
        data: req,
        url: Produrl + "/Home/GetProductDetail_Popup",
        success: function (response) {
            if (response) {

                $("#ProductDetailInfo").empty();
                $("#ProductDetailInfo").append(response);
                $('#loadingmessage').hide();


            } else {

            }
        },
    });
}
function GetProductsAccordingRates(Rate) {
    InsertMileStone(tbName + ":Rate" + Rate);
    var tabId = "";

    tabId = GlobalTabName;
    if ($("#IsPartner").val() != "y") {
        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/kwh" + Rate);
    }
    $('#loadingmessage').show();
    var serialize1 = {
        tabId: tabId,
        kwh_Choice: Rate
    };
    $.ajax({
        type: 'POST',
        data: serialize1,
        url: Produrl + "/Home/GetProductDetail",
        success: function (response) {

            if (response) {
                if (response[0] != "") {
                    $("#signUpmsg").empty();
                    $("#signUpmsg").append(response[0]);
                }
                else {
                    $("#signUpmsg").empty();
                    //  $("#signUpmsg").append("Sign up for a new plan and get our Happiness Guarantee!");
                }
                if (response[1] != "") {
                    $("#chooseMsg").empty();
                    $("#chooseMsg").append(response[1]);
                }
                else {
                    $("#chooseMsg").empty();
                    //   $("#chooseMsg").append("Choose the electricity plan that’s right for you from a range of competitively priced plans designed to meet your needs. Plus, you’ll get our 60-Day Happiness Guarantee*.");
                }
                if (response[2] != "") {
                    $("#divInfo").empty();
                    $("#divInfo").append(response[2]);

                    $("#divInfo2").empty();
                    $("#divInfo2").append(response[2]);
                }

                if (response[4] != "") {
                    $("#tabFooter").empty();
                    $("#tabFooter").append(response[4]);
                    sessionStorage.setItem("tabFooter", response[4]);
                }

                $('#loadingmessage').hide();

            } else {
                //  alert('failed');
            }
        },
    });
}
function checkPromocode() {
    var PromoCode = $("#txtPromocode").val();
    var ZipCode = $("#txtZipCode3").val();
    var serialize1 = {
        pcode: PromoCode,
    };
    $.ajax({

        type: 'GET',
        data: serialize1,
        url: Produrl + "/Home/CheckPromoValue",
        success: function (response) {
            var resp1 = response[0];
            var resp2 = response[1];

            if (resp1.toLowerCase() == "done") {
                sessionStorage.setItem("pc", PromoCode);
                if (resp2 != "") {
                    window.location.href = Produrl + "/Home/Index?PC=" + PromoCode + "&Zip=" + ZipCode + resp2;
                }

                else {
                    window.location.href = Produrl + "/Home/Index?PC=" + PromoCode + "&Zip=" + ZipCode;
                }
            }
            else {
                $("#lbPromoCodemsg").show();
                $("#lbPromoCodemsg").empty();
                $("#lbPromoCodemsg").append("The promo code entered is invalid or has expired. Please try again with a different promo code or close this window to continue viewing plans without a promo code.");
            }
        }
    });
    //var PromoCode = $("#txtPromocode").val();
    //if (PromoCode == "") {
    //    $("#lbPromoCodemsg").empty();
    //    $("#lbPromoCodemsg").append("Please enter any Promo Code");
    //    $("#lbPromoCodemsg").css('color', '#E15739');
    //}
    //else {
    //    $('#loadingmessage').show();
    //    var serialize1 = {
    //        PromoCode: PromoCode,
    //    };
    //    $.ajax({
    //        type: 'POST',
    //        data: serialize1,
    //        url: Produrl + "/Home/CheckPromoCode",
    //        success: function (response) {
    //            if (response) {
    //                if (response != "Valid") {
    //                    $("#lbPromoCodemsg").empty();
    //                    $("#lbPromoCodemsg").append(response);
    //                    $("#lbPromoCodemsg").css('color', '#E15739');
    //                }
    //                else {
    //                    $("#lbPromoCodemsg").empty();
    //                    $("#lbPromoCodemsg").append("Your Promo code is valid");
    //                    $("#lbPromoCodemsg").css('color', 'green');
    //                }
    //                $('#loadingmessage').hide();
    //            }
    //        },
    //    });
    //}
}
function addressPopUpReset() {
    $("#serAddMsg").empty();

    //$("#txtZipCode3").val($("#lbzipcode").text());
    //$("#txtZipCode2").val($("#lbzipcode").text());
    //$("#txtZipCode3").val($("#lbzipcode").text());
    //$("#txtZipCode2").val($("#lbzipcode").text());
    $("#txtZipCode3").val(gbZIPCode);
    // $("#txtZipCode2").val(gbZIPCode);
    $("#txtZipCode3").val(gbZIPCode);
    // $("#txtZipCode2").val(gbZIPCode);
}

function showTabsFirstLoad() {
    // var tabId = 1;

    //  var tabId = "Display All";
    var tabId = "Recommended";
    Glbtabid = tabId;
    GlobalTabName = tabId;
    // var kwhC = 500;
    var kwhC = $("#kwhc").val();
    if (kwhC == "500") {
        $('#500kwh').prop('checked', true);
    }
    else if (kwhC == "1000") {
        $('#1000kwh').prop('checked', true);
    }
    else if (kwhC == "2000") {
        $('#2000kwh').prop('checked', true);
    }
    // window.history.replaceState("object or string", "Title", Produrl + "/Home/DisplayAll/kwh" + kwhC);

    //$('#loadingmessage').show();
    var serialize = {
        tabId: tabId,
        kwh_Choice: kwhC,
        firstLoad: 'Y'
    };
    $.ajax({
        type: 'POST',
        data: serialize,
        url: Produrl + "/Home/GetProductDetail",
        success: function (response) {
            if (response) {

                $("#TabsInfo").addClass("nav-link active");
                if (response[0] != "") {
                    $("#signUpmsg").empty();
                    $("#signUpmsg").append(response[0]);
                }
                else {
                    $("#signUpmsg").empty();
                    //    $("#signUpmsg").append("Sign up for a new plan aarnd get our Happiness Guarantee!");
                }
                if (response[1] != "") {
                    $("#chooseMsg").empty();
                    $("#chooseMsg").append(response[1]);
                }
                else {
                    $("#chooseMsg").empty();
                    //     $("#chooseMsg").append("Choose the electricity plan that’s right for you from a range of competitively priced plans designed to meet your needs. Plus, you’ll get our 60-Day Happiness Guarantee*.");
                }
                if (response[2] != "") {
                    $("#divInfo").empty();
                    $("#divInfo").append(response[2]);

                    $("#divInfo2").empty();
                    $("#divInfo2").append(response[2]);
                }

                if (response[3] != "" && response[3] != null) {
                    gbZIPCode = response[3];
                    sessionStorage.setItem("TDSPCODE", response[8]);
                    sessionStorage.setItem("TDSPName", response[5]);
                    sessionStorage.setItem("ZipCode", gbZIPCode);
                    var partnerV = response[9];
                    // document.getElementById("lbzipcode").textContent = response[3];
                    sessionStorage.setItem("IsPartner", partnerV);
                    sessionStorage.setItem("PartnerQueryString", response[10]);
                    if (partnerV != "") {

                        $("#IsPartner").val("y");

                    }
                    if ($("#IsPartner").val() != "y") {
                        window.history.replaceState("object or string", "Title", Produrl + "/Home/Recommended/kwh" + kwhC);
                    }
                    sessionValue();
                }
                if (response[4] != "") {
                    //$("#tabFooter").empty();
                    //$("#tabFooter").append(response[4]);
                    if (response[4] != null) {
                        $("#tabFooter").empty();
                        $("#tabFooter").append(response[4]);
                        sessionStorage.setItem("tabFooter", response[4]);
                    }
                    else {
                        sessionStorage.setItem("tabFooter", "");
                    }
                }
                if (response[5] != "") {

                    //TDSPCODE
                }
                if (response[6] != "") {

                    //var imagePath = $("#hdnHdrImagePath").val();
                    //imagePath = imagePath + response[6];

                    //$("#tabHeaderImage").attr("src", imagePath);

                }
                if (response[8] != "") {
                    gbZIPCode = response[3];
                    sessionStorage.setItem("TDSPCODE", response[8]);
                    sessionStorage.setItem("TDSPName", response[5]);
                    sessionStorage.setItem("ZipCode", gbZIPCode);
                    var partnerV = response[9];
                    // document.getElementById("lbzipcode").textContent = response[3];
                    sessionStorage.setItem("IsPartner", partnerV);
                    sessionStorage.setItem("PartnerQueryString", response[10]);
                    if (partnerV != "") {

                        $("#IsPartner").val("y");

                    }
                    if ($("#IsPartner").val() != "y") {
                        window.history.replaceState("object or string", "Title", Produrl + "/Home/Recommended/kwh" + kwhC);
                    }
                    sessionValue();
                }
                if (response[7] != "Yes") {
                    $('#myAddressZipModal2').modal('show');
                    $('#closeBt').hide();
                    if ($('#txtZipCode3').val() == '') {
                        $('#btAddress2').hide();
                    }
                    else {
                        $('#btAddress2').show();
                    }
                }

            } else {
                alert('failed');
            }
            $('#loadingmessage').hide();
        },
    });
    return false;

}
function load_AllSelectedInformation(address) {

    var userAddress = $("#txtAddress").val();
    var NewZipCode = $("#txtZipCode2").val();

    $('#loadingmessage2').show();
    var req = {
        address: userAddress,
        NewZipCode: NewZipCode
    }
    $.ajax({
        type: 'POST',
        data: req,
        url: Produrl + "/Home/GetSelectedProductDetails",
        success: function (response) {
            if (response) {

                if (response == "NewZip") {
                    $('#loadingmessage').hide();
                    $('.loadingmessage2').hide();
                    //     InsertMileStone("OrderNow:ZipChange:" + NewZipCode);
                    //    SetAddressZip(userAddress, NewZipCode, GlobalTabName);
                    //$("#myAddressModal").modal('hide');

                    $('#myDifferntTDSPMsg').modal({
                        backdrop: 'static',
                        keyboard: false
                    })
                    $('#myDifferntTDSPMsg').modal('show');
                    $('#myAddressModal').css({ 'opacity': 0.5 });

                }
                else {
                    var value = address;
                    var req = {
                        address: userAddress,
                        zipcode: NewZipCode
                    };
                    $.ajax({
                        type: 'POST',
                        data: req,
                        url: Produrl + "/Home/SetServiceAddress",
                        success: function (response) {
                            if (response) {
                                //sessionValue();
                                if (response[2] == "Error") {
                                    $("#serAddMsg").empty();
                                    $("#serAddMsg").append("We are not serving this territory at this time.");
                                    $("#serAddMsg").css("color", "red");
                                    //  document.getElementById("lbzipcode").innerHTML = zipcode;
                                    //$('#btAddress2').prop('disabled', true);
                                    $('#loadingmessage').hide();
                                    $('#loadingmessage2').hide();
                                }
                                else {
                                    if (response[4] != "" && response[4] != null) {
                                        if (response[3] == "Mismatch") {
                                            $('#myDifferntTDSPMsg').modal({
                                                backdrop: 'static',
                                                keyboard: false
                                            })
                                            $('#myDifferntTDSPMsg').modal('show');
                                            $('#myAddressZipModal2').css({ 'opacity': 0.7 });
                                            $('#myAddressModal').css({ 'opacity': 0.7 });
                                            $("#serAddMsg").empty();
                                            $("#serAddMsg").css("color", "red");
                                            $('#loadingmessage2').hide();
                                        }
                                        else {
                                            $("#serAddMsg").empty();
                                            $("#serAddMsg").append("We couldn't find your service address. Please check your street abbreviation and try again.");
                                            $("#serAddMsg").css("color", "red");
                                            $('#loadingmessage').hide();
                                            $('#loadingmessage2').hide();
                                        }
                                        return false;
                                    }
                                    else {
                                        value = value + "::ESIID:" + response[6];
                                        var req = {
                                            MileStone: "AddressSelected",
                                            value: value
                                        };
                                        $.ajax({
                                            type: 'POST',
                                            data: req,
                                            url: Produrl + "/Home/InsertMileStone",
                                            success: function (response) {
                                            },
                                        });

                                        sessionValue();
                                        $("#myAddressModal").modal('hide');
                                        $('#loadingmessage2').hide();
                                        $('#loadingmessage').hide();
                                        window.location.href = Produrl + "/Home/Enrollment";
                                    }
                                }
                            }
                        },
                    });
                }
                // $('#loadingmessage').hide();

            }
        },
    });
}
function fillData() {

    $("#divPlanDetail").append(AllInfo);
    var isPartner = sessionStorage.getItem("IsPartner");
    if (isPartner != null) {
        $("#IsPartner").val("y");
    }

    TDSPCode = sessionStorage.getItem("TDSPCODE");

    TDSPName = sessionStorage.getItem("TDSPName");
    TDSPZip = sessionStorage.getItem("ZipCode");
    if (TDSPCode != null) {
        $("#hdnTDSPCode").val(TDSPCode);
        sessionStorage.setItem("TDSPCODE", TDSPCode);
    }
    if (TDSPName != null) {
        $("#hdnTDSPName").val(TDSPName);
    }
    if (TDSPZip != null) {
        $("#hdnTDSPZip").val(TDSPZip);
    }
}
function sessionValue() {

    var isPartner = sessionStorage.getItem("IsPartner");
    if (isPartner != null) {
        $("#IsPartner").val("y");
    }
    TDSPCode = sessionStorage.getItem("TDSPCODE");
    if (sessionStorage.getItem("TDSPName") != "null") {

        TDSPName = sessionStorage.getItem("TDSPName");
    }
    else {
        TDSPName = "";
    }
    TDSPZip = sessionStorage.getItem("ZipCode");

    if (TDSPCode != null) {
        $("#hdnTDSPCode").val(TDSPCode);
    }
    if (TDSPName != null) {
        $("#hdnTDSPName").val(TDSPName);
        $("#lbzipcode").text(TDSPName);
    }
    if (TDSPZip != null) {
        $("#hdnTDSPZip").val(TDSPZip);
    }
    var tabFooter = sessionStorage.getItem("tabFooter");

    if (tabFooter != null) {
        $("#tabFooter").empty();
        $("#tabFooter").append(tabFooter);
        //  $("#tabFooter2").empty();
        // $("#tabFooter2").append(tabFooter);

    }
}
function load_AllEditSelectedInformation(address) {

    var userAddress = $("#txtAddress").val();
    var NewZipCode = $("#txtZipCode2").val();
    $('#loadingmessage2').show();
    var req = {
        address: userAddress,
        NewZipCode: NewZipCode
    }
    $.ajax({
        type: 'POST',
        data: req,
        url: Produrl + "/Home/EditServiceAddress",
        success: function (response) {
            console.log(response);

            if (response) {
                document.getElementById("serviceAddress").innerHTML = response[0];
                document.getElementById("ESID").innerHTML = response[1];
                $("#myAddressModal").modal('hide');
            }
        },
    });
}

function ChangeServiceAddress() {
    var userAddress = $("#txtAddress").val();
    var zipcode = $("#txtZipCode2").val();
    var ser = {
        userAddress: userAddress,
        zipcode: zipcode
    };
    $('#loadingmessage2').show();
    $.ajax({
        type: 'POST',
        data: ser,
        url: Produrl + "/Home/EditServiceAddress",
        success: function (response) {
            if (response) {
                document.getElementById("serviceAddress").innerHTML = response[0];
                document.getElementById("ESID").innerHTML = response[1];
                $("#myAddressModal").modal('hide');
            }
            else {
            }
        },
    });
}
function EditChangeServiceAddress() {

    var userAddress = $("#txtAddress").val();
    var zipcode = $("#txtZipCode2").val();
    $("#loading_message").show();
    //$("#loadingmessage").show();
    var ser = {
        userAddress: userAddress,
        zipcode: zipcode
    };
    $("#btnhide").hide();
    $.ajax({
        type: 'POST',
        data: ser,
        url: Produrl + "/Home/UpdateEditServiceAddress",
        success: function (response) {
            $("#loading_message").hide();
            //$("#loadingmessage").hide();
            if (response != null) {
                var response1 = response.UserInfo;
                var response2 = response.ProductBox;
                $("#myAddressModal").modal('show');
                if (response2 == "NewZip") {
                    $("#EditModal").modal('show');
                }
                else {
                    $("#myAddressModal").modal('hide');
                    $("#btAddress").css("display", "none");
                    document.getElementById("serviceAddress").innerHTML = response1[0];
                    document.getElementById("ESID").innerHTML = response1[1];
                }

            } else {
            }
        },
    });
}
function CheckSelectedAddress(pricePlanId, productId) {

    $('#changethezip').hide();
    $('#ordernow').show();
    $('#zippMsg').hide();
    $('#ordernowMsg').show();
    $('#loadingmessage').show();
    InsertMileStone(tbName + ":OrderNow:Product:" + productId);
    $("#serAddMsg").empty();
    $("#btAddress").css("display", "none");
    $("#btContinue").css("display", "none");
    var req = {
        productId: productId,
        pricePlanId: pricePlanId
    };
    $.ajax({
        type: 'POST',
        data: req,
        url: Produrl + "/Home/CheckSelAddress",
        success: function (response) {
            
            if (response) {

                $("#ProductDetails").modal('hide');

                if (response[1] == "No" && response[0] != "No") {
                    $('#loadingmessage').hide();
                    $("#txtZipCode2").val(gbZIPCode);
                    // $("#txtZipCode2").val(response[0]);
                    $("#myAddressModal").modal('show');

                    if ($("#IsPartner").val() != "y") {
                        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/OrderNow");
                    }
                }
                else if (response[0] == "No" && response[1] == "No") {
                    $('#loadingmessage').hide();
                    $("#myAddressModal").modal('show');
                    //$("#txtZipCode2").val();

                    //document.getElementById("txtZipCode2").value = '';
                    if ($("#IsPartner").val() != "y") {
                        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/OrderNow");
                    }
                    //document.location. = "asdfasfa";
                }
                else {
                    var NewZipCode = response[0];
                    var address = response[1];
                    var esiid = response[2];
                    var value = address + "::ESIID:" + esiid;
                    var req = {
                        MileStone: "AddressSelected",
                        value: value
                    };
                    $.ajax({
                        type: 'POST',
                        url: Produrl + "/Home/InsertMileStone",
                        data: req,
                        success: function (response) {

                        },
                    });
                    var req1 = {
                        address: address,
                        NewZipCode: NewZipCode
                    };
                    $.ajax({
                        type: 'POST',
                        data: req1,
                        url: Produrl + '/Home/GetSelectedProductDetails',
                        success: function (response) {
                            if (response) {
                                sessionValue();
                                $('#loadingmessage').hide();
                                window.location.href = Produrl + "/Home/Enrollment";
                                $("#myAddressModal").modal('hide');

                            }
                        },
                    });
                }
            } else {

            }

        },
    });
}
function SetAddressZip(address, zipcode, tbname) {
    // var tzip = document.getElementById("txtZipCode2");
    $('#changethezip').show();
    $('#zippMsg').show();
    $('#ordernowMsg').hide();
    var vzip = zipcode;
    if (vzip == "" || vzip.length != 5) {
        taddr.disabled = true;
        document.getElementById("txtAddress").value = "";
        // lzip.innerHTML = "Invalid Zip Code";
        tzip.style.border = "2px Red Solid";
        $("#btAddress").css("display", "none"); es
        $("#btContinue").css("display", "none");
        $("#serAddMsg").empty();

        $("#serAddMsg").append("Enter Zip Code with minimum 5 numerics");
        $("#serAddMsg").css("color", "red");
        tzip.focus();
    }
    else {
        var req = {
            address: address,
            zipcode: zipcode
        };
        $.ajax({
            type: 'POST',
            data: req,
            url: Produrl + '/Home/SetServiceAddress',
            success: function (response) {
                if (response) {
                    changethezip = "y";
                    if (response[2] == "Error") {
                        $("#serAddMsg2").empty();
                        $("#serAddMsg2").append("We are not serving this territory at this time.");
                        $("#serAddMsg2").css("color", "red");
                        // document.getElementById("lbzipcode").innerHTML = zipcode;
                        //$('#btAddress2').prop('disabled', true);
                    }
                    else {
                        //$("#TabsInfo").removeClass("nav-link active");
                        // $("#TabsInfo").addClass("active");
                        gbZIPCode = response[1];
                        document.getElementById("lbzipcode").innerHTML = response[5];
                        // document.getElementById("lbzipcode").innerHTML = response[1];
                        $("#txtAddress2").text(address);
                        if (response[2] == "New") {
                            if (firstLoad != 'Y') {
                                $("#success-alert").show();
                                $("#success-alert").fadeOut(8000);
                            }
                            else {
                                firstLoad = '';
                            }
                            $("#txtZipCode2").val(response[1]);
                            // $("#txtAddress").val(response[6]);
                            sessionStorage.setItem("TDSPCODE", response[7]);
                            sessionStorage.setItem("TDSPName", response[8]);

                            sessionStorage.setItem("ZipCode", zipcode);
                            sessionValue();
                        }

                        if (response == "") {
                            $('#btAddress2').prop('disabled', true);
                        }
                        if (response[4] != "" && response[4] != null) {
                            if (response[3] == "Mismatch") {
                                //$('#myDifferntTDSPMsg').modal({
                                //    backdrop: 'static',
                                //    keyboard: false
                                //})
                                //$('#myDifferntTDSPMsg').modal('show');
                                //$("#serAddMsg2").empty();
                                //$("#serAddMsg2").css("color", "red");
                                $('#loadingmessage2').hide();
                            }
                            else {
                                $("#serAddMsg2").empty();
                                $("#serAddMsg2").append("We couldn't find your service address. Please check your street abbreviation and try again.");
                                $("#serAddMsg2").css("color", "red");
                                $('#loadingmessage').hide();
                            }
                            //return false;
                        }
                        else {
                            $('#btAddress2').prop('disabled', false);
                        }

                        $("#myAddressZipModal2").modal('hide');
                        showTabs(GlobalTabName);
                        //if (ltbName == "") {

                        //}
                        //else {
                        //    $("#TabsInfo").removeClass("active");

                        //     //showTabs(ltbName);
                        //    showTabs(ltbName);
                        //}
                        //if (tbname == '') {
                        //    $("#TabsInfo").removeClass("nav-link active");
                        //    showTabs(ltbName);
                        //}
                        //else {

                        //}

                    }
                }
            },
        });
    }
}
function CheckAllClick() {

    var chkAccept = document.getElementById("chkAccept");
    var chkAutorizedPerson = document.getElementById("IsTCPAAgreed");
    var chkAuthorizeSwitch = document.getElementById("chkAutorizedSwitch");
    var chkAll = document.getElementById("chkAll");
    if (chkAll.checked) {
        chkAccept.checked = chkAutorizedPerson.checked = true;
        if (chkAuthorizeSwitch != null)
            chkAuthorizeSwitch.checked = true;
    }
    else {
        chkAccept.checked = chkAutorizedPerson.checked = false;
        if (chkAuthorizeSwitch != null)
            chkAuthorizeSwitch.checked = false;
    }
}

$(function () {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = mm + '/' + dd + '/' + yyyy;

    $("#SysDate_HF").val(today);

    var ddlExpMonth = $("#ddlExpMonth");

    ddlExpMonth.empty().append('<option selected="selected" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "GET",
        url: Produrl + "/Home/GetMonths",
        success: function (response) {

            ddlExpMonth.empty().append('<option selected="selected" value="0">Please select</option>');

            $.each(response, function () {
                ddlExpMonth.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            //   alert(response.responseText);
        },
        error: function (response) {
            //    alert(response.responseText);
        }
    });
    var ddlExpYear = $("#ddlExpYear");

    ddlExpYear.empty().append('<option selected="selected" value="0" disabled = "disabled">Loading.....</option>');
    $.ajax({
        type: "GET",
        url: Produrl + "/Home/GetYears",
        dataType: "json",
        success: function (response) {
            ddlExpYear.empty().append('<option selected="selected" value="0">Please select</option>');
            $.each(response, function () {
                ddlExpYear.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        },
        failure: function (response) {
            //   alert(response.responseText);
        },
        error: function (response) {
            //    alert(response.responseText);
        }
    });
});
$(function () {
    //var dtToday = new Date();

    //var month = dtToday.getMonth() + 1;
    //var day = dtToday.getDate() + 1;
    //var year = dtToday.getFullYear();
    //if (month < 10)
    //    month = '0' + month.toString();
    //if (day < 10)
    //    day = '0' + day.toString();

    //var maxDate = year + '-' + month + '-' + day;



    //$('#ddlSwitchDates').attr('min', maxDate);
});

function Validation() {

    var FirstName_flag = 0;
    var MiddleName_flag = 0;
    var LastName_flag = 0;
    var CellNo_flag = 0;
    var AlternateCellNo_flag = 0;
    var EmailId_flag = 0;
    var Email2_flag = 0;
    var Promocode_flag = 0;
    var IsPLessBilling_flag = 0;
    var IsBilling_flag = 0;
    var StreetName_flag = 0;
    var StreetNo_flag = 0;
    var APT_flag = 0;
    var City_flag = 0;
    var State_flag = 0;
    var ZipCode_flag = 0;
    var IsPoBox_flag = 0;
    var PoBox_flag = 0;
    var PreferedLanguage_flag = 0;
    var LocationByName_flag = 0;
    var switchDate_flag = 0;
    var CreditCheck_flag = 0;
    var SSN_flag = 0;
    var SSN2_flag = 0;
    var SSN3_flag = 0;
    var DateOfBirth_flag = 0;
    var CreditAutoPay_flag = 0;
    var CreditCardType_flag = 0;
    var CreditCardNumber_flag = 0;
    var CreditCardHolder_flag = 0;
    var BillingZipCode_flag = 0;
    var ExpirationMonth_flag = 0;
    var ExpirationYear_flag = 0;
    var CVVCode_flag = 0;
    var PaymentMethod_flag = 0;
    var ddlContactOptions_flag = 0;
    var Chkall_flag = 0;
    var Chkall_flag2 = 0;
    var reenterdEmailId_flag = "";
    var StreetNamePrevious_flag2 = 0;
    var APTPrevious_flag2 = 0;
    var CityPrevious_flag2 = 0;
    var StatePrevious_flag2 = 0;
    var ZipCodePrevious_flag2 = 0;
    var txtauthUserFirstName_Flag = 0;
    var txtauthUserLastName_Flag = 0;
    var txtauthUserPhoneNumber_Flag = 0;
    var NameOnBankAccount_Flag = 0;
    var RoutingNumber_Flag = 0;
    var AccountNumber_Flag = 0;
    var AccountType_Flag = 0;
    var GovIdNumber_Flag = 0;

    if ($("#txtName").val() == "") {


        FirstName_flag = 1;

        $("#txtName").css('border-color', 'red');
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        FirstName_flag = 0;
        $("#txtName").css('border-color', '')
    }
    if ($("#txtLName").val() == "") {
        LastName_flag = 1;

        $("#txtLName").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        LastName_flag = 0;
        $("#txtLName").css('border-color', '')
    }
    if ($("#txtCell1").val() == "") {
        CellNo_flag = 1;

        $("#txtCell1").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        CellNo_flag = 0;
        $("#txtCell1").css('border-color', '')
    }
    if ($("#txtEmail").val() == "") {

        EmailId_flag = 1;
        $("#txtEmail").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        var testEmail = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
        if (testEmail.test($("#txtEmail").val())) {
            EmailId_flag = 0;
            $("#txtEmail").css('border-color', '')
        }
        else {
            EmailId_flag = 1;

            $("#txtEmail").css('border-color', 'red')
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }


    }
    if ($("#txtEmail2").val() == "") {
        Email2_flag = 1;

        //   $("#txtEmail2").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {

        var testEmail = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i;
        if (errormsg == "") {
            Email2_flag = 0;
            errormsg = "Please Enter Valid Re-enter EmailId";


        }
        else {
            errormsg += "<br /> Please Valid Enter Re-enter EmailId";
        }
        if (testEmail.test($("#txtEmail").val())) {
            if ($("#txtEmail2").val() != $("#txtEmail").val()) {
                //   $("#txtEmail2").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
            }
            else {
                $("#txtEmail2").css('border-color', '')
            }
        }
        else {
            Email2_flag = 1;

            $("#txtEmail2").css('border-color', 'red')
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }

    }

    var isBilliingSame = document.getElementById("rbBillingSameYes").checked;
    if (isBilliingSame == true) {
        IsBilling_flag = 0;
        $("#txtStreetNo_Billing").css('border-color', '');
        $("#txtStreetName_Billing").css('border-color', '');
        $("#txtAptNo").css('border-color', '');
        $("#txtCity_Billing").css('border-color', '');
        $("#ddlState").css('border-color', '');
    }

    else {
        var isPOBoxchk = document.getElementById("chkPOBox").checked;
        if (isPOBoxchk == true) {
            if ($("#txtPOBoxNo").val() == "") {
                IsPoBox_flag = 1;
                //        $("#txtPOBoxNo").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
                $(".txtPOBoxNoEror").show();
                $("#txtPOBoxNo").css('border-color', 'red');

            }
            else {
                IsPoBox_flag = 0;
                $(".txtPOBoxNoEror").hide();
                $("#txtPOBoxNo").css('border-color', '');
            }
            if ($("#ddlState").val() == "") {
                State_flag = 1;
                //    $("#ddlState").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                State_flag = 0;
                $("#ddlState").css('border-color', '');
            }
            if ($("#txtZip_Billing").val() == "") {
                ZipCode_flag = 1;
                $("#txtZip_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                ZipCode_flag = 0;
                $("#txtZip_Billing").css('border-color', '');
            }
        }
        else {
            if ($("#txtStreetNo_Billing").val() == "") {
                StreetNo_flag = 1;
                $("#txtStreetNo_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                StreetNo_flag = 0;
                $("#txtStreetNo_Billing").css('border-color', '');
            }

            if ($("#txtStreetName_Billing").val() == "") {
                StreetName_flag = 1;
                //     $("#txtStreetName_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                StreetName = 0;
                $("#txtStreetName_Billing").css('border-color', '');
            }

            if ($("#txtAptNo_Billing").val() == "") {

                APT_flag = 1;
                //   $("#txtAptNo_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                APT_flag = 0;
                $("#txtAptNo_Billing").css('border-color', '');
            }

            if ($("#txtCity_Billing").val() == "") {
                City_flag = 1;
                $("#txtCity_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                City_flag = 0;
                $("#txtCity_Billing").css('border-color', '');
            }

            if ($("#ddlState").val() == "") {
                State_flag = 1;
                $("#ddlState").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                State_flag = 0;
                $("#ddlState").css('border-color', '');
            }
            if ($("#txtZip_Billing").val() == "") {
                ZipCode_flag = 1;
                $("#txtZip_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                ZipCode_flag = 0;
                $("#txtZip_Billing").css('border-color', '');
            }
        }
    }
    var Ischecked = document.getElementById("I'm Moving").checked;
    if (Ischecked == true) {

        LocationByName_flag = 0;
        //            document.getElementById("chkLoc").style.color = "";


        if ($("#txtStreetName_BillingPrevious").val() == "") {
            StreetNamePrevious_flag2 = 1;
            $("#txtStreetName_BillingPrevious").css('border-color', 'red');
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            StreetNamePrevious_flag2 = 0;
            $("#txtStreetName_BillingPrevious").css('border-color', '');
        }

        //if ($("#txtAptNo_BillingPrevious").val() == "") {

        //    APTPrevious_flag2 = 1;
        //    $("#txtAptNo_BillingPrevious").css('border-color', 'red');
        //    $("#errormsg").text("* Please check all mandatory fields");
        //    $("#errormsg").css('color', 'red');
        //}
        //else {
        //    APTPrevious_flag2 = 0;
        //    $("#txtAptNo_BillingPrevious").css('border-color', '');
        //}

        if ($("#txtCity_BillingPrevious").val() == "") {
            CityPrevious_flag2 = 1;
            $("#txtCity_BillingPrevious").css('border-color', 'red');
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            CityPrevious_flag2 = 0;
            $("#txtCity_BillingPrevious").css('border-color', '');
        }

        if ($("#ddlStatePrevious").val() == "") {
            State_flag2 = 1;
            $("#ddlStatePrevious").css('border-color', 'red');
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            StatePrevious_flag2 = 0;
            $("#ddlStatePrevious").css('border-color', '');
        }
        if ($("#txtZip_BillingPrevious").val() == "") {
            ZipCodePrevious_flag2 = 1;
            $("#txtZip_BillingPrevious").css('border-color', 'red');
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            ZipCodePrevious_flag2 = 0;
            $("#txtZip_BillingPrevious").css('border-color', '');
        }

    }
    else {
        IsBilling_flag = 0;
        $("#txtStreetName_BillingPrevious").css('border-color', '');
        $("#txtAptNoPrevious").css('border-color', '');
        $("#txtCity_BillingPrevious").css('border-color', '');
        $("#ddlStatePrevious").css('border-color', '');
        $("#txtZip_BillingPrevious").css('border-color', '');
    }


    if ($("#ddlSwitchDates").val() == "") {

        switchDate_flag = 1;
        $('#ddlSwitchDates').attr('style', 'border-color:red !important');
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {

        switchDate_flag = 0;
        $("#ddlSwitchDates").css('border-color', '')
    }
    SocialSecurity == $("#vType").val();

    if (SocialSecurity == "SSN") {
        if ($("#IdentityVerification").css('display') == 'none') {
            SSN_flag = 0;
            SSN2_flag = 0;
            SSN3_flag = 0;
            $("#txtSSN1").css('border-color', '');
            $("#txtSSN2").css('border-color', '');
            $("#txtSSN3").css('border-color', '');

        }
        else {

            if ($("#txtSSN1").val().length < 3) {
                SSN_flag = 1;

                $("#txtSSN1").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                SSN_flag = 0;

                $("#txtSSN1").css('border-color', '')
            }
            if ($("#txtSSN2").val().length < 2) {
                SSN2_flag = 1;

                $("#txtSSN2").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                SSN2_flag = 0;
                $("#txtSSN2").css('border-color', '')
            }
            if ($("#txtSSN3").val().length < 4) {
                SSN3_flag = 1;

                $("#txtSSN3").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                SSN3_flag = 0;
                $("#txtSSN3").css('border-color', '')
            }

        }
    }
    else {
        SSN_flag = 0;
        SSN2_flag = 0;
        SSN3_flag = 0;
        $("#txtSSN1").css('border-color', '')
        $("#txtSSN2").css('border-color', '')
        $("#txtSSN3").css('border-color', '')

        if ($("#txtIdNumber").val() == "") {
            SSN_flag = 1;

            $("#txtIdNumber").css('border-color', 'red')
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            SSN_flag = 0;

            $("#txtIdNumber").css('border-color', '')
        }

    }



    if ($("#DateofBirth").val() == "") {

        DateOfBirth_flag = 1;
        $('#DateofBirth').attr('style', 'border-color:red !important');
        // $("#DateofBirth").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {

        var v2 = document.getElementById('DateofBirth');
        var date = new Date();
        date2 = new Date(v2.value),
        d1 = date2.getTime(),
        //d2 = new Date('1/1/1920').getTime(),
        d2 = new Date('1/1/1895').getTime(),
        d3 = new Date('1/1/2001').getTime();


        if (d1 > d2 && d1 < d3) {
            DateOfBirth_flag = 0;
            $("#DateofBirth").css('border-color', '')
        } else {
            $("#DateofBirth").css('border-color', 'red')
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }

    }

    var el = document.getElementById("ddlPaymentMethod2");

    var autoP = document.getElementById("AutoPay").checked;
    if (autoP == true) {

        if (PayBy == 'Credit') {

            if ($("#txtCreditCardNo").val() == "") {
                CreditCardNumber_flag = 1;
                $("#txtCreditCardNo").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                CreditCardNumber_flag = 0;
                $("#txtCreditCardNo").css('border-color', '');
            }

            var vv = $("#txtNameOnCard");

            if (vv.val() == "") {

                CreditCardHolder_flag = 1;
                $("#txtNameOnCard").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                CreditCardHolder_flag = 0;
                $("#txtNameOnCard").css('border-color', '')
            }

            if ($("#txtBillingZipCode").val() == "") {
                BillingZipCode_flag = 1;
                $("#txtBillingZipCode").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                BillingZipCode_flag = 0;
                $("#txtBillingZipCode").css('border-color', '');
            }

            if ($("#ddlExpMonth").val() == "0") {

                ExpirationMonth_flag = 1;
                $("#ddlExpMonth").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                ExpirationMonth_flag = 0;
                $("#ddlExpMonth").css('border-color', '')
            }

            if ($("#ddlExpYear").val() == "0") {
                ExpirationYear_flag = 1;
                $("#ddlExpYear").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                ExpirationYear_flag = 0;
                $("#ddlExpYear").css('border-color', '')
            }

            if ($("#txtCvcCode").val() == "") {
                CVVCode_flag = 1;
                $("#txtCvcCode").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                CVVCode_flag = 0;
                $("#txtCvcCode").css('border-color', '')
            }
        }

        if (PayBy == 'ECheck') {
            $("#txtNameOnCard").css('border-color', '');
            $("#txtBillingZipCode").css('border-color', '');
            $("#txtCreditCardNo").css('border-color', '')
            $("#ddlExpYear").css('border-color', '');
            $("#ddlExpMonth").css('border-color', '');
            $("#txtBillingZipCode").css('border-color', '')
            if ($("#AccountName").val() == "") {
                BillingZipCode_flag = 1;
                $("#AccountName").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                BillingZipCode_flag = 0;
                $("#AccountName").css('border-color', '')
            }
            if ($("#AccountNumber").val() == "") {
                BillingZipCode_flag = 1;
                $("#AccountNumber").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                BillingZipCode_flag = 0;
                $("#AccountNumber").css('border-color', '')
            }

            if ($("#AccountType").val() == "0") {
                BillingZipCode_flag = 1;
                $("#AccountType").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                BillingZipCode_flag = 0;
                $("#AccountType").css('border-color', '')
            }
            if ($("#RoutingNumber").val() == "") {
                BillingZipCode_flag = 1;
                $("#RoutingNumber").css('border-color', 'red')
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                BillingZipCode_flag = 0;
                $("#RoutingNumber").css('border-color', '')
            }


        }


    }
    else {
        //   $("#lbAutoPay").css('color', 'red');
        // flag = 1;
        PayBy = "Cash"
    }

    //if ($("#chkAll").is(":checked")) {
    //    Chkall_flag = 0;
    //    document.getElementById("spp").style.color = "";
    //}
    //else if ($("#chkAll").is(":not(:checked)")) {
    //    Chkall_flag = 1;
    //    $("#spp").css('color', 'red');
    //    $("#errormsg").text("* Please check all mandatory fields");
    //    $("#errormsg").css('color', 'red');
    //}
    //else {
    //    Chkall_flag = 0;
    //    document.getElementById("spp").style.color = "";
    //}
    //if ($("#IsTCPAAgreed").is(":checked")) {
    //    Chkall_flag = 0;
    //    document.getElementById("spp").style.color = "";
    //}
    //else if ($("#IsTCPAAgreed").is(":not(:checked)")) {
    //    Chkall_flag = 1;
    //    $("#spp1").css('color', 'red');
    //    $("#errormsg").text("* Please check all mandatory fields");
    //    $("#errormsg").css('color', 'red');
    //}
    //else {
    //    Chkall_flag = 0;
    //    document.getElementById("spp1").style.color = "";
    //}


    if ($("#chkAutorizedSwitch").is(":checked")) {

        Chkall_flag2 = 0;
        document.getElementById("spp").style.color = "";
    }
    else if ($("#chkAutorizedSwitch").is(":not(:checked)")) {
        Chkall_flag2 = 1;
        $("#spp").css('color', 'red');
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        Chkall_flag2 = 0;
        document.getElementById("spp").style.color = "";
    }

    var authU = document.getElementById("AnotherAuthUser").checked;
    if (authU == true) {
        if ($("#txtautUserFirstName").val() == "") {
            txtauthUserFirstName_Flag
            $("#txtautUserFirstName").css('border-color', 'red')
            $("#txtautUserFirstName").text("* Please check all mandatory fields");

        }
        else {
            txtauthUserFirstName_Flag = 0;
            $("#txtautUserFirstName").css('border-color', '')
        }
        if ($("#txtautUserLastName").val() == "") {
            txtauthUserLastName_Flag = 1;
            $("#txtautUserLastName").css('border-color', 'red')
            $("#txtautUserLastName").text("* Please check all mandatory fields");

        }
        else {
            txtauthUserLastName_Flag = 0;
            $("#txtautUserLastName").css('border-color', '')
        }

        if ($("#txtautUserContactNumber").val() == "") {
            txtauthUserPhoneNumber_Flag = 1;
            $("#txtautUserContactNumber").css('border-color', 'red')
            $("#txtautUserContactNumber").text("* Please check all mandatory fields");

        }
        else {
            txtauthUserPhoneNumber_Flag = 0;
            $("#txtautUserContactNumber").css('border-color', '')
        }

    }
    else {
    }

    flag = FirstName_flag + LastName_flag + CellNo_flag + EmailId_flag + Email2_flag + IsBilling_flag + IsPLessBilling_flag + StreetName_flag + StreetNo_flag + APT_flag + City_flag + State_flag + PoBox_flag + ZipCode_flag + IsPoBox_flag + LocationByName_flag + switchDate_flag + CreditCheck_flag + SSN_flag + SSN2_flag + SSN3_flag + DateOfBirth_flag + CreditAutoPay_flag + CreditCardHolder_flag + CreditCardNumber_flag + ExpirationMonth_flag + ExpirationYear_flag + CVVCode_flag + BillingZipCode_flag + Chkall_flag + Chkall_flag2 + StreetNamePrevious_flag2 + CityPrevious_flag2 + StatePrevious_flag2 + ZipCodePrevious_flag2 + APTPrevious_flag2 + txtauthUserFirstName_Flag + txtauthUserLastName_Flag + txtauthUserPhoneNumber_Flag;






    return flag;
}
$(function () {
    //$(".ValidateControl").blur(function () {
    //    Validation();
    //})
});

function show_billingAddr_panel() {

    var isTrue = document.getElementById("rbBillingSameYes").checked;
    if (isTrue == false) {
        $("#PSBillingSameAddress").css("display", "block");
        $("#chkPOBox").prop('checked', false);
        var isPOBoxTrue = document.getElementById("chkPOBox").checked;
        if (isPOBoxTrue == false) {
            $("#POBoxDiv").css("display", "none");
            document.getElementById("txtStreetNo_Billing").focus();
        }
        else {
            document.getElementById("txtPOBoxNo").focus();
        }
    }
    else {
        $("#PSBillingSameAddress").css("display", "none");
    }
}

function show_POBoxDiv() {
    //$("#POBoxDiv").css("display", "block");
    var isPOBoxTrue = document.getElementById("chkPOBox").checked;
    if (isPOBoxTrue == false) {
        $("#PSBillingSameAddress").css("display", "block");
        $("#POBoxDiv").css("display", "none");
        document.getElementById("txtStreetNo_Billing").focus();
    }
    else {
        $("#POBoxDiv").css("display", "block");
        document.getElementById("txtPOBoxNo").focus();
    }
}

$(function () {
    var chkAccept = document.getElementById("chkAccept");
    var chkAutorizedPerson = document.getElementById("IsTCPAAgreed");
    var chkAuthorizeSwitch = document.getElementById("chkAutorizedSwitch");
    var chkAll = document.getElementById("chkAll");
    $('#IsTCPAAgreed').click(function () {
        if ($(this).is(":checked")) {
        }
        else if ($(this).is(":not(:checked)")) {
            if (chkAll != null)
                chkAll.checked = false;
        }
    });

    $('#chkAccept').click(function () {
        if ($(this).is(":checked")) {
        }
        else if ($(this).is(":not(:checked)")) {
            if (chkAll != null)
                chkAll.checked = false;
        }
    });
    $('#chkAutorizedSwitch').click(function () {
        if ($(this).is(":checked")) {
        }
        else if ($(this).is(":not(:checked)")) {
            if (chkAll != null)
                chkAll.checked = false;
        }
    });
});

var FirstName = "";
var MiddleName = "";
var LastName = "";
var CellNo = "";
var AlternateCellNo = "";
var EmailId = "";
var Email2 = "";
var Promocode = "";
var IsPLessBilling = "";
var IsPLessCommunication = "";
var IsBilling = "";
var StreetName = "";
var StreetNo = "";
var APT = "";
var City = "";
var State = "";
var ZipCode = "";
var IsPoBox = "";
var PoBox = "";
var PreferedLanguage = "";
var IsMoving = "";
var switchDate = "";
var CreditCheck = "";
var SSN = "";
var DateOfBirth = "";
var CreditAutoPay = "";
var CreditCardType = "";
var CreditCardNumber = "";
var CreditCardHolder = "";
var BillingZipCode = "";
var ExpirationMonth = "";
var ExpirationYear = "";
var CVVCode = "";
var PaymentMethod = "";
var ddlContactOptions = "";
var StreetNamePrevious = "";
var APTPrevious = "";
var CityPrevious = "";
var StatePrevious = "";
var ZipCodePrevious = "";
var AnotherAuthrizedUser = "";
var txtauthUserFirstName = "";
var txtauthUserLastName = "";
var txtauthUserPhoneNumber = "";
var NameOnBankAccount = "";
var RoutingNumber = "";
var AccountNumber = "";
var AccountType = "";
var GovIdNumber = "";
var GovIdType = "";
var GovIdState = "";
function formatDate(input) {

    var datePart = input.match(/\d+/g),
    //year = datePart[0].substring(0), // get only two digits
    //month = datePart[1], day = datePart[2];

    //return day + '/' + month + '/' + year;
    month = datePart[0].substring(0), // get only two digits
    day = datePart[1], year = datePart[2];

    return day + '/' + month + '/' + year;
}
$(function () {

    $("#Enroll").click(function () {
        //checkSession();
        //   DateOfBirth = formatDate(dob);
        $.ajax({
            url: Produrl + "/Home/CheckSessionValidity",
            type: "POST",
            success: function (result) {

                if (result == "False") {
                    window.location.href = Produrl + "/Home/Index";
                }
                else {

                    if ($("#vType").val() == "") {
                        SocialSecurity = "SSN";
                    }
                    else {

                        SocialSecurity = $("#vType").val();
                    }
                    //var first_n = Validation();
                    var f = Validation();
                    if (ValidationFirstname() == false || Validationlastname() == false || ValidationaUserFirstName() == false ||
                        ValidationaUserLastName() == false || ValidationCity() == false || ValidationCityprevious() == false || checkEmail() == false ||
                        ValidationStreetName() == false || validatePhoneNumber() == false) {
                        f += 1;
                    }
                    if (SocialSecurity == "SSN") {
                        if (IsValidSSNValue() == false) {
                            f += 1;
                        }

                    }
                    if (f > 0) {
                        //alert("Please Review Errors");

                        $("#EditReviewModal").modal('show');
                    }

                    else {

                        $("#errormsg").text("");

                        //Personal Information
                        FirstName = $("#txtName").val();
                        MiddleName = $("#txtMName").val();
                        LastName = $("#txtLName").val();
                        var str = $("#txtCell1").val();
                        var res = str.replace(/\D/g, "");
                        CellNo = res;
                        //DateOfBirth = $("#DateofBirth").val();
                        var dob = $("#DateofBirth").val();
                        dob = dob.replace("-", '/');
                        dob = dob.replace("-", '/');
                        DateOfBirth = formatDate(dob);

                        AlternateCellNo = $("#tdalternate").val();
                        EmailId = $("#txtEmail").val();
                        Email2 = $("#txtEmail2").val();
                        Promocode = $("#txtPromoCode").val();
                        ddlContactOptions = $("#ddlContactOptions").val();
                        if (PlessComm == "Yes")
                            IsPLessBilling = "Yes"
                        else {
                            var ischecked = document.getElementById("rbPaperlessBilling_Yes").checked;
                            if (ischecked == true)
                                IsPLessBilling = "Yes";
                            else
                                IsPLessBilling = "No";
                        }
                        var ischecked2 = document.getElementById("rbBillingSameYes").checked;
                        if (ischecked2 == true)
                            IsBilling = "Yes";
                        else {
                            IsBilling = "No";
                            StreetNo = $("#txtStreetNo_Billing").val();
                            StreetName = $("#txtStreetName_Billing").val();
                            APT = $("#txtAptNo_Billing").val();
                            City = $("#txtCity_Billing").val();
                            State = $("#ddlState").val();
                            ZipCode = $("#txtZip_Billing").val();
                            var ischeckedPoBox2 = document.getElementById("chkPOBox").checked;
                            if (ischeckedPoBox2 == true) {
                                IsPoBox = "yes"
                                PoBox = $("#txtPOBoxNo").val();
                            }
                            else
                                IsPoBox = "No"
                        }
                        var IscheckedLanguage = document.getElementById("english").checked;
                        if (IscheckedLanguage == true)
                            PreferedLanguage = "English";
                        else
                            PreferedLanguage = "Spanish";


                        //Service Information


                        var Ischecked = document.getElementById("I'm Moving").checked;
                        if (Ischecked == true) {
                            IsMoving = "Yes";
                            StreetNamePrevious = $("#txtStreetName_BillingPrevious").val();
                            APTPrevious = $("#txtAptNo_BillingPrevious").val();
                            CityPrevious = $("#txtCity_BillingPrevious").val();
                            ZipCodePrevious = $("#txtZip_BillingPrevious").val();
                            StatePrevious = $("#ddlStatePrevious").val();
                        }
                        else {
                            IsMoving = "No";
                        }

                        switchDate = $("#ddlSwitchDates").val();
                        var authU = document.getElementById("AnotherAuthUser").checked;
                        if (authU == true) {
                            AnotherAuthrizedUser = "Yes";
                            txtauthUserFirstName = $("#txtautUserFirstName").val();
                            txtauthUserLastName = $("#txtautUserLastName").val();
                            txtauthUserPhoneNumber = $("#txtautUserContactNumber").val();
                        }
                        else {
                            AnotherAuthrizedUser = "No";
                        }

                        //var IsCreditCheck = document.getElementById("rbCreditCheck_Yes").checked;
                        // if (IsCreditCheck == true) {

                        if (SocialSecurity == "SSN") {

                            if ($("#IdentityVerification").css('display') == 'none') {
                                CreditCheck = "No";
                            }
                            else {
                                CreditCheck = "Yes";
                                SSN = $("#txtSSN1").val() + $("#txtSSN2").val() + $("#txtSSN3").val();
                            }
                        }
                        else {
                            CreditCheck = "No";
                            GovIdType = $("#GovId option:selected").text()

                            GovIdState = $("#GovIdState").val();
                            GovIdNumber = $("#txtIdNumber").val();

                        }
                        //DateOfBirth = $("#DateofBirth").val();

                        //Credit Card Information

                        if (PayBy == "Credit") {
                            PaymentMethod = "CREDIT";
                            var Ischecked = document.getElementById("chkCreditAutoPay").checked;

                            if (Ischecked == true) {
                                CreditAutoPay = "Yes";
                            }
                            else {
                                CreditAutoPay = "No";
                            }
                            CreditCardType = $("#ddlCardType").val();
                            CreditCardNumber = $("#txtCreditCardNo").val();
                            CreditCardHolder = $("#txtNameOnCard").val();
                            BillingZipCode = $("#txtBillingZipCode").val();
                            ExpirationMonth = $("#ddlExpMonth").val();
                            ExpirationYear = $("#ddlExpYear").val();
                            CVVCode = $("#txtCvcCode").val();
                        }
                        else if (PayBy == "ECheck") {
                            PaymentMethod = "ECheck";
                            NameOnBankAccount = $("#AccountName").val();
                            RoutingNumber = $("#RoutingNumber").val();
                            AccountNumber = $("#AccountNumber").val();
                            AccountType = $("#AccountType").val();
                        }
                        else {
                            PaymentMethod = "CASH";
                        }

                        var TCPA = document.getElementById("IsTCPAAgreed").checked;
                        var IsTCPA = "";

                        if (TCPA == true) {
                            IsTCPA = "Y";
                        }
                        else {
                            IsTCPA = "N";
                        }

                        var formData = new FormData();

                        formData.append("first_name", FirstName);
                        formData.append("middle_name", MiddleName);
                        formData.append("last_name", LastName);
                        formData.append("contactno", CellNo);
                        formData.append("ddlContactOptions", ddlContactOptions);
                        //formData.append("AlternateCellNo", AlternateCellNo);
                        formData.append("email", EmailId);
                        formData.append("email2", Email2);
                        // formData.append("Promocode", Promocode);
                        formData.append("IsPLessBilling", IsPLessBilling);
                        formData.append("IsPLessCommu", PlessComm);
                        formData.append("IsBilling", IsBilling);
                        formData.append("StreetName", StreetName);
                        formData.append("StreetNum", StreetNo);
                        formData.append("AptNum", APT);
                        formData.append("CityName", City);
                        formData.append("StateName", State);
                        formData.append("ZipCode", ZipCode);
                        formData.append("IsPoBox", IsPoBox);
                        formData.append("PoBox", PoBox);
                        formData.append("PreferedLanguage", PreferedLanguage);
                        formData.append("IsMoving", IsMoving);
                        formData.append("StreetNamePrevious", StreetNamePrevious);
                        formData.append("APTPrevious", APTPrevious);
                        formData.append("CityPrevious", CityPrevious);
                        formData.append("StatePrevious", StatePrevious);
                        formData.append("ZipCodePrevious", ZipCodePrevious)
                        formData.append("switchDate", switchDate);
                        formData.append("date_of_birth", DateOfBirth);
                        formData.append("Payment_Method", PaymentMethod);
                        formData.append("CreditAutoPay", CreditAutoPay);
                        formData.append("CreditCardNumber", CreditCardNumber);
                        formData.append("CreditCardType", CreditCardType);
                        formData.append("CreditCardHolder", CreditCardHolder);
                        formData.append("BillingZipCode", BillingZipCode);
                        formData.append("ExpirationMonth", ExpirationMonth);
                        formData.append("ExpirationYear", ExpirationYear);
                        formData.append("CVVCode", CVVCode);
                        formData.append("NameOnBankAccount", NameOnBankAccount);
                        formData.append("AccountNumber", AccountNumber),
                        formData.append("RoutingNumber", RoutingNumber);
                        formData.append("AccountType", AccountType);
                        formData.append("AnotherAuthrizedUser", AnotherAuthrizedUser);
                        formData.append("AuthrizedUserFirstName", txtauthUserFirstName);
                        formData.append("AuthrizedUserLastName", txtauthUserLastName);
                        formData.append("AuthrizedUserContactNumber", txtauthUserPhoneNumber);
                        formData.append("ProviderType", $("#CarrierType").val());
                        formData.append("CreditCheck", CreditCheck);
                        formData.append("SocialSecurity", SocialSecurity);
                        formData.append("ssn", SSN);
                        formData.append("GovermentIdNumber", GovIdNumber);
                        formData.append("GovIdType", GovIdType);
                        formData.append("GovIdState", GovIdState);
                        formData.append("IsTCPA", IsTCPA);
                        $("#loading_message").show();
                        // $("#loadingmessage").show();
                        $.ajax({
                            type: 'POST',
                            cache: false,
                            contentType: false,
                            processData: false,
                            data: formData,
                            url: Produrl + "/Home/InsertOrderDetail",
                            success: function (response) {

                                //fillConfirmationvalue(response);
                                var resp = response;
                                if (resp.toLowerCase() != "error") {
                                    fillConfirmationvalue(response);
                                }
                                else {
                                    $("#loading_message").hide();
                                    $("#errormsg").text("* Unable to place your order. Please try again.");
                                    $("#EditReviewModal").modal('show');
                                    $("#eMsg").empty();
                                    $("#eMsg").append("Unable to place your order. Please try again.");
                                    $("#EditButtonReview_no2").css("display", "block");
                                    $("#EditButtonReview_no").css("display", "none");
                                }

                            },
                            failure: function (response) {
                                //   alert(response.responseText);
                            },
                            error: function (response) {
                                //    alert(response.responseText);
                            }

                        });

                    }
                }
            },
            complete: function () {
                setupSessionTimeoutCheck();
            }
        });
    })

});
function fillConfirmationvalue() {
    window.location.href = Produrl + "/Home/confirmation";


}

function toggleMsg() {
    $("#Echeckmsg").toggle("slow");
}
function toggleMsg2() {
    $("#CashLocations").toggle("slow");
}
$(function () {
    var GovId = $("#GovId");
    $.ajax({
        type: "GET",
        url: Produrl + "/Home/GetGovermentId",
        success: function (response) {
            GovId.empty().append('<option selected="selected" value="0">Please select</option>');
            $.each(response, function () {
                GovId.empty();//.append('<option selected="selected" value="0">Please select</option>');
                $.each(response, function () {
                    GovId.append($("<option></option>").val(this['Id']).html(this['GovermentId_Name']));
                });

            });
        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
});
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})
function setInputFilter(textbox, inputFilter) {
    ["input", "keydown", "keyup", "mousedown", "mouseup", "select", "contextmenu", "drop"].forEach(function (event) {
        if (textbox != null) {
            textbox.addEventListener(event, function () {
                if (inputFilter(this.value)) {
                    this.oldValue = this.value;
                    this.oldSelectionStart = this.selectionStart;
                    this.oldSelectionEnd = this.selectionEnd;
                } else if (this.hasOwnProperty("oldValue")) {
                    this.value = this.oldValue;
                    this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
                }
            });
        }
    });
}
$(function () {

    // Restrict input to digits and '.' by using a regular expression filter.
    setInputFilter(document.getElementById("txtSSN1"), function (value) {
        return /^\d*\.?\d*$/.test(value);
    });
    setInputFilter(document.getElementById("txtSSN2"), function (value) {
        return /^\d*\.?\d*$/.test(value);
    });
    setInputFilter(document.getElementById("txtSSN3"), function (value) {
        return /^\d*\.?\d*$/.test(value);
    });
    setInputFilter(document.getElementById("txtCell1"), function (value) {
        return /^\d*\.?\d*$/.test(value);
    });
    setInputFilter(document.getElementById("txtautUserContactNumber"), function (value) {
        return /^\d*\.?\d*$/.test(value);
    });

    //  Date control

    //$("#DateofBirth").datepicker({
    //    //maxDate: "01/01/2001",
    //    //format: 'mm-dd-yyyy'
    //    showOn: 'button',
    //    dateFormat: 'mm-dd-yyyy',
    //    buttonImageOnly: true,
    //    startDate: '01-01-1920',//previous date
    //    endDate: '01-01-1990'//current date

    //});
    //$("#DateofBirth").on("change", function () {
    //    var fromdate = $(this).val();

    //});
    if (QueryString != "") {
        $("#txtPromocode").val(QueryString);
    }
})
//Promocode check
var QueryString = "";
$(function () {
    $.ajax({
        type: 'GET',
        url: Produrl + "/Home/QueryStringPromocode",
        success: function (response) {
            if (response != "") {

                document.getElementById("PCode").innerHTML = response;
                $("#clearPromo").css("display", "block");
                QueryString = response;
                $("#txtPromocode").val(QueryString);
            }
        },
    });
})
function clearPromoCode() {

    var pc = sessionStorage.getItem("pc");

    // if (pc == "") 
    {
        $.ajax({
            type: 'GET',
            url: Produrl + "/Home/ClearPromocode",
            success: function (response) {
                if (response != "") {
                    $("#clearPromo").css("display", "none");
                    document.getElementById("PCode").innerHTML = "Enter Promo/Referral Code";
                    var ZipCode = $("#txtZipCode3").val();

                    window.location.href = Produrl + "/Home/Index?Zip=" + ZipCode;

                }
            },
        });
    }

    //$.ajax({
    //    type: 'GET',
    //    url: Produrl + "/Home/ClearPromo",
    //    success: function (response) {
    //        $("#clearPromo").css("display", "none");
    //        showTabsFirstLoad();
    //        document.getElementById("PCode").innerHTML = "Enter Promo/Referral Code";
    //    },
    //});
}

//End//
//For Pre-Pay show carrier
$(function () {
    var ddlCarrierType = $("#CarrierType");
    $.ajax({
        type: 'GET',
        url: Produrl + "/Home/GetProvider",
        success: function (response) {
            if (response != "") {
                ddlCarrierType.empty().append('<option selected="selected" value="0">Please select</option>');
                $.each(response, function () {
                    ddlCarrierType.append($("<option></option>").val(this['Value']).html(this['Text']));
                });
            }
        },
    });
})

//Confirmation Page
function QueryPromoCode() {
    $("#txtPromocode").val(QueryString);
    if ($("#IsPartner").val() != "y") {
        window.history.replaceState("object or string", "Title", Produrl + "/Home/" + tbName + "/Promocode");
    }
    $('#myModal').on('shown.bs.modal', function () {
        $('#txtPromocode').focus();
        if ($('#txtPromocode').val() != '') {
            $('#btnPromoReferalCode').show();
        }
        else {
            $('#btnPromoCode').attr('disabled', 'disabled');
            $('#btnPromoReferalCode').hide();
        }
    })
}

function checkPromoValue() {
    var PromoValue = document.getElementById("txtPromocode").value;
    if (PromoValue.length > 0) {
        document.getElementById("btnPromoCode").disabled = false;
        $('#btnPromoReferalCode').show();
    }
    else {
        document.getElementById("btnPromoCode").disabled = true;
        $('#btnPromoReferalCode').hide();
        $("#lbPromoCodemsg").hide();
    }
}

function InsertMileStone(Milestone) {
    var req = {
        MileStone: Milestone
    };
    $.ajax({
        type: 'POST',
        url: Produrl + "/Home/InsertMileStone",
        data: req,
        success: function (response) {
        },
    });
}
//EnrollmentEnrollment Page Milestoe
function CustomerInformation() {

    //if ($("#txtName").val() != "" && $("#txtLName").val() != "" && $("#txtCell1").val() != "" && $("#txtEmail").val() != "") {

    //    InsertMileStone("CustomerInformation");
    //}
}
function serviceInformation() {

    if ($("#ddlSwitchDates").val() != "0") {

        InsertMileStone("ServiceInformation");
    }
}
function LoadBrandData() {
    $("#hdnHdrImagePath").val("")
    var aTag = document.createElement('a');
    var link = $("#hdnlogolink").val();
    if (link != "") {
        aTag.setAttribute('href', link);

        $('.logo').wrap(aTag);
    }
    var brandName = "";
    var brandDescription = "";
    var brandCopyRightNote = "";
    var brandTollFreeNo = "";
    $.ajax({
        type: 'GET',
        url: Produrl + "/Home/GetBrandInfo",
        success: function (response) {

            brandName = response[0];
            brandDescription = response[1];
            brandTollFreeNo = response[2];
            brandCopyRightNote = response[3];
            brandCode = response[5];
            $("#ContactNo").text(brandTollFreeNo);

            $("#ContactNo2").text(brandTollFreeNo);

            //alert("Session value="+sessionStorage.getItem("PartnerPhone"));
            //if ($("#PartnerPhone").val() != '') {
            //    $("#ContactNo").text($("#PartnerPhone").val());
            //     $("#ContactNo2").text($("#PartnerPhone").val());

            //}

            if (sessionStorage.getItem("PartnerPhone") != '') {
                $("#ContactNo").text(sessionStorage.getItem("PartnerPhone"));
                $("#ContactNo2").text(sessionStorage.getItem("PartnerPhone"));

            }
            if ($("#BrandCode").val().toLowerCase() == "gexaix") {
                //changing the icon according to frontier or gexa
                $("#favicon_Font_Brand").attr("href", Produrl + "/images/favicon-32x32.png");
            }
            else {
                $("#favicon_Font_Brand").attr("href", Produrl + "/images/frontier.png");
            }
            //if ($("#BrandCode").val().toLowerCase() == "gexaix") {
            //    //changing the icon according to frontier or gexa
            //    window.dataLayer = window.dataLayer || [];
            //    function gtag() { dataLayer.push(arguments); }
            //    gtag('js', new Date());

            //    gtag('config', $("#GoogleForGexa").val());
            //}
            //else {
            //    window.dataLayer = window.dataLayer || [];
            //    function gtag() { dataLayer.push(arguments); }
            //    gtag('js', new Date());
            //    gtag('config', $("#GoogleForFrontier").val());
            //}
            $("#copyRight").empty();
            $("#copyRight").append(response[3]);
            $("#brandcNumber").val(response[2]);
            document.title = brandName;
        }
    });

}
function Loadbrandlinks() {

    var yourElementtou = document.getElementById("termsofUse");
    var yourElementp = document.getElementById('privacy');
    var yourElemenlNT = document.getElementById('lNT');
    var yourElementFacebook = document.getElementById("facebook");
    var yourElementTwitter = document.getElementById("twitter");
    var yourElementLinkdin = document.getElementById("linkdin");
    var yourElementbbb = document.getElementById("bbb");
    var yourElementlegal = document.getElementById("lNT");
    var yourElementlegalNotice = document.getElementById("legalNotice");
    var yourElementhdnImagePath = document.getElementById("hdnHdrImagePath");
    if ($("#BrandCode").val().toLowerCase() == "gexaix") {

        yourElementlegal.setAttribute('display', 'inline-block');
        yourElementlegalNotice.setAttribute('href', 'https://www.gexaenergy.com/legal-notices-and-terms/');
        yourElementtou.setAttribute('href', 'https://www.gexaenergy.com/terms-of-use');
        yourElementp.setAttribute('href', 'https://www.gexaenergy.com/privacy-policy');
        yourElementFacebook.setAttribute("href", "https://www.facebook.com/GexaEnergy/");
        yourElementLinkdin.style.cssText = "display:none";
        yourElementTwitter.setAttribute("href", "https://twitter.com/gexavoice");
        yourElementbbb.setAttribute("href", "https://www.bbb.org/us/tx/houston/profile/electric-companies/gexa-energy-lp-0915-16000036");
        $("#Font_Brand").css("color", "#50B948");
        $("#legalTerm").empty();
        $("#legalTerm").append("I confirm that I am at least eighteen years of age and legally authorized to change REPs for the address listed above. I understand that I am authorizing Gexa Energy L.P.(Gexa Energy) to become my new Retail Electric Provider (REP) in place of my current REP, if applicable.");


    }
    else {
        $("#lNT").css("display", "none");
        $("#Font_Brand").css("color", "#f58522");
        yourElementtou.setAttribute('href', 'https://www.frontierutilities.com/TERMS-OF-USE');
        yourElementp.setAttribute("href", "https://www.frontierutilities.com/PRIVACY-POLICY");
        yourElementFacebook.setAttribute("href", "https://www.facebook.com/frontierutilities");
        yourElementLinkdin.style.cssText = "display:'inline-block'";
        yourElementTwitter.setAttribute("href", 'https://twitter.com/frontieru');
        yourElementLinkdin.setAttribute("href", 'http://www.linkedin.com/company/2495751?trk=tyah&trkInfo=tas%3Afrontier%20ut');
        yourElementbbb.setAttribute("href", 'https://www.bbb.org/us/tx/houston/profile/electric-companies/frontier-utilities-llc-0915-90012559');

    }
}
function showButton() {

    if ($("#txtZipCode3").val() != '') {
        $("#btAddress2").show();
    }
    else {
        var tdsp = ''
        setTDSPCode(tdsp);
        $("#btAddress2").hide();
    }
    var len = $("#txtZipCode3").val().length;
    if (len <= 4) {

        setTDSPCode(tdsp);
    }
    if (len > 4) {
        //checkDifferTDSP("", $("#txtZipCode3").val());
        var zipcode = $("#txtZipCode3").val();
        var serialize1 = {
            zipcode: zipcode
        };
        $.ajax({
            type: 'POST',
            url: Produrl + "/Home/GetAllTDSP",
            data: serialize1,
            dataType: "json",
            success: function (response) {

                if (response == "diff") {
                    checkDifferTDSP('', $("#txtZipCode3").val());
                }
                else {
                    var rows = "";

                    $("#tdspInfo").empty();
                    var k = 0;
                    $.each(response, function (i) {
                        k = k + 1;

                        rows += "<input type='radio' value='" + response[i] + "' name='tdsp' /> " + (response[i]) + "<br/>";
                    })
                    if (k > 1) {
                        $("#txtZipCode3").attr('readonly', true);
                        $("#txtZipCode3").addClass('input-disabled');
                        $("#txtAddress2").attr('readonly', true);
                        $("#txtAddress2").addClass('input-disabled');
                        pTDSP = "True";
                        $("#tdspselection").show();
                        $("#tdspInfo").append(rows);
                        $('input[type=radio]').change(function () {
                            setTDSPCode($('input[type=radio][name=tdsp]:checked').val());
                        });
                    }
                }
            },
        });
    }
}
function showButton2() {
    if ($("#txtZipCode2").val() == '') {

        var tdsp = ''
        setTDSPCode(tdsp);
        $("#btAddress2").hide();
    }

    var len = $("#txtZipCode2").val().length;
    if (len <= 4) {

        setTDSPCode(tdsp);
    }
    if (len > 4) {

        var zipcode = $("#txtZipCode2").val();

        var serialize1 = {
            zipcode: zipcode
        };
        $.ajax({
            type: 'POST',
            url: Produrl + "/Home/GetAllTDSP",
            data: serialize1,
            dataType: "json",
            success: function (response) {


                if (response == "diff") {
                    checkDifferTDSP('', $("#txtZipCode2").val());
                }
                else {
                    var rows = "";

                    $("#tdspInfo").empty();
                    var k = 0;
                    $.each(response, function (i) {
                        k = k + 1;

                        rows += "<input type='radio' value='" + response[i] + "' name='tdsp' /> " + (response[i]) + "<br/>";
                    })
                    if (k > 1) {
                        //$("#txtZipCode2").attr('readonly', true);
                        //$("#txtZipCode2").addClass('input-disabled');
                        //$("#txtAddress").attr('readonly', true);
                        //$("#txtAddress").addClass('input-disabled');
                        $("#btAddress2").attr("disabled", true);
                        pTDSP = "True";
                        $("#tdspselection").show();
                        $("#tdspInfo").append(rows);
                        $('input[type=radio]').change(function () {
                            setTDSPCode($('input[type=radio][name=tdsp]:checked').val());
                        });
                    }
                }
            },
        });
    }
}
function setTDSPCode(TDSP) {

    var len=0;
    if (TDSP) {
        var len = TDSP.length;
    }
    if (len > 3) {
        var serialize1 = {
            TDSP: TDSP
        };
        $.ajax({
            type: 'POST',
            url: Produrl + "/Home/setTDSP",
            data: serialize1,
            dataType: "json",
            success: function (response) {

                if (response == "same") {
                    $("#tdspselection").hide();
                    $("#txtZipCode3").attr('readonly', false);
                    $("#txtZipCode3").addClass('input-enabled');
                    $("#txtAddress2").attr('readonly', false);
                    $("#txtAddress2").addClass('input-enabled');

                    $("#txtZipCode2").attr('readonly', false);
                    $("#txtZipCode2").addClass('input-enabled');
                    $("#txtAddress").attr('readonly', false);
                    $("#txtAddress").addClass('input-enabled');
                    $("#btAddress2").attr("disabled", false);
                }
                else {


                    if (changethezip == 'y') {
                        $('#myDifferntTDSPMsg').modal({
                            backdrop: 'static',
                            keyboard: false
                        })
                        $('#myDifferntTDSPMsg').modal('show');
                        $('#myAddressZipModal2').css({ 'opacity': 0.7 });
                        $('#myAddressModal').css({ 'opacity': 0.7 });
                    }
                    else {
                        $("#tdspselection").hide();
                        $("#txtZipCode3").attr('readonly', false);
                        $("#txtZipCode3").addClass('input-enabled');
                        $("#txtAddress2").attr('readonly', false);
                        $("#txtAddress2").addClass('input-enabled');

                        $("#txtZipCode2").attr('readonly', false);
                        $("#txtZipCode2").addClass('input-enabled');
                        $("#txtAddress").attr('readonly', false);
                        $("#txtAddress").addClass('input-enabled');
                        $("#btAddress2").attr("disabled", false);
                    }
                }
            },
        });
    }
}
function ValidationFirstname() {

    var firstname = document.getElementById("txtName");
    var alpha = /^[^\s][a-zA-Z-,]+(\s{0,1}[a-zA-Z-, ])*$/;
    if (!firstname.value.match(alpha)) {
        $(".txtNameEror").show();
        $("#txtName").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtNameEror").hide();
        $("#txtName").css('border-color', '');
        return true;
    }
}
function blockSpecialChar(e) {
    var k;
    document.all ? k = e.keyCode : k = e.which;
    //return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57));
    return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32);
}
function Validationlastname() {
    var lastname = document.getElementById("txtLName");
    var alpha = /^[^-\s][a-zA-Z-,]+(\s{0,1}[a-zA-Z-, ])*$/;
    if (!lastname.value.match(alpha)) {
        $(".txtLNameEror").show();
        $("#txtLName").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtLNameEror").hide();
        $("#txtLName").css('border-color', '');
        return true;
    }
}

function ValidationaUserFirstName() {
    var isTrue = document.getElementById("AnotherAuthUserNo").checked;
    if (isTrue == true) {
        return true;
    }
    var UserFirstName = document.getElementById("txtautUserFirstName");
    var alpha = /^[^-\s][a-zA-Z-,]+(\s{0,1}[a-zA-Z-, ])*$/;
    if (!UserFirstName.value.match(alpha)) {
        $(".txtaUserFirstNameEror").show();
        $("#txtautUserFirstName").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtaUserFirstNameEror").hide();
        $("#txtautUserFirstName").css('border-color', '');
        return true;
    }
}
function ValidationaUserLastName() {
    var isTrue = document.getElementById("AnotherAuthUserNo").checked;
    if (isTrue == true) {
        return true;
    }
    var UserLastName = document.getElementById("txtautUserLastName");
    var alpha = /^[^-\s][a-zA-Z-,]+(\s{0,1}[a-zA-Z-, ])*$/;
    if (!UserLastName.value.match(alpha)) {
        $(".txtaUserLastNameEror").show();
        $("#txtautUserLastName").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtaUserLastNameEror").hide();
        $("#txtautUserLastName").css('border-color', '');
        return true;
    }
}
function ValidationCity() {
    var isTrue = document.getElementById("rbBillingSameYes").checked;
    if (isTrue == true) {
        return true;
    }
    var txtCity_BillingName = document.getElementById("txtCity_Billing");
    var alpha = /^[^-\s][A-Za-z0-9 ]+$/;
    if (!txtCity_BillingName.value.match(alpha)) {
        $(".txtCityBillingEror").show();
        $("#txtCity_Billing").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtCityBillingEror").hide();
        $("#txtCity_Billing").css('border-color', '');
        return true;
    }
}
function ValidationCityprevious() {
    var isTrue = document.getElementById("I'm Switching").checked;
    if (isTrue == true) {
        return true;
    }
    var txtCity_BillingPreviousName = document.getElementById("txtCity_BillingPrevious");
    var alpha = /^[^-\s][A-Za-z0-9 ]+$/;
    if (!txtCity_BillingPreviousName.value.match(alpha)) {
        $(".txtCityBillingPreviousEror").show();
        $("#txtCity_BillingPrevious").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtCityBillingPreviousEror").hide();
        $("#txtCity_BillingPrevious").css('border-color', '');
        return true;
    }
}
function ValidationStreetName() {
    var isTrue = document.getElementById("rbBillingSameYes").checked;
    if (isTrue == true) {
        return true;
    }
    var txtStreetNameBilling = document.getElementById("txtStreetName_Billing");
    var alpha = /^[^-\s][A-Za-z0-9 ]+$/;
    if (!txtStreetNameBilling.value.match(alpha)) {
        $(".txtStreetEror").show();
        $("#txtStreetName_Billing").css('border-color', 'red');
        return false;
    }
    else {
        $(".txtStreetEror").hide();
        $("#txtStreetName_Billing").css('border-color', '');
        return true;
    }
}
function IsValidSSNValue() {
    var TxtSSN1 = document.getElementById("txtSSN1").value;
    var TxtSSN2 = document.getElementById("txtSSN2").value;
    var TxtSSN3 = document.getElementById("txtSSN3").value;
    var ssnValue = TxtSSN1.concat("-", TxtSSN2, "-", TxtSSN3);
    var regex = /^([0-9])(?!\1{2}-\1{2}-\1{4})[0-9]{2}-[0-9]{2}-[0-9]{4}$/;
    if (regex.test(ssnValue) == false || ssnValue == "123-45-6789" ||
         TxtSSN1 == 000 || TxtSSN2 == 00 || TxtSSN3 == 0000) {
        return false;
    }
    return true;
}
function validationtxtSSN() {

    if (!IsValidSSNValue()) {
        $(".socialsecurityEror").show();
        $("#txtSSN1").css('border-color', 'red')
        $("#txtSSN2").css('border-color', 'red')
        $("#txtSSN3").css('border-color', 'red')
    }
    else {
        $(".socialsecurityEror").hide();
        $("#txtSSN1").css('border-color', '')
        $("#txtSSN2").css('border-color', '')
        $("#txtSSN3").css('border-color', '')
    }
}
function checkEmail() {
    var email = document.getElementById('txtEmail').value;
    var regex = /^[A-Z0-9_'%=+!`#~$*?^{}&|-]+([\.][A-Z0-9_'%=+!`#~$*?^{}&|-]+)*@[A-Z0-9-]+(\.[A-Z0-9-]+)+$/i;
    var substringIndex = email.indexOf("@");
    var substring = email.substring(substringIndex, email.length);
    var lastindex = substring.lastIndexOf(".");
    var index = substring.indexOf(".");
    //if (email.match(regex) && lastindex == index) {
    if (email.match(regex)) {
        $(".txtEmailEror").hide();
        $("#txtEmail").css('border-color', '');
        return true;
    }
    else {
        $(".txtEmailEror").show();
        $("#txtEmail").css('border-color', 'red');
        return false;
    }
}
function AddEnrollInformtion() {

    var firstname = $("#txtName").val();
    var lastname = $("#txtLName").val();
    var emailid = $("#txtEmail").val();
    var phoneno = $("#txtCell1").val();
    var dob = $("#DateofBirth").val();
    var IscheckedLanguage = document.getElementById("english").checked;
    var preferLang = "";
    if (IscheckedLanguage == true)
        preferLang = "English";
    else
        preferLang = "Spanish";
    var ischecked2 = document.getElementById("rbBillingSameYes").checked;
    var isbill = "";
    if (ischecked2 == true) {
        isbill = "Yes";
        StreetNo = "";
        StreetName = "";
        APT = "";
        City = "";
        State = "";
        ZipCode = "";
        IsPoBox = "no"
    }
    else {
        isbill = "No";
        StreetNo = $("#txtStreetNo_Billing").val();
        StreetName = $("#txtStreetName_Billing").val();
        APT = $("#txtAptNo_Billing").val();
        City = $("#txtCity_Billing").val();
        State = $("#ddlState").val();
        ZipCode = $("#txtZip_Billing").val();
        var ischeckedPoBox2 = document.getElementById("chkPOBox").checked;
        if (ischeckedPoBox2 == true) {
            IsPoBox = "yes"
            PoBox = $("#txtPOBoxNo").val();
        }
        else
            IsPoBox = "No"
    }
    var Ischecked = document.getElementById("I'm Moving").checked;
    if (Ischecked == true) {
        IsMoving = "Yes";
        StreetNamePrevious = $("#txtStreetName_BillingPrevious").val();
        APTPrevious = $("#txtAptNo_BillingPrevious").val();
        CityPrevious = $("#txtCity_BillingPrevious").val();
        ZipCodePrevious = $("#txtZip_BillingPrevious").val();
        StatePrevious = $("#ddlStatePrevious").val();
    }
    else {
        IsMoving = "No";
    }

    switchDate = $("#ddlSwitchDates").val();
    var authU = document.getElementById("AnotherAuthUser").checked;
    if (authU == true) {
        AnotherAuthrizedUser = "Yes";
        txtauthUserFirstName = $("#txtautUserFirstName").val();
        txtauthUserLastName = $("#txtautUserLastName").val();
        txtauthUserPhoneNumber = $("#txtautUserContactNumber").val();
    }
    else {
        AnotherAuthrizedUser = "No";
    }

    //var IsCreditCheck = document.getElementById("rbCreditCheck_Yes").checked;
    // if (IsCreditCheck == true) {

    if (SocialSecurity == "SSN") {

        if ($("#IdentityVerification").css('display') == 'none') {
            CreditCheck = "No";
        }
        else {
            CreditCheck = "Yes";
            SSN = $("#txtSSN1").val() + $("#txtSSN2").val() + $("#txtSSN3").val();
        }
    }
    else {
        CreditCheck = "No";
        GovIdType = $("#GovId option:selected").text()

        GovIdState = $("#GovIdState").val();
        GovIdNumber = $("#txtIdNumber").val();

    }
    //DateOfBirth = $("#DateofBirth").val();

    //Credit Card Information

    if (PayBy == "Credit") {
        PaymentMethod = "CREDIT";
        var Ischecked = document.getElementById("chkCreditAutoPay").checked;

        if (Ischecked == true) {
            CreditAutoPay = "Yes";
        }
        else {
            CreditAutoPay = "No";
        }
    }
    else if (PayBy == "ECheck") {
        PaymentMethod = "ECheck";
        NameOnBankAccount = $("#AccountName").val();
        RoutingNumber = $("#RoutingNumber").val();
        AccountNumber = $("#AccountNumber").val();
        AccountType = $("#AccountType").val();
    }
    else {
        PaymentMethod = "CASH";
    }

    var TCPA = document.getElementById("IsTCPAAgreed").checked;
    var IsTCPA = "";

    if (TCPA == true) {
        IsTCPA = "Y";
    }
    else {
        IsTCPA = "N";
    }

    if (PlessComm == "Yes")
        IsPLessBilling = "Yes"
    else {
        var ischecked = document.getElementById("rbPaperlessBilling_Yes").checked;
        if (ischecked == true)
            IsPLessBilling = "Yes";
        else
            IsPLessBilling = "No";
    }
    var formData = new FormData();
    formData.append("firstname", firstname);
    formData.append("lastname", lastname);
    formData.append("emailid", emailid);
    formData.append("phoneno", phoneno);
    formData.append("dob", dob);
    formData.append("IsPLessBilling", IsPLessBilling);
    formData.append("PlessComm", PlessComm);
    formData.append("preferLang", preferLang);
    formData.append("isbill", isbill);
    formData.append("StreetName", StreetName);
    formData.append("StreetNum", StreetNo);
    formData.append("AptNum", APT);
    formData.append("CityName", City);
    formData.append("StateName", State);
    formData.append("ZipCode", ZipCode);
    formData.append("IsPoBox", IsPoBox);
    formData.append("PoBox", PoBox);
    formData.append("PreferedLanguage", PreferedLanguage);
    formData.append("IsMoving", IsMoving);
    formData.append("StreetNamePrevious", StreetNamePrevious);
    formData.append("APTPrevious", APTPrevious);
    formData.append("CityPrevious", CityPrevious);
    formData.append("StatePrevious", StatePrevious);
    formData.append("ZipCodePrevious", ZipCodePrevious)
    formData.append("switchDate", switchDate);
    formData.append("date_of_birth", DateOfBirth);
    formData.append("BillingZipCode", BillingZipCode);

    formData.append("AnotherAuthrizedUser", AnotherAuthrizedUser);
    formData.append("AuthrizedUserFirstName", txtauthUserFirstName);
    formData.append("AuthrizedUserLastName", txtauthUserLastName);
    formData.append("AuthrizedUserContactNumber", txtauthUserPhoneNumber);
    formData.append("ProviderType", $("#CarrierType").val());
    //formData.append("CreditCheck", CreditCheck);
    //formData.append("SocialSecurity", SocialSecurity);
    //formData.append("ssn", SSN);
    //formData.append("GovermentIdNumber", GovIdNumber);
    //formData.append("GovIdType", GovIdType);
    //formData.append("GovIdState", GovIdState);
    formData.append("IsTCPA", IsTCPA);
    //var serialize1 = {
    //    firstname: firstname,
    //    lastname: lastname,
    //    emailid: emailid,
    //    phoneno: phoneno,
    //    dob: dob,
    //    PlessComm: PlessComm,
    //    preferLang: preferLang
    //};
    $.ajax({
        type: 'POST',
        cache: false,
        contentType: false,
        processData: false,
        data: formData,
        url: Produrl + "/Home/AddEnrollinfo",
        success: function (response) {
            if (response) {
                var isPartner = sessionStorage.getItem("IsPartner");

                if (isPartner) {
                    var queryString = sessionStorage.getItem("PartnerQueryString");
                    var qS = escape(queryString);

                    window.location.href = Produrl + "/Home?" + unescape(qS);
                }
                else {
                    window.location.href = Produrl + "/Home/Index";
                }

            }
        },
    });
}

function Partnerpage() {
    // AddPersonalInformtion();
    var isPartner = sessionStorage.getItem("IsPartner");

    if (isPartner) {
        var queryString = sessionStorage.getItem("PartnerQueryString");
        var qS = escape(queryString);

        window.location.href = Produrl + "/Home?" + unescape(qS);
    }
    else {

    }
}
function checkDifferTDSP(address, zipcode) {

    var value = address;
    var req = {
        address: address,
        zipcode: zipcode
    };
    $.ajax({
        type: 'POST',
        data: req,
        url: Produrl + "/Home/checkDifferTDSP",
        success: function (response) {
            if (response) {
                if (response == "same" || response == "first") {
                    //SetAddressZip(address, zipcode,'');
                }
                else {
                    $('#myDifferntTDSPMsg').modal({
                        backdrop: 'static',
                        keyboard: false
                    })
                    $('#myDifferntTDSPMsg').modal('show');
                    $('#myAddressZipModal2').css({ 'opacity': 0.7 });
                    $('#myAddressModal').css({ 'opacity': 0.7 });
                }
            }

        }
    });
}
function changeQstring(zipcode) {
    var req = {
        zipcode: zipcode
    };
    $.ajax({
        type: 'POST',
        data: req,
        url: Produrl + "/Home/changeQstring",
        success: function (response) {


            var resp1 = response[1];
            var resp2 = response[2];

            if (response[0]) {
                window.location.href = Produrl + "/Home?" + response[0];
            }
            else if (response[1] != null && response[1] != "" && (response[2] == null || response[2] == "")) {

                window.location.href = Produrl + "/Home?zip=" + zipcode + "&tdsp=" + resp1;
            }
            else if (response[1] != null && response[1] != "" && response[2] != null && response[2] != "")
            {
                window.location.href = Produrl + "/Home?refid=" + resp2 + "&tdsp=" + resp1 + "&zip=" + zipcode;
            }

        }
    });
}
function disablebutton1() {

    $('#myAddressZipModal2').css({ 'opacity': 1 });
    $('#myAddressModal').css({ 'opacity': 1 });
    $("#txtAddress2").val('');
    $("#txtZipCode3").val(gbZIPCode);
    var element = document.getElementById("txtZipCode3");
    element.classList.remove("input-disabled");
    $("#txtZipCode3").attr('readonly', false);
    //$("#txtZipCode3").addClass('input-enabled');
    //$("#txtZipCode3").addClass('input-enabled');
    //$("#txtZipCode3").remove('input-disabled');
    $("#tdspselection").hide();
    //  $("#btAddress2").prop('disabled', true);
    // $("#txtAddress2").prop('disabled', true);
}
function disablebutton2() {
    $('#myAddressModal').css({ 'opacity': 1 });
    $('#myAddressZipModal2').css({ 'opacity': 1 });
    $('#myAddressModal').css({ 'opacity': 1 });
    $("#txtAddress").val('');
    $("#txtZipCode2").val(gbZIPCode);
    $("#tdspselection").hide();
    $("#txtZipCode3").addClass('input-enabled');
    //  $("#btAddress").prop('disabled', true);
    $.ajax({
        url: Produrl + "/Home/cleanAddress",
        type: "POST",
        success: function (result) {

          
        },
        
    });
}
function setforedit() {
    changethezip = "y";
}
function checkSession() {

    $.ajax({
        url: Produrl + "/Home/CheckSessionValidity",
        type: "POST",
        success: function (result) {

            if (result == "False") {
                window.location.href = Produrl + "/Home/Index";
            }
        },
        complete: function () {
            setupSessionTimeoutCheck();
        }
    });
}

function setupSessionTimeoutCheck() {
    clearTimeout(checkTimeout);
    checkTimeout = setTimeout(checkSession, 900000);
}
function GotoIndexpage() {
    window.location.href = Produrl + "/Home/Index";


}