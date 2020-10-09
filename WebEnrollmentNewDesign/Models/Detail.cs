using System;
using System.Collections.Generic;
using WebEnrollmentNewDesign.CommonService;

namespace WebEnrollmentNewDesign.Models
{
    public class FUNBaseRequest
    {
        public string appName { get; set; }
        public string appVersion { get; set; }
        public string deviceID { get; set; }
        public int dversion { get; set; }
        public string ipAddress { get; set; }
        public string sessionID { get; set; }
    }

    public class FUNBaseResponse
    {

        public int requestStatus { get; set; } // 1 and above are success and –1 failed cases, there is no zero.

        public string requestMessage { get; set; }

        public override string ToString()
        {
            return " requestStatus:" + requestStatus + " requestMessage:" + requestMessage;
        }
    }
    public class FUNAccountRequest
    {
        public string accessToken { get; set; }
    }
    public class WaiverTypeList : FUNBaseResponse
    {
        public List<WaiverType> lstWaiverTypes { get; set; }
    }

    public class WaiverType
    {
        public int waiverTypeID { get; set; }
        public string waiverType { get; set; }
    }
    public class WaiverTypeReq : FUNBaseRequest
    {
        public int vendorID { get; set; }
    }

    public class FunEnrollment : FUNBaseRequest
    {
        public ProductDetails ProdDetails { get; set; }
        public ServiceAddress serviceAddressObj { get; set; }
        public string dwelling_type { get; set; }
        public PersonalInformation personalInfo { get; set; }
        public bool isBillingSame { get; set; }
        public BillingAddress billingAddress { get; set; }
        public string isPreviousAddress { get; set; }
        public BillingAddress previousAddress { get; set; }
        public bool creditCheckYesNo { get; set; }
        public ContactInformation contactInfo { get; set; }
        public CreditCardInfo creditCardInfo { get; set; }
        public CreditScoreResponse depAmtResp { get; set; }
        public FeeInfo feeInfo { get; set; }
        public SwitchMoveResp SwitchMoveResp { get; set; }
        public string paymentType { get; set; }
        public string comments { get; set; }
        public string creditCheckAgreed { get; set; }
        public EnrollResult enrRes { get; set; }
        public decimal purchasedkWh { get; set; }
        public decimal purchasedAmount { get; set; }
        public string Ref_Code { get; set; }
        public bool AutoPaySelected { get; set; }
        public string Note { get; set; }

        public string hearAboutUs { get; set; }
        public string refAccountNumber { get; set; }
        public string bond_selection_rule_code { get; set; }

        public string SalespersonCode { get; set; }
        public string Source { get; set; }
        public Int64 creditcheck_status_id { get; set; }
        public bool IsPaperless { get; set; }
        public string agentName { get; set; }
        public string sourcePhone { get; set; }
        public string EnrollmentType { get; set; }
        public string InternalRuleMes { get; set; }
        public string ReasonCode { get; set; }
        public string IsTCPAAgreed { get; set; }
        public string GovermentIdNo { get; set; }
        public string AuthrizedUserFirstName { get; set; }
        public string AuthrizeduserLastName { get; set; }
        public string BrandCode { get; set; }
        public string TCPAPhoneType { get; set; }
        public string AuthrizeduserContactNumber { get; set; }
        public string Existing_Cust_No { get; set; }
        public string Existing_Cust_Account_id { get; set; }
        public string Close_Existing_YN { get; set; }
        public string CurrentBalance { get; set; }
        public string Badbebt_matched_cust_nos { get; set; }
        public string QueryString { get; set; }
        public CreditScoreReqData CsReqData { get; set; }
        public BillingAddress MoveInAddress { get; set; }
    }

    public class EnrollmentData
    {
        public List<AgentFeeInfo> lstAgentFee { get; set; }
        //public List<FeeAndTaxInfo> lstFeeTaxInFo { get; set; }
        public List<ProductDetails> lstProducts { get; set; }
        public List<ServiceAddress> lstServAddr { get; set; }
        public List<TabNames> TabsInfo { get; set; }

        public BillingAddress billingAddress { get; set; }
        public ContactInformation contactInfo { get; set; }
        public CreditCardInfo creditCardInfo { get; set; }
        public CreditScoreResponse depAmtResp { get; set; }
        //public DwellingInfo dwellingInfo { get; set; }
        public EnrollResult enrRes { get; set; }
        public FeeInfo feeInfo { get; set; }
        public PersonalInformation personalInfo { get; set; }
        public ProductDetails ProdDetails { get; set; }
        public ServiceAddress serviceAddressObj { get; set; }
        public SwitchMoveResp SwitchMoveResp { get; set; }
        public ZipDetailsResp ZipResp { get; set; }

        //  public string accountInfo { get; set; }
        //  public string CultureInfo { get; set; }
        public string dwelling_type { get; set; }
        public string ipAddress { get; set; }

