 function CreditCardValidation(){
            var ccnoret= true;
             var cvcret= true;
             var ymret = false;
           
            var tccno = document.getElementById("txtCreditCardNo");
            var tnamcc = document.getElementById("txtNameOnCard");
            var tmoncc = document.getElementById("ddlExpMonth");
            var tyrcc = document.getElementById("ddlExpYear");
            var tcvccode = document.getElementById("txtCvcCode");
            var tzipcc = document.getElementById("txtBillingZipCode");

            var cdate= document.getElementById("SysDate_HF").value;

            var vccno=true;var vccna=true;var vccmon=true;var vccyr=true;var vcccode = true;  var vcczip=true; 

            if(tccno.value=="" || OnBlur_CC(tccno) != true ) vccno=false;
            SetBorderColor(tccno, vccno);
            //alert("tccno: " + vccno);
            if (tnamcc.value == "") vccna = false; SetBorderColor(tnamcc, vccna);
            //alert("tnamcc: " + vccna);
            //month validation
            if(tmoncc.value=="")  vccmon =false;
            SetBorderColor(tmoncc, vccmon);
            //alert("tmoncc: " + vccmon);
            //year validation
            if(tyrcc.value=="" || Val_ccExpiryDate()!= true) vccyr=false;
            SetBorderColor(tyrcc,vccyr);
            //alert("tyrcc: " + vccyr);
            if (tcvccode.value == "" || OnBlur_CVC(tcvccode) != true) vcccode = false;
            SetBorderColor(tcvccode, vcccode);
            //alert("tcvccode: " + vcccode);
            if (tzipcc.value == "" || tzipcc.value.length != tzipcc.maxLength) vcczip = false;
            SetBorderColor(tzipcc, vcczip);
            //alert("tzipcc: " + vcczip);
            if (vccno == true && vccna == true && vccmon == true && vccyr == true && vcccode == true && vcczip == true) {
                cancelEvent("onbeforeunload");
                //alert("cc valid");
                return true;
            }
            else {
               
             return false; }
    }

    function OnBlur_CC(ct) {
    var regex = "";
    var ctype = document.getElementById("ddlCardType").value;
    switch (ctype) {
        case "Visa": 
            {
                regex = /^4[0-9]{12}(?:[0-9]{3})?$/;
                break;
            }
        case "Amex": 
            {
                regex = /^3[47][0-9]{13}$/;
                break;
            }
        case "Master": 
            {
                regex = /^5[1-5][0-9]{14}$/;
                break;
            }
        case "Discover": 
            {
                regex = /^6(?:011|5[0-9]{2})[0-9]{12}$/;
                break;
            }
    }
    var val = ct.value;
    if (!regex.test(val)) {
        ct.style.border = "2px Red Solid";
        return false;
    }
    ct.style.border = "2px Green Solid";
    return true;
}



function ChangeCC_RegExs() {
    var txtcvc = document.getElementById("txtCvcCode");
    //var cctype = document.getElementById('<%=ddlCardType.ClientID%>').options[document.getElementById('<%=ddlCardType.ClientID%>').selectedIndex].text;
    var cctype = document.getElementById("ddlCardType").value;
    txtcvc.maxLength = (cctype == "Amex") ? 4 : 3;

}


function Val_ccExpiryDate() {
    
    var cdate = document.getElementById("SysDate_HF").value;
    
    var cyear = parseInt(cdate.toString().substr(8, 2)); //dt.getFullYear().toString().substr(2, 2);
    var cmon = parseInt(cdate.toString().substr(0, 2));
    var ty = document.getElementById("ddlExpYear");
    var tm = document.getElementById("ddlExpMonth");
    //alert('tm length: ' + tm.value.length + ', ty Length :' + ty.value.length);
    //alert(tm.maxLength);
    //if (tm.value.length == tm.maxLength && tm.value > 0) {
        //alert('tm valid');
        //if (ty.value.length == ty.maxLength) {
            //alert('ty valid');
            var tmon = parseInt(tm.value);
            var tyear = parseInt(ty.value);

            //alert('cyear:' + cyear + ',tyear:' + tyear);
            //alert('cmon:' + cmon + ',tmon:' + tmon);

            if (tmon <= 12 && tyear >= cyear) {
                if (tyear > cyear) {
                    //alert('tyear > cyear');
                    change_color(ty, "Green"); change_color(tm, "Green"); return true;
                }
                else if (cyear = tyear) {
                    //alert('tyear = cyear');
                    if (tmon >= cmon) {
                        //alert('tmon >= cmon: true');
                        change_color(tm, "Green"); change_color(ty, "Green"); return true;
                    }
                    else {
                        //alert('tmon >= cmon: false');
                        change_color(tm, "Red"); change_color(ty, "Red"); return false;
                    }
                }
                else if (cyear > tyear) {
                    // alert('cyear > tyear: false');
                    change_color(tm, "Red"); change_color(ty, "Red"); return false;
                }
                else { change_color(tm, "Red"); change_color(ty, "Red"); return false; }
            }
        //} else { change_color(tm, "Red"); change_color(ty, "Red"); return false; }
    //}
    //else { change_color(tm, "Red"); change_color(ty, "Red"); return false; }
}

function validate_ccValGroup() {
    Val_ccExpiryDate();
    var prodtype = document.getElementById("Prepay_Postpay_HF").value.toString();
    var selID = document.getElementById("ddlPaymentMethod");
    var inputs = selID.getElementsByTagName("input");
    var autopay = false;
    if(document.getElementById('chkCreditAutoPay').checked)
    {
        autopay = true;
    } 
    var payamount = $("#MainContent_txtCcDeposit").text();
    var paytype;
    if (inputs[0].checked == true)
        paytype = "Credit Card";
    else if (inputs[1].checked == true)
        paytype = "Cash";
    
    //alert(paytype);
    var ret = true;
    if (prodtype == "Y")
    {
        // alert(prodtype);
      
        if (paytype == "Credit Card") {
            if (payamount > 0)
            {
                ret = CreditCardValidation()
                return ret;
            }
            else
            {
                if(autopay == true)
                {
                    ret = CreditCardValidation()
                    return ret;
                }
                else return ret;
            }
        }
        else {
            cancelEvent('onbeforeunload');
            return true;
        }
    }
    else {
        if (paytype == "Credit Card") {
            if (payamount > 0) {
                ret = CreditCardValidation()
                return ret;
            }
            else {
                if (autopay == true) {
                    ret = CreditCardValidation()
                    return ret;
                }
                else return ret;
            }
        }
        else (paytype == "Cash")
        {
            cancelEvent('onbeforeunload');
            return true;
        }
    }
    var chkAutoPay = document.getElementById("chkCreditAutoPay");
    if (chkAutoPay != null) {
        if (chkAutoPay.checked == false) {
            //      alert("chkCreditAutoPay false");
            cancelEvent("onbeforeunload");
        } else {
            //     alert("chkCreditAutoPay true");
                
        }
    }
    ret = CreditCardValidation();
    //  alert("returned here");
    return ret;
}

//PaymentType
function ChangePaymentType() {
    $get('btnPaymentType').click();
}

