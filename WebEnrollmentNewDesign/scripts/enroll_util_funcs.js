function onlyAlphabets(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if ((charCode > 31 && (charCode < 48 || charCode > 57)) || charCode == 8 || charCode == 9) {
        return true;
    }
    return false;
}
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;

    if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 8 && charCode != 9) {
        return false;
    }
    return true;
}

function movetoNext(current, nextFieldID) {
   if (current.value.length >= current.maxLength) {
        if (document.getElementById(nextFieldID) != null) {
            document.getElementById(nextFieldID).focus();
        }
    }
}

function movetoNext_cvc(current, nextFieldID) {
    var cctype = document.getElementById("ddlCardType").value;
    if (cctype == "Amex") {
        if (current.value.length == 4) { document.getElementById(nextFieldID).focus(); }
    }
    else {
        if (current.value.length == 3) { document.getElementById(nextFieldID).focus(); }
    }
}

function validate_inputs(ct1, ct2, ct3) {
    if (ct1.value.length == 0 && ct2.value.length == 0 && ct3.value.length == 0)
    { return true; }
    else {
        var ret1 = true, ret2 = true, ret3 = true;
        if (ct1.value.length != ct1.maxLength) { inputgroup_borderchange(ct1, ct2, ct3, "Red"); ret1 = false; }
        if (ct2.value.length != ct2.maxLength) { inputgroup_borderchange(ct1, ct2, ct3, "Red"); ret2 = false; }
        if (ct3.value.length != ct3.maxLength) { inputgroup_borderchange(ct1, ct2, ct3, "Red"); ret3 = false; }
        if (ret1 == true && ret2 == true & ret3 == true) { inputgroup_borderchange(ct1, ct2, ct3, "Green"); return true; }
        else return false;
    }
}