function updateSwitchMoveLabel() {
    var ddlist = document.getElementById("ddlSwitchDates");
    var sdate = new Date(ddlist.value);
    var nextdt = new Date();
    nextdt.setTime(sdate.getTime() + (13 * 24 * 60 * 60 * 1000));
    var lblMoveSwitchDT = document.getElementById("lblMoveSwitchDate");
    var weekday = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
    var str = "" + weekday[nextdt.getDay()] + ',' + nextdt.format("MMM") + ',' + nextdt.getDate() + ' ' + nextdt.getFullYear();
    if (lblMoveSwitchDT != null) lblMoveSwitchDT.innerHTML = str;
    document.getElementById("swithmoveDateHF");
}

function valiDateETF() {
    var tssn1 = document.getElementById("txtSSN1");
    var tssn2 = document.getElementById("txtSSN2");
    var tssn3 = document.getElementById("txtSSN3");
    var ccfree = document.getElementById("Post_CCF").value.toString();
    var prodtype = document.getElementById("Prepay_Postpay_HF").value.toString();

    var vssn1 = true; var vssn2 = true; var vssn3 = true;

    if (prodtype == "N" && ccfree == "N") {//postpaid
        // if (document.getElementById("rbCreditCheck_Yes").checked == true) {//crchk reqd
        //alert('rbCreditCheck:Yes');
        if ($("#rbCreditCheck_Yes").is(':checked')) {
            if (tssn1.value == "" || tssn1.value.length != tssn1.maxLength) vssn1 = false;
            SetBorderColor(tssn1, vssn1);
            //alert("tssn1 :" + vssn1);
            if (tssn2.value == "" || tssn2.value.length != tssn2.maxLength) vssn2 = false;
            SetBorderColor(tssn2, vssn2);
            //alert("tssn2 :" + vssn2);
            if (tssn3.value == "" || tssn3.value.length != tssn3.maxLength) vssn3 = false;
            SetBorderColor(tssn3, vssn3);
        }
        //alert("tssn3 :" + vssn3);
        //}
    }
    var selID = document.getElementById("rbSwitch_MoveIn");
    var inputs = selID.getElementsByTagName("input");
    var chkETF = document.getElementById("chkAcceptContract");
    var expalert = document.getElementById("expiryalert");
    var vrbs = true; var vchk = true;

    //alert("switch:" + inputs[0].checked);
    //alert("move:" + inputs[1].checked);

    // alert("rbs");
    if (inputs[0].checked == false && inputs[1].checked == false) vrbs = false; SetBorderColor(selID, vrbs);
    //alert("chk");
    if (inputs[0].checked == true && chkETF.checked == false && expalert.value == "Y") vchk = false;
    if (vchk == false) {
        //alert("i understand:" + chkETF.checked); alert("vchk :" + vchk); 
        SetBorderColor(chkETF, vchk);
    }

    if (vssn1 == true && vssn2 == true && vssn3 == true && vrbs == true && vchk == true) {
        //alert("func true");
        var vssngr = validate_inputs(tssn1, tssn2, tssn3);
        if (vssngr == true) {
            //alert("validate_inputs:" +vssngr);
            return true;
        }
        else {
            //alert("validate_inputs:" +vssngr);
            return false;
        }
    }
    else {
        //alert("func false");
        return false;
    }
}

//Data entry Acccordian
function CheckChange() {
    $get('btnSwtchMovResp').click();
}

function CreditAutoPay() {
    $get('btncreditAutopay').click();
}

function AcceptContractPopup() {
    $get('btnAcceptContract').click();
    //    var caption = document.getElementById("lblCaption");
    //    var msg = document.getElementById("lblMessage");
    //    var bYes = document.getElementById("btnYes_mpext");
    //    var bNo = document.getElementById("btnNo_mpext");
    //    var popup = document.getElementById("mpext_msg");
    //    caption.innerHTML = "Current Electicity Contract";
    //    msg.innerHTML = "Please be aware that if you are still under contract with another electric company as of the selected date above, this enrollment will not overrule  any Early Termination Fees you may incur from breaking your contract. ";
    //    bNo.Value = "";
    //    bYes.value = "OK";
    //    bNo.style.display="none";
    //    bYes.style.display = "block";
}
function ebillClick() {
    $get('btnebill').click();
}