function validateconactInfoGroup()
       {

       var fone1=  document.getElementById("txtPrimaryPhone1");
       var fone2=  document.getElementById("txtPrimaryPhone2");
       var fone3 = document.getElementById("txtPrimaryPhone3");

       var temail1= document.getElementById("txtEmail");
       var temail2= document.getElementById("txtEmail2");

        var tcell1= document.getElementById("txtCell1");
        var tcell2= document.getElementById("txtCell2");
        var tcell3 = document.getElementById("txtCell3");
        var tName = document.getElementById("txtName");
        var tLName = document.getElementById("txtLName");

        var vfon1=true;vfon2=true;vfon3=true;
        var vemail1=true;var vemail2=true;
        var vcel1 = true; var vcel2 = true; var vcel3 = true; var vname = true;

        if (tName.value.trim() == "") vname = false;
        SetBorderColor(tName, vname);
                if(tcell1.value=="" || tcell1.value.length != tcell1.maxLength) vcel1=false;
        SetBorderColor(tcell1,vcel1);
    //alert("tcell1: " + vcel1);

        if (tLName.value.trim() == "") vname = false;
        SetBorderColor(tLName, vname);
        if (tcell1.value == "" || tcell1.value.length != tcell1.maxLength) vcel1 = false;
        SetBorderColor(tcell1, vcel1);
       
        if(tcell2.value=="" || tcell2.value.length != tcell2.maxLength) vcel2=false;
        SetBorderColor(tcell2,vcel2);
        //alert("tcell2: " + vcel2);

        if(tcell3.value=="" || tcell3.value.length != tcell3.maxLength) vcel3=false;
        SetBorderColor(tcell3,vcel3);
        //alert("tcell3: " + vcel3);

        if(temail1.value=="" ||  OnBlur_Email(temail1)!= true ) vemail1 =false;
        SetBorderColor(temail1,vemail1);
        //alert("temail1: " + vemail1);

        if(temail2.value=="" ||  OnBlur_Email(temail2)!= true ) vemail2 =false;
        SetBorderColor(temail2,vemail2);
        //alert("temail2: " + vemail2);

         var  email_ret= OnBlur_Email_comp(temail1,temail2);
         SetBorderColor(temail1,email_ret);
         SetBorderColor(temail2,email_ret);
         //alert("email compare: " + email_ret);

         if (vcel1 == true && vcel2 == true && vcel3 == true && email_ret == true && vname==true) {
          //alert("cell no,Emails valid");
           var fon_ret= validate_inputs(fone1,fone2,fone3);
              if(fon_ret==true) {
                  if (Validate_PersonalInfo())
                      return true;
                  else
                      return false;
              }
              else{
                  //alert("Fone nos: " +fon_ret );
                  return false;
              }
         }
         else {
            //alert("func: false");
            return false;
         }
  }
 function OnBlur_EmailValidate(ct) {
    var val = ct.value;
    var re = /^([a-zA-Z0-9_\.])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!re.test(val)) {
        ct.style.border = "2px Red Solid";
        return false;
    }
    else {
        ct.style.border = "2px Green Solid";
        return true;
    }
}
function OnBlur_Email(ct) {
    var val = ct.value;
    var re = /^([a-zA-Z0-9_\.])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!re.test(val)) {
        ct.style.border = "2px Red Solid";
        return false;
    }
    ct.style.border = "2px Green Solid";
    return true;
}

function OnBlur_Email_comp(ct, ct2) {
    var val = ct.value;
    var val2 = ct2.value;
    var re = /^([a-zA-Z0-9_\.])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (re.test(val)) {
        if (re.test(val2)) {
            if (val == val2) {
                ct.style.border = "2px Green Solid";
                ct2.style.border = "2px Green Solid";
                return true;
            }
            else {
                ct.style.border = "2px Red Solid";
                ct2.style.border = "2px Red Solid";
                return false;
            }
        }
        else {
            ct.style.border = "2px Red Solid";
            ct2.style.border = "2px Red Solid";
            return false;
        }
    }
    else {
        ct.style.border = "2px Red Solid";
        ct2.style.border = "2px Red Solid";
        return false;
    }
}

        