        //public string SwitchMove { get; set; }
        //public string SwitchMoveDate { get; set; }

        public bool creditCheckYesNo { get; set; }
        public bool isBillingSame { get; set; }

        public string paymentType { get; set; }
        public string creditCheckAgreed { get; set; }

        public string isPreviousAddress { get; set; }
        public BillingAddress previousAddress { get; set; }
        public decimal purchasedkWh { get; set; }
        //  public decimal purchasedAmount { get; set; }
        public string Ref_Code { get; set; }
        public string Agent_Code { get; set; }
        public Boolean AutoPaySelected { get; set; }

        public decimal paymentAmount { get; set; }
        public string Note { get; set; }
        public Int64 creditcheck_status_id { get; set; }
        public bool IsPaperless { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDesc { get; set; }
        public bool IsFromVerify { get; set; }
        public string InternalRuleMes { get; set; }
        public decimal BadDebtAmt { get; set; }
        public string EnrollmentType { get; set; }
        public CreditScoreReqData CsReqData { get; set; }
        public CreditScoreResponse CrScrResp { get; set; }
        public string IsTCPAAgreed { get; set; }
        public string GovermentIdNo { get; set; }
        public string AuthrizedUserFirstName { get; set; }
        public string AuthrizeduserLastName { get; set; }
        public string BrandCode { get; set; }
        public string TCPAPhoneType { get; set; }
        public string AuthrizeduserContactNumber { get; set; }
        public string Existing_Cust_No { get; set; }
        public string Existing_Cust_Account_id { get; set; }
        public string Close_Existing_YN { get; set; }
        public string DriverLicense { get; set; }

        public string DriverLicenseState { get; set; }
        public string CurrentBalance { get; set; }
        public string Badbebt_matched_cust_nos{get;set;}
        public string QueryString { get; set; }
    }


    #region Products

    public class ProductDetails
    {
        /* public string Product_ID { get; set; }
         public string Price_Plan_ID { get; set; }
         public string Product_Code { get; set; }
         public string ProductTitle { get; set; }
         public string ProductHeader { get; set; }
         public string ProductDesc3 { get; set; }
         public string ProductTarget { get; set; }//Who's for it
         public string EarlyTerFee { get; set; }
         public string Unit_Price { get; set; }
         public string Term { get; set; }
         public string Promo_Code { get; set; }

         public string Prepay_YN { get; set; }
         public string Feature_Image { get; set; }
         public string Rate_Type { get; set; }
         public string TDSP_Code { get; set; }
         public string ActiveAvgUsage { get; set; }//Monthly_Fee


         public string EFL_Rate { get; set; }//EFl rateat
         public string Prod_Note { get; set; }

         public string OnlineOnly_YN { get; set; }
         public string Auto_pay { get; set; }
         public string E_bill { get; set; }
         public string AdvMeter { get; set; }
         public string TabID { get; set; }*/
        public string Product_ID { get; set; }
        public string Price_Plan_ID { get; set; }
        public string Product_Code { get; set; }
        public string ProductTitle { get; set; }
        public string ProductHeader { get; set; }
        public string ProductDesc3 { get; set; }
        public string ProductTarget { get; set; }
        public string EarlyTerFee { get; set; }
        public string Unit_Price { get; set; }
        public string Term { get; set; }
        public string Promo_Code { get; set; }
        public string Prepay_YN { get; set; }
        public string Feature_Image { get; set; }
        public string Rate_Type { get; set; }
        public string TDSP_Code { get; set; }
        public string ActiveAvgUsage { get; set; } // Monthly Fee
        public string EFL_Rate { get; set; }
        public string EFLRateAt { get; set; }
        public string ProductNote { get; set; }
        public string OnlineOnly_YN { get; set; }
        public string Auto_pay { get; set; }
        public string E_bill { get; set; }
        public string AdvMeter { get; set; }
        public string TabID { get; set; }
        public string Bundled { get; set; }
        public string ActiveMonthlyUsage { get; set; }
        public string MonthlyFee { get; set; }
        public string split_deposit_yn { get; set; }
        public decimal split_percentage { get; set; }
        public int split_days { get; set; }
        public string CCheckFree { get; set; }
        public string ProductDescTop { get; set; }
        public string IsSplitDeposit { get; set; }
        public string IsAgentVerificationRequired { get; set; }
        public string TDSPFixedRate { get; set; }
        public string TDSPVariableRate { get; set; }
        public string EFLkwh_val { get; set; }
        public string DisplayTab { get; set; }
        public string BrandCode { get; set; }
        public string ActiveAvgUsage_EFL_Rate { get; set; }
        public int SortOrder { get; set; }
        public bool IsFeatured { get; set; }
        public string ChargeDesc { get; set; }
        public string Disclaimer { get; set; }
    }

    public class TabNames
    {
        public string TabID { get; set; }
        public string TabName { get; set; }
        public string TabImage { get; set; }
        public string TabHeader { get; set; }
        public string TabDetail { get; set; }
        public string TabFooter { get; set; }
    }

