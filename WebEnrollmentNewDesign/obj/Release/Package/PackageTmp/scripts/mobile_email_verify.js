function ChooseAnotherMethodCode() {
    //alert("ChooseAnotherMethodCode");
    $get('divVerifyMain').style.display = 'block';
    $get('divEmailText').style.display = 'none';
    $get('divPhone').style.display = 'none';
    resetRadioList();
}

function changeVeriType(rbveriType) {
    //alert("changeVeriType");
    $get('divVerifyMain').style.display = 'none';
    var list = document.getElementById("txtVeriLabel");
    var msg = "Verification code has been sent";
    if (rbveriType == 'Email') {
        $get('divEmailText').style.display = 'block';
        $get('divPhone').style.display = 'none';
        list.innerHTML = "Check your email. A message is on its way with an identification code. Once you receive it, please enter it below.";
        sendCode('Email', msg);
    }
    else if (rbveriType == 'Text') {
        $get('divEmailText').style.display = 'block';
        $get('divPhone').style.display = 'none';
        list.innerHTML = "Check your phone. A message is on its way with an identification code. Once you receive it, please enter it below.";
        sendCode('Text', msg);
    }
    else {
        $get('divEmailText').style.display = 'none';
        $get('divPhone').style.display = 'block';

    }

}



function resetRadioList() {
    //alert("resetRadioList");
    var list = document.getElementById("rbVerifyType");
    var inputs = list.getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].checked = false;
    }
}

function resendSMSEmailCode() {
   // alert("resendSMSEmailCode");
    var list = document.getElementById("rbVerifyType");
    var inputs = list.getElementsByTagName("input");
    var selected;
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            selected = inputs[i];
            break;
        }
    }
    //  alert(selected.value);
    var msg = "Verification code has been resent";
    sendCode(selected.value, msg);
}


function sendCode(veritype, msg) {
    //alert("sendCode");
    //  alert("sendCode" + veritype);
    var msgStatus = $get('lblMsgStatus');
    msgStatus.innerHTML = "";
    var varStr = '';
    if (veritype == "Email") {
        typ = 'Email';
        varStr = $get('hidden_email_vericode');
    } else if (veritype == "Text") {
        typ = 'SMS';
        varStr = $get('hidden_vericode');
    }

    $.ajax({
        type: "POST",
        url: 'GetDataFromServer.aspx?sentFrom=Veri&typ=' + typ + '&str=' + varStr.value,
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            msgStatus.innerHTML = msg;
            //alert("data: " + data +", msg :"+ msg   );
            //alert(msgStatus.innerHTM);
        },
        onFailure: function () {
            msgStatus.innerHTML = "Failed while sending message";
            //alert(msg);
            //alert(msgStatus.innerHTM);
        },
        statusCode: {
            404: function () {
                msgStatus.innerHTML = "Error while sending message";
                //alert(msg);
                //alert(msgStatus.innerHTM);
            }
        },
        error: function (data, errorThrown) {
            msgStatus.innerHTML = data.statusText;
           // alert(msg);
            //alert(msgStatus.innerHTM);
        }
    });
}