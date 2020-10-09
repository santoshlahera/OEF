
var paymethodcode = '';
var MinPaymentAmount = 0;
var IsWriteoffAmount = 0;
var Email = '';
var ContactNumber = '';
var CustNo = '';
var ccProfileId = '';
var ddProfileId = '';
var custid = 0;
var autopaystatus = '';
var ccprofiletype = '';
var ddprofiletype = '';
var firstname = '';
var lastname = '';
var contracttype = '';
var bankaccountno = '';
var creditcardno = '';
var autopay = '';
var Language = '';
var debit_type = '';
var IsBankDraftShow = false;
var DishonouredCount = 0;
var CreditCardExpiry = '';
var ConnectStatus = '';
var credit_card_type_code = '';
var EstPowerBalance = 0;
var ZipCode = '';

$(document).ready(function () {

    bindEvent(window, 'message', function (e) {
        try {
            var stringify = JSON.parse(e.data);
            var insertPCI = stringify.IsInsertPCILog;
            var SessionId = stringify.PCISessionId;
            var CustomerNo = stringify.CustomerNumber;            
            if (insertPCI == "1") {
                InsertPCITransactionLog(CustomerNo,SessionId);
            }
            else {
                CustNo = stringify.custno;
                Email = stringify.email;
                IsWriteoffAmount = stringify.IsWriteoffAmount;
                ContactNumber = stringify.phoneno;
                ccProfileId = stringify.ccProfileId;
                ddProfileId = stringify.ddProfileId;
                custid = stringify.custid;
                autopaystatus = stringify.autopaystatus;
                ccprofiletype = stringify.ccprofiletype;
                ddprofiletype = stringify.ddprofiletype;
                firstname = stringify.firstname;
                lastname = stringify.lastname;
                paymethodcode = stringify.paymethodcode;
                contracttype = stringify.contracttype;
                bankaccountno = stringify.bankaccountno;
                creditcardno = stringify.creditcardno;
                autopay = stringify.autopay;
                Language = stringify.Language;
                debit_type = stringify.debit_type;
                IsBankDraftShow = stringify.IsBankDraftShow;
                DishonouredCount = stringify.DishonouredCount;
                CreditCardExpiry = stringify.CreditCardExpiry;
                ConnectStatus = stringify.ConnectStatus;
                credit_card_type_code = stringify.credit_card_type_code;
                MinPaymentAmount = parseInt($("#hdnMinPaymentAmount").val());
                EstPowerBalance = stringify.EstPowerBalance;
                ZipCode = stringify.ZipCode                
                RefreshData();
            }
        }
        catch (err) {
            // the JSON.parse call failed, handle the error appropriately
            //alert(err);
        }
    });

    $("#txtCardNumber").keyup(function () {
        var dInput = $(this).val();
        if (dInput.length == 1) {
            if (dInput == 3 || dInput == 4 || dInput == 5 || dInput == 6) {
                if (dInput == 3) {
                    $("#txtCardNumber").removeClass();
                    $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumber").addClass('ae-card-icon');
                    $('input[name="hdncreditcardtype"]').val("American Express");
                    return true;
                }
                if (dInput == 4) {
                    $("#txtCardNumber").removeClass();
                    $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumber").addClass('visa-card-icon');
                    $('input[name="hdncreditcardtype"]').val("Visa");
                    return true;
                }
                if (dInput == 5) {
                    $("#txtCardNumber").removeClass();
                    $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumber").addClass('master-card-icon');
                    $('input[name="hdncreditcardtype"]').val("Master");
                    return true;
                }
                if (dInput == 6) {
                    $("#txtCardNumber").removeClass();
                    $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumber").addClass('discover-card-icon');
                    $('input[name="hdncreditcardtype"]').val("Discover");
                    return true;
                }
            }
            else {
                $("#txtCardNumber").removeClass();
                $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                return false;
            }
        } else {
            return true;
        }
    });

    $("#txtCardNumberAutopay").keyup(function () {
        var dInput = $(this).val();
        if (dInput.length == 1) {
            if (dInput == 3 || dInput == 4 || dInput == 5 || dInput == 6) {
                if (dInput == 3) {
                    $("#txtCardNumberAutopay").removeClass();
                    $("#txtCardNumberAutopay").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumberAutopay").addClass('ae-card-icon');
                    $('input[name="hdncreditcardtypeAutopay"]').val("American Express");
                    return true;
                }
                if (dInput == 4) {
                    $("#txtCardNumberAutopay").removeClass();
                    $("#txtCardNumberAutopay").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumberAutopay").addClass('visa-card-icon');
                    $('input[name="hdncreditcardtypeAutopay"]').val("Visa");
                    return true;
                }
                if (dInput == 5) {
                    $("#txtCardNumberAutopay").removeClass();
                    $("#txtCardNumberAutopay").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumberAutopay").addClass('master-card-icon');
                    $('input[name="hdncreditcardtypeAutopay"]').val("Master");
                    return true;
                }
                if (dInput == 6) {
                    $("#txtCardNumberAutopay").removeClass();
                    $("#txtCardNumberAutopay").addClass('k-textbox f-textbox fl');
                    $("#txtCardNumberAutopay").addClass('discover-card-icon');
                    $('input[name="hdncreditcardtypeAutopay"]').val("Discover");
                    return true;
                }
            }
            else {
                return false;
            }
        } else {
            return true;
        }
    });

    //10-08-2017//Change Pay Now button when auto pay is selected--Credit Card
    $('#chkAutopayOff').click(function () {
        if ($("#chkAutopayOff").is(':checked'))
            $("#btnCreditcardPaynow").text('Setup AutoPay & Pay Now');  // checked
        else
            $("#btnCreditcardPaynow").text('Pay Now');  // unchecked
    });
    //End
    //Change Pay Now button when auto pay is selected--Bank Draft
    $('#chkAutopayOff2').click(function () {
        if ($("#chkAutopayOff2").is(':checked'))
            $("#btnCheckPaynow").text('Setup AutoPay & Pay Now');  // checked
        else
            $("#btnCheckPaynow").text('Pay Now');  // unchecked
    });
    //End

    //Auto Pay --Pay by Card or Pay by Bank Account
    $('#divCCorBA input:radio').click(function () {
        var tabStrip = $("#tabstrip_Payment").kendoTabStrip().data("kendoTabStrip");
        if ($(this).val() == '1') {
            $('#CCAutoPay').show();
            $('#BAAutoPay').hide();
            if (tabStrip.select().index() == 2) {
                $("#divMainAutopay").addClass("autopay-div-height");
                $("#divautopaysec").addClass("autopay-main-sec");
            }
            $("#divbapayfrmleft").removeClass("addmargin-divbapayfrmleft");
            $("#divMainAutopay").removeClass("autopaybankdraft-div-height");
            $("#divautopaysec").removeClass("autopaybankdraft-main-sec");
            if (paymethodcode == "CRDCARD") {
                $('#btnUnSetupAutoPay').attr('title', 'De-Enroll Auto Pay');
                $("#btnUnSetupAutoPay").show();
            }
            else {
                $('#btnUnSetupAutoPay').attr('title', 'Not Enrolled in Credit Card');
                $("#btnUnSetupAutoPay").hide();
            }
            if (paymethodcode != "CHEQUE") {
                $("#btnSetupAutoPay").text("Update Card Details");
            }

        } else if ($(this).val() == '2') {
            $('#CCAutoPay').hide();
            $('#BAAutoPay').show();
            $("#divMainAutopay").removeClass("autopay-div-height");
            $("#divautopaysec").removeClass("autopay-main-sec");
            if (tabStrip.select().index() == 1) {
                $("#divbapayfrmleft").addClass("addmargin-divbapayfrmleft");

            }
            if (tabStrip.select().index() == 2) {
                $("#divMainAutopay").addClass("autopaybankdraft-div-height");
                $("#divautopaysec").addClass("autopaybankdraft-main-sec");
            }
            if (paymethodcode == "DIRECT") {
                $('#btnUnSetupAutoPay').attr('title', 'Disable/De-Enroll Auto Pay');
                $('#btnUnSetupAutoPay').show();
            }
            else {
                $('#btnUnSetupAutoPay').attr('title', 'Not Enrolled in Bank Draft');
                $('#btnUnSetupAutoPay').hide();
            }
            if (paymethodcode != "CHEQUE") {
                $("#btnSetupAutoPay").text("Update Bank Details");
            }
        }

    });

    $('input[type=radio][name=optradio]').change(function () {
        $("#divAutopaymessage").hide();
        $("#divAutoPayIcon").hide();
        $("#lblAutoPayMessage").text('');
    });

    //Pay using Stored Credit Card click
    $('#chkPayusingSCC').click(function () {
        if ($("#chkPayusingSCC").is(':checked')) {
            $("#divPaybyCC").hide();  // checked
            $("#divpaybycchid").show();
        }
        else {
            $("#divPaybyCC").show();  // unchecked
            $("#divpaybycchid").hide();
        }
    });
    //Pay using Stored Bank Account click
    $('#chkPayusingSBA').click(function () {
        if ($("#chkPayusingSBA").is(':checked')) {
            $("#divPaybyBA").hide(); // checked
            $("#divpaybybahid").show();
        }
        else {
            $("#divPaybyBA").show();// unchecked
            $("#divpaybybahid").hide();
        }
    });
    //Tooltips
    
    $("#CreditCard_tab").attr('title', 'Pay with Credit Card');
    $("#BankDraft_tab").attr('title', 'Pay with Bank Account');
    $("#Autopay_tab").attr('title', 'Manage Auto Pay');
    $("#Cash_tab").attr('title', 'Cash Locations');
    //End
    $("#imgsearchlocations").click(function () {
        
        if ($("#txtZipCode").val() != "") {
            var serializedForm = {
                AccountNo: CustNo,
                zip: $("#txtZipCode").val()
            };
            kendo.ui.progress($("#dvdashboard-body"), true);
            $.ajax({
                url: '/OEFPaymentFlow/GetLocations',
                type: "POST",
                data: serializedForm,
                success: function (result) {
                    $("#payment-grid").data("kendoGrid").dataSource.data(result.Data);
                    kendo.ui.progress($("#dvdashboard-body"), false);
                },
                error: function (result) {
                    kendo.ui.progress($("#dvdashboard-body"), false);
                }
            });
        }
    });

    $("#txtRechargeAmount").keyup(function () {

        var dInput = parseFloat($(this).val());
        $("#lblpurchaseamount").html((dInput - parseFloat($("#lblTotalFeesDues").html())).toFixed(2));
        $("#lblprepayPurchased").html(((parseFloat($("#lblpurchaseamount").html()) / parseFloat($("#lblprepayRate").html())) * 100).toFixed(2));
        $("#lblprepayActualPurchased").html((parseFloat($("#lblprepayEstimate").html()) + parseFloat($("#lblprepayPurchased").html())).toFixed(2));
        $("#lblprepayDaysleft").html((parseFloat($("#lblprepayActualPurchased").html()) / parseFloat($("#lblprepayDUsage").html())).toFixed(2));
        $("#txtAmount").val(dInput);
        $("#lblpurchaseamount").html(checkisnan($("#lblpurchaseamount").html()));
        $("#lblprepayPurchased").html(checkisnan($("#lblprepayPurchased").html()));
        $("#lblprepayActualPurchased").html(checkisnan($("#lblprepayActualPurchased").html()));
        $("#lblprepayDaysleft").html(checkisnan($("#lblprepayDaysleft").html()));

    });

});
//ACH payment--Bank Draft
function CheckPayment() {
    document.getElementById("divCheckAuthmessage").style.display = 'none';
    document.getElementById("divCheckPaymentIcon").style.display = 'none';
    var sendconfirmationtype = "NONE";

    if ($("#chkBAEmail").is(':checked'))
        sendconfirmationtype = "EMAIL";
    if ($("#chkBASMS").is(':checked'))
        sendconfirmationtype = "SMS";
    if ($("#chkBASMS").is(':checked') && $("#chkBAEmail").is(':checked'))
        sendconfirmationtype = "BOTH";
    if ((!$("#chkBASMS").is(':checked')) && (!$("#chkBAEmail").is(':checked')))
        sendconfirmationtype = "NONE";

    if (!$("#chkPayusingSBA").is(':checked')) {//Checking Payby profile is selected
        if ($("#txtAccountName").val() != "" && $("#txtAccountNo").val() != "" && $("#txtRoutingNo").val() != "" && $("#txtCheckAmount").val() != "" && $('input[name=optatradio]:checked').val() != "" && $("#txtConfirmAccountNo").val() != "") {
            if ($("#txtConfirmAccountNo").val() == $("#txtAccountNo").val()) {

                var autopay = false;
                if ($("#chkAutopayOff2").attr("checked") == "checked") {
                    autopay = true;
                }
                if ($("#txtCheckAmount").val() < MinPaymentAmount) {
                    document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                    document.getElementById("divCheckPaymentIcon").style.display = 'block';
                    document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                    $("#lblCheckAuthMessage").text("Minimum payment amount should be $ " + MinPaymentAmount);
                    $("#lblCheckAuthMessage").removeClass("green-text");
                    $("#lblCheckAuthMessage").addClass("orange-text");
                    $("#txtCheckAmount").val('');
                    return false;
                }
                ConfirmPayPopUp($("#txtAccountNo").val().substr($("#txtAccountNo").val().length - 4), $("#txtCheckAmount").val());
                $("#btnConfirm").click(function () {

                    var serialized = {
                        CustId: custid,
                        Amount: parseFloat($("#txtCheckAmount").val()).toFixed(2),
                        BankAccountNumber: $("#txtAccountNo").val(),
                        BankAccountName: $("#txtAccountName").val(),
                        AccountType: $('input[name=optatradio]:checked').val(),
                        CustomerNumber: CustNo,
                        BankRoutingNumber: $("#txtRoutingNo").val(),
                        SetupAutoPay: autopay,
                        PaymentorAutopay: "PayNow",
                        ConfirmationType: sendconfirmationtype,
                        Email: Email,
                        ContactNumber: ContactNumber,
                        Source: "MyAccountWeb",
                        FirstName: firstname,
                        LastName: lastname,
                        IsWriteoffAmount: IsWriteoffAmount
                    };
                    kendo.ui.progress($("#dvdashboard-body"), true);
                    $.ajax({
                        url: 'MyAccount/FrontierPayCheckPayment',
                        type: "POST",
                        data: serialized,
                        success: function (result) {
                            if (result.ResultCode == 1) {
                                SendResponseToParent(result, parseFloat($("#txtCheckAmount").val()).toFixed(2), "FrontierPayCheckPayment");
                                $("#txtCheckAmount").val("");
                                $("#txtAccountNo").val("");
                                $("#txtAccountName").val("");
                                $("#txtRoutingNo").val("");
                                $('input:radio[name="optatradio"]').filter('[value="C"]').attr('checked', true);
                                $("#chkBAEmail").prop('checked', false);
                                $("#chkBASMS").prop('checked', false);
                                $("#txtConfirmAccountNo").val("");
                                document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                                document.getElementById("divCheckPaymentIcon").className = "message-icon-payment";
                                document.getElementById("divCheckPaymentIcon").style.display = 'block';
                                $("#lblCheckAuthMessage").removeClass("orange-text");
                                $("#lblCheckAuthMessage").addClass("green-text");

                                //Success Response //Profile Response
                                $("#lblCheckAuthMessage").text("This Transaction has been approved. Confirmation No: " + result.AuthCode);
                                $("#divCCAutopaySection").hide();
                                $("#divBAAutopaySection").hide();
                                $("#chkAutopayOff2").prop('checked', false);
                                kendo.ui.progress($("#dvdashboard-body"), false);
                            }
                            else if (result.ResultCode == -100) {
                                document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                                document.getElementById("divCheckPaymentIcon").style.display = 'block';
                                document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                                $("#lblCheckAuthMessage").removeClass("green-text");
                                $("#lblCheckAuthMessage").addClass("orange-text");
                                $("#lblCheckAuthMessage").text("Session has expired. Please login again.");
                                $("#txtCheckAmount").val("");
                                $("#txtAccountNo").val("");
                                $("#txtAccountName").val("");
                                $("#txtRoutingNo").val("");
                                $("#txtConfirmAccountNo").val("");
                                $('input:radio[name="optatradio"]').filter('[value="C"]').attr('checked', true);
                                $("#txtConfirmAccountNo").val("");
                                kendo.ui.progress($("#dvdashboard-body"), false);
                            }
                            else {
                                document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                                document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                                document.getElementById("divCheckPaymentIcon").style.display = 'block';
                                $("#lblCheckAuthMessage").removeClass("green-text");
                                $("#lblCheckAuthMessage").addClass("orange-text");
                                $("#txtCheckAmount").val("");
                                $("#txtAccountNo").val("");
                                $("#txtAccountName").val("");
                                $("#txtRoutingNo").val("");
                                $("#txtConfirmAccountNo").val("");
                                $("#chkAutopayOff2").prop('checked', false);
                                $('input:radio[name="optatradio"]').filter('[value="C"]').attr('checked', true);
                                $("#txtConfirmAccountNo").val
                                kendo.ui.progress($("#dvdashboard-body"), false);
                                if (result.ResultCode == -2) {
                                    $("#lblCheckAuthMessage").text("This Transaction is not approved.Session has Expired.");
                                }
                                else {
                                    $("#lblCheckAuthMessage").text("This Transaction is not approved." + result.ResultMessage);
                                }
                                kendo.ui.progress($("#dvdashboard-body"), false);
                            }
                            if (result.profileStatusMessage != "" && result.profileStatusMessage != null) {
                                if (result.profileStatusCode == 1) {
                                    $("#lblBAProfileStatus").removeClass("orange-text");
                                    $("#lblBAProfileStatus").addClass("green-text");
                                }
                                else {
                                    $("#lblBAProfileStatus").removeClass("green-text");
                                    $("#lblBAProfileStatus").addClass("orange-text");
                                }
                                $("#lblBAProfileStatus").text("Profile Status: " + result.profileStatusMessage + ".");
                            }
                            else {
                                $("#lblBAProfileStatus").text('');
                            }
                        },
                        error: function (result) {
                            document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                            document.getElementById("divCheckPaymentIcon").style.display = 'block';
                            document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                            $("#lblCheckAuthMessage").removeClass("green-text");
                            $("#lblCheckAuthMessage").addClass("orange-text");
                            $("#lblCheckAuthMessage").text("This Transaction is not approved.");
                            $("#txtCheckAmount").val("");
                            $("#txtAccountNo").val("");
                            $("#txtAccountName").val("");
                            $("#txtRoutingNo").val("");
                            $("#txtConfirmAccountNo").val("");
                            $('input:radio[name="optatradio"]').filter('[value="C"]').attr('checked', true);
                            $("#txtConfirmAccountNo").val("");
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                    });
                });
            }
            else {
                document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                document.getElementById("divCheckPaymentIcon").style.display = 'block';
                document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                $("#lblCheckAuthMessage").text("Please enter Confirm Account No same as Account No");
                $("#lblCheckAuthMessage").removeClass("green-text");
                $("#lblCheckAuthMessage").addClass("orange-text");
                return false;
            }
        }
        else {
            document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
            document.getElementById("divCheckPaymentIcon").style.display = 'block';
            document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
            $("#lblCheckAuthMessage").text("Please fill all the details to make the Payment.");
            $("#lblCheckAuthMessage").removeClass("green-text");
            $("#lblCheckAuthMessage").addClass("orange-text");
            return false;
        }
    }
    else {//Pay by Bank Account Profile
        if ($("#txtCheckAmount").val() != "") {
            if ($("#txtCheckAmount").val() < MinPaymentAmount) {
                document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                document.getElementById("divCheckPaymentIcon").style.display = 'block';
                document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                $("#lblCheckAuthMessage").text("Minimum payment amount should be $ " + MinPaymentAmount);
                $("#lblCheckAuthMessage").removeClass("green-text");
                $("#lblCheckAuthMessage").addClass("orange-text");
                $("#txtCheckAmount").val('');
                return false;
            }

            var serialized = {
                ProfileID: ddProfileId,
                Amount: parseFloat($("#txtCheckAmount").val()).toFixed(2),
                Cust_no: CustNo,
                Type: "ACH",
                ConfirmationType: sendconfirmationtype,
                Email: Email,
                ContactNumber: ContactNumber,
                Source: "MyAccountWeb",
                FirstName: firstname,
                LastName: lastname
            };
            ConfirmPayPopUp(bankaccountno, $("#txtCheckAmount").val());
            $("#btnConfirm").click(function () {
                kendo.ui.progress($("#dvdashboard-body"), true);
                $.ajax({
                    url: 'MyAccount/FrontierPayByProfile',
                    type: "POST",
                    data: serialized,
                    success: function (result) {
                        if (result.ResultCode == 1) {
                            SendResponseToParent(result, parseFloat($("#txtCheckAmount").val()).toFixed(2), "FrontierPayByProfile");
                            $("#txtCheckAmount").val("");
                            $("#chkBAEmail").prop('checked', false);
                            $("#chkBASMS").prop('checked', false);
                            document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                            document.getElementById("divCheckPaymentIcon").className = "message-icon-payment";
                            document.getElementById("divCheckPaymentIcon").style.display = 'block';
                            $("#lblCheckAuthMessage").removeClass("orange-text");
                            $("#lblCheckAuthMessage").addClass("green-text");

                            //Success Response //Profile Response
                            $("#lblCheckAuthMessage").text("This Transaction has been approved. Confirmation No: " + result.AuthCode);
                            $('#chkPayusingSBA').prop('checked', false);
                            $("#divPaybyBA").show();
                            $("#divpaybybahid").hide();
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                        else if (result.ResultCode = -100)
                        {
                            document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                            document.getElementById("divCheckPaymentIcon").style.display = 'block';
                            document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                            $("#lblCheckAuthMessage").removeClass("green-text");
                            $("#lblCheckAuthMessage").addClass("orange-text");
                            $("#lblCheckAuthMessage").text("Session has expired. Please login again.");
                            $("#txtCheckAmount").val("");
                            $('#chkPayusingSBA').prop('checked', false);
                            $("#divPaybyBA").show();
                            $("#divpaybybahid").hide();
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                        else {
                            document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                            document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                            document.getElementById("divCheckPaymentIcon").style.display = 'block';
                            $("#lblCheckAuthMessage").removeClass("green-text");
                            $("#lblCheckAuthMessage").addClass("orange-text");
                            $('#chkPayusingSBA').prop('checked', false);
                            $("#divPaybyBA").show();
                            $("#divpaybybahid").hide();
                            kendo.ui.progress($("#dvdashboard-body"), false);
                            if (result.ResultCode == -2) {
                                $("#lblCheckAuthMessage").text("This Transaction is not approved.Session has Expired.");
                            }
                            else {
                                $("#lblCheckAuthMessage").text("This Transaction is not approved." + result.ResultMessage);
                            }
                        }
                    },
                    error: function (result) {
                        document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
                        document.getElementById("divCheckPaymentIcon").style.display = 'block';
                        document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
                        $("#lblCheckAuthMessage").removeClass("green-text");
                        $("#lblCheckAuthMessage").addClass("orange-text");
                        $("#lblCheckAuthMessage").text("This Transaction is not approved.");
                        $("#txtCheckAmount").val("");
                        $('#chkPayusingSBA').prop('checked', false);
                        $("#divPaybyBA").show();
                        $("#divpaybybahid").hide();
                        kendo.ui.progress($("#dvdashboard-body"), false);
                    }
                });
            });
        }
        else {
            document.getElementById("divCheckPaymentIcon").className = "message-icon-fail";
            document.getElementById("divCheckPaymentIcon").style.display = 'block';
            document.getElementById("divCheckAuthmessage").style.display = 'inline-block';
            $("#lblCheckAuthMessage").text("Please fill the Amount to make the Payment.");
            $("#lblCheckAuthMessage").removeClass("green-text");
            $("#lblCheckAuthMessage").addClass("orange-text");
            return false;
        }
    }
}

//Pay Now button click
//Shravan - 02/25/2014
function CreditcardPaynow() {    
    document.getElementById("divCreditcardAuthmessage").style.display = 'none';
    document.getElementById("divCreditcardPaymentIcon").style.display = 'none';
    var sendconfirmationtype = "NONE";

    if ($("#chkEmail").is(':checked'))
        sendconfirmationtype = "EMAIL";
    if ($("#chkSMS").is(':checked'))
        sendconfirmationtype = "SMS";
    if ($("#chkSMS").is(':checked') && $("#chkEmail").is(':checked'))
        sendconfirmationtype = "BOTH";
    if ((!$("#chkSMS").is(':checked')) && (!$("#chkEmail").is(':checked')))
        sendconfirmationtype = "NONE";

    if (!$("#chkPayusingSCC").is(':checked')) {//Checking Payby profile is selected for CC
        if ($("#txtCardNumber").val() != "" && $("#txtNameOntheCard").val() != "" && $("#txtCVCCode").val() != "" && $("#txtBillingZip").val() != "" && $("#txtAmount").val() != "" && $("#expyear").val() != "" && $("#expmonth").val() != "") {
            var ccExpYear = parseInt(20 + $("#expyear").val());
            var ccExpMonth = parseInt($("#expmonth").val());
            var expDate = new Date();
            expDate.setFullYear(ccExpYear, ccExpMonth, 31);
            var today = new Date();
            if (expDate < today) {
                document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                document.getElementById("divCreditcardPaymentIcon").style.display = 'inline-block';
                document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                $("#lblCreditcardAuthMessage").text("This credit card has expired! Please use another card.");
                $("#lblCreditcardAuthMessage").removeClass("green-text");
                $("#lblCreditcardAuthMessage").addClass("orange-text");
            } else {
                var today = new Date();
                var expiry = $("#expmonth").val() + "/" + $("#expyear").val();

                var autopay;
                if ($("#chkAutopayOff").attr("checked") == "checked") {
                    autopay = true;
                } else {
                    autopay = false;
                }

                if ($("#txtAmount").val() < MinPaymentAmount) {
                    document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                    document.getElementById("divCreditcardPaymentIcon").style.display = 'inline-block';
                    document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                    $("#lblCreditcardAuthMessage").text("Minimum payment amount should be $ " + MinPaymentAmount);
                    $("#lblCreditcardAuthMessage").removeClass("green-text");
                    $("#lblCreditcardAuthMessage").addClass("orange-text");
                    $("#txtAmount").val('');
                    return false;
                }
                var serialized = {
                    CustId: custid,
                    Amount: parseFloat($("#txtAmount").val()).toFixed(2),
                    CardNumber: $("#txtCardNumber").val(),
                    CardName: $("#txtNameOntheCard").val(),
                    CardType: $("#hdncreditcardtype").val(),
                    Expiry: expiry,
                    ZipCode: $("#txtBillingZip").val(),
                    CustomerNumber: CustNo,
                    VerificationCode: $("#txtCVCCode").val(),
                    SetupAutoPay: autopay,
                    PaymentorAutopay: "PayNow",
                    ConfirmationType: sendconfirmationtype,
                    Email: Email,
                    ContactNumber: ContactNumber,
                    Source: "MyAccountWeb",
                    FirstName: firstname,
                    LastName: lastname,
                    IsWriteoffAmount: IsWriteoffAmount
                };
                ConfirmPayPopUp($("#txtCardNumber").val().substr($("#txtCardNumber").val().length - 4), $("#txtAmount").val());
                $("#btnConfirm").click(function () {
                    kendo.ui.progress($("#dvdashboard-body"), true);
                    $.ajax({
                        url: 'MyAccount/MakePayment',
                        type: "POST",
                        data: serialized,
                        success: function (result) {
                            if (result.ResultCode == 1) {
                                SendResponseToParent(result, parseFloat($("#txtAmount").val()).toFixed(2), "MakePayment");
                                $("#txtNameOntheCard").val("");
                                $("#txtCardNumber").val("");
                                $("#txtCardNumber").removeClass();
                                $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                                $("#txtBillingZip").val("");
                                $("#txtCVCCode").val("");
                                $("#txtAmount").val("");
                                $('#expmonth').data('kendoDropDownList').value(-1);
                                $('#expyear').data('kendoDropDownList').value(-1);
                                $("#chkAutopayOff").prop('checked', false);
                                $("#chkEmail").prop('checked', false);
                                $("#chkSMS").prop('checked', false);
                                document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                                document.getElementById("divCreditcardPaymentIcon").className = "message-icon-payment";
                                document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                                $("#lblCreditcardAuthMessage").removeClass("orange-text");
                                $("#lblCreditcardAuthMessage").addClass("green-text");

                                $("#lblCreditcardAuthMessage").text("This Transaction has been approved. Confirmation No: " + result.AuthCode);

                                $("#divCCAutopaySection").hide();
                                $("#divBAAutopaySection").hide();
                                $("#chkAutopayOff").prop('checked', false);
                                kendo.ui.progress($("#dvdashboard-body"), false);
                            }
                            else if (result.ResultCode == -100)
                            {
                                document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                                document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                                document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';                                
                                $("#lblCreditcardAuthMessage").removeClass("green-text");
                                $("#lblCreditcardAuthMessage").addClass("orange-text");
                                $("#lblCreditcardAuthMessage").text("Session has expired. Please login again.");
                                $("#txtNameOntheCard").val("");
                                $("#txtCardNumber").val("");
                                $("#txtCardNumber").removeClass();
                                $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                                $("#txtBillingZip").val("");
                                $("#txtCVCCode").val("");
                                $("#txtAmount").val("");
                                $('#expmonth').data('kendoDropDownList').value(-1);
                                $('#expyear').data('kendoDropDownList').value(-1);
                                kendo.ui.progress($("#dvdashboard-body"), false);
                            }
                            else {
                                document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                                document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                                document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                                $("#lblCreditcardAuthMessage").removeClass("green-text");
                                $("#lblCreditcardAuthMessage").addClass("orange-text");
                                $("#chkAutopayOff").prop('checked', false);
                                $("#txtNameOntheCard").val("");
                                $("#txtCardNumber").removeClass();
                                $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                                $("#txtCardNumber").val("");
                                $("#txtBillingZip").val("");
                                $("#txtCVCCode").val("");
                                $("#txtAmount").val("");
                                $('#expmonth').data('kendoDropDownList').value(-1);
                                $('#expyear').data('kendoDropDownList').value(-1);
                                kendo.ui.progress($("#dvdashboard-body"), false);
                                if (result.ResultCode == -2) {
                                    $("#lblCreditcardAuthMessage").text("This Transaction is not approved.Session has expired");
                                }
                                else {
                                    $("#lblCreditcardAuthMessage").text("This Transaction is not approved. " + result.ResultMessage);
                                }
                            }
                            if (result.profileStatusMessage != "" && result.profileStatusMessage != null) {
                                if (result.profileStatusCode == 1) {
                                    $("#lblProfileStatus").removeClass("orange-text");
                                    $("#lblProfileStatus").addClass("green-text");
                                }
                                else {
                                    $("#lblProfileStatus").removeClass("green-text");
                                    $("#lblProfileStatus").addClass("orange-text");
                                }
                                $("#lblProfileStatus").text("Profile Status: " + result.profileStatusMessage);
                            }
                            else {
                                $("#lblProfileStatus").text('');
                            }

                        },
                        error: function (result) {
                            document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                            document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                            document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                            $("#lblCreditcardAuthMessage").text("This Transaction is declined.");
                            $("#lblCreditcardAuthMessage").removeClass("green-text");
                            $("#lblCreditcardAuthMessage").addClass("orange-text");
                            $("#txtNameOntheCard").val("");
                            $("#txtCardNumber").val("");
                            $("#txtCardNumber").removeClass();
                            $("#txtCardNumber").addClass('k-textbox f-textbox fl');
                            $("#txtBillingZip").val("");
                            $("#txtCVCCode").val("");
                            $("#txtAmount").val("");
                            $('#expmonth').data('kendoDropDownList').value(-1);
                            $('#expyear').data('kendoDropDownList').value(-1);
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                    });
                });
            }
        }
        else {
            document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
            document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
            document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
            $("#lblCreditcardAuthMessage").text("Please fill all the details to make the Payment");
            $("#lblCreditcardAuthMessage").removeClass("green-text");
            $("#lblCreditcardAuthMessage").addClass("orange-text");
            $("#lblProfileStatus").text('');
            kendo.ui.progress($("#dvdashboard-body"), false);
            return false;
        }
    }
    else {
        if ($("#txtAmount").val() != "") {
            if ($("#txtAmount").val() < MinPaymentAmount) {
                document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                $("#lblCreditcardAuthMessage").text("Minimum payment amount should be $ " + MinPaymentAmount);
                $("#lblCreditcardAuthMessage").removeClass("green-text");
                $("#lblCreditcardAuthMessage").addClass("orange-text");
                $("#txtAmount").val('');
                return false;
            }

            var serialized = {
                ProfileID: ccProfileId,
                Amount: parseFloat($("#txtAmount").val()).toFixed(2),
                Cust_no: CustNo,
                Type: "",
                ConfirmationType: sendconfirmationtype,
                Email: Email,
                ContactNumber: ContactNumber,
                Source: "MyAccountWeb",
                FirstName: firstname,
                LastName: lastname
            };
            ConfirmPayPopUp(creditcardno, $("#txtAmount").val());
            $("#btnConfirm").click(function () {
                kendo.ui.progress($("#dvdashboard-body"), true);
                $.ajax({
                    url: 'MyAccount/FrontierPayByProfile',
                    type: "POST",
                    data: serialized,
                    success: function (result) {
                        if (result.ResultCode == 1) {
                            SendResponseToParent(result, parseFloat($("#txtAmount").val()).toFixed(2), "FrontierPayByProfile");
                            $("#txtAmount").val("");
                            $("#chkEmail").prop('checked', false);
                            $("#chkSMS").prop('checked', false);
                            document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                            document.getElementById("divCreditcardPaymentIcon").className = "message-icon-payment";
                            document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                            $("#lblCreditcardAuthMessage").removeClass("orange-text");
                            $("#lblCreditcardAuthMessage").addClass("green-text");

                            //Success Response //Profile Response
                            $("#lblCreditcardAuthMessage").text("This Transaction has been approved. Confirmation No: " + result.AuthCode);
                            $("#chkPayusingSCC").prop('checked', false);
                            $("#divPaybyCC").show();
                            $("#divpaybycchid").hide();
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                        else if (result.ResultCode == -100) {
                            document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                            document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                            document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                            $("#lblCreditcardAuthMessage").text("Session has expired. Please login again.");
                            $("#lblCreditcardAuthMessage").removeClass("green-text");
                            $("#lblCreditcardAuthMessage").addClass("orange-text");
                            $("#txtAmount").val("");
                            $("#chkPayusingSCC").prop('checked', false);
                            $("#divPaybyCC").show();
                            $("#divpaybycchid").hide();
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                        else {
                            document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                            document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                            document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                            $("#lblCreditcardAuthMessage").removeClass("green-text");
                            $("#lblCreditcardAuthMessage").addClass("orange-text");
                            $("#txtAmount").text('');
                            $("#chkPayusingSCC").prop('checked', false);
                            $("#divPaybyCC").show();
                            $("#divpaybycchid").hide();
                            kendo.ui.progress($("#dvdashboard-body"), false);
                            if (result.ResultCode == -2) {
                                $("#lblCreditcardAuthMessage").text("This Transaction is not approved.Session has expired");
                            }
                            else {
                                $("#lblCreditcardAuthMessage").text("This Transaction is not approved. " + result.ResultMessage);
                            }
                        }
                    },
                    error: function (result) {
                        document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
                        document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
                        document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
                        $("#lblCreditcardAuthMessage").text("This Transaction is declined.");
                        $("#lblCreditcardAuthMessage").removeClass("green-text");
                        $("#lblCreditcardAuthMessage").addClass("orange-text");
                        $("#txtAmount").val("");
                        $("#chkPayusingSCC").prop('checked', false);
                        $("#divPaybyCC").show();
                        $("#divpaybycchid").hide();
                        kendo.ui.progress($("#dvdashboard-body"), false);
                    }
                });
            });
        }
        else {
            document.getElementById("divCreditcardPaymentIcon").className = "message-icon-fail";
            document.getElementById("divCreditcardPaymentIcon").style.display = 'block';
            document.getElementById("divCreditcardAuthmessage").style.display = 'inline-block';
            $("#lblCreditcardAuthMessage").text("Please fill the Amount to make the Payment");
            $("#lblCreditcardAuthMessage").removeClass("green-text");
            $("#lblCreditcardAuthMessage").addClass("orange-text");
            return false;
        }
    }
}

function SetupAutoPayfun() {
    //Credit Card Setup Autopay
    var sendconfirmationtype = "NONE";

    if ($("#chkAutoPayEmail").is(':checked'))
        sendconfirmationtype = "EMAIL";
    if ($("#chkAutoPaySMS").is(':checked'))
        sendconfirmationtype = "SMS";
    if ($("#chkAutoPaySMS").is(':checked') && $("#chkAutoPayEmail").is(':checked'))
        sendconfirmationtype = "BOTH";
    if ((!$("#chkAutoPaySMS").is(':checked')) && (!$("#chkAutoPayEmail").is(':checked')))
        sendconfirmationtype = "NONE";

    if ($('input[name=optradio]:checked').val() == 1) {
        if ($("#txtCardNumberAutopay").val() != "" && $("#txtNameOntheCardAutopay").val() != "" && $("#txtBillingZipAutopay").val() != "" && $("#txtCVCCodeAutopay").val() != "") {
            var ccExpYear = parseInt(20 + $("#expyearAutopay").val());
            var ccExpMonth = parseInt($("#expmonthAutopay").val());
            var expDate = new Date();
            expDate.setFullYear(ccExpYear, ccExpMonth, 31);
            var today = new Date();
            if (expDate < today) {
                document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                document.getElementById("divAutoPayIcon").style.display = 'block';
                document.getElementById("divAutopaymessage").style.display = 'inline-block';
                $("#lblAutoPayMessage").removeClass("green-text");
                $("#lblAutoPayMessage").addClass("orange-text");
                $("#lblAutoPayMessage").text("This credit card has expired! Please use another card.");
            } else {
                var expiry = $("#expmonthAutopay").val() + "/" + $("#expyearAutopay").val();
                var serialized = {
                    CustId: custid,
                    Amount: "0",
                    CardNumber: $("#txtCardNumberAutopay").val(),
                    CardName: $("#txtNameOntheCardAutopay").val(),
                    CardType: $("#hdncreditcardtypeAutopay").val(),
                    Expiry: expiry,
                    ZipCode: $("#txtBillingZipAutopay").val(),
                    CustomerNumber: CustNo,
                    VerificationCode: $("#txtCVCCodeAutopay").val(),
                    SetupAutoPay: true,
                    PaymentorAutopay: "AutoPay",
                    ConfirmationType: sendconfirmationtype,
                    Email: Email,
                    ContactNumber: ContactNumber,
                    Source: "MyAccountWeb",
                    FirstName: firstname,
                    LastName: lastname,
                    IsWriteoffAmount: IsWriteoffAmount
                };
                kendo.ui.progress($("#dvdashboard-body"), true);
                $.ajax({
                    url: 'MyAccount/MakePayment',
                    type: "POST",
                    data: serialized,
                    success: function (result) {
                        $("#txtCardNumberAutopay").val("");
                        $("#txtCardNumberAutopay").removeClass();
                        $("#txtCardNumberAutopay").addClass('k-textbox f-textbox fl');
                        $("#txtNameOntheCardAutopay").val("");
                        $("#txtBillingZipAutopay").val("");
                        $("#txtCVCCodeAutopay").val("");
                        $('#expmonthAutopay').data('kendoDropDownList').value(-1);
                        $('#expyearAutopay').data('kendoDropDownList').value(-1);
                        $("#chkAutoPayEmail").prop('checked', false);
                        $("#chkAutoPaySMS").prop('checked', false);
                        if (result.resu == -100)
                        {
                            document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                            document.getElementById("divAutoPayIcon").style.display = 'block';
                            document.getElementById("divAutopaymessage").style.display = 'inline-block';
                            $("#lblAutoPayMessage").removeClass("green-text");
                            $("#lblAutoPayMessage").addClass("orange-text");
                            $("#lblAutoPayMessage").text("Session has expired. Please login again.");
                            kendo.ui.progress($("#dvdashboard-body"), false);
                        }
                        else {
                            if (result.profileStatusCode == 1) {
                                SendResponseToParent(result, 0, "MakePayment");
                                document.getElementById("divAutoPayIcon").className = "message-icon-payment";
                                document.getElementById("divAutoPayIcon").style.display = 'block';
                                document.getElementById("divAutopaymessage").style.display = 'inline-block';
                                $("#lblAutoPayMessage").removeClass("orange-text");
                                $("#lblAutoPayMessage").addClass("green-text");
                                if ($("#btnSetupAutoPay").text() == "Enroll Auto Pay") {
                                    $("#lblAutoPayMessage").text("You have successfully enrolled in Credit Card Auto Pay.");
                                }
                                else if ($("#btnSetupAutoPay").text() == "Update Card Details") {
                                    $("#lblAutoPayMessage").text("You have successfully updated Credit Card details for Auto Pay.");
                                }
                                kendo.ui.progress($("#dvdashboard-body"), false);
                            }
                            else {
                                document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                                document.getElementById("divAutoPayIcon").style.display = 'block';
                                document.getElementById("divAutopaymessage").style.display = 'inline-block';
                                $("#lblAutoPayMessage").removeClass("green-text");
                                $("#lblAutoPayMessage").addClass("orange-text");
                                kendo.ui.progress($("#dvdashboard-body"), false);
                                if (result.profileStatusCode == -2) {
                                    $("#lblAutoPayMessage").text("Credit Card Details are not updated for Auto Pay.Session has expired");
                                }
                                else {
                                    $("#lblAutoPayMessage").text(result.profileStatusMessage);
                                }
                            }
                        }
                    },
                    error: function (result) {
                        $("#txtCardNumberAutopay").val("");
                        $("#txtCardNumberAutopay").removeClass();
                        $("#txtCardNumberAutopay").addClass('k-textbox f-textbox fl');
                        $("#txtNameOntheCardAutopay").val("");
                        $("#txtBillingZipAutopay").val("");
                        $("#txtCVCCodeAutopay").val("");
                        $('#expmonthAutopay').data('kendoDropDownList').value(-1);
                        $('#expyearAutopay').data('kendoDropDownList').value(-1);
                        document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                        document.getElementById("divAutoPayIcon").style.display = 'block';
                        document.getElementById("divAutopaymessage").style.display = 'inline-block';
                        $("#lblAutoPayMessage").removeClass("green-text");
                        $("#lblAutoPayMessage").addClass("orange-text");
                        $("#lblAutoPayMessage").text("Credit Card Details are not updated for Auto Pay");
                        kendo.ui.progress($("#dvdashboard-body"), false);
                    }
                });
            }
        }
        else {
            document.getElementById("divAutopaymessage").style.display = 'inline-block';
            document.getElementById("divAutoPayIcon").className = "message-icon-fail";
            document.getElementById("divAutoPayIcon").style.display = 'block';
            $("#lblAutoPayMessage").text("Please fill all the Credit Card details.");
            $("#lblAutoPayMessage").removeClass("green-text");
            $("#lblAutoPayMessage").addClass("orange-text");
            kendo.ui.progress($("#dvdashboard-body"), false);
            return false;
        }
    }
        //ACH Setup Autopay
    else if ($('input[name=optradio]:checked').val() == 2) {

        if ($("#txtBAAccountName").val() != "" && $("#txtBAAccountNo").val() != "" && $("#txtBARoutingNo").val() != "" && $('input[name=optbaatradio]:checked').val() != "" && $("#txtBAConfirmAccountNo").val() != "") {
            if ($("#txtBAConfirmAccountNo").val() == $("#txtBAAccountNo").val()) {

                var serialized = {
                    CustId: custid,
                    Amount: "0",
                    BankAccountNumber: $("#txtBAAccountNo").val(),
                    BankAccountName: $("#txtBAAccountName").val(),
                    AccountType: $('input[name=optbaatradio]:checked').val(),
                    CustomerNumber: CustNo,
                    BankRoutingNumber: $("#txtBARoutingNo").val(),
                    SetupAutoPay: true,
                    PaymentorAutopay: "AutoPay",
                    ConfirmationType: sendconfirmationtype,
                    Email: Email,
                    ContactNumber: ContactNumber,
                    Source: "MyAccountWeb",
                    FirstName: firstname,
                    LastName: lastname,
                    IsWriteoffAmount: IsWriteoffAmount
                };
                kendo.ui.progress($("#dvdashboard-body"), true);
                $.ajax({
                    url: 'MyAccount/FrontierPayCheckPayment',
                    type: "POST",
                    data: serialized,
                    success: function (result) {
                        if (result.ResultCode == -100)
                        {
                            $("#txtBAAccountName").val("");
                            $("#txtBAAccountNo").val("");
                            $("#txtBARoutingNo").val("");
                            $('input:radio[name="optrbaatadio"]').filter('[value="1"]').attr('checked', true);
                            $("#txtBAConfirmAccountNo").val("");
                            $("#chkAutoPayEmail").prop('checked', false);
                            $("#chkAutoPaySMS").prop('checked', false);
                            document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                            document.getElementById("divAutoPayIcon").style.display = 'block';
                            $("#lblAutoPayMessage").removeClass("green-text");
                            $("#lblAutoPayMessage").addClass("orange-text");
                            document.getElementById("divAutopaymessage").style.display = 'inline-block';
                            $("#lblAutoPayMessage").text("Session has expired. Please login again.");
                        }
                        else {
                            if (result.profileStatusCode == 1) {
                                SendResponseToParent(result, 0, "FrontierPayCheckPayment");
                                $("#txtBAAccountName").val("");
                                $("#txtBAAccountNo").val("");
                                $("#txtBARoutingNo").val("");
                                $('input:radio[name="optrbaatadio"]').filter('[value="1"]').attr('checked', true);
                                $("#txtBAConfirmAccountNo").val("");
                                $("#chkAutoPayEmail").prop('checked', false);
                                $("#chkAutoPaySMS").prop('checked', false);
                                document.getElementById("divAutoPayIcon").className = "message-icon-payment";
                                document.getElementById("divAutoPayIcon").style.display = 'block';
                                $("#lblAutoPayMessage").removeClass("orange-text");
                                $("#lblAutoPayMessage").addClass("green-text");
                                document.getElementById("divAutopaymessage").style.display = 'inline-block';

                                if ($("#btnSetupAutoPay").text() == "Enroll Auto Pay") {
                                    $("#lblAutoPayMessage").text("You have successfully enrolled in Bank Draft Auto Pay.");
                                }
                                else if ($("#btnSetupAutoPay").text() == "Update Bank Details") {
                                    $("#lblAutoPayMessage").text("You have successfully updated Bank Draft details for Auto Pay.");
                                }
                            }
                            else {
                                $("#txtBAAccountName").val("");
                                $("#txtBAAccountNo").val("");
                                $("#txtBARoutingNo").val("");
                                $('input:radio[name="optrbaatadio"]').filter('[value="1"]').attr('checked', true);
                                $("#txtBAConfirmAccountNo").val("");
                                document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                                document.getElementById("divAutoPayIcon").style.display = 'block';
                                $("#lblAutoPayMessage").removeClass("green-text");
                                $("#lblAutoPayMessage").addClass("orange-text");
                                document.getElementById("divAutopaymessage").style.display = 'inline-block';
                                if (result.profileStatusCode == -2) {
                                    $("#lblAutoPayMessage").text("Details are not enrolled/updated.Session has expired");
                                }
                                else {
                                    $("#lblAutoPayMessage").text(result.profileStatusMessage);
                                }
                            }
                        }
                        kendo.ui.progress($("#dvdashboard-body"), false);
                    },
                    error: function (result) {
                        $("#txtBAAccountName").val("");
                        $("#txtBAAccountNo").val("");
                        $("#txtBARoutingNo").val("");
                        $('input:radio[name="optrbaatadio"]').filter('[value="1"]').attr('checked', true);
                        $("#txtBAConfirmAccountNo").val("");
                        $("#chkAutoPayEmail").prop('checked', false);
                        $("#chkAutoPaySMS").prop('checked', false);
                        document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                        document.getElementById("divAutoPayIcon").style.display = 'block';
                        $("#lblAutoPayMessage").removeClass("green-text");
                        $("#lblAutoPayMessage").addClass("orange-text");
                        document.getElementById("divAutopaymessage").style.display = 'inline-block';
                        $("#lblAutoPayMessage").text("Bank Account Details are not updated for Auto Pay.");
                        kendo.ui.progress($("#dvdashboard-body"), false);
                    }
                });
            }
            else {
                document.getElementById("divAutopaymessage").className = "message-icon-fail";
                document.getElementById("divAutopaymessage").style.display = 'inline-block';
                $("#lblAutoPayMessage").removeClass("green-text");
                $("#lblAutoPayMessage").addClass("orange-text");
                $("#lblAutoPayMessage").text("Please enter Confirm Account No same as Account No");
                return false;
            }
        }
        else {
            document.getElementById("divAutopaymessage").style.display = 'inline-block';
            document.getElementById("divAutoPayIcon").className = "message-icon-fail";
            document.getElementById("divAutoPayIcon").style.display = 'block';
            $("#lblAutoPayMessage").text("Please fill all the Bank Account details.");
            $("#lblAutoPayMessage").removeClass("green-text");
            $("#lblAutoPayMessage").addClass("orange-text");
            kendo.ui.progress($("#dvdashboard-body"), false);
            return false;
        }

    }
}
//Disable or De Enroll Autopay
function DisableAutoPay() {
    var sendconfirmationtype = "NONE";

    if ($("#chkAutoPayEmail").is(':checked'))
        sendconfirmationtype = "EMAIL";
    if ($("#chkAutoPaySMS").is(':checked'))
        sendconfirmationtype = "SMS";
    if ($("#chkAutoPaySMS").is(':checked') && $("#chkAutoPayEmail").is(':checked'))
        sendconfirmationtype = "BOTH";
    if ((!$("#chkAutoPaySMS").is(':checked')) && (!$("#chkAutoPayEmail").is(':checked')))
        sendconfirmationtype = "NONE";
    var type = "";
    ConfirmPopUp("De-Enroll Auto Pay", "Are you sure you want to De-Enroll from Auto Pay?");
    $("#btnConfirm").click(function () {

        //$("#divconfirmpopup").data("kendoWindow").close();
        if ($('input[name=optradio]:checked').val() == 2) {
            type = "ACH";
        }
        var serialized = {
            Type: type,
            ConfirmationType: sendconfirmationtype,
            Email: Email,
            ContactNumber: ContactNumber,
            Source: "MyAccountWeb",
            FirstName: firstname,
            LastName: lastname,
            Cust_no: CustNo
        };
        kendo.ui.progress($("#dvdashboard-body"), true);
        $.ajax({
            url: 'MyAccount/FrontierPayDeletePaymentProfile',
            type: "POST",
            data: serialized,
            success: function (result) {
                if (result.ResultCode == -100)
                {
                    document.getElementById("divAutopaymessage").style.display = 'inline-block';
                    document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                    document.getElementById("divAutoPayIcon").style.display = 'block';
                    $("#lblAutoPayMessage").removeClass("green-text");
                    $("#lblAutoPayMessage").addClass("orange-text");
                    $("#lblAutoPayMessage").text("Session has expired. Please login again.");
                    $("#chkAutoPayEmail").prop('checked', false);
                    $("#chkAutoPaySMS").prop('checked', false);
                }
                else {
                    if (result.profileStatusCode == 1) {
                        SendResponseToParent(result, 0, "FrontierPayDeletePaymentProfile");
                        document.getElementById("divAutoPayIcon").className = "message-icon-payment";
                        document.getElementById("divAutoPayIcon").style.display = 'block';
                        document.getElementById("divAutopaymessage").style.display = 'inline-block';
                        $("#lblAutoPayMessage").removeClass("orange-text");
                        $("#lblAutoPayMessage").addClass("green-text");
                        $("#chkAutoPayEmail").prop('checked', false);
                        $("#chkAutoPaySMS").prop('checked', false);
                        $("#lblAutoPayMessage").text("Auto Pay disabled successfully.");
                    }
                    else {
                        document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                        document.getElementById("divAutoPayIcon").style.display = 'block';
                        document.getElementById("divAutopaymessage").style.display = 'inline-block';
                        $("#lblAutoPayMessage").removeClass("green-text");
                        $("#lblAutoPayMessage").addClass("orange-text");
                        if (result.profileStatusCode == -2) {
                            $("#lblAutoPayMessage").text("Bank Account Details are not updated for Auto Pay.Session has expired");
                        }
                        else {
                            $("#lblAutoPayMessage").text(result.profileStatusMessage);
                        }
                        
                    }
                }
                kendo.ui.progress($("#dvdashboard-body"), false);
            },
            error: function (result) {
                document.getElementById("divAutopaymessage").style.display = 'inline-block';
                document.getElementById("divAutoPayIcon").className = "message-icon-fail";
                document.getElementById("divAutoPayIcon").style.display = 'block';
                $("#lblAutoPayMessage").removeClass("green-text");
                $("#lblAutoPayMessage").addClass("orange-text");
                $("#lblAutoPayMessage").text("Bank Account Details are not updated for Auto Pay.");
                $("#chkAutoPayEmail").prop('checked', false);
                $("#chkAutoPaySMS").prop('checked', false);
                kendo.ui.progress($("#dvdashboard-body"), false);
            }
        });
    });
}

function FilterInput(event) {
    var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
    if ((chCode < 46 || chCode > 57) && chCode != 8 && chCode != 0)
        return false;
    return true;
}

function onpaymentDataBound() {
    CreateToolTip();
}

function onAdditionalData() {

    return {
        AccountNo: CustNo,
        Zip: ZipCode
    };

}

function checkisnan(value) {
    var retval = 0;
    if (value.indexOf("NaN") != -1 || value.indexOf("Infinity") != -1 || value == "")
        retval = 0;
    else
        retval = value;

    return retval;
}

function DisplayMessage(text, onExit) {
    var kendoWindow = $("<div id='divMessage'/>").kendoWindow({
        title: "Message ",
        resizable: false,
        position: { top: 10, left: 10 },
        modal: true,
        deactivate: function () {
            this.destroy();
            if (onExit)
                onExit();
        },
    });
    kendoWindow.data("kendoWindow")
            .content('<div style="width:400px; padding:10px;"><label id="lblMessage" class="f-label"></label></div>').
            center().open();
    kendoWindow
 .find(".No")
 .click(function () {
     kendoWindow.data("kendoWindow").close();
 })
 .end();
    $("#lblMessage").html(text);
}

function FilterInput(event) {
    var chCode = ('charCode' in event) ? event.charCode : event.keyCode;
    if ((chCode < 46 || chCode > 57) && chCode != 8 && chCode != 0)
        return false;
    return true;
}

function tabstrip_select(e) {
    var x = e.item;
    var selectedIndex = $(e.item).index();
    $("#lblProfileStatus").text('');
    $("#lblBAProfileStatus").text('');
    $("#lblCreditcardAuthMessage").text('');
    $("#lblAutoPayMessage").text('');
    $("#lblCheckAuthMessage").text('');
    $("#divCreditcardPaymentIcon").hide();
    $("#divAutoPayIcon").hide();
    $("#divCheckPaymentIcon").hide();
    if (selectedIndex == 2) {
        $("#divbapayfrmleft").addClass("addmargin-divbapayfrmleft");
        $("#divMainAutopay").removeClass("bankdraft-div-height");
        $("#divbasec").removeClass("bankdraft-main-sec");
        if ($('input[name=optradio]:checked').val() == 1) {
            $("#divMainAutopay").addClass("autopay-div-height");
            $("#divautopaysec").addClass("autopay-main-sec");
            $("#divbapayfrmleft").removeClass("addmargin-divbapayfrmleft");
        }
        if ($('input[name=optradio]:checked').val() == 2) {
            $("#divMainAutopay").addClass("autopaybankdraft-div-height");
            $("#divautopaysec").addClass("autopaybankdraft-main-sec");
        }
    }
    else if (selectedIndex == 1) {
        $("#divMainAutopay").addClass("bankdraft-div-height");
        $("#divbasec").addClass("bankdraft-main-sec");
        $("#divMainAutopay").removeClass("autopaybankdraft-div-height");
    }
    else {
        $("#divMainAutopay").removeClass("autopay-div-height");
        $("#divautopaysec").removeClass("autopay-main-sec");
        $("#divbapayfrmleft").removeClass("addmargin-divbapayfrmleft");
        $("#divMainAutopay").removeClass("bankdraft-div-height");
        $("#divbasec").removeClass("bankdraft-main-sec");
        $("#divMainAutopay").removeClass("autopaybankdraft-div-height");
    }
}

function bindEvent(element, eventName, eventHandler) {
    if (element.addEventListener) {
        element.addEventListener(eventName, eventHandler, false);
    } else if (element.attachEvent) {
        element.attachEvent('on' + eventName, eventHandler);
    }
}

function ConfirmPopUp(title, message) {
    var kendoWindow = $("<div id='divConfirmpopup' />").kendoWindow({
        title: title,
        resizable: false,
        position: { top: 10, left: 10 },
        modal: true,
        actions: [

        ],
        deactivate: function () {
            this.destroy();
        },
    });
    kendoWindow.data("kendoWindow")
            .content($("#confirmpopup").html()).
            center().open();
    kendoWindow
 .find(".popupcancel")
 .click(function () {
     kendoWindow.data("kendoWindow").close();
 })
 .end();

    $("#confirmpopupbody").text(message);
}

function ConfirmPayPopUp(cardno, amount) {

    var kendoWindow = $("<div id='divConfirmpay' />").kendoWindow({
        title: "Payment-Confirmation",
        resizable: false,
        position: { top: 10, left: 10 },
        modal: true,
        actions: [

        ],
        deactivate: function () {
            this.destroy();
        },
    });
    kendoWindow.data("kendoWindow")
            .content($("#template-confirmpay").html()).
            center().open();
    $("#lblcustaccountno").html(cardno);
    $("#lblamount").html(parseFloat(amount).toFixed(2));
    $("#lblCustName").text(firstname + ' ' + lastname);
    kendoWindow
 .find(".popupcancel")
 .click(function () {
     kendoWindow.data("kendoWindow").close();
 })
 .end();
}

function SendResponseToParent(payresponse, Amount, methodname) {
    $.extend(payresponse, { MethodName: methodname, Amount: Amount, IsValidate: "0" });
    window.parent.postMessage(JSON.stringify(payresponse), '*');
}
//Prepay wizard values
function funPrepayWizad() {
    kendo.ui.progress($('#divPrepay'), true);
    var serialize = {
        accountno: CustNo
    }
    $("#txtRechargeAmount").val("50");
    $.ajax({
        url: 'MyAccount/PrepayWizad',
        data: serialize,
        type: "POST",
        success: function (result) {
            if (result != null) {
                var AmountLeftforpurchase = 0;
                var str = "";
                if (result.Data.length <= 0) {
                    $(".prepay-box-2").hide();
                }
                $("#lblprepayRate").text(parseFloat(result.Rate).toFixed(1));
                $("#lblprepayEstimate").text(result.Balance);
                $("#lblprepayDUsage").text(parseFloat(result.AvgUsage).toFixed(2));
                $("#spanAvgDailyUsage").html(parseFloat(result.AvgUsage).toFixed(2) + " kWh");
                for (var i = 0; i < result.Data.length; i++) {

                    str = str + "<div class='form-element-10'>" +
                        "<label class='right-padding-10 fl ss-label ' style='width: 120px'>" + result.Data[i].charge_desc + " :" + "</label>" +
                        "<span class='green-text fl'>$ </span><label class='right-padding-10 fl ss-label font-bold green-text' >" + result.Data[i].total_amount + "</label>" +
                        "<div class='clear'></div>" +
                    "</div>";
                    AmountLeftforpurchase = AmountLeftforpurchase + result.Data[i].total_amount;
                }
                $('#divfeedues').html(str);
                $("#lblTotalFeesDues").text(AmountLeftforpurchase);
                $("#txtRechargeAmount").keyup();
                if (parseFloat(result.Rate) > 0 && parseFloat(result.AvgUsage) != 0) {
                    var avgusag;
                    avgusag = (parseFloat(result.AvgUsage) * parseFloat(result.Rate)) / 100;
                    $("#calAvgdailyusage").html(" [ $ " + avgusag.toFixed(2) + " /day ]")
                }

                if (parseFloat(result.Rate) > 0 && parseFloat(EstPowerBalance) != 0) {
                    var estbal;
                    estbal = (parseFloat(EstPowerBalance) * parseFloat(result.Rate)) / 100;
                    $("#calEstPowerBalance").html(" [ $ " + estbal.toFixed(2) + " ]")
                }
                // Current Estimate days left=Est Power Balance /Average Daily Usage KWH
                $("#lblEstDaysLeftwithprebal").text(Math.round(parseFloat(EstPowerBalance) / result.AvgUsage.toFixed(2)));
                if ($("#lblEstDaysLeftwithprebal").text().indexOf("NaN") != -1 || $("#lblEstDaysLeftwithprebal").text().indexOf("Infinity") != -1 || $("#lblEstDaysLeftwithprebal").text() == "")
                    $("#lblEstDaysLeftwithprebal").text(0);

            }
            kendo.ui.progress($('#divPrepay'), false);
        },
        error: function (result) {
            kendo.ui.progress($('#divPrepay'), false);
        }
    });
}

function RefreshData() {
    
    $("#lblCCNumber").text(creditcardno);//Card type to display
    $("#lblBANumber").text(bankaccountno);//Card type to display
    $("#lblAutopayStatus").text(autopaystatus);//Card type to display
    $("#lblCCNo").text(creditcardno);
    $("#lblBANo").text(bankaccountno);

    $("#btnCheckPaynow").text("Pay Now");
    $("#btnCreditcardPaynow").text("Pay Now");
    $("#chkAutopayOff2").prop('checked', false);
    $("#chkAutopayOff").prop('checked', false);

    if (contracttype == "AMPREPAY") {
        $("#Autopay_tab").hide();
        MinPaymentAmount = parseInt($("#hdnMinPrePaymentAmount").val());
    }
    else {
        $("#Autopay_tab").show();
    }
    if (contracttype != "AMPREPAY" && (IsBankDraftShow == true || IsBankDraftShow == "True") && ConnectStatus != "PENDING" && ConnectStatus != "DISCONNECT" && DishonouredCount == 0) {
        $("#BankDraft_tab").show();
    }
    else {
        $("#BankDraft_tab").hide();
    }
    if (contracttype == "AMPREPAY") {
        funPrepayWizad();
        var tabStrip = $("#tabstrip_Payment").kendoTabStrip().data("kendoTabStrip");
        var lastIndex = tabStrip.items().length - 1;
        $(tabStrip.items()[lastIndex]).show();
        tabStrip.enable($(tabStrip.items()[lastIndex]), true);

    } else {
        var tabStrip = $("#tabstrip_Payment").kendoTabStrip().data("kendoTabStrip");
        var lastIndex = tabStrip.items().length - 1;
        $(tabStrip.items()[lastIndex]).hide();
        tabStrip.disable($(tabStrip.items()[lastIndex]), true);
    }
    var grid = $("#payment-grid").data("kendoGrid");
    if (grid != null) {
        grid.dataSource.fetch();
    }
    if (autopay == "Y") {
        //Displaying card type
        if (paymethodcode == "CRDCARD") {
            $('input:radio[name="optradio"]').filter('[value="1"]').attr('checked', true);
            $("#btnSetupAutoPay").text("Update Card Details");
            $("#btnCreditcardPaynow").text("Pay Now");
            $('input:radio[name="optradio"]').filter('[value="1"]').click();
        }
        else if (paymethodcode == "DIRECT") {
            $('input:radio[name="optradio"]').filter('[value="2"]').attr('checked', true);
            $("#btnSetupAutoPay").text("Update Bank Details");
            $('input:radio[name="optradio"]').filter('[value="2"]').click();
        }
        //hiding Autopay
        $("#divCCAutopaySection").hide();
        $("#divBAAutopaySection").hide();
        $("#lblAutopayStatus").text("Enrolled");
        $("#btnUnSetupAutoPay").show();

        //Removig auto pay styles wich applied dynamically.
        $("#divCCMain").removeClass("new-right-form");
        $("#divCCsub").removeClass("new-amount-section");
        $("#divBAMain").removeClass("payment-new-right");

        //Postpaid and AutoPay enrolled with creditcard
        if (contracttype != "AMPREPAY" && (ccprofiletype == "FROPAYCC" || ccprofiletype == "FROPAYWFCC") && paymethodcode == "CRDCARD") {
            $("#divCCNo").show();//displaying credit card details div in Autopay tab
            $("#divPayusingSCC").show();//displaying secured credit card details div in Credit Card tab
            $("#lblCCNumber").text("(X-" + creditcardno + ")"); //displaying credit card Number in Credit Card tab
            $("#lblCCNo").text("CC No: X-" + creditcardno + ",Exp: " + CreditCardExpiry + ",Card Type: " + credit_card_type_code);//displaying credit card Number in Autopay tab
        }
        else {
            $("#divCCNo").hide();
            $("#divPayusingSCC").hide();
            $("#lblCCNumber").text('');
            $("#lblCCNo").text('');
            $("#divCCMain").addClass("new-right-form");
            $("#divCCsub").addClass("new-amount-section");
        }
        //Postpaid and AutoPay enrolled with Bank Account 
        if (contracttype != "AMPREPAY" && (ddprofiletype == "FROPAYDD" || ddprofiletype == "FROPAYWFDD") && paymethodcode == "DIRECT") {
            $("#divBANo").show();//displaying Bank Account details div in Autopay tab
            $("#divPayusingSBA").show();//displaying secured Bank Account details div
            $("#lblBANumber").text("(X-" + bankaccountno + ")");//displaying Bank Account Number in Bank Draft tab
            $("#lblBANo").text("A/c No: X-" + bankaccountno + ", A/c Type: " + debit_type);//displaying Bank Account Number in Autopay tab
        }
        else {
            $("#divBANo").hide();
            $("#divPayusingSBA").hide();
            $("#lblBANumber").text('');
            $("#lblBANo").text('');
            $("#divBAMain").addClass("payment-new-right");
        }
    }
    else {
        $("#btnUnSetupAutoPay").hide();
        $("#btnSetupAutoPay").text("Enroll Auto Pay");
        $("#lblAutopayStatus").text("Not Enrolled");
        $("#divCCNo").hide();
        $("#divPayusingSCC").hide();
        $("#lblCCNumber").text('');
        $("#lblCCNo").text('');
        $("#divBANo").hide();
        $("#divPayusingSBA").hide();
        $("#lblBANumber").text('');
        $("#lblBANo").text('');
        $("#divCCMain").addClass("new-right-form");
        $("#divCCsub").addClass("new-amount-section");
        $("#divBAMain").addClass("payment-new-right");
        $("#divCCAutopaySection").show();
        $("#divBAAutopaySection").show();
        if (contracttype == "AMPREPAY") {
            $("#divCCAutopaySection").hide();
            $("#divBAAutopaySection").hide();
        }
        else {
            $("#divCCAutopaySection").show();
            $("#divBAAutopaySection").show();
        }
    }
    var tabStrip = $("#tabstrip_Payment").kendoTabStrip().data("kendoTabStrip");
    if (tabStrip.select().index() != 2) {
        $("#divMainAutopay").removeClass("autopay-div-height");
    }

    //Bind the Values to the month and year dropdowns in Credit Card tab
    $("#expmonth").kendoDropDownList({
        dataTextField: "monthvalue",
        dataValueField: "monthid",
        dataSource: Json_months,
    });

    $("#expyear").kendoDropDownList({
        dataTextField: "yearvalue",
        dataValueField: "yearid",
        dataSource: Json_years,
    });
    $("#expmonthAutopay").kendoDropDownList({
        dataTextField: "monthvalue",
        dataValueField: "monthid",
        dataSource: Json_months,
    });

    $("#expyearAutopay").kendoDropDownList({
        dataTextField: "yearvalue",
        dataValueField: "yearid",
        dataSource: Json_years,
    });
}

function InsertPCITransactionLog(CustomerNo, SessionId) {
    var serializedform = {
        Source: "MyAccountWeb",
        PCISessionId: SessionId,
        CustomerNumber: CustomerNo
    };
    $.ajax({
        url: 'MyAccount/InsertPCITransactionLog',
        type: "POST",
        data: serializedform,
        success: function (result) {
            // Post message to Parent application
            PostToParent(result.ChildUniqueId, result.PCISessionId);
        },
        error: function (result) {
        }
    });
}
function PostToParent(ChildUniqueId, PCISessionId) {
    var serializedobject = {
        ChildUniqueId: ChildUniqueId,
        PCISessionId: PCISessionId,
        IsValidate: "1"
    }
    var sendMessage = function (msg) {
        window.parent.postMessage(msg, '*');
    };
    sendMessage(JSON.stringify(serializedobject));
}