    public class MobileProvidersResponse : FUNBaseResponse
    {
        public List<MobileProvider> lst { get; set; }
    }

    public class MobileProvider
    {
        public int mobile_provider_id { get; set; }
        public string mobile_provider_code { get; set; }
        public string mobile_provider_desc { get; set; }
        public string text_email_suffix { get; set; }

    }
    public class CustomerTestimonialArray : FUNBaseResponse
    {
        public List<CustomerTestimonial> lstCustomerTestimonial { get; set; }
    }


    public class CustomerTestimonial
    {
        public string testimonial { get; set; }
        public string cust_name { get; set; }
    }
    public class HtmlTemplate
    {
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public int Position { get; set; }
        public string ImageUrl { get; set; }
    }

    public class HtmlTemplateList : FUNBaseResponse
    {
        public List<HtmlTemplate> lstHtmlTemplate { get; set; }
    }
    public class ProductDetailsListResp : FUNBaseResponse
    {
        public List<ProductDetails> productDetailsList { get; set; }
        public List<TabNames> tabList { get; set; }
    }

    //for single product
    //   'CNP','','GOSAVE',980
    public class ProductDetailReq : FUNBaseRequest
    {
        public string TDSP_Code { get; set; }
        public string RefID { get; set; }
        public int TabID { get; set; }
        public string Promo_Code { get; set; }
        public int Product_ID { get; set; }
        public string BrandCode { get; set; }
        public int EFLKWH { get; set; }
        public string TabName { get; set; }
        public int ViewAll { get; set; }
        public string DisplayTab { get; set; }
        public string PPid { get; set; }
        public string ProductCode { get; set; }
    }
    public class ProductRefRequest : FUNBaseRequest
    {
        public string Ref_ID { get; set; }
    }
    public class ProductDetailsResp : FUNBaseResponse
    {
        public ProductDetails productDetails { get; set; }
        public ProductRefResp productRefDetails { get; set; }
    }
    public class ProductRefResp
    {
        public string Ref_ID { get; set; }
        public string Ref_Name { get; set; }
        public string Ref_Image_Name { get; set; }
        public string Ref_PhoneNO { get; set; }
        public string Agent_Code { get; set; }
    }

    #endregion

    #region ServiceAddress

    public class ServiceAddress
    {
        public string serviceAddress { get; set; }
        public string ESIID { get; set; }
        public string Address { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string StreetNum { get; set; }
        public string StreetName { get; set; }
        public string AptNum { get; set; }
        public string ZipCode { get; set; }
        public string SiteStatus { get; set; }
        public string ServiceClass { get; set; }
        public string AM_YN { get; set; }
        public string TDSPCode { get; set; }
        public string accnt_status_class_code { get; set; }
        public string switch_hold { get; set; }
        public override string ToString()
        {
            return serviceAddress;
        }
    }
    public class ServiceAddressStatus : FUNBaseResponse
    {
        public string serviceAddrStatus { get; set; }
        public string cust_no { get; set; }
        public decimal CurrentBalance { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonCodeDesc { get; set; }
        public string Existing_Cust_No { get; set; }
        public string Existing_Cust_Account_id { get; set; }
        public string Badbebt_matched_cust_nos { get; set; }
    }
    public class ServiceAddressReq : FUNBaseRequest
    {
        public string serviceAddressLike { get; set; }
        public string zipCode { get; set; }
    }

    public class ServiceAddressResp : FUNBaseResponse
    {
        public List<ServiceAddress> lstServAddr { get; set; }
    }
    #endregion

    public class MySendEmailReq : FUNBaseRequest
    {
        public int typeOfEmail { get; set; }
        public string toAddress { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string verificationCode { get; set; }
    }




    /**
     *  Billing Address in case if it is differant.
     */
    public class BillingAddress
    {
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string StreetNum { get; set; }
        public string StreetName { get; set; }
        public string AptNum { get; set; }
        public string ZipCode { get; set; }

    }

    public class SwitchMoveReq : FUNBaseRequest
    {
        public string ProductType { get; set; }
        public string SwitchMoveType { get; set; }
        public string TDSPCode { get; set; }
        public string ASAP_On { get; set; }
        public string AMS_YN { get; set; }
    }

    public class SwitchMoveResp : FUNBaseResponse
    {
        public DateTime SwitchMoveDate { get; set; }
        public decimal Fee { get; set; }
        public string PriorityCode { get; set; }
        public string MessageText { get; set; }
        public string SwitchMoveType { get; set; }
        public string SelectedSwitchMoveDate { get; set; }
        public string chargeType { get; set; }
    }

    public class SwitchMoveData
    {
        public string SCalDate { get; set; }
        public string MoveDate { get; set; }
        public Decimal Fee { get; set; }
        public string PriorityCode { get; set; }
        public string chargeType { get; set; }
    }


