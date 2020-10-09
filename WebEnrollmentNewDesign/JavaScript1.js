var Glbtabid = "";
var GlbpricePlanId = "";
var AllInfo = "";
var PayBy = "Credit";
var SocialSecurity = "ssn";
function GetPaymentMethod(pMethod) {
    PayBy = pMethod;
}
function GetSocialSecurityType(SSSecurity) {
    SocialSecurity = SSSecurity;
}
$(function () {
    $('#loadingmessage').show();
    $("#tabs").tabs();
    $('#loadingmessage').hide();
});
$(function () {
    $("#txtAddress").keyup(function () {
        // var ZipCode = $("#txtZipCode2").val();
        var serAdd = $("#txtAddress").val();
        var ZipCode = $("#txtZipCode2").val();
        $('#loadingAddress').show();
        $.ajax({
            type: 'POST',
            //  type:'GET',
            contentType: "application/json; charset=utf-8",
            cache: false,
            contentType: false,
            processData: false,
            url: "/OnlineEnrollment/Home/GetServiceAddress?serAdd=" + serAdd + "&ZipCode=" + ZipCode,
            // data:{serAdd:serAdd,ZipCode:ZipCode},
            success: function (response) {
                $('#loadingAddress').hide();
                if (response == "") {

                }
                else {
                    autocomplete(document.getElementById("txtAddress"), response);
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

        var serAdd = $("#txtAddress2").val();
        var ZipCode = $("#txtZipCode3").val();
        $("#loadingAddress2").show();
        $.ajax({
            type: 'POST',
            //  type:'GET',
            contentType: "application/json; charset=utf-8",
            cache: false,
            contentType: false,
            processData: false,
            url: "/OnlineEnrollment/Home/GetServiceAddress?serAdd=" + serAdd + "&ZipCode=" + ZipCode,
            // data:{serAdd:serAdd,ZipCode:ZipCode},
            success: function (response) {
                $("#loadingAddress2").hide();
                if (response == "") {

                }
                else {
                    autocomplete2(document.getElementById("txtAddress2"), response);
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
                //    alert("sending value="+inp.value);
                CheckServiceAddress_Info(inp.value);
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

//Service Address 

function CheckServiceAddress_Info(selVal) {
    var NewsetVal = encodeURIComponent(selVal);

    //  $('#loadingmessage').show();

    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",

        url: "/OnlineEnrollment/Home/CheckServiceAddress?serviceAdd=" + NewsetVal,
        // url: '@Url.Action("CheckServiceAddress","Home")',
        // data: { 'selVal': selVal },
        success: function (response) {
            $("#btAddress").css("display", "block");
            $("#serAddMsg").empty();
            $("#serAddMsg").append(response);
            $("#serAddMsg").css("color", "red");
            $("#serAddMsg2").empty();
            $("#serAddMsg2").append(response);
            $("#serAddMsg2").css("color", "red");
            //    alert(response);
            if (response) {


                //$("#ProductDetailInfo").empty();
                //$("#ProductDetailInfo").append(response);
                //$('#loadingmessage').hide();


            } else {

            }
        },
        failure: function (response) {

        },
        error: function (response) {
            alert(response);
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
});
function autocomplete2(inp, arr) {
    
    // $("#btAddress").css("display", "none");
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
                //    alert("sending value="+inp.value);
                CheckServiceAddress_Info(inp.value);
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

    var vzip = tzip.value;
    taddr.disabled = false;
    document.getElementById("txtAddress").value = "";
    if (vzip == "" || vzip.length != 5) {

        taddr.disabled = true;
        document.getElementById("txtAddress").value = "";
        // lzip.innerHTML = "Invalid Zip Code";
        tzip.style.border = "2px Red Solid";
        $("#btAddress").css("display", "none");
        tzip.focus();
    } else {
        tzip.style.border = "2px Green Solid";
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
    var vzip = tzip.value;
    taddr.disabled = false;

    if (vzip == "" || vzip.length != 5) {

        taddr.disabled = true;
        document.getElementById("txtAddress2").value = "";
        // lzip.innerHTML = "Invalid Zip Code";
        tzip.style.border = "2px Red Solid";
        $("#btAddress").css("display", "none");
        tzip.focus();
    } else {
        tzip.style.border = "2px Green Solid";
        //  lzip.innerHTML = ""; laddr.innerHTML = "";
        taddr.disabled = false;
        taddr.focus();

    }
    if (vzip.length == 5 && taddr.innerHTML != "") {
        $("#btAddress").css("display", "block");
    }
}

function showTabs(tabId) {
  
    Glbtabid = tabId;
    var kwhC="";
      
    var radios = document.getElementsByName('kwh_choice');

    for (var i = 0, length = radios.length; i < length; i++) {
        if (radios[i].checked) {
            // do whatever you want with the checked radio
                
            kwhC=radios[i].value
            // only one radio can be logically checked, don't check the rest
            break;
        }
    }
       
    $('#loadingmessage').show();
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        url: '/OnlineEnrollment/Home/GetProductDetail',
        data: { tabId: tabId, kwh_Choice: kwhC },

      / //data:{'tabId':tabId,'kwh_Choice':kwhC,
        success: function (response) {
            if (response) {
                $("#divInfo").empty();
                $("#divInfo").append(response);

                $('#loadingmessage').hide();

            } else {
                //  alert('failed');
            }
        },
    });

}
function showTabsDetails(pricePlanid, tabId, kwh_Choice) {
    
    GlbpricePlanId = pricePlanid;
    $('#loadingmessage').show();
      
    $.ajax({
        type: 'POST',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",

        url: "/OnlineEnrollment/Home/GetProductDetail_Popup?pPId=" + pricePlanid + "&tabId=" + tabId + "&kwh_Choice=" + kwh_Choice,
        //  data: tabId,
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
    var tabId = "";
    //alert("asdfasf" + Rate);
    tabId = Glbtabid;
    $('#loadingmessage').show();
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        //url: '@Url.Action("GetProductDetail","Home")?tabId=' + tabId,
        // url: location.href = "Home\\GetProductDetail?tabId=" + tabId,
        //url: "/Home/GetProductDetail?tabId=" + tabId + "&kwh_Choice=" + Rate,
        url: '/OnlineEnrollment/Home/GetProductDetail',
        data: { tabId: tabId, kwh_Choice: Rate},
        //  data: tabId,
        success: function (response) {
            if (response) {
                $("#divInfo").empty();
                $("#divInfo").append(response);

                $('#loadingmessage').hide();

            } else {
                //  alert('failed');
            }
        },
    });
        
}
function checkPromocode() {

    var PromoCode = $("#txtPromocode").val();
    if (PromoCode == "") {
        $("#lbPromoCodemsg").empty();
        $("#lbPromoCodemsg").append("Please enter any Promo Code");
        $("#lbPromoCodemsg").css('color', '#E15739');
    }
    else {
        $('#loadingmessage').show();
        $.ajax({
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            //url: "/Home/CheckPromoCode?PromoCode=" + PromoCode,
            url: '/OnlineEnrollment/Home/CheckPromoCode',
            data:{PromoCode:PromoCode},
            success: function (response) {
                if (response) {
                    if (response != "Valid") {
                        $("#lbPromoCodemsg").empty();
                        $("#lbPromoCodemsg").append(response);
                        $("#lbPromoCodemsg").css('color', '#E15739');
                    }
                    else {
                        $("#lbPromoCodemsg").empty();
                        $("#lbPromoCodemsg").append("Your Promo code is valid");
                        $("#lbPromoCodemsg").css('color', 'green');

                    }
                    $('#loadingmessage').hide();


                } else {

                }
            },
        });
    }
}
function addressPopUpReset() {

    $("#txtAddress").text('');
    $("#btAddress").css("display", "none");
}
   
// $("#500kwh").click(GetProductsAccordingRates(500));
function showTabsFirstLoad() {
        
    var tabId = 1;
    Glbtabid = tabId;
    var kwhC =500;

        

    $('#loadingmessage').show();
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
            
        url: '/OnlineEnrollment/Home/GetProductDetail',
        data: { tabId: tabId, kwh_Choice: kwhC },
        success: function (response) {
            if (response) {
                $("#TabsInfo").addClass("nav-link active");
                $("#divInfo").empty();
                $("#divInfo").append(response);

                $('#loadingmessage').hide();

            } else {
                //  alert('failed');
            }
        },
    });

}
function load_AllSelectedInformation(address)
{
        
    var userAddress = $("#txtAddress").val();
       
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
            
        url: '/OnlineEnrollment/Home/GetSelectedProductDetails',
        data: { address:address},

        //  url: '/Home/GetProductDetail',

        //data:{'tabId':tabId,'kwh_Choice':kwhC,
        success: function (response) {
             
			 
            if (response) {
                window.location.href = "/OnlineEnrollment/Home/Index";
                   
                   
            } else {
                //  alert('failed');
            }
        },
    });

}
function fillData() {

        
    $("#divPlanDetail").append(AllInfo);
}
function ChangeServiceAddress(address) {
       
    var userAddress = $("#txtAddress").val();
        
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
          
        url: '/OnlineEnrollment/Home/EditServiceAddress',
        data: { userAddress: userAddress },

        
        success: function (response) {

            if (response) {
                   
                document.getElementById("serviceAddress").innerHTML =response[0];
                document.getElementById("ESID").innerHTML = response[1];
                $("#myAddressModal").modal('hide');
            } else {
                   
            }
        },
    });
}
function CheckSelectedAddress(pricePlanId,productId) {
    var address = "xyz";
    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",

        url: '/OnlineEnrollment/Home/CheckSelAddress',

        data: {productId:productId,pricePlanId:pricePlanId},


        success: function (response) {
               
            if (response) {
                   
                if (response[1] == "No" && response[0] != "No") {

                    $("#txtZipCode2").val(response[0]);
                    $("#myAddressModal").modal('show');
                }
                else if (response[0] == "No" && response[1] == "No") {
                    $("#myAddressModal").modal('show');
                }
                else {

                    var NewZipCode = response[0];
                    var address = response[1];

                    $.ajax({
                        type: 'GET',
                        dataType: 'json',
                        contentType: "application/json; charset=utf-8",
                        url: '/OnlineEnrollment/Home/GetSelectedProductDetails',
                        data: { address: address, NewZipCode: NewZipCode },
                        success: function (response) {

                            if (response) {
                                window.location.href = "home/index";


                            } else {

                            }
                        },
                    });
                }
            } else {

            }
        },
    });
}
function SetAddressZip(address,zipcode) {
        

    $.ajax({
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",

        url: '/OnlineEnrollment/Home/SetServiceAddress',
        data: { address: address,zipcode:zipcode },


        success: function (response) {

            if (response) {
                  
                document.getElementById("lbzipcode").innerHTML = response;
               
                $("#myAddressZipModal2").modal('hide');
                $("#TabsInfo").addClass("nav-link");
               
                showTabs(Glbtabid);
            } else {

            }
        },
    });
}
function CheckAllClick() {

    var chkAccept = document.getElementById("chkAccept");
    var chkAutorizedPerson = document.getElementById("chkAutorizedPerson");
    var chkAuthorizeSwitch = document.getElementById("chkAuthorizeSwitch");
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
        url: "/OnlineEnrollment/Home/GetMonths",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
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
        url: '/OnlineEnrollment/Home/GetYears',
        data: '{}',
        contentType: "application/json; charset=utf-8",
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
    var dtToday = new Date();

    var month = dtToday.getMonth() + 1;
    var day = dtToday.getDate() + 1;
    var year = dtToday.getFullYear();
    if (month < 10)
        month = '0' + month.toString();
    if (day < 10)
        day = '0' + day.toString();

    var maxDate = year + '-' + month + '-' + day;

    $('#ddlSwitchDates').attr('min', maxDate);
});
var flag = 0;
var errorMsg = "";


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

        $("#txtEmail2").css('border-color', 'red')
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
                $("#txtEmail2").css('border-color', 'red')
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

    var isBilliingSame = document.getElementById("rbBillingSame").checked;
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
                $("#txtPOBoxNo").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                IsPoBox_flag = 0;
                $("#txtPOBoxNo").css('border-color', '');
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
                $("#txtStreetName_Billing").css('border-color', 'red');
                $("#errormsg").text("* Please check all mandatory fields");
                $("#errormsg").css('color', 'red');
            }
            else {
                StreetName = 0;
                $("#txtStreetName_Billing").css('border-color', '');
            }

            if ($("#txtAptNo_Billing").val() == "") {

                APT_flag = 1;
                $("#txtAptNo_Billing").css('border-color', 'red');
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

        if ($("#txtAptNo_BillingPrevious").val() == "") {

            APTPrevious_flag2 = 1;
            $("#txtAptNo_BillingPrevious").css('border-color', 'red');
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            APTPrevious_flag2 = 0;
            $("#txtAptNo_BillingPrevious").css('border-color', '');
        }

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

        $("#ddlSwitchDates").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        switchDate_flag = 0;
        $("#ddlSwitchDates").css('border-color', '')
    }
    if (SocialSecurity == "ssn") {
        if ($("#txtSSN1").val() == "") {
            SSN_flag = 1;

            $("#txtSSN1").css('border-color', 'red')
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            SSN_flag = 0;

            $("#txtSSN1").css('border-color', '')
        }
        if ($("#txtSSN2").val() == "") {
            SSN2_flag = 1;

            $("#txtSSN2").css('border-color', 'red')
            $("#errormsg").text("* Please check all mandatory fields");
            $("#errormsg").css('color', 'red');
        }
        else {
            SSN2_flag = 0;
            $("#txtSSN2").css('border-color', '')
        }
        if ($("#txtSSN3").val() == "") {
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

        $("#DateofBirth").css('border-color', 'red')
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        DateOfBirth_flag = 0;
        $("#DateofBirth").css('border-color', '')
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
        PayBy="Cash"
    }
        
    if ($("#chkAll").is(":checked")) {
        Chkall_flag = 0;
        document.getElementById("spp").style.color = "";
    }
    else if ($("#chkAll").is(":not(:checked)")) {
        Chkall_flag = 1;
        $("#spp").css('color', 'red');
        $("#errormsg").text("* Please check all mandatory fields");
        $("#errormsg").css('color', 'red');
    }
    else {
        Chkall_flag = 0;
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
        
    flag = FirstName_flag + LastName_flag + CellNo_flag + EmailId_flag + Email2_flag + IsBilling_flag + IsPLessBilling_flag + StreetName_flag + StreetNo_flag + APT_flag + City_flag + State_flag + PoBox_flag + ZipCode_flag + IsPoBox_flag + LocationByName_flag + switchDate_flag + CreditCheck_flag + SSN_flag + SSN2_flag + SSN3_flag + DateOfBirth_flag + CreditAutoPay_flag + CreditCardHolder_flag + CreditCardNumber_flag + ExpirationMonth_flag + ExpirationYear_flag + CVVCode_flag + BillingZipCode_flag + Chkall_flag + StreetNamePrevious_flag2 + CityPrevious_flag2 + StatePrevious_flag2 + ZipCodePrevious_flag2 + APTPrevious_flag2 + txtauthUserFirstName_Flag + txtauthUserLastName_Flag + txtauthUserPhoneNumber_Flag;






    return flag;
}
$(function () {
    $(".ValidateControl").blur(function () {
        Validation();
    })
});
    
$(function () {
    var chkAccept = document.getElementById("chkAccept");
    var chkAutorizedPerson = document.getElementById("chkAutorizedPerson");
    var chkAuthorizeSwitch = document.getElementById("chkAuthorizeSwitch");
    var chkAll = document.getElementById("chkAll");
    $('#chkAutorizedPerson').click(function () {
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
var StreetName2 = "";
var APT2 = "";
var City2 = "";
var State2 = "";
var ZipCode2 = "";
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
$(function () {
    $("#Enroll").click(function () {
            
        var f = Validation();

        if (f > 0) {
            alert("Please Review Errors");
        }

        else {
            $("#errormsg").text("");
            //Personal Information
            FirstName = $("#txtName").val();
            MiddleName = $("#txtMName").val();
            LastName = $("#txtLName").val();
            CellNo = $("#txtCell1").val();
            AlternateCellNo = $("#tdalternate").val();
            EmailId = $("#txtEmail").val();
            Email2 = $("#txtEmail2").val();
            Promocode = $("#txtPromoCode").val();
            ddlContactOptions = $("#ddlContactOptions").val();
            var ischecked = document.getElementById("rbPaperlessBilling_Yes").checked;
            if (ischecked == true)
                IsPLessBilling = "Yes";
            else
                IsPLessBilling = "No";
            var ischecked2 = document.getElementById("rbBillingSame").checked;
            if (ischecked2 == true)
                IsBilling = "Yes";
            else {
                IsBilling = "No";
                StreetNo = $("#txtStreetNo_Billing").val();
                StreetName = $("#txtStreetName_Billing").val();
                APT = $("#txtAptNo").val();
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
                IsMoving="Yes";
                StreetName2 = $("#txtStreetName_Billing2").val();
                APT2 = $("#txtAptNo_Billing2").val();
                City2 = $("#txtCity_Billing2").val();
                ZipCode2 = $("#txtZip_Billing2").val();
                State2 = $("#ddlState2").val();
            }
            else
            {
                IsMoving="No";
            }

            switchDate = $("#ddlSwitchDates").val();
            var authU = document.getElementById("AnotherAuthUser").checked;
            if (authU == true) {
                AnotherAuthrizedUser = "Yes";
                txtauthUserFirstName_Flag = $("#txtautUserFirstName").val();
                txtauthUserLastName = $("#txtautUserLastName").val();
                txtauthUserPhoneNumber = $("#txtautUserContactNumber").val();
            }
            else {
                AnotherAuthrizedUser = "No";
            }

            //var IsCreditCheck = document.getElementById("rbCreditCheck_Yes").checked;
            // if (IsCreditCheck == true) {
            if(SocialSecurity == "ssn")
            {
                //CreditCheck = "Yes";
                SSN     = $("#txtSSN1").val() + $("#txtSSN2").val() + $("#txtSSN3").val();
            }
            else {
                // CreditCheck = "No";
                GovIdType = $("#GovId").val();
                GovIdState = $("#GovIdState").val();
                GovIdNumber = $("#txtIdNumbe").val();

            }
            //DateOfBirth = $("#DateofBirth").val();

            //Credit Card Information
            var IsCreditAuthrized = document.getElementById("btncreditAutopay").checked;
            if (IsCreditAuthrized == true)
                CreditAutoPay = "Yes";
            // var el = document.getElementById("ddlPaymentMethod");
            //if (el.className == "nav-link active") {
            if (PayBy == "Credit")
            {
                PaymentMethod = "CREDIT";
                var IsCreditCardSelected = document.getElementById("btncreditAutopay").checked;
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
            formData.append("StreetName2",StreetName2);
            formData.append("APT2",APT2);
            formData.append("City2",City2);
            formData.append("State2", State2);
            formData.append("ZipCode2",ZipCode2)

            formData.append("switchDate", switchDate);
            formData.append("CreditCheck", CreditCheck);
                
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
                
            formData.append("SocialSecurity", SocialSecurity);
            formData.append("ssn", SSN);
            formData.append("GovermentIdNumber", GovIdNumber);
            formData.append("GovIdType", GovIdType);
            formData.append("GovIdState", GovIdState);


            $.ajax({
                type: "POST",
                // url: '@Url.Action("InsertOrderDetail","Home")',
                url: '/OnlineEnrollment/Home/InsertOrderDetail',
                // data:JSON.stringify(formData),
                //processData: false,
                //contentType: false,
                //contentType: "application/json; charset=utf-8",
                //dataType: "json",
                cache: false,
                contentType: false,
                processData: false,
                data: formData,
                success: function (response) {

                    // alert(response);
                    $("#errormsg").text(response);

                    $("#errormsg").css('color', 'red');
                    $("#errormsg").css('font-size', 'medium');

                },
                failure: function (response) {
                    //   alert(response.responseText);
                },
                error: function (response) {
                    //    alert(response.responseText);
                }
            });
        }
    })
});
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})
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
        url: '/OnlineEnrollment/Home/GetGovermentId',
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
           
           

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
showTabsFirstLoad();
fillData();
