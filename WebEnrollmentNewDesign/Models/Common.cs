using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using WebEnrollmentNewDesign.PayService;

namespace WebEnrollmentNewDesign.Models
{
    public static class Common
    {
        static FrontierPayClient objFrontierPayClient;
        static PayWSDBClient objPayWSDBClient;
        public static string Encrypt(string encrypt)
        {
            SymmetricAlgorithm sa = SymmetricAlgorithm.Create("TripleDES");
            sa.Key = Convert.FromBase64String("HRSF1P3b6fHiW/DXrK8ZJks5KAiyNpP9");
            sa.IV = Convert.FromBase64String("YFKA0QlomKY=");
            byte[] inputByteArray = Encoding.ASCII.GetBytes(encrypt);
            MemoryStream mS = new MemoryStream();
            ICryptoTransform trans = sa.CreateEncryptor();
            byte[] buf = new byte[2049];
            CryptoStream cs = new CryptoStream(mS, trans, CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(mS.ToArray());
        }

        //Credit Card Payment
        public static PaymentResponse FrontierPayMakePayment(CreditCardPaymentRequest req)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var payResponse = new PaymentResponse();
            string batchCode = System.Configuration.ConfigurationManager.AppSettings["BatchCode"];
            try
            {
                if (req.CustomerNumber != null && req.CustId != 0)
                {
                    FrontierPayClient objFrontierPay = new FrontierPayClient();
                    WebEnrollmentNewDesign.PayService.FrontierPayResponse frontierPayResponse = new WebEnrollmentNewDesign.PayService.FrontierPayResponse();

                    if (req.CardNumber.Trim().StartsWith("4")) // Visa
                        req.CardType = "001";
                    else if (req.CardNumber.Trim().StartsWith("5") || req.CardNumber.Trim().StartsWith("2"))  // Master Card
                        req.CardType = "002";
                    else if (req.CardNumber.Trim().StartsWith("3")) //American Express
                        req.CardType = "003";
                    else if (req.CardNumber.Trim().StartsWith("6")) //Discover
                        req.CardType = "004";
                    else
                        req.CardType = "000";

                    var payObject = new WebEnrollmentNewDesign.PayService.FrontierPayRequest()
                    {
                        AuthKey = req.AuthKey,
                        CustomerNumber = req.CustomerNumber,
                        BatchCode = batchCode,
                        SetupAutoPay = "N",
                        PostReceipt = "Y",
                        ConfirmationType = req.ConfirmationType,

                        AccountName = req.CardName,
                        CardType = req.CardType,
                        CardNumber = Common.Encrypt(req.CardNumber),
                        ExpirationMonth = req.Expiry.Substring(0, 2),
                        ExpirationYear = "20" + req.Expiry.Substring(3, 2),
                        CVCCode = Common.Encrypt(req.VerificationCode),
                        ZipCode = req.ZipCode,
                        Amount = Convert.ToString(req.Amount),
                        Email = req.Email,
                        PhoneNumber = req.ContactNumber,
                        Source = req.Source,
                        SiteIdentifier = req.SiteIdentifier
                    };
                    List<WebEnrollmentNewDesign.PayService.AddAdjustmentRequest> lstAdj = new List<WebEnrollmentNewDesign.PayService.AddAdjustmentRequest>();
                    decimal TotfeeAmt = 0;
                    if (req.IsCFWaived == "0" && req.Source == "Superscreen") //Convenience Fee is not waived 
                    {
                        if (req.Addr_State.ToUpper() == "TX")
                        {
                            decimal CustomerServiceConvenienceFee_Amount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["CustomerServiceConvenienceFee_Amount"].ToString());
                            decimal CreditCardProcessingFee_Amount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["CreditCardProcessingFee_Amount"].ToString());
                            if (CustomerServiceConvenienceFee_Amount > 0 || CreditCardProcessingFee_Amount > 0)
                            {
                                payObject.AddAdjustment = "Y";
                                if (CustomerServiceConvenienceFee_Amount > 0)
                                {
                                    WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adj1 = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                                    adj1.AdjustmentAmount = Convert.ToString(CustomerServiceConvenienceFee_Amount);
                                    adj1.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["CustomerServiceConvenienceFee_ReasonCode"].ToString();
                                    adj1.Note = "";
                                    lstAdj.Add(adj1);
                                }
                                if (CreditCardProcessingFee_Amount > 0)
                                {
                                    WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adj2 = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                                    adj2.AdjustmentAmount = Convert.ToString(CreditCardProcessingFee_Amount);
                                    adj2.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["CreditCardProcessingFee_ReasonCode"].ToString();
                                    adj2.Note = "";
                                    lstAdj.Add(adj2);
                                }
                                TotfeeAmt = (CustomerServiceConvenienceFee_Amount);//CustomerServiceConvenienceFee_Amount
                              
                                payObject.AddAdjustmentRequestDetails = lstAdj;
                            }
                            else
                            {
                                payObject.AddAdjustment = "N";
                            }
                        }
                    }
                    if (req.IsWriteoffAmount == 1)
                    {
                        if (payObject.AddAdjustment != "Y")
                            payObject.AddAdjustment = "Y";

                        WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adjbaddebit = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                        adjbaddebit.AdjustmentAmount = Convert.ToString(req.Amount - TotfeeAmt);
                        adjbaddebit.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["BadDebtReversal_ReasonCode"].ToString();
                        adjbaddebit.Note = "";
                        lstAdj.Add(adjbaddebit);
                        payObject.AddAdjustmentRequestDetails = lstAdj;
                    }
                    else
                    {
                        //IS Settled Amount
                        decimal PaidAmount = (req.Amount - TotfeeAmt);
                        if (req.IsSettledAmount == 1 && req.IsWriteoffAmount == 0 && req.ARBalanceAmount > PaidAmount && req.Addr_State.ToUpper() == "TX")
                        {
                            if (payObject.AddAdjustment != "Y")
                                payObject.AddAdjustment = "Y";
                            WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adjsettledamtreq = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                            adjsettledamtreq.AdjustmentAmount = Convert.ToString(req.ARBalanceAmount - PaidAmount);
                            adjsettledamtreq.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["IsSettled_ReasonCode"].ToString();
                            adjsettledamtreq.Note = "";
                            lstAdj.Add(adjsettledamtreq);
                         //   payObject.AddAdjustmentRequestDetails = lstAdj;
                        }
                    }

                    try
                    {
                        frontierPayResponse = objFrontierPay.PayByCard(payObject);
                    }
                    catch (Exception ex)
                    {
                        InsertClientErrorLog(req.CustomerNumber, batchCode, req.Source, ex, "Common_FrontierPayMakePayment");
                    }

                    payResponse.ResultMessage = frontierPayResponse.ResponseMessage;
                    payResponse.ResultCode = Convert.ToInt32(frontierPayResponse.StatusCode);
                    payResponse.AuthCode = frontierPayResponse.ConfirmationNo;
                }
                else
                {
                    payResponse.ResultCode = -1;
                    payResponse.ResultMessage = "Invalid Request";
                }
                return payResponse;
            }
            catch (Exception ex)
            {
                payResponse.ResultCode = -3;
                payResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(req.CustomerNumber, batchCode + "CardName: " + req.CardName + ";CardType" + req.CardType, req.Source, ex, "Common_FrontierPayMakePayment");
                return payResponse;
            }
        }

        //BankDraft Payment
        public static PaymentResponse FrontierCheckPayment(CheckInfo request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var payResponse = new PaymentResponse();
            string batchCode = System.Configuration.ConfigurationManager.AppSettings["CheckPaymantBatchCode"];
            try
            {
                FrontierPayClient objFrontierPay = new FrontierPayClient();
                WebEnrollmentNewDesign.PayService.FrontierPayResponse frontierPayResponse = new WebEnrollmentNewDesign.PayService.FrontierPayResponse();
                var payObject = new WebEnrollmentNewDesign.PayService.FrontierPayRequest()
                {
                    AuthKey = request.AuthKey,
                    CustomerNumber = request.CustomerNumber,
                    BatchCode = batchCode,
                    SetupAutoPay = "N",
                    PostReceipt = "Y",
                    ConfirmationType = request.ConfirmationType,

                    AccountName = request.BankAccountName,
                    BankAccountNumber = Encrypt(request.BankAccountNumber),
                    AccountType = request.AccountType,
                    BankRoutingNumber = request.BankRoutingNumber,

                    Amount = Convert.ToString(request.Amount),
                    Email = request.Email,
                    PhoneNumber = request.ContactNumber,
                    Source = request.Source,
                    SiteIdentifier = request.SiteIdentifier
                };
                List<WebEnrollmentNewDesign.PayService.AddAdjustmentRequest> lstAdj = new List<WebEnrollmentNewDesign.PayService.AddAdjustmentRequest>();
                if (request.IsWriteoffAmount == 1)
                {
                    if (payObject.AddAdjustment != "Y")
                        payObject.AddAdjustment = "Y";

                    WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adjbaddebit = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                    adjbaddebit.AdjustmentAmount = Convert.ToString(request.Amount);
                    adjbaddebit.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["BadDebtReversal_ReasonCode"].ToString();
                    adjbaddebit.Note = "";
                    lstAdj.Add(adjbaddebit);
                  //  payObject.AddAdjustmentRequestDetails = lstAdj;
                }
                else
                {
                    //IS Settled Amount
                    if (request.IsSettledAmount == 1 && request.IsWriteoffAmount == 0 && request.ARBalanceAmount > request.Amount)
                    {
                        if (payObject.AddAdjustment != "Y")
                            payObject.AddAdjustment = "Y";
                        WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adjsettledamtreq = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                        adjsettledamtreq.AdjustmentAmount = Convert.ToString(request.ARBalanceAmount - request.Amount);
                        adjsettledamtreq.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["IsSettled_ReasonCode"].ToString();
                        adjsettledamtreq.Note = "";
                        lstAdj.Add(adjsettledamtreq);
                    //    payObject.AddAdjustmentRequestDetails = lstAdj;
                    }
                }
                try
                {
                    frontierPayResponse = objFrontierPay.PayByCheck(payObject);
                }
                catch (Exception ex)
                {
                    InsertClientErrorLog(request.CustomerNumber, batchCode, request.Source, ex, "Common_FrontierCheckPayment");
                }

                payResponse.ResultMessage = frontierPayResponse.ResponseMessage;
                payResponse.ResultCode = Convert.ToInt32(frontierPayResponse.StatusCode);
                payResponse.AuthCode = frontierPayResponse.ConfirmationNo;

                return payResponse;

            }
            catch (Exception ex)
            {
                payResponse.ResultCode = -3;
                payResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(request.CustomerNumber, batchCode, request.Source, ex, "Common_FrontierCheckPayment");
                return payResponse;
            }
        }

        //Create Profile if autopay selected CreditCard
        public static PaymentResponse ManagePaymentProfile(CreditCardPaymentRequest payment)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var payResponse = new PaymentResponse();
            string batchCode = System.Configuration.ConfigurationManager.AppSettings["CardManageProfileBatchCode"];
            try
            {
                FrontierPayClient objFrontierPay = new FrontierPayClient();
                WebEnrollmentNewDesign.PayService.PaymentProfileResponse frontierPaymentProfileResponse = new WebEnrollmentNewDesign.PayService.PaymentProfileResponse();

                if (payment.CardNumber.Trim().StartsWith("4")) // Visa
                    payment.CardType = "VISA";
                else if (payment.CardNumber.Trim().StartsWith("5") || payment.CardNumber.Trim().StartsWith("2"))  // Master Card
                    payment.CardType = "MASTERCARD";
                else if (payment.CardNumber.Trim().StartsWith("3")) //American Express
                    payment.CardType = "AMEX";
                else if (payment.CardNumber.Trim().StartsWith("6")) //Discover
                    payment.CardType = "DISCOVER";
                else
                    payment.CardType = "NA";

                var payObject = new WebEnrollmentNewDesign.PayService.PaymentProfileRequest()
                {
                    AuthKey = payment.AuthKey,
                    BatchCode = batchCode,
                    ConfirmationType = payment.ConfirmationType,
                    CustomerNumber = payment.CustomerNumber,

                    CardType = payment.CardType,
                    CardName = payment.CardName,
                    CardNumber = Encrypt(payment.CardNumber),
                    ExpirationMonth = payment.Expiry.Substring(0, 2),
                    ExpirationYear = payment.Expiry.Substring(3, 2),
                    CVCCode = Encrypt(payment.VerificationCode),
                    CardZip = payment.ZipCode,
                    CardEncrypted = "Y",
                    RequestType = "CREDITCARD",
                    RequestAction = "UPDATE",
                    Email = payment.Email,
                    PhoneNumber = payment.ContactNumber,
                    Source = payment.Source,
                    FirstName = payment.FirstName,
                    LastName = payment.LastName

                };

                try
                {
                    frontierPaymentProfileResponse = objFrontierPay.ManagePaymentProfile(payObject);
                }
                catch (Exception ex)
                {
                    InsertClientErrorLog(payment.CustomerNumber, batchCode, payment.Source, ex, "Common_ManagePaymentProfile");
                }

                if (frontierPaymentProfileResponse.Message == null || string.IsNullOrEmpty(frontierPaymentProfileResponse.Message))
                    frontierPaymentProfileResponse.Message = "An Error has occured. Please try again later.";

                payResponse.ResultMessage = frontierPaymentProfileResponse.Message;
                payResponse.ResultCode = Convert.ToInt32(frontierPaymentProfileResponse.Status);
                payResponse.profileStatusMessage = frontierPaymentProfileResponse.Message;
                payResponse.profileStatusCode = Convert.ToInt32(frontierPaymentProfileResponse.Status);

                return payResponse;
            }
            catch (Exception ex)
            {
                payResponse.ResultCode = -3;
                payResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(payment.CustomerNumber, batchCode, payment.Source, ex, "Common_ManagePaymentProfile");
                return payResponse;
            }
        }

        //Create Profile if autopay selected BankDraft
        public static PaymentResponse CreateProfile(CheckPaymentRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var payResponse = new PaymentResponse();
            string batchCode = System.Configuration.ConfigurationManager.AppSettings["CheckPaymantBatchCode"];
            try
            {
                FrontierPayClient objFrontierPay = new FrontierPayClient();
                WebEnrollmentNewDesign.PayService.FrontierPayResponse frontierPayResponse = new WebEnrollmentNewDesign.PayService.FrontierPayResponse();

                var payObject = new WebEnrollmentNewDesign.PayService.FrontierPayRequest()
                {
                    AuthKey = request.AuthKey,
                    CustomerNumber = request.CustomerNumber,
                    BatchCode = batchCode,
                    ConfirmationType = "NONE",
                    AccountName = request.BankAccountName,
                    BankAccountNumber = Encrypt(request.BankAccountNumber),
                    AccountType = request.AccountType,
                    BankRoutingNumber = request.BankRoutingNumber,
                    SetupAutoPay = Convert.ToString(request.SetupAutoPay),
                    Source = request.Source
                };

                try
                {
                    frontierPayResponse = objFrontierPay.CreateCheckProfile(payObject);
                }
                catch (Exception ex)
                {
                    InsertClientErrorLog(request.CustomerNumber, batchCode, payObject.Source, ex, "Common_CreateProfile");
                }

                payResponse.ResultMessage = frontierPayResponse.ResponseMessage;
                payResponse.ResultCode = Convert.ToInt32(frontierPayResponse.StatusCode);
                payResponse.profileStatusCode = Convert.ToInt64(frontierPayResponse.ProfileID);

                return payResponse;
            }
            catch (Exception ex)
            {
                payResponse.ResultCode = -3;
                payResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(request.CustomerNumber, batchCode, "WebEnrollmentNewDesign - CreateProfile - " + request.Source, ex, "Common_CreateProfile");
                return payResponse;
            }
        }

        //PaybyProfile Credit Card/Bank Draft Payment
        public static PaymentResponse FrontierPayByProfile(ProfilePaymentRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var payResponse = new PaymentResponse();
            string batchCode = System.Configuration.ConfigurationManager.AppSettings["PayByProfileBatchCode"];
            if (request.Type != null && request.Type.ToUpper() == "ACH")
                batchCode = System.Configuration.ConfigurationManager.AppSettings["PayByProfileCheckPaymantBatchCode"];
            try
            {
                if (request.Cust_no != null && request.Cust_no != string.Empty)
                {
                    FrontierPayClient objFrontierPay = new FrontierPayClient();
                    WebEnrollmentNewDesign.PayService.FrontierPayResponse frontierPayResponse = new WebEnrollmentNewDesign.PayService.FrontierPayResponse();

                    var payObject = new WebEnrollmentNewDesign.PayService.FrontierPayRequest()
                    {
                        AuthKey = request.AuthKey,
                        CustomerNumber = request.Cust_no,
                        BatchCode = batchCode,
                        PostReceipt = "Y",
                        ConfirmationType = request.ConfirmationType,

                        Amount = Convert.ToString(request.Amount),
                        ProfileID = request.ProfileID,
                        Email = request.Email,
                        PhoneNumber = request.ContactNumber,
                        Source = request.Source,
                        SiteIdentifier = request.SiteIdentifier
                    };
                    if (request.IsCFWaived == "0" && request.Source == "Superscreen") //Convenience Fee is not waived 
                    {
                        if (request.State.ToUpper() == "TX")
                        {
                            decimal CustomerServiceConvenienceFee_Amount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["CustomerServiceConvenienceFee_Amount"].ToString());
                            decimal CreditCardProcessingFee_Amount = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["CreditCardProcessingFee_Amount"].ToString());
                            if (CustomerServiceConvenienceFee_Amount > 0 || CreditCardProcessingFee_Amount > 0)
                            {
                                payObject.AddAdjustment = "Y";
                                List<WebEnrollmentNewDesign.PayService.AddAdjustmentRequest> lstAdj = new List<WebEnrollmentNewDesign.PayService.AddAdjustmentRequest>();
                                if (CustomerServiceConvenienceFee_Amount > 0)
                                {
                                    WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adj1 = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                                    adj1.AdjustmentAmount = Convert.ToString(CustomerServiceConvenienceFee_Amount);
                                    adj1.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["CustomerServiceConvenienceFee_ReasonCode"].ToString();
                                    adj1.Note = "";
                                    lstAdj.Add(adj1);
                                }
                                if (CreditCardProcessingFee_Amount > 0)
                                {
                                    WebEnrollmentNewDesign.PayService.AddAdjustmentRequest adj2 = new WebEnrollmentNewDesign.PayService.AddAdjustmentRequest();
                                    adj2.AdjustmentAmount = Convert.ToString(CreditCardProcessingFee_Amount);
                                    adj2.ReasonCode = System.Configuration.ConfigurationManager.AppSettings["CreditCardProcessingFee_ReasonCode"].ToString();
                                    adj2.Note = "";
                                    lstAdj.Add(adj2);
                                }
                                payObject.AddAdjustmentRequestDetails = lstAdj;
                            }
                            else
                            {
                                payObject.AddAdjustment = "N";
                            }
                        }
                    }
                    try
                    {
                        frontierPayResponse = objFrontierPay.PayByProfile(payObject);
                    }
                    catch (Exception ex)
                    {
                        InsertClientErrorLog(request.Cust_no, batchCode, request.Source, ex, "Common_FrontierPayByProfile");
                    }

                    payResponse.ResultMessage = frontierPayResponse.ResponseMessage;
                    payResponse.ResultCode = Convert.ToInt32(frontierPayResponse.StatusCode);
                    payResponse.AuthCode = frontierPayResponse.ConfirmationNo;

                    if (string.IsNullOrEmpty(payResponse.ResultMessage))
                    {
                        payResponse.ResultMessage = "";
                    }

                }
                else
                {
                    payResponse.ResultCode = -1;
                    payResponse.ResultMessage = "Invalid Request";
                }
            }
            catch (Exception ex)
            {
                payResponse.ResultCode = -3; //Exception
                payResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(request.Cust_no, batchCode, request.Source, ex, "Common_FrontierPayByProfile");
                return payResponse;
            }

            return payResponse;
        }
        //Setup BankDraft/Check Autopay --Autopay Update
        public static PaymentResponse SetupACHAutoPay(CheckInfo chkRequest)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var resp = new PaymentResponse();
            try
            {
                if (chkRequest.CustomerNumber != null && chkRequest.CustomerNumber != string.Empty)
                {
                    resp = ManageACHPaymentProfile(chkRequest);
                }
                else
                {
                    resp.ResultCode = -1;
                    resp.ResultMessage = "Invalid Request";
                }
                return resp;
            }
            catch (Exception ex)
            {
                resp.ResultCode = -3; //Exception
                resp.ResultMessage = ex.Message;
                InsertClientErrorLog(chkRequest.CustomerNumber, "", chkRequest.Source, ex, "Common_SetupACHAutoPay");
                return resp;
            }
        }

        //Enabling ACH Autopay
        public static PaymentResponse ManageACHPaymentProfile(CheckInfo payment)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            PaymentResponse pmtResponse = new PaymentResponse();
            string batchCode = System.Configuration.ConfigurationManager.AppSettings["ACHManageProfileBatchCode"];
            try
            {
                FrontierPayClient objFrontierPay = new FrontierPayClient();
                WebEnrollmentNewDesign.PayService.PaymentProfileResponse frontierPaymentProfileResponse = new WebEnrollmentNewDesign.PayService.PaymentProfileResponse();

                if (payment.AccountType.ToUpper() == "C") // CHECKING
                    payment.AccountType = "CHECKING";
                else if (payment.AccountType.ToUpper() == "S") // SAVING
                    payment.AccountType = "SAVING";
                else if (payment.AccountType.ToUpper() == "X") // Corporate checking
                    payment.AccountType = "BUSINESS";
                else
                    payment.AccountType = "CHECKING";

                var payObject = new WebEnrollmentNewDesign.PayService.PaymentProfileRequest()
                {
                    AuthKey = payment.AuthKey,
                    BatchCode = batchCode,
                    ConfirmationType = payment.ConfirmationType,
                    CustomerNumber = payment.CustomerNumber,

                    AccountName = payment.BankAccountName,
                    BankAccountNumber = Encrypt(payment.BankAccountNumber),
                    BankRoutingNumber = payment.BankRoutingNumber,
                    AccountType = payment.AccountType,

                    BankAccountEncrypted = "Y",
                    ProfileType = "FROPAYDD",
                    RequestType = "DIRECTDEBIT",
                    RequestAction = "UPDATE",
                    Email = payment.Email,
                    PhoneNumber = payment.ContactNumber,
                    Source = payment.Source,
                    FirstName = payment.FirstName,
                    LastName = payment.LastName
                };

                try
                {
                    frontierPaymentProfileResponse = objFrontierPay.ManagePaymentProfile(payObject);
                }
                catch (Exception ex)
                {
                    InsertClientErrorLog(payment.CustomerNumber, batchCode, payment.Source, ex, "Common_ManageACHPaymentProfile");
                }

                if (frontierPaymentProfileResponse.Message == null || string.IsNullOrEmpty(frontierPaymentProfileResponse.Message))
                    frontierPaymentProfileResponse.Message = "An Error has occured. Please try again later.";

                pmtResponse.profileStatusMessage = frontierPaymentProfileResponse.Message;
                pmtResponse.profileStatusCode = Convert.ToInt32(frontierPaymentProfileResponse.Status);
                pmtResponse.ResultMessage = frontierPaymentProfileResponse.Message;
                pmtResponse.ResultCode = Convert.ToInt32(frontierPaymentProfileResponse.Status);

            }
            catch (Exception ex)
            {
                pmtResponse.ResultCode = -3; //Exception
                pmtResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(payment.CustomerNumber, batchCode, payment.Source, ex, "Common_ManageACHPaymentProfile");
                return pmtResponse;

            }
            return pmtResponse;
        }

        public static PaymentResponse FrontierPayDeletePaymentProfile(ProfilePaymentRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var resp = new PaymentResponse();
            try
            {
                if (request.Cust_no != null && request.Cust_no != string.Empty)
                {
                    resp = DeletePaymentProfile(request);
                    resp.ResultMessage = resp.profileStatusMessage;
                    resp.ResultCode =Convert.ToInt32(resp.profileStatusCode);
                }
                else
                {
                    resp.ResultCode = -1;
                    resp.ResultMessage = "Invalid Request";
                }

                return resp;

            }
            catch (Exception ex)
            {
                resp.ResultCode = -3; //Exception
                resp.ResultMessage = ex.Message;
                InsertClientErrorLog(request.Cust_no, "", request.Source, ex, "Common_FrontierPayDeletePaymentProfile");
                return resp;
            }
        }

        public static PaymentResponse DeletePaymentProfile(ProfilePaymentRequest request)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            PaymentResponse pmtResponse = new PaymentResponse();
            string batchCode = string.Empty;
            try
            {
                FrontierPayClient objFrontierPay = new FrontierPayClient();
                WebEnrollmentNewDesign.PayService.PaymentProfileResponse frontierPaymentProfileResponse = new WebEnrollmentNewDesign.PayService.PaymentProfileResponse();

                //default to delete card profile settings
                batchCode = "WFCARDDELETEPROFILE";
                string profileType = "FROPAYCC";
                string requestType = "CREDITCARD";

                if ((!string.IsNullOrEmpty(request.Type)) && request.Type.ToUpper() == "ACH")// for delete ACH profile
                {
                    batchCode = "WFACHDELETEPROFILE";
                    profileType = "FROPAYDD";
                    requestType = "DIRECTDEBIT";
                }
                var payObject = new WebEnrollmentNewDesign.PayService.PaymentProfileRequest()
                {
                    AuthKey = request.AuthKey,
                    BatchCode = batchCode,
                    ConfirmationType = request.ConfirmationType,
                    CustomerNumber = request.Cust_no,

                    ProfileType = profileType,
                    RequestType = requestType,
                    RequestAction = "DELETE",
                    Email = request.Email,
                    PhoneNumber = request.ContactNumber,
                    Source = request.Source,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };

                try
                {
                    frontierPaymentProfileResponse = objFrontierPay.ManagePaymentProfile(payObject);
                }
                catch (Exception ex)
                {
                    InsertClientErrorLog(request.Cust_no, batchCode, request.Source, ex, "Common_DeletePaymentProfile");
                }

                if (frontierPaymentProfileResponse.Message == null || string.IsNullOrEmpty(frontierPaymentProfileResponse.Message))
                    frontierPaymentProfileResponse.Message = "An Error has occured. Please try again later.";

                pmtResponse.profileStatusMessage = frontierPaymentProfileResponse.Message;
                pmtResponse.profileStatusCode = Convert.ToInt32(frontierPaymentProfileResponse.Status);

            }
            catch (Exception ex)
            {
                pmtResponse.ResultCode = -3; //Exception
                pmtResponse.ResultMessage = ex.Message;
                InsertClientErrorLog(request.Cust_no, batchCode, request.Source, ex, "Common_DeletePaymentProfile");
                return pmtResponse;
            }
            return pmtResponse;
        }

        public static PaymentsLocationsResponse GetPaymentLocations(string accountno)
        {
            objPayWSDBClient = new PayWSDBClient();
            var resp = new PaymentsLocationsResponse();
            try
            {
                resp.Data = objPayWSDBClient.GetLocations(accountno);
                return resp;
            }
            catch (Exception ex)
            {
                resp.ResultCode = -3;
                resp.ResultMessage = ex.Message;
                InsertClientErrorLog("", "", "WebEnrollmentNewDesign - GetPaymentLocations", ex, "Common_GetPaymentLocations");
                return resp;
            }
        }

        public static SaveNotesRequest GetCallNoteTemplateByCallType(SaveCallNoteTypeRequest req)
        {
            objPayWSDBClient = new PayWSDBClient();
            var callnotetemplate = new SaveNotesRequest();
            try
            {
                callnotetemplate = objPayWSDBClient.GetCallNoteTemplateByCallType(req);
            }
            catch (Exception ex)
            {
                InsertClientErrorLog("", req.CallNoteType, "WebEnrollmentNewDesign - GetCallNoteTemplateByCallType", ex, "Common_GetCallNoteTemplateByCallType");
                return callnotetemplate;
            }
            return callnotetemplate;
        }

        public static Response SaveCallNotes(string PartyCode, string Type, string Notes, string RequiredDays, string username)
        {
            var objresp = new Response();
            try
            {
                var date = Convert.ToDateTime(RequiredDays);
                TimeSpan ts = date.Date - DateTime.Now.Date;
                double TotalDays = ts.Days;
                if (!string.IsNullOrEmpty(PartyCode))
                {
                    if (System.Configuration.ConfigurationManager.AppSettings["CallNotesToQueue"].ToString().Equals("1"))
                    {
                        objPayWSDBClient = new PayWSDBClient();
                        objresp = objPayWSDBClient.SaveCallNote(PartyCode, Type, Notes, username, Convert.ToInt32(TotalDays), "N", "WebEnrollmentNewDesign - SaveCallNotes");
                    }
                }
                else
                {
                    objresp.Message = "Invalid Data";
                    objresp.ErrorCode = "-4";
                }
            }
            catch (Exception ex)
            {
                objresp.Message = ex.Message + ex.InnerException;
                objresp.ErrorCode = "-2";
                InsertClientErrorLog("", PartyCode, "WebEnrollmentNewDesign - SaveCallNotes", ex, "Common_SaveCallNotes");
            }
            return objresp;
        }

        public static Response AddActivity(string CustNo, string Type, string Notes, string RequiredDays, string username, string Category, string SiteIdentifier, string LoginUser)
        {
            var objresp = new Response();
            try
            {
                if (!string.IsNullOrEmpty(CustNo))
                {
                    var date = Convert.ToDateTime(RequiredDays);
                    TimeSpan ts = date.Date - DateTime.Now.Date;
                    int TotalDays = ts.Days;
                    objPayWSDBClient = new PayWSDBClient();
                    objresp = objPayWSDBClient.AddActivity(CustNo, SiteIdentifier, Category, Type, Notes, "NA", "LOW", username, Convert.ToInt32(TotalDays), "N", LoginUser, "SuperScreen");
                }
                else
                {
                    objresp.Status = "-1";
                    objresp.Message = "Invalid Request";
                }

            }
            catch (Exception ex)
            {
                InsertClientErrorLog(CustNo, Type, "WebEnrollmentNewDesign - AddActivity", ex, "Common_SaveCallNotes");
            }
            return objresp;
        }

        public static void InsertClientErrorLog(string CustmerNumber, string BatchCode, string Source, Exception exp, string MethodName)
        {
            try
            {
                objPayWSDBClient = new PayWSDBClient();
                objPayWSDBClient.InsertClientErrorLog(CustmerNumber, BatchCode, Source, ExceptionToString(exp), MethodName);
            }
            catch (Exception ex)
            {
            }
        }
        private static string ExceptionToString(Exception ex)
        {
            var excp = ex.Message + Environment.NewLine + ex.StackTrace;
            if (ex.InnerException != null)
                excp += ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace;
            return excp;
        }

        public static bool ValidateSession(string PaymentSessionID)
        {
            bool valid = false;
            FrontierPayClient objFrontierPay = new FrontierPayClient();
            objPayWSDBClient = new PayWSDBClient();
            valid = objPayWSDBClient.ValidateRequestPayment(PaymentSessionID);
            return valid;
        }

        public static WebEnrollmentNewDesign.PayService.FrontierPayResponse AuthorizeCard(CreditCardInfo objReq)
        {
            string Autopay = "N";
            var payresponse = new WebEnrollmentNewDesign.PayService.FrontierPayResponse();
            try
            {
                if (objReq.CardNumber.Trim().StartsWith("4")) // Visa
                    objReq.CardType = "001";
                else if (objReq.CardNumber.Trim().StartsWith("5") || objReq.CardNumber.Trim().StartsWith("2"))  // Master Card
                    objReq.CardType = "002";
                else if (objReq.CardNumber.Trim().StartsWith("3")) //American Express
                    objReq.CardType = "003";
                else if (objReq.CardNumber.Trim().StartsWith("6")) //Discover
                    objReq.CardType = "004";
                else
                    objReq.CardType = "000";
                if (objReq.AutoPay)
                    Autopay = "Y";
                FrontierPayClient objFrontierPayClient = new FrontierPayClient();
                string batchCode = System.Configuration.ConfigurationManager.AppSettings["AuthCardBatchCode"];

                var payObject = new WebEnrollmentNewDesign.PayService.FrontierPayRequest()
                {
                    AuthKey = objReq.AuthKey,
                    BatchCode = batchCode,
                    PostReceipt = "Y",
                    SetupAutoPay = Autopay,
                    ConfirmationType = "NONE",
                    AccountName = objReq.CardName,
                    CardType = objReq.CardType,
                    CardNumber = Common.Encrypt(objReq.CardNumber),
                    ExpirationMonth = objReq.CardExpiry.Substring(0, 2),
                    ExpirationYear = "20" + objReq.CardExpiry.Substring(3, 2),
                    CVCCode = Common.Encrypt(objReq.CVVCode),
                    ZipCode = objReq.ZipCode,
                    Amount = Convert.ToString(objReq.DepositAmount),
                    Source = objReq.Source
                };

                payresponse = objFrontierPayClient.AuthorizeCard(payObject);
                if (payresponse != null)
                {
                    objPayWSDBClient = new PayWSDBClient();
                    objPayWSDBClient.FPUpdateRequestPayment(string.Empty, payresponse.AuthorizedRequestID, objReq.PaymentSessionID, objReq.RequestId);

                    objReq.CardNumber = "";
                    objReq.CVVCode = "";
                }
                return payresponse;
            }
            catch (Exception ex)
            {
                Common.InsertClientErrorLog(string.Empty, string.Empty, objReq.Source, ex, "AuthorizeCard");
                return null;
            }
        }

        public static WebEnrollmentNewDesign.PayWSDLService.FUNBaseResponse  InsertPCITransactionLog(string Source, string PCISessionId, string ChildId, string CustomerNo)
        {
            WebEnrollmentNewDesign.PayWSDLService.FUNBaseResponse objresp = new WebEnrollmentNewDesign.PayWSDLService.FUNBaseResponse();
            try
            {
                objPayWSDBClient = new PayWSDBClient();
                objresp =objPayWSDBClient.InsertPCITransactionLog(Source, PCISessionId, ChildId, CustomerNo);
            }
            catch (Exception ex)
            {
                objresp.requestStatus = -2;
                objresp.requestMessage = ExceptionToString(ex);
                Common.InsertClientErrorLog(string.Empty, string.Empty, Source, ex, "InsertPCITransactionLog");
            }
            return objresp;
        }
    }
}