    public class SwitchDataArray : FUNBaseResponse
    {
        public List<SwitchMoveData> lstSwitchData { get; set; }
    }

    public class HearAboutUsResp : FUNBaseResponse
    {
        public List<string> lstHearAboutUs { get; set; }
    }
    public class CurrentProvidersResp : FUNBaseResponse
    {
        public List<string> lstCurrentProviders { get; set; }
    }
    public class FeeAndTaxInfo
    {
        public string FeeType { get; set; }
        public string Charge { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxPercentage { get; set; }
    }

    public class FeeAndTaxInfoListResp : FUNBaseResponse
    {
        public List<FeeAndTaxInfo> feeTaxList { get; set; }
    }
    public class PaymentFees
    {
        decimal AccountSetupFee { get; set; }
        decimal MoveIn_SwitchFee { get; set; }
        decimal Deposit { get; set; }
        decimal Pre_Purchase_Usage { get; set; }
        decimal PUC_AssessmentFee { get; set; }
        decimal CitySalesTax { get; set; }
        decimal kWhRequired { get; set; }
    }

    public class EnrollResult : FUNBaseResponse
    {
        public string pv_customer_no { get; set; }
        public string pv_auth_code { get; set; }
        public string recordID { get; set; }
        public PaymentResponse paymentResponse { get; set; }
        public PaymentResponse createAuthProfile { get; set; }
    }

    public class PersonalInformation
    {
        public string first_name { get; set; }
        public string middle_initial { get; set; }
        public string last_name { get; set; }
        public string date_of_birth { get; set; }
        public string ssn { get; set; }
        public string hashSSN { get; set; }
        public string hashDOB { get; set; }

    }

    public class ContactInformation
    {
        public string email { get; set; }
        public string contactno { get; set; }
        public string providers { get; set; }
        public string mobile_no { get; set; }
        public string verifyNum { get; set; }
        public string fieldVerified { get; set; }
        public bool verified { get; set; }
        public string contactType { get; set; }
        public string langCode { get; set; }
        public bool eBill { get; set; }

    }

    public class ProductInfo
    {
        public string Prepay_YN { get; set; }
        public string Online_Only_YN { get; set; }
        public string Contract_Term { get; set; }
        public string TDSP_Code { get; set; }
        public string Product_Desc { get; set; }
        public string Unit_Price { get; set; }
        public string EFLFileName { get; set; }
        public string Promo_Code { get; set; }
        public string EarlyTerFee { get; set; }
        public string Product_ID { get; set; }
        public string PricePlanID { get; set; }
        public string Auto_pay { get; set; }
        public string E_bill { get; set; }
        public string AdvMeter { get; set; }
    }
    public class ZipDetailsReq : FUNBaseRequest
    {
        public string zipCode { get; set; }
        public float lattitude { get; set; }
        public float longitude { get; set; }
    }
    public class ZipDetailsResp : FUNBaseResponse
    {
        public string TDSPCode { get; set; }
        public string TDSPName { get; set; }
        public string ZipCode { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
    }

    public class AddressInfoResponse
    {
        public string Address { get; set; }
        public string ESIID { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string ZipCode { get; set; }
        public string AM_YN { get; set; }
        public string StreetNum { get; set; }
        public string StreetName { get; set; }
        public string AptNum { get; set; }
        public string TDSPCode { get; set; }
    }

    public class AddressInfoRespList
    {
        public List<AddressInfoResponse> AddressInfoResponse { get; set; }
    }

    public class CreditScoreReqData : FUNBaseRequest
    {
        public string first_name { get; set; }
        public string middle_initial { get; set; }
        public string last_name { get; set; }
        public string ssn { get; set; }
        public string hashssn { get; set; }
        public string addresss { get; set; }
        public string addr_no { get; set; }
        public string apt_no { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; } // TX
        public string zip_code { get; set; }
        public string date_of_birth { get; set; } //(dd-mmm-yyy)
        public string hashDOB { get; set; }
        public string score_module { get; set; } //(transunion)
        public string dwelling_tpye { get; set; }
        public string site_identifier { get; set; }
        public string phone { get; set; }
        public string email_id { get; set; }
        public string credit_module_type { get; set; }
        public string source { get; set; }
        public string callerId { get; set; }
        public string language { get; set; }
        public string Brandcode { get; set; }
        public string DriverLicense { get; set; }
        public string DriverLicenseState { get; set; }
        public int ProductId { get; set; }
        public int PricePlanId { get; set; }
        public string PromoCode { get; set; }
    }

    //public class CreditScoreResponse : FUNBaseResponse
    //{
    //    public decimal depositAmount { get; set; }
    //    public int credit_score { get; set; }
    //    public String credit_module { get; set; }
    //    public int result_code { get; set; }
    //    public string result_msg { get; set; }
    //}

    public class ProductType : FUNBaseRequest
    {
        public string Product_Type { get; set; }
    }

