
 function validate_prevAddGroup() 
   {
        //alert('validate_prevAddGroup');
        var tstnop= document.getElementById("txtPrevStreetNo");
        var tstnap= document.getElementById("txtPrevStreetName");
        var tcityp= document.getElementById("txtPrevCity");
        var tzipcp= document.getElementById("txtPrevZip");

        var tssn1p= document.getElementById("txtPrevSSN1");
        var tssn2p= document.getElementById("txtPrevSSN2");
        var tssn3p= document.getElementById("txtPrevSSN3");

      var tssn1= document.getElementById("txtSSN1");
      var tssn2= document.getElementById("txtSSN2");
      var tssn3= document.getElementById("txtSSN3");

        var vstnop = true;var vstnap = true;var vcityp = true;var vzipp = true;var vssn1p = true; var vssn2p = true;var vssn3p = true;

        if (tstnop.value == "") vstnop = false; SetBorderColor(tstnop, vstnop);

        if (tstnap.value == "") vstnap = false; SetBorderColor(tstnap, vstnap);

        if (tcityp.value == "") vcityp = false; SetBorderColor(tcityp, vcityp);

        if (tzipcp.value == "" || tzipcp.value.length != tzipcp.maxLength) vzipp = false; SetBorderColor(tzipcp, vzipp);

        if (tssn1p.value == "" || tssn1p.value.length != tssn1p.maxLength) vssn1p = false;   SetBorderColor(tssn1p, vssn1p);

            if (tssn2p.value == "" || tssn2p.value.length != tssn2p.maxLength) vssn2p = false;  SetBorderColor(tssn2p, vssn2p);

            if (tssn3p.value == "" || tssn3p.value.length != tssn3p.maxLength) vssn3p = false; SetBorderColor(tssn3p, vssn3p);

            if (vstnop == true && vstnap == true && vcityp == true && vzipp == true && vssn1p == true && vssn2p == true && vssn3p == true) {
                if (tssn1p.value + tssn2p.value + tssn3p.value == tssn1.value + tssn2.value + tssn3.value)
                    return true;
                else {
                    SetBorderColor(tssn1p, false);
                    SetBorderColor(tssn2p, false);
                    SetBorderColor(tssn3p, false);
                    return false;
                }
            }
            else return false;
    }

    
