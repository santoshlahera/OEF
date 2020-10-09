/// <reference path="../ProductsInfo.js" />
var IsMoving = "No";

var Produrl = "";
$(document).ready(function () {
    Produrl = $("#hnProdUrl").val();
});
//function show_Moviing_Panel() {

//    var Ischecked = document.getElementById("I'm Moving").checked;
//    if (Ischecked == true) {
//        IsMoving = "Yes";
//        $("#MovingDetail").show();
//        $("#Move_Or_Switch").empty();
//        $("#Move_Or_Switch").append("Move Date");
//    }
//    else {
//        IsMoving = "No";
//        $("#MovingDetail").hide();
//        $("#Move_Or_Switch").empty();
//        $("#Move_Or_Switch").append("Switch Date");
//    }
//    *@(IsMoving);
//}

function show_hideSwitchorMove_img() {
    var IschkdLang = document.getElementById("I'm Switching").checked;
    if (IschkdLang == true) {
        $("#imgSwitching").show();
        $("#imgMoving").hide();
    }
    else {
        $("#imgSwitching").hide();
        $("#imgMoving").show();
    }

}
$(function () {



});
//function *@(IsMoving) {

//    var ddlSwitcchDates = $("#ddlSwitchDates");

//    ddlSwitcchDates.empty().append('<option selected="selected" value="0" disabled = "disabled">Loading.....</option>');
//    $("#loading_message").show();
//    $.ajax({
//        type: 'POST',
//        dataType: 'json',
//        contentType: "application/json; charset=utf-8",

//        //url: "/OnlineEnrollment" + "/Home/*@?Switch_Or_Move=" + IsMoving,
//        url: Produrl + "/Home/*@?Switch_Or_Move=" + IsMoving,
//        success: function (response) {

//            ddlSwitcchDates.empty().append('<option selected="selected" value="0">Please select</option>');
//            $.each(response, function (i) {

//                ddlSwitcchDates.append($("<option></option>").val(response[i]).html(response[i]));
//            });
//            $("#loading_message").hide();
//        },
//    });
//    show_hideSwitchorMove_img();
//}

function show_AutoPay_Panel() {
    var Ischecked = document.getElementById("AutoPay").checked;

    if (Ischecked == true) {
        GetPaymentMethod('Credit');
        $("#AutoPayDetail").show();
        $("#CashLocations").hide();
    }
    else {
        GetPaymentMethod('Cash');
        $("#AutoPayDetail").hide();
    }

}
function show_CashLocation_Panel() {
    $("#CashLocations").toggle("slow");
    var Ischecked = document.getElementById("AutoPay").checked;
    if (Ischecked == true) {
        $("#AutoPay").prop("checked", false);
        $("#AutoPayDetail").hide();
    }
    // $("#AutoPayDetail").toggle("slow");

}
function show_AnotherUser_Panel() {
    var Ischecked = document.getElementById("AnotherAuthUser").checked;
    if (Ischecked == true) {
        $("#AnotherUserInfo").show();
    }
    else {
        $("#AnotherUserInfo").hide();

    }
    show_hideAnotherUser_img();
}
function show_hideAnotherUser_img()
{
    var Ischecked = document.getElementById("AnotherAuthUser").checked;
    if (Ischecked == true) {
        $("#imgOtrUserYes").show();
        $("#imgOtrUserNo").hide();

    }
    else {
        $("#imgOtrUserNo").show();
        $("#imgOtrUserYes").hide();

    }

}
function show_UserIdentity_Panel() {
    var selectedVal = $("#GovId").val();

    if (selectedVal == "1" || selectedVal == "2") {
        $("#statePanel").show();
        $("#IdNumberPanel").hide();
    }
    else
        $("#statePanel").hide();
    $("#IdNumberPanel").show();
}
function show_billingAddr_panel() {
    //   alert("show_billingAddr_panel");
    var ischecked = document.getElementById("rbBillingSameYes").checked;
    if (ischecked == true) {
        document.getElementById("PSBillingSame").style.display = "none";
        $("#imgBYes").show();
        $("#imgBNo").hide();
    }
    else {
        document.getElementById("PSBillingSame").style.display = "block";
        $("#imgBYes").hide();
        $("#imgBNo").show();
    }
}

function display_Prepaid_div(btn) {
    //alert("display_Prepaid_div");
    if (btn.value.trim() == "Shop Prepaid") {
        document.getElementById("div_prepaid_prod").style.display = "Block";
    }
}

function Show_CrChkErr_Div(str) {
    // alert("Show_CrChkErr_Div");
    if (str == "No") {
        $get('ifNoDiv').style.display = 'block';
        $get('ifYesDiv').style.display = 'none';

    }
    else {
        $get('ifNoDiv').style.display = 'none';
        $get('ifYesDiv').style.display = 'block';
    }
    $get('topDiv').style.display = 'none';
}

function ShowModalPopup(cb) {
    var selID = document.getElementById("rbSwitch_MoveIn");
    var inputs = selID.getElementsByTagName("input");
    if (inputs[0].checked == false && inputs[1].checked == false) {
        SetBorderColor(selID, false);
        return false;
    }
    //alert("ShowModalPopup");
    var btnocrchk = document.getElementById('btnNoCreditCheck');
    var btncrchk = document.getElementById('btnCreditCheck');
    var tdcrunchk = document.getElementById("crUnChk_td");

    btnocrchk.style.display = !cb.checked ? "block" : "none";
    btncrchk.style.display = !cb.checked ? "none" : "block";
    if (document.getElementById("lnkPrepaid_crUnChk").innerText != "")
        tdcrunchk.style.display = !cb.checked ? "block" : "none";
    else
        tdcrunchk.style.display = "none";
    return true;
}

function show_POBoxDiv() {
    var ischecked = document.getElementById("chkPOBox").checked;
    var strDiv = document.getElementById("StrNoDiv");
    var pobDiv = document.getElementById("POBoxDiv");
    strDiv.style.display = ischecked == true ? "none" : "block";
    pobDiv.style.display = ischecked == true ? "block" : "none";
}