    public class DespositAmountReqData : FUNBaseRequest
    {
        public string dwelling_tpye { get; set; }
        public int credit_score { get; set; }
        public int product_id { get; set; }
        public int price_plan_id { get; set; }
        public string promo_code { get; set; }
    }

    public class CreditCardPaymentRequest : FUNBaseRequest
    {

        public bool setupAutoPay { get; set; }
        public string cust_no { get; set; }
        public CreditCardInfo creditCardInfo { get; set; }

        //Added on 06-11-2019
        public long CustId { get; set; } // PartyID

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public string CardName { get; set; }

        public DateTime ReceiptDate { get; set; }

        public string CardType { get; set; }

        public string Expiry { get; set; }

        public string ZipCode { get; set; }

        public string CustomerNumber { get; set; }

        public string VerificationCode { get; set; }

        public bool SetupAutoPay { get; set; }

        public string PaymentorAutopay { get; set; }

        public string Addr_State { get; set; }

        public string IsCFWaived { get; set; }

        public string ConfirmationType { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string Source { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SiteIdentifier { get; set; }

        public int IsWriteoffAmount { get; set; }

        public int IsSettledAmount { get; set; }

        public decimal ARBalanceAmount { get; set; }

        public string AuthKey { get; set; }


    }
    public class PaymentResponse : FUNBaseResponse
    {
        public int ResultCode { get; set; }
        public string ResultMsg { get; set; }
        public string ResultMessage { get; set; }
        public decimal PurchaseAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Rate { get; set; }
        public decimal KWHPurchased { get; set; }





        public string AuthCode { get; set; }

        public string orionResult { get; set; }

        public Int64 profileStatusCode { get; set; }

        public string profileStatusMessage { get; set; }
    }
    public class IVRRequest
    {
        public string AccessToken { get; set; }
    }


    public class ProfilePaymentRequest : IVRRequest
    {

        public string ProfileID { get; set; }

        public decimal Amount { get; set; }

        public string Cust_no { get; set; }

        public string Type { get; set; }

        public string ConfirmationType { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IsCFWaived { get; set; }

        public string State { get; set; }

        public string SiteIdentifier { get; set; }

        public string Source { get; set; }

        public string AuthKey { get; set; }
    }
    public class CreditCardPayment
    {
        public long PartyID { get; set; }
        public decimal Amount { get; set; }
        public DateTime RecieptDate { get; set; }
        public string CardNumber { get; set; }
        public string Expiry { get; set; }
        public string ZipCode { get; set; }
        public string CustomerNumber { get; set; }
        public string VerificationCode { get; set; }
    }

    public class MyMobileVerificationReq : FUNBaseRequest
    {
        public string mobileNo { get; set; }
        public string verificationCode { get; set; }
        public string DeviceID { get; set; }
        public int dversion { get; set; }
    }

    public class MyMobileVerificationResp
    {
        public string mvStatus { get; set; }
        public string message { get; set; }
    }

    public class CreditCardInfo
    {
        public long PartyID { get; set; }
        public decimal Amount { get; set; }
        public string RecieptDate { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public string Expiry { get; set; }
        public string ZipCode { get; set; }
        public string CustomerNumber { get; set; }
        public string VerificationCode { get; set; }
        public bool autoPay { get; set; }

        public int RequestId { get; set; }

        public string PaymentMode { get; set; }

        public bool AutoPay { get; set; }
        public string CardExpiry { get; set; }

        public string CVVCode { get; set; }
        public decimal DepositAmount { get; set; }

        public string PaymentSessionID { get; set; }

        public string Source { get; set; }

        public string AuthKey { get; set; }
        public string CustomerNo { get; set; }

    }

    public class FeeInfo
    {
        public decimal SetupFee { get; set; }
        public decimal MoveSwitchFee { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal PUCAandGRTTax { get; set; }
        public decimal CSTax { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PurchasedAmount { get; set; }

    }

    public class DwellingInfo
    {
        public string DwellingType { get; set; }
        public int Noofbedrooms { get; set; }
        public int MinSqft { get; set; }
        public int MaxSqft { get; set; }
        public decimal MinSqftKWH { get; set; }
        public decimal MaxSqftKWH { get; set; }
    }

    public class AppInfo
    {
        public string appName { get; set; }
        public string appVersion { get; set; }
    }

    public class PaymentLocations
    {
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Today { get; set; }
        public string Tomorrow { get; set; }
        //   public decimal Distance { get; set; }
        //  public decimal Longitude { get; set; }
        //  public decimal Latitude { get; set; }

    }

    public class PaymentLocationsList : FUNBaseResponse
    {
        public List<PaymentLocations> paymentLocations { get; set; }

    }

    public class FunQAndComments : FUNBaseRequest
    {
        public string account_no { get; set; }
        public string comments { get; set; }
        public string noOfPeople { get; set; }
        public string energyImportance { get; set; }
        public string hearAboutUs { get; set; }
        public string currentProvider { get; set; }
        public string recordID { get; set; }
    }

    public class ServiceAddressStatusReq : FUNBaseRequest
    {
        public string esiid { get; set; }
        public string BrandCode { get; set; }
    }

    public class ErrorLogger : FUNBaseRequest
    {
        public int errorType { get; set; }
        public string FunName { get; set; }
        public string AccountNo { get; set; }
        public string ReqData { get; set; }
        public string ResData { get; set; }
        public string ExceptionText { get; set; }
    }

    public class FUNTrackerInfo : FUNBaseRequest
    {
        public string methodName { get; set; }
        public string requestData { get; set; }
    }
    public class GetCurrentProviderList
    {
        public string str { get; set; }
    }

    public class AgentFeeReq : FUNBaseRequest
    {
        public string ProductID { get; set; }
        public string TDSP_Code { get; set; }
        public string ChargeType { get; set; }
        public override string ToString()
        {
            return "ProductID:" + ProductID + " TDSPCode:" + TDSP_Code +
                " ChargeType:" + ChargeType;

        }

    }
    public class AgentFeeResp : FUNBaseResponse
    {
        public List<AgentFeeInfo> lstAgentFee { get; set; }
    }

    public class AgentFeeInfo
    {
        public string fee_type { get; set; }
        public string fee_desc { get; set; }
        public decimal Charges { get; set; }
        public decimal MaxWaiverPercent { get; set; }
        public string WaivePercentList { get; set; }
    }

    public class TaxRequest : FUNBaseRequest
    {
        public decimal TotalFees { get; set; }
        public decimal AmountPurchased { get; set; }
        public decimal Unit_Rate { get; set; }
        public string ProductType { get; set; }

    }
    public class Reward : FUNBaseRequest
    {
        public string RewardText { set; get; }
    }


    public class FUNWebSession_PersonalInformation : FUNBaseRequest
    {

        public string PromoCode { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string AlternatePhoneNumber { get; set; }

        public string Email { get; set; }

        public string PreferredCoummunication { get; set; }

        public string PreferredLanguage { get; set; }

        public bool ReceiveEBills { get; set; }

        public bool BillingAddressSameAsService { get; set; }

        public bool IsPOBox { get; set; }

        public string StreetNo { get; set; }

        public string StreetName { get; set; }

        public string ApartmentNumber { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string POBoxNo { get; set; }

        public string Address { get; set; }
    }


    public class FUNWebSession_ServiceInformation : FUNBaseRequest
    {

        public bool IsCurrentLocation { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string HashDOB { get; set; }

        //public string SSN { get; set; }

        public string HashSSN { get; set; }

        public bool AgreeCreditCheck { get; set; }
    }


    public class FUNWebSession_PaymentInformation : FUNBaseRequest
    {

        public string PaymentMode { get; set; }

        public bool EnrollAutoPay { get; set; }

        public string CreditCardType { get; set; }

        public string CreditCardNumber { get; set; }

        public string NameOnCard { get; set; }

        public string CardExpiry { get; set; }

        public string CVVCode { get; set; }

        public string BillingZipCode { get; set; }

        public decimal Deposit { get; set; }

        public decimal DueNow { get; set; }

        public decimal SplitBalance { get; set; }

        public int SplitDays { get; set; }

        public decimal ConnectionFee { get; set; }

        public decimal OtherCharges { get; set; }

        public decimal SplitPercentage { get; set; }

        public decimal Total { get; set; }
    }

    public class BondReq : FUNBaseRequest
    {
        public string TDSP { get; set; }
        public int product_id { get; set; }
    }

    public class BondResp : FUNBaseResponse
    {
        public List<Bond> Bondrules { get; set; }
    }

    public class Bond
    {
        public int product_id { get; set; }
        public int price_plan_id { get; set; }
        public string split_deposit_yn { get; set; }
        public string bond_selection_rule_code { get; set; }
        public decimal split_percentage { get; set; }

    }

    public class PartnerDetailsReq : FUNBaseRequest
    {
        public string PartnerName { get; set; }
        public string BrandCode { get; set; }
    }

    public class PartnerDetailsResp : FUNBaseResponse
    {
        public int PartnerID { get; set; }
        public string PartnerName { get; set; }
        public string PartnerPhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public int TabID { get; set; }
        public string PromoCode { get; set; }
        public string PartnerBenefits { get; set; }
        public bool ShowPrepay { get; set; }
        public string PageTitle { get; set; }
        public string MetaDescription { get; set; }
        public string PartnerImageText { get; set; }
        public string TabName { get; set; }
        public string TabHeader { get; set; }
        public string TabDetail { get; set; }
        public string TabFooter { get; set; }
    }

    public class IPAddressCountResponse : FUNBaseResponse
    {
        public int IPAddressCount { get; set; }
    }

    public class IsAllowIPAddressResponse : FUNBaseResponse
    {
        public bool IsAllowIPAddress { get; set; }
    }

    public class MileStoneReq : FUNBaseRequest
    {
        public string Source { get; set; }
        public string SessionId { get; set; }
        public string MileStone { get; set; }
        public string Value { get; set; }
    }
    public class dropdownValues
    {
        public string value { get; set; }
        public string text { get; set; }
    }
    public class dropdownValuesList
    {
        public List<dropdownValues> dropdownvalues { get; set; }

    }
    //*********************Created By Santosh Kumar30/08/2019**********************
    #region CustomerObject
    //Created By Santosh Kumar 30/8/2019
    public class CustomerEnrollInfo
    {
        public string Moving_Apt { get; set; }
        //public string Moving_city { get; set; }
        public string Moving_StreetName { get; set; }
        public string Customer_no { get; set; }
        public string Source { get; set; }
        public string Customer_FirstName { get; set; }
        public string Customer_LastName { get; set; }
        public string Customer_PhoneNumber { get; set; }
        public string Customer_EmailId { get; set; }
        public bool Contact_Rel_By_Mail { get; set; }
        public string Preferred_Language { get; set; }
        public string Service_Address { get; set; }
        public string Service_State { get; set; }
        public string Zip_Code { get; set; }
        public string ESID { get; set; }
        public bool IS_Bill_Add_Same_Serv { get; set; }
        public bool Is_PO_Box { get; set; }
        public string PO_Box_Num { get; set; }
        public string Diff_Service_Address { get; set; }
        public string Diff_StreetNo { get; set; }
        public string Diff_StreeName { get; set; }
        public string Diff_AptNo { get; set; }
        public string Diff_State { get; set; }
        public string Diff_City { get; set; }
        public string Diff_ZipCode { get; set; }
        public string Switching_Moving { get; set; }
        public string Moving_Address { get; set; }
        public string Moving_State { get; set; }
        public string Moving_ZipCode { get; set; }
        public DateTime Move_Or_Switch_Date { get; set; }
        public bool GoPaperLess { get; set; }
        public bool AutoPay { get; set; }
        public string Payment_Type { get; set; }
        public bool Is_Auth_To_Save_CCard { get; set; }
        public string Card_Type { get; set; }
        public string Card_Number { get; set; }
        public string Card_Name { get; set; }
        public string Card_BillingZipCode { get; set; }
        public string Card_ExpiryMonth { get; set; }
        public string Card_ExpirtyYear { get; set; }
        public string Card_CVV { get; set; }
        public string ECheck_UserName { get; set; }
        public string ECheck_Routing { get; set; }
        public string ECheck_AccountNumber { get; set; }
        public string ECheck_AccountType { get; set; }
        public bool Is_Otr_aut_User { get; set; }
        public string Otr_aut_FirstName { get; set; }
        public string Otr_aut_LastName { get; set; }
        public string Otr_aut_PhoneNumber { get; set; }
        public string SSN { get; set; }
        public string Govt_Id_type { get; set; }
        public string Govt_ID_State { get; set; }
        public string Govt_ID_Number { get; set; }
        public string Verify_auth_Service { get; set; }
        public int ProductPP_ID { get; set; }
        public int TabId { get; set; }
        public int KWH { get; set; }
        public string Promocode { get; set; }
        public DateTime Created_On { get; set; }
        // public DateTime DateOfBirth { get; set; }
        public string DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public bool IsVerifyIdentity { get; set; }
        public string ProductId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string BillingCity { get; set; }
        public string Moving_City { get; set; }
        public string Brandcode { get; set; }
        public string ProductTitle { get; set; }
        public string ActualSSN { get; set; }
    }
    #endregion
    #region GovermentId
    public class GovermentId : FUNBaseRequest
    {
        public string Id { get; set; }
        public string GovermentId_Name { get; set; }
        public List<GovermentId> GovermentIdList { get; set; }
    }
    #endregion
    #region Zipcodedetails
    public class ZipCodeDetail : FUNBaseRequest
    {
        public string zipcode { get; set; }
        public string commodity { get; set; }
        public string tdsp { get; set; }
        public string tdspName { get; set; }
    }
    public class zipCodeListDetail : FUNBaseResponse
    {
        public List<ZipCodeDetail> zipCodeList { get; set; }

    }
    #endregion
    #region PendingEnrollment
    public class PendingEnrollmentRequest : FUNBaseRequest
    {
        public FunEnrollment Sent_XML { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string VerificationCode { get; set; }
        public string Source { get; set; }
        public string ReasonCode { get; set; }
        public string CallerId { get; set; }
        public string EnrollmentType { get; set; }
        public string BrandCode { get; set; }
        public string IdentityType { get; set; }
        public string IdentityState { get; set; }
        public string IdentityNumber { get; set; }
    }
    #endregion
    public class CreditResultCompareResponse
    {
        public string InputFirstNameFirst { get; set; }
        public string InputFirstNameSecond { get; set; }
        public string InputLastNameFirst { get; set; }
        public string InputLastNameSecond { get; set; }
        public string InputDOBFirst { get; set; }
        public string InputDOBSecond { get; set; }
        public string InputAddressFirst { get; set; }
        public string InputAddressSecond { get; set; }
        public string OutputFirstName1 { get; set; }
        public string OutputLastName1 { get; set; }
        public string OutputFirstName2 { get; set; }
        public string OutputLastName2 { get; set; }
        public string OutputFirstName3 { get; set; }
        public string OutputLastName3 { get; set; }
        public string OutputDOB { get; set; }
        public string OutputAddress1 { get; set; }
        public string OutputAddress2 { get; set; }
        public string OutputAddress3 { get; set; }
        public string IsNameMatchedFirst { get; set; }
        public string IsNameMatchedSecond { get; set; }
        public string IsDOBMatchedFirst { get; set; }
        public string IsDOBMatchedSecond { get; set; }
        public string IsAddressMatchedFirst { get; set; }
        public string IsAddressMatchedSecond { get; set; }
        public string SessionId { get; set; }
    }
    public class QueryStringObject
    {
        public string zip_Code { get; set; }
        public string ProdCode { get; set; }
        public string ref_id { get; set; }
        public string tdsp_code { get; set; }
        public string kWh { get; set; }
        public string Source { get; set; }
        public string PromotionalCode { get; set; }
        public string getString()
        {
            return Source + " " + zip_Code + " " + ProdCode + " " + ref_id + " " + tdsp_code;
        }
    }
    public class CheckPaymentRequest : IVRRequest
    {

        public long CustId { get; set; } // PartyId

        public decimal Amount { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankAccountName { get; set; }

        public string AccountType { get; set; }

        public string CustomerNumber { get; set; }

        public string BankRoutingNumber { get; set; }

        public bool SetupAutoPay { get; set; }

        public string PaymentorAutopay { get; set; }

        public string ConfirmationType { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string SiteIdentifier { get; set; }

        public int IsWriteoffAmount { get; set; }

        public int IsSettledAmount { get; set; }

        public decimal ARBalanceAmount { get; set; }

        public string AuthKey { get; set; }

        public string State { get; set; }

        public string Source { get; set; }
    }
    public class CheckInfo : IVRRequest
    {
        public long CustId { get; set; }

        public bool SetupAutoPay { get; set; }

        public decimal Amount { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankAccountName { get; set; }

        public string AccountType { get; set; }

        public string CustomerNumber { get; set; }

        public string BankRoutingNumber { get; set; }

        public string ConfirmationType { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Source { get; set; }

        public int IsWriteoffAmount { get; set; }

        public string AuthKey { get; set; }

        public string PaymentorAutopay { get; set; }

        public string SiteIdentifier { get; set; }

        public int IsSettledAmount { get; set; }

        public decimal ARBalanceAmount { get; set; }

        public string State { get; set; }
    }
    public class MyPaymentLocationsRequest
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public string zip { get; set; }
    }
    public class PCITransactionLog
    {
        public string Source { get; set; }
        public string SessionId { get; set; }
        public string PCISessionId { get; set; }
        public string ChildUniqueId { get; set; }
    }
    //public class PaymentsLocationsResponse : IVRResponse
    //{
    //    public List<spSS_GetLocations_Result> Data { get; set; }
    //}
    public class FUNAccountResponse
    {
        public int resultCode { get; set; }
        public string resultMessage { get; set; }
        public string requestStatus { get; set; }
        public string requestMessage { get; set; }
    }

    public class BrandInfoReq : FUNBaseRequest
    {
        public string BrandCode { get; set; }

        public override string ToString()
        {
            return "esiid:" + BrandCode +
                   " ipAddress:" + ipAddress;
        }
    }

    public class BrandInfoResp : FUNBaseResponse
    {

        public List<BrandInfo> BrandInfo { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public override string ToString()
        {
            return " BrandInfo: " +
                 " BrandCode:" + BrandCode + " BrandName:" + BrandName +
                 " requestStatus:" + requestStatus + " requestMessage:" + requestMessage;
        }
    }

    public class BrandInfo
    {
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string BrandDescription { get; set; }
        public string PUCNo { get; set; }
        public string BrandAddress { get; set; }
        public string TollfreeNumber { get; set; }
        public string CopyRightYear { get; set; }
        public string CopyrightNote { get; set; }
    }
    public class IpInfo
    {
        public string ip { get; set; }
        public string zip { get; set; }
        public string country_name { get; set; }

    }
    public class TabDetailsResp : FUNBaseResponse
    {
        public List<TabNames> lstTabs { get; set; }
    }
    public class EditserviceResponse
    {
        public string[] UserInfo { get; set; }
        public string ProductBox { get; set; }

    }
    public class ZipDetailsListResp : FUNBaseResponse
    {

        public List<ZipCodeDetail> zipDetailResp { get; set; }

    }

}