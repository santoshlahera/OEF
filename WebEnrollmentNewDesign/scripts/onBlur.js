
function OnBlur_Fixed(ct) {
    if (ct.value.length == ct.maxLength) {
        ct.style.border = "2px Green Solid";
        return true;
    }
    else {
        ct.style.border = "2px Red Solid";
        return false;
    } 
}

function OnBlur_Num(ct) {
    var val = ct.value;
    if (val.length > 0) {
        if (val.match(/^[0-9 ]*$/)) {
            ct.style.border = "2px Green Solid";
            return true;
        } 
    }
    ct.style.border = "2px Red Solid";
    return false;
}
function OnBlur_AptNo(ct) {
    var val = ct.value;
    if (val.length > 0) {
        if (val.match(/^[0-9 ]*$/)) {
            ct.style.border = "2px Green Solid";
            return true;
        }
        else
        { ct.style.border = "2px Red Solid";return false;}
    }
    
    return true;
}
function OnBlur_Alpha(ct) {
    var val = ct.value;
    if (val.length > 0) {
        if (val.match(/^[a-zA-Z ]*$/)) {
            ct.style.border = "2px Green Solid";
            return true;
        } 
    }
    ct.style.border = "2px Red Solid";
    return false;
}
function OnBlur_AlphaNum(ct) {
    var val = ct.value;
    if (val.length > 0) {
        if (val.match(/^[a-zA-Z0-9_ ]*$/)) {
            ct.style.border = "2px Green Solid";
            return true;
        } 
    }
    ct.style.border = "2px Red Solid";
    return false;
}
function OnBlur_Empty(ct) {
    if (ct.value.length > 0) {

        if (ct.value.length == ct.maxLength)
            ct.style.border = "2px Green Solid";
        else
            ct.style.border = "2px Red Solid";
    }
}

function OnBlur_ccmonth(ct) {
    if (ct.value.length == ct.maxLength) {
        if (ct.value <= 12 && ct.value >0) { ct.style.border = "2px Green Solid"; return true; }
        else { ct.style.border = "2px Red Solid"; return false; }
    }
    else { ct.style.border = "2px Red Solid"; return false; }
}


// to validate creditcard no

function OnBlur_ccyear(ty) {
    var tm = document.getElementById("txtMonth");
    var cdate = document.getElementById("SysDate_HF").value;
    Val_ccExpiryDate(ty, tm, cdate);
}

function OnBlur_CVC(ct) {
    //alert('OnBlur_CVC');
    var cctype = document.getElementById("ddlCardType").value;
    //var cctype = document.getElementById('ddlCardType.ClientID').options[document.getElementById('ddlCardType.ClientID').selectedIndex].text;
    if (cctype == "Amex") {
        // alert('Amex');     
        if (ct.value.length == 4) { ct.style.border = "2px Green Solid"; return true; }
        else { ct.style.border = "2px Red Solid"; return false; }
    }
    else {
        if (ct.value.length == 3) { ct.style.border = "2px Green Solid"; return true; }
        else { ct.style.border = "2px Red Solid"; return false; }
    }
}

function OnBlur_ssn_pa(ct) {
    if (ct.value.length == ct.maxLength) {
        ct.style.border = "2px Green Solid";
        return true;
    }
    else {
        ct.style.border = "2px Red Solid";
        return false;
    }
}



function OnBlur_ssn(ct) {
    var prodtype = document.getElementById("Prepay_Postpay_HF");
    if (prodtype == "Y") {
        if (ct.value.length > 0) {
            if (ct.value.length == ct.maxLength) {
                ct.style.border = "2px Green Solid";
            }
            else {
                ct.style.border = "2px Red Solid";
            }
        }
    }
    else {
        if (ct.value.length == ct.maxLength) {
            ct.style.border = "2px Green Solid";
        }
        else {
            ct.style.border = "2px Red Solid";
        }
    }   
}
function OnBlur_ssnlast(ct) {
    var prodtype = document.getElementById("Prepay_Postpay_HF");
    if (prodtype == "Y") {
        if (ct.value.length > 0) {
            if (ct.value.length == ct.maxLength) {
                ct.style.border = "2px Green Solid";
            }
            else {
                ct.style.border = "2px Red Solid";
            }
        }
    }
    else {
        if (ct.value.length == ct.maxLength) {
            ct.style.border = "2px Green Solid";
        }
        else {
            ct.style.border = "2px Red Solid";
        }
    }  
    validationtxtSSN();
}
function OnBlur_Emailcomp() {
    var temail1 = document.getElementById("txtEmail");
    var temail2 = document.getElementById("txtEmail2");
    return OnBlur_Email_comp(temail2, temail1);

}   