function Validate_PersonalInfo() {
    var isChecked_ba = document.getElementById("rbBillingSame").checked ? true : false;
    var isChecked_po = document.getElementById("chkPOBox").checked;

   

    var tstno = document.getElementById("txtStreetNo_Billing");
    var tstna = document.getElementById("txtStreetName_Billing");
    var tcity = document.getElementById("txtCity_Billing");
    var tzipc = document.getElementById("txtZip_Billing");

    var tfnam = document.getElementById("txtName");

    
   
   
    var vstno = true; vstna = true; vcity = true; vzip = true;
    var vfnam = true; vlnam = true; vpob = true;

    if (isChecked_ba == false) {
        if (isChecked_po == false) {
            if (tstno.value == "") vstno = false; SetBorderColor(tstno, vstno);
            //alert("tstno :" + vstno);
            if (tstna.value == "") vstna = false; SetBorderColor(tstna, vstna);
            //alert("tstna :" + vstna);
        }
        else {
            if ((tpob.value.length < 2 && tpob.value.length >10 ) || (tpob.value =="" )) vpob = false; SetBorderColor(tpob, vpob);
        }
        if (tcity.value == "") vcity = false; SetBorderColor(tcity, vcity);
        //alert("tcity :" + vcity);
        if (tzipc.value == "" || tzipc.value.length != tzipc.maxLength) vzip = false;
        SetBorderColor(tzipc, vzip);
        //alert("tzipc :" + vzip);
    }

    if (tfnam.value.trim == "") vfnam = false; SetBorderColor(tfnam, vfnam);
    //alert("tfnam :" + vfnam);
    
    //alert("tlnam :" + vlnam);

    



    if ( vstno == true && vstna == true && vcity == true && vzip == true && vfnam == true && vpob==true) {
        //alert("All true");
        return true;
        
    }
    else {
        //alert("func: false");
        return false;

    }
}