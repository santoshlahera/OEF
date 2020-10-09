using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Configuration;
using WebEnrollmentNewDesign.Models;
using FUNWebEnrollment;
using WebEnrollmentNewDesign.CommonService;
using System.Security.Cryptography;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;


namespace WebEnrollmentNewDesign.Controllers
{
    [RoutePrefix("Home")]

    public class HomeController : Controller
    {
        #region Properties

        EnrollmentData enrollmentObject = new EnrollmentData();
        ServiceAddress serviceAddressObj = new ServiceAddress();
        BillingAddress billingAddress = new BillingAddress();
        CustomerEnrollInfo custEnrollInfo = new CustomerEnrollInfo();
        private clsDataLayer clsDtLayer = new clsDataLayer();
        private CreditScoreResponse DepAmtRes = new CreditScoreResponse();
        private ZipDetailsResp zipResp = new ZipDetailsResp();
        SwitchMoveReq switchMoveInReq = new SwitchMoveReq();
        List<SwitchMoveData> lstSwMovData = new List<SwitchMoveData>();
        EnrollResult enrollmentResp = new EnrollResult();
        PersonalInformation personalInfo = new PersonalInformation();
        ContactInformation contactInfo = new ContactInformation();
        string cardType = "Visa";
        public static string isPrepay { get; set; }
        List<DwellingInfo> dwellingList = new List<DwellingInfo>();
        string url = string.Empty;
        System.Web.HttpBrowserCapabilitiesBase browser;
        static string initialAmount;
        private QueryStringObject querStringObj = new QueryStringObject();
        clsEncryptDecrypter objclsEncryptDecrypter = new clsEncryptDecrypter();
        WebEnrollmentNewDesign.CommonService.CommonServiceClient obj = new CommonService.CommonServiceClient();
        string ipAddress = string.Empty;
        string oldTDSPCode = string.Empty;
        int TabID = 0;
        string imagepath = System.Configuration.ConfigurationManager.AppSettings["ImagePath"].ToString();
        string DisplayFeatureImage = System.Configuration.ConfigurationManager.AppSettings["DisplayFeatureImage"].ToString();

        public string Source
        {
            get
            {
                return "TX";
            }
        }

        #endregion

        public ActionResult Index()
        {
            TempData["InEnroll"] = null;

            VerifyForMobileBrowser();
            List<TabNames> taBVal = new List<TabNames>();
            string TabName = "";
            // TabName = "1Default";

            try
            {
                enrollmentObject.BrandCode = System.Configuration.ConfigurationManager.AppSettings["BrandCode"].ToString();
                if (Session["enrollmentObject2"] != null)
                {
                    enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                    if (enrollmentObject != null && enrollmentObject.ZipResp != null)
                    {
                        TempData["ZipCode"] = enrollmentObject.ZipResp.ZipCode;

                    }
                    if (Session["kwhValueforBack"] != null && Session["kwhValueforBack"].ToString() != "")
                    {
                        TempData["kWh"] = Session["kwhValueforBack"].ToString();
                    }
                }
                else
                {
                    Session["enrollmentObject"] = enrollmentObject;
                }
                CheckForPartnerPages();
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (Request.QueryString["RC"] != null && Request.QueryString["RC"] != string.Empty)
                {
                    Session["ReferralCode"] = Request.QueryString["RC"].ToString();
                }
                if (Session["RequestPage"] != null && Session["RequestPage"].ToString() != "")
                {
                    BindTemplates();
                    PartnerDetailsResp partnerDetailsResp = new PartnerDetailsResp();
                    partnerDetailsResp = (PartnerDetailsResp)Session["PartnerDetailResp"];
                    if (partnerDetailsResp != null)
                    {
                        TabName = partnerDetailsResp.TabName;
                        TempData["PartnerOfferImage1"] = partnerDetailsResp.Image1;
                        TempData["PartnerOfferImage2"] = partnerDetailsResp.Image2;
                        TempData["PartnerOfferImage3"] = partnerDetailsResp.Image3;
                        TempData["PartnerContactNo"] = partnerDetailsResp.PartnerPhoneNumber;
                        Session["PartnerBenifits"] = partnerDetailsResp.PartnerBenefits;
                        ZipDetailsReq zipreq = new ZipDetailsReq();
                        if (querStringObj != null && (string.IsNullOrEmpty(querStringObj.zip_Code)))
                        {
                            GetZipByIPAddress(zipreq);
                        }
                    }
                    //if (partnerDetailsResp.PartnerBenefits == "")
                    //{
                    //    TempData["PTabHeader"] = partnerDetailsResp.TabHeader;
                    //    TempData["PTabDetail"] = partnerDetailsResp.TabDetail;
                    //    TempData["PTabFooter"] = partnerDetailsResp.TabFooter;
                    //}
                    if (enrollmentObject.ZipResp != null)
                    {
                        TempData["TDSPCODE"] = enrollmentObject.ZipResp.TDSPCode;

                        TempData["TDSPName"] = enrollmentObject.ZipResp.TDSPName;
                        Session["TDSPCODE"] = enrollmentObject.ZipResp.TDSPCode;

                        Session["TDSPName"] = enrollmentObject.ZipResp.TDSPName;
                        if (TempData["ZipCode"] == null && enrollmentObject.ZipResp != null && enrollmentObject.ZipResp.ZipCode != "")
                        {
                            Session["ZipCode"] = enrollmentObject.ZipResp.ZipCode;
                            TempData["ZipCode"] = enrollmentObject.ZipResp.ZipCode;
                        }
                    }
                }
                else
                {
                    if (enrollmentObject != null)
                    {
                        DefaultSettings();
                        if (enrollmentObject.ZipResp != null)
                        {
                            if (TempData["TDSPAccordAdd"] != null)
                            {
                                string tdsp = TempData["TDSPAccordAdd"].ToString();
                                TempData.Keep();
                                TempData["TDSPCODE"] = tdsp;
                                enrollmentObject.ZipResp.TDSPCode = tdsp;
                                TempData.Keep();
                            }
                            else
                            {
                                TempData["TDSPCODE"] = enrollmentObject.ZipResp.TDSPCode;
                                TempData["ZipCode"] = enrollmentObject.ZipResp.ZipCode;
                                TempData["TDSPName"] = enrollmentObject.ZipResp.TDSPName;
                            }

                        }
                    }
                }
                TabDetailsResp ProdDetResp = new TabDetailsResp();
                ProductDetailReq ProdDetReq = new ProductDetailReq();
                ProdDetReq.BrandCode = enrollmentObject.BrandCode;

                // if (Session["Prodcode"] != null && Session["Prodcode"].ToString() == "" && Session["RequestPage"] == "")
                if ((Session["Prodcode"] != null && Session["Prodcode"] != "" && Session["Prodcode"].ToString() == "") || (Session["Prodcode"] != null && Session["RequestPage"] != ""))
                {
                    if (Session["RequestPage"] != "")
                    {
                        PartnerDetailsResp partnerDetailsResp = new PartnerDetailsResp();
                        partnerDetailsResp = (PartnerDetailsResp)Session["PartnerDetailResp"];
                        if (partnerDetailsResp != null)
                        {
                            ProdDetReq.TabName = partnerDetailsResp.TabName;
                        }
                    }
                }
                //  ProdDetReq.TabName = TabName;
                var s = new JavaScriptSerializer();
                string jsonAccReq = s.Serialize(ProdDetReq);
                string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetActiveTabsByTabName");
                if (resp != null)
                {
                    ProdDetResp = s.Deserialize<TabDetailsResp>(resp);
                    if (ProdDetResp.lstTabs.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ProdDetResp.lstTabs[0].TabName))
                        {
                            TempData["DefaultTabId"] = ProdDetResp.lstTabs[0].TabName.ToString();
                        }
                        if (!string.IsNullOrEmpty(ProdDetResp.lstTabs[0].TabImage))
                        {
                            TempData["TabImage"] = ProdDetResp.lstTabs[0].TabImage.ToString();
                        }
                        for (int i = 0; i <= ProdDetResp.lstTabs.Count - 1; i++)
                        {
                            if (ProdDetResp.lstTabs[i].TabName != "")
                            {
                                taBVal.Add(new TabNames
                                {
                                    TabID = ProdDetResp.lstTabs[i].TabID,
                                    TabImage = ProdDetResp.lstTabs[i].TabImage,
                                    TabName = ProdDetResp.lstTabs[i].TabName,
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("Index::Loading", Ex, -11, clsDtLayer.ObjectToJson(enrollmentObject), "");
                return View();
            }
            Session["enrollmentObject"] = enrollmentObject;
            return View(taBVal);

        }

        public JsonResult GetPartnerBenifits()
        {
            string PBenifit = "";
            if (!(string.IsNullOrEmpty(Session["PartnerBenifits"].ToString())))
            {
                PBenifit = Session["PartnerBenifits"].ToString();
            }
            return Json(PBenifit, JsonRequestBehavior.AllowGet);
        }

        [Route("Enroll")]
        public ActionResult Enrollment()
        {
            if (TempData["PrePayYN"] != null)//to check product is selected or not
            {
                PersonalInformation pInfo = new PersonalInformation();
                ContactInformation cInfo = new ContactInformation();
                BillingAddress ba = new BillingAddress();
                if (Session["pInfo"] != null)
                {
                    pInfo = (PersonalInformation)Session["pInfo"];
                }

                else
                {
                    pInfo.first_name = "";
                    pInfo.last_name = "";
                    pInfo.date_of_birth = "";
                    Session["pInfo"] = pInfo;
                }
                if (Session["cInfo"] != null)
                {
                    cInfo = (ContactInformation)Session["cInfo"];
                }
                else
                {
                    cInfo.email = "";
                    cInfo.contactno = "";
                    cInfo.eBill = false;
                    cInfo.langCode = "";

                    Session["cInfo"] = cInfo;
                }
                if (Session["enrollmentObject2"] != null)
                {
                    enrollmentObject = (EnrollmentData)Session["enrollmentObject2"];
                }
                else
                {
                    Session["PoBoxno"] = "";
                    Session["IsPoBox"] = "no";
                    ba.AptNum = "";
                    ba.CityName = "";
                    ba.StreetName = "";
                    ba.StreetNum = "";
                    ba.StateName = "";
                    ba.ZipCode = "";
                    enrollmentObject.billingAddress = ba;
                    enrollmentObject.previousAddress = ba;
                    enrollmentObject.isBillingSame = true;
                    enrollmentObject.isPreviousAddress = "No";
                    Session["enrollmentObject2"] = enrollmentObject;
                }
                TempData.Keep();
                TempData["InEnroll"] = "Enrollment";
                JavaScriptSerializer s = new JavaScriptSerializer();
                ZipDetailsReq ZipReq = new ZipDetailsReq();
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                try
                {
                    Session["enrollmentObject"] = enrollmentObject;
                    TempData["DepositAmount"] = GetDepoitAmountFromCommonService().ToString();
                    return View();
                }
                catch
                {

                    return RedirectToAction("Index");
                }
            }
            else
            {

                return RedirectToAction("Index");
            }
        }

        public void CheckForPartnerPages()
        {
            Session["RequestPage"] = "";
            try
            {
                if (Request.QueryString != null && Request.QueryString.ToString() != "")
                {
                    string vv = Request.QueryString.ToString();
                    enrollmentObject.QueryString = Server.HtmlEncode(Request.QueryString.ToString());
                    TempData["IsPartner"] = "Yes";
                    Session["IsPartner"] = "Yes";
                    Session["QueryString"] = Server.HtmlDecode(enrollmentObject.QueryString);
                }
                clsDtLayer.FunTracker_Log("CheckForPartnerPages", Request.Url.Query);
                if (!(string.IsNullOrEmpty(Request.QueryString["Zip"])))
                {
                    ZipDetailsReq zipreq = new ZipDetailsReq();
                    zipreq.zipCode = Request.QueryString["Zip"].ToString();
                    TempData["ZipCode"] = Request.QueryString["Zip"].ToString();
                    TempData["URLZipCode"] = Request.QueryString["Zip"].ToString();
                    if (clsDtLayer.IsUsZipCode(Request.QueryString["Zip"]))
                        querStringObj.zip_Code = Request.QueryString["Zip"];
                    GetZipDetails(zipreq);
                }
                else
                {
                    if (TempData["ZipCode"] == null)
                    {
                        TempData["ZipCode"] = "";
                    }
                    else
                    {
                        if (Session["enrollmentObject2"] == null)
                        {
                            TempData["ZipCode"] = "";
                            //TempData.Keep();
                        }

                    }
                }
                if (!(string.IsNullOrEmpty(Request.QueryString["Code"]))) // Product Code
                {
                    querStringObj.ProdCode = Server.UrlEncode(Request.QueryString["Code"].ToString());
                    Session["Prodcode"] = Request.QueryString["Code"].ToString();
                    //   TempData["Pcode"] = Request.QueryString["Code"].ToString();
                }
                else
                {
                    Session["Prodcode"] = "";
                }
                if (!(string.IsNullOrEmpty(Request.QueryString["PC"]))) // Promo Code
                {
                    querStringObj.PromotionalCode = Server.UrlEncode(Request.QueryString["PC"].ToString());
                    Session["Promocode"] = querStringObj.PromotionalCode;
                    try
                    {
                        enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                        TempData["Ref_ID"] = querStringObj.PromotionalCode;
                        querStringObj.ref_id = enrollmentObject.Ref_Code = querStringObj.PromotionalCode;

                        PartnerDetailsReq partnerDetailsReq = new PartnerDetailsReq();
                        PartnerDetailsResp partnerDetailsResp = new PartnerDetailsResp();
                        partnerDetailsReq.PartnerName = Server.UrlDecode(querStringObj.PromotionalCode);
                        partnerDetailsReq.BrandCode = enrollmentObject.BrandCode;
                        var s = new JavaScriptSerializer();
                        string jsonAccReq = s.Serialize(partnerDetailsReq);
                        string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetPartnerDetails");
                        partnerDetailsResp = s.Deserialize<PartnerDetailsResp>(resp);

                        if (partnerDetailsResp.requestMessage != "failure")
                        {
                            Session["PartnerDetailResp"] = partnerDetailsResp;
                            Session["RequestPage"] = "PartnerPage";
                            this.TabID = partnerDetailsResp.TabID;
                            Session["PhoneNo"] = partnerDetailsResp.PartnerPhoneNumber.ToString();
                            Session["UserName"] = partnerDetailsResp.UserName.ToString();
                        }
                        else
                        {
                            Session["RequestPage"] = "";
                            Session["UserName"] = "";
                            Session["PartnerDetailResp"] = "";
                            Session["PhoneNo"] = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDtLayer.WriteErrLog("Partner Pages:QueryString", ex, -11);
                    }
                }
                else
                {
                    Session["Promocode"] = "";
                }
                if (!(string.IsNullOrEmpty(Request.QueryString["TDSP"])))
                {
                    try
                    {
                        TempData["TDSPCODE"] = Request.QueryString["TDSP"].ToString();
                        TempData["URLTDSP"] = Request.QueryString["TDSP"].ToString();
                        querStringObj.tdsp_code = Request.QueryString["TDSP"].ToString();
                        ZipDetailsResp Zip_Resp = new ZipDetailsResp();
                        ZipDetailsListResp ziplistResp = new ZipDetailsListResp();
                        zipResp.TDSPCode = querStringObj.tdsp_code;
                        ZipCodeDetail zipCodeDetail = new ZipCodeDetail();
                        clsDataLayer clsDL = new clsDataLayer();
                        zipCodeDetail.tdsp = querStringObj.tdsp_code;
                        ZipDetailsListResp zipcodeDretailres = new ZipDetailsListResp();
                        var ss = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string jsonAccReq0 = ss.Serialize(zipCodeDetail);

                        string res = clsDL.ExecuteWebService(jsonAccReq0, "/GetTDSP");

                        if (res != null && res != "")
                        {
                            ziplistResp = ss.Deserialize<ZipDetailsListResp>(res);
                            if (ziplistResp != null && ziplistResp.zipDetailResp.Count > 0)
                            {
                                TempData["TDSPCODE"] = ziplistResp.zipDetailResp[0].tdsp;
                                TempData["TDSPName"] = ziplistResp.zipDetailResp[0].tdspName;
                                Session["TDSPCODE"] = ziplistResp.zipDetailResp[0].tdsp;
                                Session["TDSPName"] = ziplistResp.zipDetailResp[0].tdspName;
                                if (TempData["ZipCode"] != null)
                                {
                                    Zip_Resp.ZipCode = TempData["ZipCode"].ToString();
                                    TempData.Keep();
                                }
                                else
                                {
                                    Zip_Resp.ZipCode = "";
                                }
                                Zip_Resp.TDSPCode = ziplistResp.zipDetailResp[0].tdsp;
                                Zip_Resp.TDSPName = ziplistResp.zipDetailResp[0].tdspName;
                                enrollmentObject.ZipResp = Zip_Resp;
                                Session["enrollmentObject"] = enrollmentObject;

                            }
                            //else
                            //{
                            //    //Zip_Resp.TDSPCode = "CNP";
                            //    //Zip_Resp.ZipCode = (TempData["ZipCode"].ToString() != null || TempData["ZipCode"].ToString() != "") ? TempData["ZipCode"].ToString() : System.Configuration.ConfigurationManager.AppSettings["ZipCode"].ToString();
                            //    //enrollmentObject.ZipResp = Zip_Resp;
                            //    //Session["enrollmentObject"] = enrollmentObject;
                            //}
                        }
                        TempData["OLDTDSP"] = Request.QueryString["TDSP"].ToString();
                    }
                    catch (Exception ex)
                    {
                        clsDtLayer.WriteErrorLog("Partner Pages:TDSP", ex, -11, clsDtLayer.ObjectToJson(querStringObj), "");

                    }
                }
                if (!(string.IsNullOrEmpty(Request.QueryString["UL"])))
                {
                    querStringObj.kWh = Request.QueryString["UL"].ToString();
                    TempData["kWh"] = Request.QueryString["UL"].ToString();
                    Session["UL"] = Request.QueryString["UL"].ToString();
                }
                else
                {
                    if (Session["kwhValueforBack"] != null && Session["kwhValueforBack"] != "")
                    {
                        TempData["kWh"] = Session["kwhValueforBack"].ToString();
                        Session["UL"] = Session["kwhValueforBack"].ToString(); ;
                    }
                    else
                    {
                        Session["kwhValueforBack"] = "";
                        querStringObj.kWh = "";
                        TempData["kWh"] = "";
                        Session["UL"] = "";
                    }
                }
                if (!(string.IsNullOrEmpty(Request.QueryString["Source"])))
                {
                    querStringObj.Source = Request.QueryString["Source"].ToString();
                    TempData["Source"] = Request.QueryString["Source"].ToString();
                }
                else
                {
                    querStringObj.Source = "Default";
                    TempData["Source"] = "Default";
                }
                if (!(string.IsNullOrEmpty(Request.QueryString["RefID"])))
                {
                    try
                    {
                        querStringObj.ref_id = enrollmentObject.Ref_Code = Request.QueryString["RefID"].ToString();
                        PartnerDetailsReq partnerDetailsReq = new PartnerDetailsReq();
                        PartnerDetailsResp partnerDetailsResp = new PartnerDetailsResp();
                        partnerDetailsReq.PartnerName = Request.QueryString["RefID"].ToString();
                        partnerDetailsReq.BrandCode = enrollmentObject.BrandCode;
                        var s = new JavaScriptSerializer();
                        string jsonAccReq = s.Serialize(partnerDetailsReq);
                        string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetPartnerDetails");
                        partnerDetailsResp = s.Deserialize<PartnerDetailsResp>(resp);
                        if (partnerDetailsResp.requestMessage != "failure")
                        {
                            TempData["Ref_ID"] = Request.QueryString["RefID"].ToString();
                            Session["PartnerDetailResp"] = partnerDetailsResp;
                            Session["RequestPage"] = "PartnerPage";
                            this.TabID = partnerDetailsResp.TabID;
                            Session["PhoneNo"] = partnerDetailsResp.PartnerPhoneNumber.ToString();
                            Session["UserName"] = partnerDetailsResp.UserName.ToString();
                        }
                        else
                        {
                            //Session["RequestPage"] = "PartnerPage";
                            Session["RequestPage"] = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDtLayer.WriteErrLog("Partner Pages:QueryString", ex, -11);
                    }
                }
                else if ((Request.QueryString["Source"] == null && Request.QueryString["RefID"] == null) && Request.QueryString["PC"] == null)
                {
                    Session["RequestPage"] = "";
                }
                if (Request.QueryString["UN"] != null)
                {
                    Session["UserName"] = Request.QueryString["UN"].ToString();
                }
                clsDtLayer.FunTracker_Log("PartnerPage", "Source:" + querStringObj.Source + ";TDSP:" + querStringObj.tdsp_code +
                           ";Zip" + querStringObj.zip_Code +
                           ";RefID:" + querStringObj.ref_id + ";Code:" + querStringObj.ProdCode + ";kWh:" + querStringObj.kWh + ";PC:" + querStringObj.PromotionalCode);
                if (enrollmentObject != null && enrollmentObject.QueryString != null)
                {

                    //clsDtLayer.InsertMileStoneDetails("Partner Page:Default", querStringObj.ref_id);
                    clsDtLayer.InsertMileStoneDetails("Partner Page:Default", enrollmentObject.QueryString);
                }
                else
                {
                    clsDtLayer.InsertMileStoneDetails("Default");
                }
                Session["queryStringObj"] = querStringObj;
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrorLog("CheckforPartner", ex, -11, Request.QueryString.ToString(), "");
            }
        }

        private void BindTemplates()
        {
            var s = new JavaScriptSerializer();
            string strRequest = s.Serialize(Source);
            try
            {
                var respHtmlTemplate = clsDtLayer.ExecuteWebService(strRequest, "/GetHtmlTemplates");
                if (respHtmlTemplate != null)
                {
                    var lstTemplates = s.Deserialize<HtmlTemplateList>(respHtmlTemplate);
                    if (lstTemplates.requestStatus == 1)
                    {
                        if (lstTemplates.lstHtmlTemplate != null && lstTemplates.lstHtmlTemplate.Count > 0)
                        {
                            StringBuilder objAboveCards = new StringBuilder();
                            StringBuilder objLeftBlock = new StringBuilder();
                            StringBuilder objRightBlock = new StringBuilder();
                            StringBuilder objBelowCards = new StringBuilder();
                            foreach (WebEnrollmentNewDesign.Models.HtmlTemplate item in lstTemplates.lstHtmlTemplate)
                            {
                                if (item.Position != -99)
                                {
                                    if (item.Position < 2) // Main Banner
                                    {
                                        objAboveCards.Append(item.ImageUrl);
                                    }
                                    else if (item.Position == 3) // Community Partner Banner
                                    {
                                        objBelowCards.Append(item.TemplateContent);
                                    }
                                    else if (item.Position > 3)
                                    {
                                        objLeftBlock.Append(item.TemplateContent);
                                    }
                                }
                                else
                                {
                                    objRightBlock.Append(item.TemplateContent);
                                }
                            }
                            TempData["divAboveCards"] = objAboveCards.ToString();
                            TempData["divLeftBox"] = objLeftBlock.ToString();
                            TempData["divRightBox"] = objRightBlock.ToString();
                            TempData["divBelowCards"] = objBelowCards.ToString();
                        }
                    }

                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("BindHtmlTemplates", Ex, -11);
            }
        }

        public JsonResult GetMonths()
        {
            var months = Enumerable.Range(1, 12).Select(x => new { Text = new DateTime(Convert.ToInt32(DateTime.Now.Year), x, 1).ToString("MM"), Value = new DateTime(Convert.ToInt32(DateTime.Now.Year), x, 1).ToString("MM") });

            return Json(months, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYears()
        {
            var years = Enumerable.Range(DateTime.Now.Year, 10).Select(x => new { Text = new DateTime(Convert.ToInt32(x), 1, 1).ToString("yy"), Value = new DateTime(Convert.ToInt32(x), 1, 1).ToString("yy") });

            return Json(years, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ContentResult InsertOrderDetail(FormCollection formCollection)
        {
            string[] Result = new string[2];
            string revertCustInfo = "";
            bool IsValid = true;
            //********************************
            //****Get All Values from Page****
            //********************************
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            bool IsBilling_Check = true;
            bool CreditCheck_Check = true;
            bool CreditCard_Auto = true;
            string revertMsg = "";
            string Switch_Or_MoveDate = "";
            string FirstName = formCollection["first_name"].Trim();
            string MiddleName = formCollection["middle_name"].Trim();
            string LastName = formCollection["last_name"].Trim();
            string ContactNumber = formCollection["contactno"].Trim();
            string DateOfBirth = formCollection["date_of_birth"].Trim();

            string Email = formCollection["email"].Trim();
            string IsPLessBilling = formCollection["IsPLessBilling"].Trim();
            string IsPLessCommunication = formCollection["IsPLessCommu"].Trim();
            string IsBilling = formCollection["IsBilling"].Trim();
            string StreetName = formCollection["StreetName"].Trim();
            string StreetNum = formCollection["StreetNum"].Trim();
            string AptNum = formCollection["AptNum"].Trim();
            string CityName = formCollection["CityName"].Trim();
            string StateName = formCollection["StateName"].Trim();
            string BilllingZipCode = formCollection["ZipCode"].Trim();
            string ZipCode = BilllingZipCode.Trim();
            string IsPoBox = formCollection["IsPoBox"].Trim();
            string PoBox = formCollection["PoBox"].Trim();

            string PreferedLanguage = formCollection["PreferedLanguage"].Trim();

            string LocationByName = formCollection["LocationByName"];
            string IsMoving = formCollection["IsMoving"];
            string StreetNamePrevious = formCollection["StreetNamePrevious"];
            string APTPrevious = formCollection["APTPrevious"].Trim();
            string CityPrevious = formCollection["CityPrevious"].Trim();
            string StatePrevious = formCollection["StatePrevious"].Trim();
            string ZipCodePrevious = formCollection["ZipCodePrevious"].Trim();
            string switchDate = formCollection["switchDate"].Trim();
            string PaymentMethod = formCollection["Payment_Method"].Trim();
            string CreditAutoPay = formCollection["CreditAutoPay"].Trim();
            string CreditCardNumber = formCollection["CreditCardNumber"].Trim();
            string CreditCardType = formCollection["CreditCardType"].Trim();
            string CreditCardHolder = formCollection["CreditCardHolder"].Trim();
            string BillingZipCode = formCollection["BillingZipCode"].Trim();
            string ExpirationMonth = formCollection["ExpirationMonth"].Trim();
            string ExpirationYear = formCollection["ExpirationYear"].Trim();
            string CVVCode = formCollection["CVVCode"].Trim();
            string ddlContactOptions = formCollection["ddlContactOptions"].Trim();
            string NameOnBankAccount = formCollection["NameOnBankAccount"].Trim();
            string AccountNumber = formCollection["AccountNumber"].Trim();
            string RoutingNumber = formCollection["RoutingNumber"].Trim();
            string AccountType = formCollection["AccountType"].Trim();
            string AnotherAuthrizeduser = formCollection["AnotherAuthrizedUser"].Trim();
            string AuthrizeduserFirstName = formCollection["AuthrizedUserFirstName"].Trim();
            string AuthrizeduserLastName = formCollection["AuthrizedUserLastName"].Trim();
            string AuthrizeduserContactNumber = formCollection["AuthrizedUserContactNumber"].Trim();
            string CarrierType = formCollection["ProviderType"].Trim();
            string CreditCheck = formCollection["CreditCheck"].Trim();
            string SocialSecurityType = formCollection["SocialSecurity"].Trim();
            string ssn = formCollection["ssn"].Trim();
            string GovermentIdNumber = formCollection["GovermentIdNumber"].Trim();
            string GovIdType = formCollection["GovIdType"].Trim();
            string GovIdState = formCollection["GovIdState"].Trim();
            string IsTCPa = formCollection["IsTCPA"].Trim();
            string doB = "";
            //********************************
            //****End****
            //********************************
            CustomerEnrollInfo customerEnrollInfo = new CustomerEnrollInfo();
            string PromoCodeRevert = "";
            bool Plbilling = false;
            bool PlCommunincation = false;
            bool IsBillsameAsServ = false;
            if (IsPLessBilling.ToLower() == "yes")
            {
                Plbilling = true;
            }
            else if (IsPLessBilling.ToLower() == "no")
            {
                Plbilling = false;
            }
            if (IsPLessCommunication.ToLower() == "yes")
            {
                PlCommunincation = true;
            }
            else if (IsPLessCommunication.ToLower() == "no")
            {
                PlCommunincation = false;
            }
            if (CreditCheck == "Yes")
            {
                CreditCheck_Check = true;
            }
            else if (CreditCheck == "No")
            {
                CreditCheck_Check = false;
            }
            if (CreditAutoPay == "Yes")
            {
                CreditCard_Auto = true;
            }
            else if (CreditAutoPay == "No")
            {
                CreditCard_Auto = false;
            }
            if (Session["Promocode"] != null && Convert.ToString(Session["Promocode"]) != "")
            {
                string vval = Session["Promocode"].ToString();
                IsValid = Validatepromocode(vval);
                if (!IsValid)
                {
                    PromoCodeRevert = "The promo code you entered is not valid.";
                }
                else
                {
                    customerEnrollInfo.Promocode = vval;
                }
            }
            if (IsBilling == "Yes")
            {
                IsBillsameAsServ = true;
            }
            else
            {
                IsBillsameAsServ = false;
            }
            bool IspoBox = false;
            if (IsPoBox == "Yes")
            {
                IspoBox = true;
            }
            else
            {
                IspoBox = false;
            }
            bool IsAnotherAuthUser = false;
            if (AnotherAuthrizeduser == "Yes")
            {
                IsAnotherAuthUser = true;
            }
            else
            {
                IsAnotherAuthUser = false;

            }
            string Moving_OR_Switch = "";
            if (IsMoving == "Yes")
            {
                Moving_OR_Switch = "MOVEIN";
            }
            else
            {
                Moving_OR_Switch = "SWITCH";
            }
            var s = new JavaScriptSerializer();
            try
            {
                //********************************
                //****Insert Customer All Information****
                //********************************
                Guid id = Guid.NewGuid();
                customerEnrollInfo.Customer_no = id.ToString();
                customerEnrollInfo.Customer_FirstName = FirstName;
                customerEnrollInfo.Customer_LastName = LastName;
                customerEnrollInfo.Customer_PhoneNumber = ContactNumber;
                try
                {
                    doB = DateTime.ParseExact(DateOfBirth.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //doB = DateOfBirth;
                    //customerEnrollInfo.DateOfBirth = DateTime.ParseExact(doB, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    customerEnrollInfo.DateOfBirth = doB.ToString();
                }
                catch (Exception ex)
                {
                    clsDtLayer.WriteErrLog("dateofBirthError", ex, -11);
                }
                customerEnrollInfo.Customer_EmailId = Email;
                customerEnrollInfo.GoPaperLess = Plbilling;

                customerEnrollInfo.IS_Bill_Add_Same_Serv = IsBillsameAsServ;
                TempData["CreditCheck"] = CreditCheck;
                customerEnrollInfo.IsVerifyIdentity = CreditCheck_Check;
                customerEnrollInfo.Preferred_Language = PreferedLanguage;
                customerEnrollInfo.Contact_Rel_By_Mail = PlCommunincation;
                if (Session["Address"] != null)
                {
                    customerEnrollInfo.Service_Address = Session["Address"].ToString();
                }
                else
                {
                    return Content("error");
                }
                ZipCode = enrollmentObject.ZipResp.ZipCode;

                customerEnrollInfo.Zip_Code = ZipCode;
                customerEnrollInfo.Diff_Service_Address = StreetNum + " " + StreetName + " " + AptNum + " " + CityName;

                custEnrollInfo.Diff_AptNo = AptNum;
                custEnrollInfo.Diff_StreeName = StreetName;
                custEnrollInfo.Diff_StreetNo = StreetNum;
                custEnrollInfo.Diff_State = StateName;
                custEnrollInfo.Diff_ZipCode = BilllingZipCode;
                custEnrollInfo.Diff_City = CityName;

                customerEnrollInfo.Diff_AptNo = AptNum;
                customerEnrollInfo.Diff_StreeName = StreetName;
                customerEnrollInfo.Diff_StreetNo = StreetNum;
                customerEnrollInfo.Diff_State = StateName;
                customerEnrollInfo.Diff_ZipCode = BilllingZipCode;
                customerEnrollInfo.Diff_City = CityName;

                customerEnrollInfo.Service_State = StateName;
                customerEnrollInfo.Zip_Code = ZipCode;
                customerEnrollInfo.Is_PO_Box = IspoBox;
                customerEnrollInfo.PO_Box_Num = PoBox;
                customerEnrollInfo.Switching_Moving = Moving_OR_Switch;
                custEnrollInfo.Moving_Apt = APTPrevious;
                custEnrollInfo.Moving_City = CityPrevious;
                customerEnrollInfo.BillingCity = "";
                customerEnrollInfo.City = CityName;

                customerEnrollInfo.Moving_Address = StreetNamePrevious + " " + APTPrevious + " " + CityPrevious + " " + StatePrevious + " " + ZipCodePrevious;
                customerEnrollInfo.Moving_State = StatePrevious;
                customerEnrollInfo.Moving_ZipCode = ZipCodePrevious;
                customerEnrollInfo.Moving_StreetName = StreetNamePrevious;
                customerEnrollInfo.Moving_City = CityPrevious;
                customerEnrollInfo.Moving_Apt = APTPrevious;
                //string Newdate = switchDate.Replace(',', ' ');
                try
                {
                    //    string cleanedString = System.Text.RegularExpressions.Regex.Replace(Newdate, @"\s+", " ");
                    //    var dateSwitch = DateTime.ParseExact(cleanedString.Substring(0, 15), "ddd MMM d yyyy", CultureInfo.InvariantCulture);
                    //    var dateSwitchsDate = dateSwitch.ToString("MM/dd/yyyy");
                    //    string dateSwitchs = dateSwitchsDate.ToString();
                    //    dateSwitchs = dateSwitchs.Replace("-", "/");
                    //    TempData["SwitchMoveDate"] = dateSwitchs;
                    //    customerEnrollInfo.Move_Or_Switch_Date = DateTime.ParseExact(dateSwitchs.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    //    Switch_Or_MoveDate = dateSwitchs.ToString();
                    switchDate = switchDate.Replace("-", "/");
                    TempData["SwitchMoveDate"] = switchDate;
                    customerEnrollInfo.Move_Or_Switch_Date = DateTime.ParseExact(switchDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    Switch_Or_MoveDate = switchDate.ToString();
                }
                catch (Exception ex)
                {
                    clsDtLayer.WriteErrLog("TestingError", ex, -11);
                }
                if (!string.IsNullOrEmpty(PaymentMethod) && PaymentMethod.ToUpper() != "CASH")
                {
                    customerEnrollInfo.AutoPay = true; ;
                }
                else
                {
                    customerEnrollInfo.AutoPay = false;
                }
                customerEnrollInfo.Is_Auth_To_Save_CCard = CreditCard_Auto;
                customerEnrollInfo.Payment_Type = PaymentMethod;
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                customerEnrollInfo.Is_Otr_aut_User = IsAnotherAuthUser;
                customerEnrollInfo.State = StateName;
                customerEnrollInfo.ZipCode = enrollmentObject.ZipResp.ZipCode;
                customerEnrollInfo.ESID = "";
                customerEnrollInfo.Description = "";
                customerEnrollInfo.Otr_aut_FirstName = AuthrizeduserFirstName;
                customerEnrollInfo.Otr_aut_LastName = AuthrizeduserLastName;
                customerEnrollInfo.Otr_aut_PhoneNumber = AuthrizeduserContactNumber;
                customerEnrollInfo.SSN = objclsEncryptDecrypter.SHA1Hash(objclsEncryptDecrypter.lastFourDigits(ssn));
                customerEnrollInfo.ActualSSN = ssn;
                custEnrollInfo.City = CityName;
                customerEnrollInfo.Govt_ID_Number = GovermentIdNumber;
                customerEnrollInfo.Govt_ID_State = GovIdState;
                customerEnrollInfo.Govt_Id_type = GovIdType;
                customerEnrollInfo.Source = ConfigurationManager.AppSettings["source"].ToString();
                if (TempData["PromoCode"] != null)
                {
                    customerEnrollInfo.Promocode = TempData["PromoCode"].ToString();
                }
                //if (TempData["ESID"] != null)
                //{
                //    customerEnrollInfo.ESID = TempData["ESID"].ToString();
                //    TempData.Keep();
                //}
                serviceAddressObj = (ServiceAddress)Session["ServAddrObj"];
                if (serviceAddressObj != null)
                {
                    customerEnrollInfo.ESID = serviceAddressObj.ESIID;
                }
                else
                {
                    return Content("error");
                }
                if (enrollmentObject.ProdDetails != null)
                {
                    customerEnrollInfo.ProductId = enrollmentObject.ProdDetails.Product_ID;
                    customerEnrollInfo.ProductTitle = enrollmentObject.ProdDetails.ProductTitle;
                }
                else
                {
                    return Content("error");
                }
                customerEnrollInfo.ProductPP_ID = Convert.ToInt32(TempData["pPid"].ToString());
                customerEnrollInfo.Brandcode = enrollmentObject.BrandCode;

                customerEnrollInfo.KWH = Convert.ToInt32(TempData["kwh_Choice"].ToString());
                TempData.Keep();
                string servierAddressMsg = "";
                if (Session["ServiceAddressMsg"] != null)
                {
                    servierAddressMsg = Session["ServiceAddressMsg"].ToString();
                }
                if (string.IsNullOrEmpty(servierAddressMsg) == false)
                {
                    customerEnrollInfo.IsActive = false;
                    customerEnrollInfo.Description = Session["ServiceAddressMsg"].ToString();
                    TempData.Keep();
                }
                else
                {
                    customerEnrollInfo.IsActive = true;
                    customerEnrollInfo.Description = "";
                }
                string jsonRequest1 = s.Serialize(customerEnrollInfo);
                clsDtLayer.ExecuteWebService(jsonRequest1, "/InsertCustomerEnrollInfo");
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrorLog("InsertCustomerEnrollInfo", ex, -11, clsDtLayer.ObjectToJson(customerEnrollInfo) + "Enrollment Object: " + clsDtLayer.ObjectToJson(enrollmentObject), clsDtLayer.ObjectToJson(custEnrollInfo));
            }
            //********************************
            //****End****
            //********************************
            personalInfo.first_name = customerEnrollInfo.Customer_FirstName;
            personalInfo.middle_initial = "";
            personalInfo.last_name = customerEnrollInfo.Customer_LastName;
            personalInfo.date_of_birth = doB;
            personalInfo.hashDOB = objclsEncryptDecrypter.SHA1Hash((personalInfo.date_of_birth));
            personalInfo.ssn = customerEnrollInfo.SSN;
            personalInfo.hashSSN = objclsEncryptDecrypter.SHA1Hash(objclsEncryptDecrypter.lastFourDigits(customerEnrollInfo.SSN));
            contactInfo.email = customerEnrollInfo.Customer_EmailId;
            contactInfo.contactno = customerEnrollInfo.Customer_PhoneNumber;
            contactInfo.mobile_no = customerEnrollInfo.Customer_PhoneNumber;
            contactInfo.langCode = customerEnrollInfo.Preferred_Language;
            contactInfo.contactType = "Phone";
            contactInfo.eBill = PlCommunincation;
            if ((enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("y")))
            {
                contactInfo.providers = CarrierType.Replace("&", "&amp;");
            }
            enrollmentObject.AuthrizedUserFirstName = customerEnrollInfo.Otr_aut_FirstName;
            enrollmentObject.AuthrizeduserLastName = customerEnrollInfo.Otr_aut_LastName;
            enrollmentObject.AuthrizeduserContactNumber = customerEnrollInfo.Otr_aut_PhoneNumber;
            enrollmentObject.GovermentIdNo = customerEnrollInfo.Govt_ID_Number;
            enrollmentObject.contactInfo = contactInfo;
            enrollmentObject.personalInfo = personalInfo;
            enrollmentObject.IsPaperless = Plbilling;
            enrollmentObject.contactInfo.eBill = PlCommunincation;
            ServiceAddress serviceAddress = new ServiceAddress();
            serviceAddress = (ServiceAddress)Session["ServAddrObj"];

            enrollmentObject.serviceAddressObj = serviceAddress;
            enrollmentObject.IsTCPAAgreed = IsTCPa;
            enrollmentObject.TCPAPhoneType = "Mobile";
            Session["enrollmentObject"] = enrollmentObject;
            fillBillingAddress(custEnrollInfo);

            ProductDetails productDetails = new ProductDetails();
            productDetails = (ProductDetails)Session["ProductDetails"];
            enrollmentObject.ProdDetails = productDetails;
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];

            // *********Check Credit Score***************************** 
            //***********************End***************************************************
            //Without Credit Check
            //******************************Start***********************************

            if (enrollmentObject.personalInfo == null)
                enrollmentObject.personalInfo = new PersonalInformation();

            enrollmentObject.personalInfo.date_of_birth = doB;
            enrollmentObject.personalInfo.hashDOB = objclsEncryptDecrypter.SHA1Hash(DateTime.ParseExact(enrollmentObject.personalInfo.date_of_birth, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yy", CultureInfo.InvariantCulture));
            enrollmentObject.personalInfo.ssn = ssn;
            enrollmentObject.personalInfo.hashSSN = objclsEncryptDecrypter.SHA1Hash(objclsEncryptDecrypter.lastFourDigits(enrollmentObject.personalInfo.ssn));
            enrollmentObject.isBillingSame = IsBilling_Check;
            Session["enrollmentObject"] = enrollmentObject;

            if (enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y")) // Prepaid Product
            {
                clsDtLayer.InsertMileStoneDetails("Selected Prepay Product: ContinueWithoutCreditCheck");

                this.DepAmtRes = new CreditScoreResponse();
                CommonService.CreditScoreResponse internalRuleResp = new CommonService.CreditScoreResponse();
                try
                {
                    internalRuleResp = VerifyInternalRule();
                    this.DepAmtRes = internalRuleResp;
                }
                catch (Exception ex)
                {
                    clsDtLayer.WriteErrLog("VerifyInternalRule", ex, -11);
                }

                enrollmentObject.depAmtResp = internalRuleResp;
                enrollmentObject.creditCheckYesNo = false;
                enrollmentObject.creditCheckAgreed = "FALSE";

                if (!string.IsNullOrEmpty(internalRuleResp.result_msg) && internalRuleResp.result_msg.ToUpper() != "PASS")
                {
                    if (internalRuleResp.pv_BadDebtAmt > 0)
                    {
                        enrollmentObject.BadDebtAmt = internalRuleResp.pv_BadDebtAmt;
                    }
                    enrollmentObject.ReasonCode = string.IsNullOrEmpty(enrollmentObject.ReasonCode) ? "IL100_INTERNALRULE" : enrollmentObject.ReasonCode;// +" and " + "IL100_INTERNALRULE";
                    string CreditCheckErrorMes = string.Format((String)HttpContext.GetGlobalResourceObject("MyFunResources", "CreditCheckError_InternalRule"), " " + internalRuleResp.result_msg, " " + internalRuleResp.pv_matched_cust_nos);
                    enrollmentObject.InternalRuleMes = CreditCheckErrorMes.Replace("</br>", "");
                    Session["enrollmentObject"] = enrollmentObject;
                }
            }
            else // Postpaid Product
            {
                try
                {
                    clsDtLayer.InsertMileStoneDetails("Selected PostPaid Product: ContinueWithCreditCheck");
                    if (Session["enrollmentObject"] != null)
                    {
                        WebEnrollmentNewDesign.Models.CreditScoreReqData csReqData = new WebEnrollmentNewDesign.Models.CreditScoreReqData();
                        bool Isprevious = false;
                        if (Moving_OR_Switch.ToUpper() == "MOVEIN")
                        {
                            Isprevious = true;
                        }
                        else
                        {
                            Isprevious = false;
                        }
                        PopulateDepositAmoutObject(csReqData, Isprevious, customerEnrollInfo); // Fill CreditScore Req Data internal object
                        this.DepAmtRes = new CreditScoreResponse();
                        CommonService.CreditScoreResponse internalRuleResp = new CommonService.CreditScoreResponse();
                        try
                        {
                            internalRuleResp = VerifyInternalRule();
                        }
                        catch (Exception ex)
                        {
                            clsDtLayer.WriteErrLog("VerifyInternalRule", ex, -11);
                        }
                        if (!string.IsNullOrEmpty(internalRuleResp.result_msg) && internalRuleResp.result_msg.ToUpper() == "PASS")
                        {
                            enrollmentObject.creditCheckYesNo = true;
                            enrollmentObject.creditCheckAgreed = "TRUE";
                            try
                            {
                                this.DepAmtRes = GetCreditScore(csReqData);
                                clsDtLayer.InsertMileStoneDetails("CreditCheck", Convert.ToString(this.DepAmtRes.credit_score));
                                enrollmentObject.creditcheck_status_id = this.DepAmtRes.creditcheck_status_id;
                            }
                            catch (Exception ex)
                            {
                                clsDtLayer.WriteErrLog("GetCreditScore", ex, -11);
                            }
                            CreditScoreResponse crScrResp = new CreditScoreResponse();
                            try
                            {
                                crScrResp = this.DepAmtRes;
                            }
                            catch (Exception ex)
                            {
                                clsDtLayer.WriteErrLog("Fill Deposit Amount Details", ex, -11);
                            }

                            if (this.DepAmtRes.EnrollmentStep != null)
                            {
                                if (this.DepAmtRes.EnrollmentStep.ToLower() == "reject")
                                {
                                    enrollmentObject.ReasonCode = DepAmtRes.EnrollmentStep;
                                }
                            }

                            if (this.DepAmtRes.freeze_flag == "Y")
                            {
                                enrollmentObject.ReasonCode = string.IsNullOrEmpty(enrollmentObject.ReasonCode) ? "CR100_FREEZE" : "CR100_FREEZE" + " and " + enrollmentObject.ReasonCode;
                                enrollmentObject.CsReqData = csReqData;
                                enrollmentObject.CrScrResp = this.DepAmtRes;
                                enrollmentObject.depAmtResp = this.DepAmtRes;
                                Session["enrollmentObject"] = enrollmentObject;
                            }
                            if (this.DepAmtRes.result_code == -33)
                            {
                                FillEnrollmentWithCreditResponse(crScrResp, "InternalRule2", csReqData);
                            }
                            else if (this.DepAmtRes.result_code == 1)
                            {
                                if (crScrResp.depositAmount == 0)
                                {
                                    enrollmentObject.CsReqData = csReqData;
                                    enrollmentObject.CrScrResp = this.DepAmtRes;
                                    enrollmentObject.depAmtResp = this.DepAmtRes;
                                    Session["enrollmentObject"] = enrollmentObject;
                                }
                                else
                                {
                                    if (crScrResp.requestStatus == 1)
                                    {
                                        FillEnrollmentWithCreditResponse(crScrResp, "CreditCheck", csReqData);
                                    }
                                    else
                                    { clsDtLayer.WriteErrInfo("Credit Check:DepAmt", "DepAmt Alert Popup Continue", 11); }
                                }
                            }
                            else if (this.DepAmtRes.result_code == -11)
                            {
                                FillEnrollmentWithCreditResponse(crScrResp, "CreditCheck", csReqData);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(csReqData.ssn))
                                    csReqData.ssn = string.Empty;
                                enrollmentObject.CsReqData = csReqData;
                                enrollmentObject.CrScrResp = this.DepAmtRes;
                                enrollmentObject.depAmtResp = this.DepAmtRes;
                                Session["enrollmentObject"] = enrollmentObject;
                            }
                        }
                        else // Internal rule fails
                        {
                            try
                            {
                                enrollmentObject.CsReqData = csReqData;
                                string result = string.Format((String)HttpContext.GetGlobalResourceObject("MyFunResources", "CreditCheckError_InternalRule"), " " + internalRuleResp.result_msg, " " + internalRuleResp.pv_matched_cust_nos);
                                enrollmentObject.InternalRuleMes = result.Replace("</br>", "");
                                enrollmentObject.depAmtResp = internalRuleResp;
                                enrollmentObject.CrScrResp = internalRuleResp;
                                enrollmentObject.creditCheckYesNo = false;
                                enrollmentObject.creditCheckAgreed = "FALSE";
                            }
                            catch (Exception ex)
                            {
                                clsDtLayer.WriteErrorLog("Internal RuleFails ", ex, -11, clsDtLayer.ObjectToJson(csReqData), clsDtLayer.ObjectToJson(internalRuleResp));
                            }
                            if (internalRuleResp.pv_BadDebtAmt > 0)
                            {
                                enrollmentObject.BadDebtAmt = internalRuleResp.pv_BadDebtAmt;
                            }
                            string CreditCheckErrorMes = string.Format((String)HttpContext.GetGlobalResourceObject("MyFunResources", "CreditCheckError_InternalRule"), " " + internalRuleResp.result_msg, " " + internalRuleResp.pv_matched_cust_nos);
                            enrollmentObject.InternalRuleMes = CreditCheckErrorMes.Replace("</br>", "");
                            enrollmentObject.ReasonCode = string.IsNullOrEmpty(enrollmentObject.ReasonCode) ? "IL100_INTERNALRULE" : enrollmentObject.ReasonCode + " and " + "IL100_INTERNALRULE";
                            Session["enrollmentObject"] = enrollmentObject;
                        }
                    }
                }

                catch (Exception ex)
                {
                    clsDtLayer.WriteErrorLog("CreditChk Agreed ", ex, -11, clsDtLayer.ObjectToJson(enrollmentObject), "");
                }
            }

            //   InsertPersonalInformation()
            //***************************************Start**********************************
            // if (string.IsNullOrEmpty(Promocode.Trim()) || IsValid)
            string mobile_verification = ConfigurationManager.AppSettings["mobile_verification"].ToString();
            WebEnrollmentNewDesign.Models.FUNBaseRequest basereq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
            basereq = clsDtLayer.FillBaseRequestInfo();

            // Insert User Personal Information
            //***************************************Start**********************************
            InsertPersonalInformation(customerEnrollInfo);

            //******************************End***********************************

            //Insert Service Information
            //***************************************Start*********************
            InsertServiceInformation(customerEnrollInfo);

            //********************************************END**************************************

            //*****************************************PAYMENT INFORMATION*************************
            try
            {
                // if (Session["enrollmentObject"] != null)

                if (enrollmentObject.ProdDetails != null && enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y"))//prepaid
                {
                    try
                    {
                        enrollmentObject.paymentType = customerEnrollInfo.Payment_Type.ToUpper();
                        enrollmentObject.purchasedkWh = Convert.ToDecimal(TempData["kwh_Choice"].ToString());
                    }
                    catch
                    { }
                }
                else
                {//postpaid

                    if (enrollmentObject.paymentType == null)
                        enrollmentObject.paymentType = PaymentMethod.ToUpper();
                }

                if (enrollmentObject.ProdDetails == null)
                    enrollmentObject.ProdDetails = new ProductDetails();

                Session["enrollmentObject"] = enrollmentObject;

                CreditCardInfo creditCardInfo = new CreditCardInfo();
                if (enrollmentObject != null)
                {
                    try
                    {
                        if (enrollmentObject.ProdDetails != null && enrollmentObject.ProdDetails.Prepay_YN.Equals("Y"))
                        {
                            try
                            {
                                creditCardInfo.Amount = (enrollmentObject.feeInfo != null ? enrollmentObject.feeInfo.TotalAmount : 0); // Convert.ToDecimal(txtkWhPurchase.Text.ToString());
                            }
                            catch
                            {
                                creditCardInfo.Amount = 0;
                            }
                        }
                        else if (enrollmentObject.depAmtResp != null && enrollmentObject.depAmtResp.depositAmount > 0)
                        {

                            creditCardInfo.Amount = (enrollmentObject.paymentAmount != null ? enrollmentObject.paymentAmount : 0);//enrollmentObject.depAmtResp.depositAmount;
                        }
                        else
                        {
                            creditCardInfo.Amount = 0;
                        }
                        creditCardInfo.autoPay = CreditCard_Auto ? true : false;


                    }
                    catch (Exception ex)
                    {
                        clsDtLayer.WriteErrorLog("CreditCardInfo:Fill ", ex, -11, "", "");
                    }
                    if (enrollmentObject.creditCardInfo == null)
                        enrollmentObject.creditCardInfo = new CreditCardInfo();

                    enrollmentObject.creditCardInfo = creditCardInfo;
                    Session["enrollmentObject"] = enrollmentObject;
                }
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                try
                {
                    basereq = clsDtLayer.FillBaseRequestInfo();

                    FUNWebSession_PaymentInformation objFUNWebSession_PaymentInformation = new FUNWebSession_PaymentInformation();
                    objFUNWebSession_PaymentInformation.sessionID = basereq.sessionID;
                    objFUNWebSession_PaymentInformation.PaymentMode = PaymentMethod;

                    int SplitDays = 0;

                    objFUNWebSession_PaymentInformation.SplitDays = SplitDays;

                    string jsonPaymentRequest = s.Serialize(objFUNWebSession_PaymentInformation);
                    //   clsDtLayer.ExecuteWebService(jsonPaymentRequest, "/InsertPaymentInformation");
                }
                catch (Exception)
                {
                }

            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                    clsDtLayer.WriteErrLog("PayInfo:ShowPreivew", ex, -11);
            }

            //*****************************************End*******************************************************
            //Confirm and Enroll
            FunEnrollment FunObj = new FunEnrollment();
            WebEnrollmentNewDesign.Models.FUNBaseRequest bReq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
            EnrollResult enrollmentResp = new EnrollResult();
            string ReasonCode = "";
            try
            {
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (Session["enrollmentObject"] != null)
                {
                    //if (chkAccept.Checked && chkAutorizedPerson.Checked)
                    {

                        string IsAgentVerification = "";
                        if (enrollmentObject != null)
                        {
                            if (enrollmentObject.ProdDetails != null)

                                IsAgentVerification = "N";

                            //var serialize = new JavaScriptSerializer();
                            //WebEnrollmentNewDesign.Models.FUNBaseRequest baseReq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                            //int IPAddressCount = Convert.ToInt32(ConfigurationManager.AppSettings["IPAddressCount"]);

                            //baseReq.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            //string jsonReq = serialize.Serialize(baseReq);
                            //string respIP = clsDtLayer.ExecuteWebService(jsonReq, "/GetIPAddressCount");
                            //var ipCount = new IPAddressCountResponse();
                            //if (respIP != null)
                            //{
                            //    ipCount = serialize.Deserialize<IPAddressCountResponse>(respIP);
                            //}

                            //int count = ipCount.IPAddressCount;
                            //// if (count >= IPAddressCount)
                            //{
                            //    enrollmentObject.ipAddress = baseReq.ipAddress;
                            //    string jsonEnrollReq = serialize.Serialize(enrollmentObject);
                            //    clsDtLayer.ExecuteWebService(jsonEnrollReq, "/InsertFUNWebIPAddressExceedLog");

                            //}

                            FillSwitchMoveRespObj(customerEnrollInfo);
                            enrollmentObject.isBillingSame = IsBillsameAsServ;
                            Session["CustomerEnrollInfo"] = customerEnrollInfo;

                            FillFunEnrollObject(FunObj, customerEnrollInfo);
                            enrollmentResp = new EnrollResult();
                            bReq = clsDtLayer.FillBaseRequestInfo();
                            FunObj.appName = bReq.appName;
                            FunObj.appVersion = bReq.appVersion;
                            FunObj.deviceID = bReq.deviceID;
                            FunObj.dversion = bReq.dversion;
                            FunObj.ipAddress = bReq.ipAddress;
                            FunObj.sessionID = bReq.sessionID;
                            if (Session["ReferralCode"] != null)
                            {
                                FunObj.refAccountNumber = Session["ReferralCode"].ToString();
                            }
                            if (Session["UserName"] != null && Session["UserName"] != "")
                            {
                                FunObj.SalespersonCode = Session["UserName"].ToString();
                            }
                            else
                            {
                                if (Session["IsMobileBrowserResponsive"] != null)
                                {
                                    if (Session["IsMobileBrowserResponsive"].ToString().ToLower() == "no")
                                    {
                                        FunObj.SalespersonCode = System.Configuration.ConfigurationManager.AppSettings["SalespersonCode"];
                                    }
                                    else
                                    {
                                        FunObj.SalespersonCode = System.Configuration.ConfigurationManager.AppSettings["MobileSalesPersonCode"];
                                    }
                                }
                            }
                            string ser_str = s.Serialize(FunObj);
                            string ret_str = "";

                            ReasonCode = enrollmentObject.ReasonCode;

                            if (string.IsNullOrEmpty(ReasonCode))
                            {
                                //  FunObj.depAmtResp.depositAmount = 0;
                                if (enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y"))
                                {
                                    ret_str = clsDtLayer.ExecuteWebService(ser_str, "/SubmitEnrollInfoWithoutPayment");
                                }
                                else
                                {
                                    //FunObj.depAmtResp.depositAmount = 0;

                                    if (FunObj.depAmtResp.depositAmount > 0)
                                    {
                                        ret_str = "Enrollment with Deposit";
                                        clsDtLayer.InsertMileStoneDetails("ContinueWithDeposit", Convert.ToString(FunObj.depAmtResp.depositAmount));
                                    }

                                    else
                                    {
                                        clsDtLayer.InsertMileStoneDetails("ContinueWithoutDeposit");
                                        ret_str = clsDtLayer.ExecuteWebService(ser_str, "/SubmitEnrollInfoWithoutPayment");
                                    }
                                }
                            }
                            else
                            {
                                PendingEnrollmentRequest objPendingEnrollmentRequest = new PendingEnrollmentRequest();
                                try
                                {
                                    clsDtLayer.InsertMileStoneDetails("PendingEnroll");
                                    enrollmentObject.EnrollmentType = "REGULAR";
                                    objPendingEnrollmentRequest.Sent_XML = FunObj;
                                    objPendingEnrollmentRequest.UserName = FunObj.SalespersonCode;
                                    objPendingEnrollmentRequest.PhoneNumber = FunObj.contactInfo.mobile_no;
                                    if ((!string.IsNullOrEmpty(enrollmentObject.ReasonCode)) && enrollmentObject.ReasonCode.ToString().ToLower() == "reject")
                                    {
                                        TempData["Reject"] = "Yes";
                                        objPendingEnrollmentRequest.Status = "REJECT";
                                    }
                                    else
                                    {
                                        TempData["Reject"] = "No";
                                        objPendingEnrollmentRequest.Status = "PENDING";

                                    }
                                    objPendingEnrollmentRequest.ReasonCode = ReasonCode;
                                    objPendingEnrollmentRequest.Source = ConfigurationManager.AppSettings["source"].ToString();
                                    objPendingEnrollmentRequest.CallerId = "";
                                    objPendingEnrollmentRequest.BrandCode = FunObj.BrandCode;
                                    objPendingEnrollmentRequest.IdentityNumber = custEnrollInfo.Govt_ID_Number;
                                    objPendingEnrollmentRequest.IdentityState = custEnrollInfo.Govt_ID_State;
                                    objPendingEnrollmentRequest.IdentityType = custEnrollInfo.Govt_Id_type;
                                    objPendingEnrollmentRequest.Sent_XML.EnrollmentType = enrollmentObject.EnrollmentType;
                                }
                                catch (Exception ex)
                                {
                                    revertMsg = "Problem at data";
                                }
                                try
                                {
                                    ret_str = clsDtLayer.ExecuteWebService(s.Serialize(objPendingEnrollmentRequest), "/SubmitPendingEnrollInfo");
                                }
                                catch (Exception ex)
                                {
                                    if (!(ex is System.Threading.ThreadAbortException))
                                        clsDtLayer.WriteErrorLog("Enroll", ex, -11, "FunObj: " + clsDtLayer.ObjectToJson(FunObj) + ", bReq: " + clsDtLayer.ObjectToJson(bReq), clsDtLayer.ObjectToJson(enrollmentResp));
                                    revertMsg = "Problem at submit";
                                }
                                WebEnrollmentNewDesign.Models.FUNBaseResponse resp = s.Deserialize<WebEnrollmentNewDesign.Models.FUNBaseResponse>(ret_str);

                                if (resp.requestStatus != 0 && resp.requestStatus != -1)
                                {
                                    revertMsg = string.Format((String)HttpContext.GetGlobalResourceObject("MyFunResources", "PendnfEnroll_Mes"), "P_" + resp.requestStatus + "");
                                    TempData["PendingMsg"] = revertMsg;
                                }
                            }
                            if (ret_str != "Enrollment with Deposit")
                            {
                                enrollmentResp = s.Deserialize<EnrollResult>(ret_str);  //clsDtLayer.EnrollCustomer(retXml);
                                Session["enrollmentResp"] = enrollmentResp;
                            }
                        }
                    }

                }
                else
                {
                    revertMsg = "Enrollment Timeout!. Error While enrolling, Please try again";
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                    clsDtLayer.WriteErrorLog("Enroll", ex, -11, "FunObj: " + clsDtLayer.ObjectToJson(FunObj) + ", bReq: " + clsDtLayer.ObjectToJson(bReq), clsDtLayer.ObjectToJson(enrollmentResp));
                revertMsg = "Enrollment Timeout!!. Error While enrolling, Please try again. Reason is" + enrollmentObject.ReasonCode;
            }

            if (revertMsg.Contains("Here is your tracking number"))
            {
                TempData["PendingMsg"] = revertMsg;
            }
            else
            {
                TempData["PendingMsg"] = "";
            }

            return Content(revertMsg);
        }

        private void FillFunEnrollObject(FunEnrollment FunObj, CustomerEnrollInfo customerEnrollInfo)
        {
            WebEnrollmentNewDesign.Models.FUNBaseRequest basereq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
            AppInfo appinfo = new AppInfo();
            appinfo = clsDtLayer.GetAppInfo();
            try
            {
                ProductDetails productDetails = new ProductDetails();
                ServiceAddress serviceAddress = new ServiceAddress();
                serviceAddress = (ServiceAddress)Session["ServAddrObj"];
                productDetails = (ProductDetails)Session["ProductDetails"];
                //   enrollmentObject = (EnrollmentData)Session["PersonalInformation"];
                // enrollmentObject = (EnrollmentData)Session["switchMoveDate"];
                // enrollmentObject =(EnrollmentData)Session["depositAmount"];
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                enrollmentObject.ProdDetails = productDetails;
                FeeInfo feeInfo = new FeeInfo();
                if (enrollmentObject.depAmtResp != null)//&& enrollmentObject.depAmtResp.depositAmount != null)
                {
                    feeInfo.DepositAmount = enrollmentObject.depAmtResp.depositAmount;
                }
                // if (enrollmentObject.paymentAmount != null)
                // {
                feeInfo.PurchasedAmount = enrollmentObject.paymentAmount;
                // }
                enrollmentObject.feeInfo = feeInfo;
                //   if (enrollmentObject.ProdDetails != null && enrollmentObject.serviceAddressObj != null && enrollmentObject.personalInfo != null && enrollmentObject.SwitchMoveResp != null && enrollmentObject.contactInfo != null)
                if (productDetails != null && customerEnrollInfo != null && serviceAddress != null)
                {
                    FunObj.appName = appinfo.appName;
                    FunObj.appVersion = appinfo.appVersion;
                    basereq = clsDtLayer.FillBaseRequestInfo();
                    FunObj.ipAddress = basereq.ipAddress;
                    FunObj.appName = basereq.appName;
                    FunObj.appVersion = basereq.appVersion;
                    FunObj.deviceID = basereq.deviceID;
                    FunObj.dversion = basereq.dversion;
                    //FunObj.ProdDetails = enrollmentObject.ProdDetails;
                    FunObj.ProdDetails = productDetails;
                    //FunObj.serviceAddressObj = enrollmentObject.serviceAddressObj;
                    FunObj.serviceAddressObj = serviceAddress;
                    FunObj.dwelling_type = TempData["Dwelling_Type"].ToString();
                    FunObj.personalInfo = enrollmentObject.personalInfo;
                    FunObj.personalInfo.date_of_birth = FunObj.personalInfo.date_of_birth.Replace("-", "/");
                    if (!string.IsNullOrEmpty(customerEnrollInfo.SSN))
                        customerEnrollInfo.SSN = string.Empty;
                    FunObj.isBillingSame = enrollmentObject.isBillingSame;
                    FunObj.creditCheckYesNo = enrollmentObject.creditCheckYesNo;
                    FunObj.contactInfo = enrollmentObject.contactInfo;
                    FunObj.contactInfo.email = customerEnrollInfo.Customer_EmailId;
                    FunObj.GovermentIdNo = enrollmentObject.GovermentIdNo;
                    //if (customerEnrollInfo.Payment_Type.ToLower() == "credit")
                    //{
                    //    FunObj.creditCardInfo = enrollmentObject.creditCardInfo;
                    //}
                    //else
                    //{
                    //    //    FunObj.creditCardInfo.CardName = "";
                    //    //    FunObj.creditCardInfo.CardNumber = "";
                    //    //    FunObj.creditCardInfo.CardType = "";
                    //    //    FunObj.creditCardInfo.Expiry = "";
                    //}
                    //     Need to check
                    if (enrollmentObject.depAmtResp != null)
                        FunObj.depAmtResp = enrollmentObject.depAmtResp;
                    if (enrollmentObject.enrRes != null)
                        FunObj.enrRes = enrollmentObject.enrRes;
                    if (enrollmentObject.feeInfo != null)
                        FunObj.feeInfo = enrollmentObject.feeInfo;
                    FunObj.SwitchMoveResp = enrollmentObject.SwitchMoveResp;

                    // FunObj.SwitchMoveResp.SwitchMoveType = enrollmentObject.SwitchMoveResp.SwitchMoveType.ToUpper().Equals("Y") ? "SWITCH" : "MOVEIN";
                    FunObj.SwitchMoveResp.SwitchMoveType = customerEnrollInfo.Switching_Moving;
                    FunObj.SwitchMoveResp.SelectedSwitchMoveDate = customerEnrollInfo.Move_Or_Switch_Date.ToString("MM-dd-yyyy");
                    FunObj.SwitchMoveResp.SelectedSwitchMoveDate = FunObj.SwitchMoveResp.SelectedSwitchMoveDate.Replace("-", "/");
                    //FunObj.paymentType = enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y") ? enrollmentObject.paymentType : "Y";
                    FunObj.paymentType = customerEnrollInfo.Payment_Type;
                    FunObj.AutoPaySelected = customerEnrollInfo.AutoPay;

                    //  FunObj.creditCheckAgreed = TempData["CreditCheck"].ToString();
                    //FunObj.creditCheckAgreed =
                    FunObj.creditCheckAgreed = enrollmentObject.creditCheckAgreed;

                    FunObj.EnrollmentType = enrollmentObject.EnrollmentType;
                    FunObj.InternalRuleMes = enrollmentObject.InternalRuleMes;
                    FunObj.IsTCPAAgreed = enrollmentObject.IsTCPAAgreed;
                    //FunObj.isPreviousAddress = enrollmentObject.isPreviousAddress;
                    //if (enrollmentObject.previousAddress != null)
                    //    FunObj.previousAddress = enrollmentObject.previousAddress;


                    // FunObj.Note = productDetails.ProductNote;
                    FunObj.Source = ConfigurationManager.AppSettings["source"].ToString();
                    FunObj.billingAddress = enrollmentObject.billingAddress;
                    FunObj.IsPaperless = enrollmentObject.IsPaperless;
                    FunObj.AuthrizedUserFirstName = enrollmentObject.AuthrizedUserFirstName;
                    FunObj.AuthrizeduserLastName = enrollmentObject.AuthrizeduserLastName;
                    FunObj.AuthrizeduserContactNumber = enrollmentObject.AuthrizeduserContactNumber;
                    FunObj.BrandCode = enrollmentObject.BrandCode.ToUpper();
                    FunObj.TCPAPhoneType = enrollmentObject.TCPAPhoneType;
                    if ((!string.IsNullOrEmpty(enrollmentObject.Close_Existing_YN)) && enrollmentObject.Close_Existing_YN.ToLower() == "y")
                    {
                        FunObj.Existing_Cust_No = enrollmentObject.Existing_Cust_No;
                        FunObj.Existing_Cust_Account_id = enrollmentObject.Existing_Cust_Account_id;
                    }
                    else
                    {
                        FunObj.Existing_Cust_No = "0";
                        FunObj.Existing_Cust_Account_id = "0";
                    }
                    FunObj.Close_Existing_YN = enrollmentObject.Close_Existing_YN;
                    // FunObj.contactInfo.eBill = enrollmentObject.contactInfo.eBill;
                    var s = new JavaScriptSerializer();
                    WebEnrollmentNewDesign.Models.FUNBaseRequest BaseReq = clsDtLayer.FillBaseRequestInfo();
                    BondReq req = new BondReq
                    {
                        appName = BaseReq.appName,
                        appVersion = BaseReq.appVersion,
                        dversion = BaseReq.dversion,
                        ipAddress = BaseReq.ipAddress,
                        sessionID = BaseReq.sessionID,
                        TDSP = productDetails.TDSP_Code,
                        product_id = Convert.ToInt32(productDetails.Product_ID)
                    };

                    string scr_str = s.Serialize(req);
                    var resp1 = clsDtLayer.ExecuteWebService(scr_str, "/GetBondSelectionCode");
                    var testi1 = s.Deserialize<BondResp>(resp1);
                    if (testi1 != null && testi1.Bondrules != null && testi1.Bondrules.Count > 0)
                    {
                        FunObj.bond_selection_rule_code = testi1.Bondrules[0].bond_selection_rule_code;
                    }

                    //FunObj.bond_selection_rule_code
                    string PreyPayYn = Session["PrePayYN"].ToString();
                    if (PreyPayYn.ToUpper().Equals("Y"))
                    {
                        FunObj.purchasedkWh = Convert.ToDecimal(productDetails.EFLkwh_val);
                        FunObj.purchasedAmount = Convert.ToDecimal(productDetails.EFL_Rate);

                    }
                    try
                    {
                        FunObj.Ref_Code = enrollmentObject.Ref_Code;
                        FunObj.creditcheck_status_id = enrollmentObject.creditcheck_status_id;
                    }
                    catch (Exception exp)
                    {

                    }
                    if (enrollmentObject.QueryString != null)
                    {
                        FunObj.QueryString = Server.HtmlDecode(enrollmentObject.QueryString);
                    }
                    if (enrollmentObject.CsReqData != null)
                    {
                        enrollmentObject.CsReqData.ssn = "";
                        FunObj.CsReqData = enrollmentObject.CsReqData;
                    }
                    if (enrollmentObject.previousAddress != null)
                    {
                        FunObj.MoveInAddress = enrollmentObject.previousAddress;
                    }
                    Session["FunObj"] = FunObj;
                }
                //  else
                //   SessionExpired();
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrorLog("FillEnrollObject", ex, -11, "FunEnrollment: " + clsDtLayer.ObjectToJson(FunObj) + " FUNBaseRequest: " + clsDtLayer.ObjectToJson(basereq) + " AppInfo: " + clsDtLayer.ObjectToJson(appinfo), "");
                //   SessionExpired();

            }
        }

        [System.Web.Services.WebMethod()]
        public static bool Validatepromocode(string promo)
        {
            clsDataLayer clsDtLayer = new clsDataLayer();
            try
            {
                var s = new JavaScriptSerializer();
                string jsonPromoRequest = s.Serialize(promo);
                bool result = Convert.ToBoolean(clsDtLayer.ExecuteWebService(jsonPromoRequest, "/ValidatePromo"));
                return result;
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("Validatepromocode::Validate", ex, -11);
            }
            return true;
        }

        //***********Getting ProductDetail In TAG******************
        [HttpPost]
        public JsonResult GetProductDetail(string tabId, string kwh_Choice, string firstLoad = "N")
        {

            string[] ReturnValue = new string[11];
            try
            {
                string box = "";
                if (Session["kwhValueforBack"] != null && Session["kwhValueforBack"] != "")
                {
                    kwh_Choice = Session["kwhValueforBack"].ToString();
                    Session["kwhValueforBack"] = "";
                }
                TempData["kwh_Choice"] = kwh_Choice;
                TempData.Keep();
                TempData["tabId"] = tabId;
                ProductDetailReq ProdDetReq = new ProductDetailReq();

                List<ProductDetails> PRodList = new List<ProductDetails>();
                List<ProductDetails> lstProdDetails = new List<ProductDetails>();
                ProductDetailsListResp ProdDetResp = new ProductDetailsListResp();
                TabDetailsResp tabresp = new TabDetailsResp();
                var s = new JavaScriptSerializer();

                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (enrollmentObject != null)
                {
                    ProdDetReq.BrandCode = enrollmentObject.BrandCode;
                    if (Session["IsPartner"] != null)
                    {
                        ReturnValue[9] = Session["IsPartner"].ToString();
                        ReturnValue[10] = Session["QueryString"].ToString();
                    }
                    else
                    {
                        ReturnValue[9] = "";
                    }
                    string requestPage = "";
                    if (!(string.IsNullOrEmpty(Session["RequestPage"].ToString())))
                    {
                        requestPage = Session["RequestPage"].ToString();
                    }
                    if (enrollmentObject.ZipResp != null)
                    {
                        string jsonAccReq = "";
                        string resp = "";
                        querStringObj = (QueryStringObject)Session["queryStringObj"];
                        string prodcode = "";
                        if (!string.IsNullOrEmpty(querStringObj.ProdCode))
                        {
                            //prodcode = HttpUtility.UrlDecode(querStringObj.ProdCode);
                            prodcode = querStringObj.ProdCode;
                        }

                        if (requestPage != "")
                        {
                            PartnerDetailsResp partnerresp = new PartnerDetailsResp();
                            querStringObj = (QueryStringObject)Session["queryStringObj"];

                            string ref_id = querStringObj.ref_id;
                            if (Session["PartnerDetailResp"] != null)
                            {
                                partnerresp = (PartnerDetailsResp)Session["PartnerDetailResp"];
                                ProdDetReq.TabID = partnerresp.TabID;

                                ProdDetReq.TDSP_Code = enrollmentObject.ZipResp.TDSPCode;
                                ProdDetReq.BrandCode = enrollmentObject.BrandCode;
                                if (string.IsNullOrEmpty(querStringObj.kWh) == false)
                                {
                                    int i = 0;
                                    bool result = int.TryParse(querStringObj.kWh, out i);
                                    if (result == true)
                                    {
                                        ProdDetReq.EFLKWH = Convert.ToInt32(querStringObj.kWh);
                                    }
                                    else
                                    {
                                        ProdDetReq.EFLKWH = Convert.ToInt32(kwh_Choice);
                                    }
                                    if (Session["UL"] != null && Session["UL"] != "" && Session["UL"] != "NA")
                                    {
                                        kwh_Choice = querStringObj.kWh;
                                        Session["UL"] = "NA";
                                        TempData["kwh_Choice"] = kwh_Choice;
                                    }
                                }

                                jsonAccReq = s.Serialize(ProdDetReq);
                                resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetProductWithEFLKWH"); // Get Products for Partner Tab 

                                ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);
                            }
                            PRodList = (ProdDetResp.productDetailsList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();


                            if (tabId == "Display All")
                            {
                                PRodList = (ProdDetResp.productDetailsList.Where(o => o.EFLkwh_val == kwh_Choice && o.DisplayTab != "Recommended")).ToList<ProductDetails>();
                            }

                            else if (tabId == "Recommended")
                            {
                                if (Session["enrollmentObject"] != null && enrollmentObject.serviceAddressObj != null && enrollmentObject.serviceAddressObj.AM_YN.Equals("N"))
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.Prepay_YN == "N" && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                                else
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();

                                    if (PRodList.Count == 0)
                                    {
                                        PRodList = (ProdDetResp.productDetailsList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                    }
                                }
                            }
                            else
                            {
                                if (Session["enrollmentObject"] != null && enrollmentObject.serviceAddressObj != null && enrollmentObject.serviceAddressObj.AM_YN.Equals("N"))
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.Prepay_YN == "N" && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                                else
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                            }

                            if (prodcode != "")
                            {
                                prodcode = System.Web.HttpUtility.UrlDecode(prodcode);
                                bool checkProduct = ProdDetResp.productDetailsList.Any(o => o.Product_Code == prodcode);
                                if (checkProduct == true)
                                {
                                    if (tabId == "Display All" || tabId == "Recommended")
                                    {
                                        PRodList = (ProdDetResp.productDetailsList.Where(o => o.Product_Code == prodcode && o.EFLkwh_val == kwh_Choice)).Take(1).ToList<ProductDetails>();
                                    }
                                    else
                                    {
                                        PRodList = (ProdDetResp.productDetailsList.Where(o => o.Product_Code == prodcode && o.DisplayTab == tabId && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                        //PRodList = (ProdDetResp.productDetailsList.Where(o => o.Product_Code == prodcode &&  o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                    }
                                }
                                else
                                {
                                    // PRodList = (ProdDetResp.productDetailsList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                    PRodList = null;
                                }
                            }
                            if (tabId != "" && tabId != "Display All" && tabId != "Recommended" && PRodList != null && PRodList.Count > 0)
                            {
                                PRodList = (PRodList.Where(o => o.DisplayTab == tabId)).ToList<ProductDetails>();
                            }

                            //if (tabId == "Recommended")
                            //{
                            //    if (PRodList.Count > 3)
                            //    {
                            //        PRodList = (PRodList.Where(o => o.SortOrder < 4)).Take(3).ToList<ProductDetails>();
                            //    }
                            //    else
                            //    {
                            //        PRodList = PRodList.ToList<ProductDetails>();
                            //    }
                            //}

                            ProdDetReq.TabID = partnerresp.TabID;
                            ProdDetReq.TabName = partnerresp.TabName;
                            ProdDetReq.BrandCode = enrollmentObject.BrandCode;
                            jsonAccReq = s.Serialize(ProdDetReq);
                            //  string resp2 = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetActiveTabsByTabId"); // Get Tabs for Partner
                            // string resp2 = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetActiveTabsByTabName"); // Get Tabs for Partner
                            //string resp2 = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetActiveTabsForPartner"); // Get Tabs for Partner
                            //if (resp2 != null)
                            //{
                            //    tabresp = s.Deserialize<TabDetailsResp>(resp2);
                            //}

                            //if (tabresp.lstTabs != null && tabresp.lstTabs.Count > 0)
                            //{
                            //    ReturnValue[0] = tabresp.lstTabs[0].TabHeader;
                            //    ReturnValue[1] = tabresp.lstTabs[0].TabDetail;
                            //    // ReturnValue[0] = ProdDetResp.tabList[0].TabHeader;
                            //    //   ReturnValue[1] = ProdDetResp.tabList[0].TabDetail;
                            //    ReturnValue[4] = tabresp.lstTabs[0].TabFooter;
                            //    TempData["tabFooter"] = ReturnValue[4];
                            //}
                            //if (string.IsNullOrEmpty(ReturnValue[4]))
                            //{
                            //  if (!(string.IsNullOrEmpty(partnerresp.TabHeader)))
                            //{
                            ReturnValue[0] = partnerresp.TabHeader;
                            ReturnValue[1] = partnerresp.TabDetail;
                            ReturnValue[4] = partnerresp.TabFooter;
                            TempData["tabFooter"] = ReturnValue[4];
                            //}

                            //else
                            //{
                            //ProdDetReq.TabName = null;
                            //jsonAccReq = s.Serialize(ProdDetReq);
                            //string resp3 = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetActiveTabsByTabName");
                            //if (resp3 != null)
                            //{
                            //    tabresp = s.Deserialize<TabDetailsResp>(resp3);
                            //    if (tabresp.lstTabs != null && tabresp.lstTabs.Count > 0)
                            //    {
                            //        ReturnValue[0] = tabresp.lstTabs[0].TabHeader;
                            //        ReturnValue[1] = tabresp.lstTabs[0].TabDetail;
                            //        ReturnValue[4] = tabresp.lstTabs[0].TabFooter;
                            //    }
                            //}
                            //  }
                            //}
                        }
                        else
                        {
                            ProdDetReq.TDSP_Code = enrollmentObject.ZipResp.TDSPCode;
                            if (prodcode != "")
                            {
                                ProdDetReq.ProductCode = System.Web.HttpUtility.UrlDecode(prodcode);

                                jsonAccReq = s.Serialize(ProdDetReq);

                                resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetProductByCode");
                            }
                            else
                            {
                                ProdDetReq.DisplayTab = tabId;
                                //  ProdDetReq.TabName = tabId;
                                jsonAccReq = s.Serialize(ProdDetReq);

                                resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetAllProducts_AccordingEFL");
                            }
                            if (resp != null)
                            {
                                ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);
                            }

                            if (tabId == "Display All" || tabId == "Recommended")
                            {
                                if (Session["enrollmentObject"] != null && enrollmentObject.serviceAddressObj != null && enrollmentObject.serviceAddressObj.AM_YN.Equals("N"))
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.Prepay_YN == "N" && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                                else
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                            }
                            else
                            {
                                if (Session["enrollmentObject"] != null && enrollmentObject.serviceAddressObj != null && enrollmentObject.serviceAddressObj.AM_YN.Equals("N"))
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.Prepay_YN == "N" && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                                else
                                {
                                    PRodList = (ProdDetResp.productDetailsList.Where(o => o.DisplayTab == tabId && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                                }
                            }
                            //bool checkProduct = ProdDetResp.productDetailsList.Any(o => o.Product_Code == prodcode);
                            //if (checkProduct == true)
                            //{
                            //    PRodList = (ProdDetResp.productDetailsList.Where(o => o.Product_Code == prodcode && o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                            //}
                            //else
                            //{
                            //    PRodList = (ProdDetResp.productDetailsList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList<ProductDetails>();
                            //}
                            if (ProdDetResp.tabList != null)
                            {
                                if (ProdDetResp.tabList.Count > 0)
                                {
                                    ReturnValue[0] = ProdDetResp.tabList[0].TabHeader;
                                    ReturnValue[1] = ProdDetResp.tabList[0].TabDetail;
                                    ReturnValue[4] = ProdDetResp.tabList[0].TabFooter;
                                    ReturnValue[6] = ProdDetResp.tabList[0].TabImage;
                                    TempData["tabFooter"] = ReturnValue[4];
                                }
                                else
                                {
                                    ReturnValue[0] = "";
                                    ReturnValue[1] = "";
                                    ReturnValue[4] = "";
                                    ReturnValue[6] = "";
                                    TempData["tabFooter"] = "";
                                }

                            }
                        }
                        if (PRodList != null)
                        {
                            if (PRodList.Count > 0)
                            {

                                if (PRodList.Count > 4)
                                {
                                    box += "<div class='container-fluid p-0' style='background-color:#efefef;'>";


                                    box += "<div class='container pb-5 p-0'>";
                                    box += "<div style='background-color:white;color:#212529;'>";
                                    box += "<div class='row  text-center cart-header' >";
                                    box += "<div class='col-lg-4'>";
                                    box += "<h5>Plan Name and Details</h5>";
                                    box += "</div>";

                                    box += "<div class='col-lg-4'>";
                                    box += "<h5>Price per kWh</h5>";
                                    box += "</div>";
                                    box += "<div class='col-lg-4'>";
                                    box += "<h5>Plan Features</h5>";
                                    box += "</div>";
                                    box += "</div>";
                                    for (int i = 0; i <= PRodList.Count - 1; i++)
                                    {
                                        enrollmentObject.ProdDetails = PRodList[i];
                                        TempData["split_deposit_yn"] = PRodList[i].split_deposit_yn.ToString();
                                        TempData["pPid"] = PRodList[i].Price_Plan_ID;
                                        TempData["ProductId"] = PRodList[i].Product_ID;
                                        string PricePlanId = TempData["pPid"].ToString();
                                        string ProductId = TempData["ProductId"].ToString();
                                        TempData.Keep();
                                        TempData["TabName"] = tabId;

                                        box += "<div class='row  text-center' style='border-top:1px solid lightgrey;'>";
                                        if (PRodList[i].IsFeatured)
                                        {
                                            box += "<div class='special_offer_flag green_flag ng-star-inserted'><span class='special_offer_flag_internal'>Featured</span></div>";
                                        }
                                        box += "<div class='col-lg-4 col-12 pt-3 p-0'>";
                                        box += "<h3>" + PRodList[i].ProductTitle + "</h3>";
                                        box += "<strong style='height: 30px;font-weight:400;color:#212529;display:block'>" + PRodList[i].ProductHeader + "</strong>";
                                        // box += "<span class='learn_more_item_text' onclick='showTabsDetails(" + PRodList[i].Price_Plan_ID + "," + tabId + "," + kwh_Choice + ")' data-toggle='modal' data-target='#ProductDetails' data-backdrop='static' data-keyboard='false'>Learn More</span>";
                                        box += "<span class='learn_more_item_text' onclick='showTabsDetails(" + PRodList[i].Price_Plan_ID + "," + kwh_Choice + ")' data-toggle='modal' data-target='#ProductDetails' data-backdrop='static' data-keyboard='false'>Learn More</span>";
                                        if (!string.IsNullOrEmpty(PRodList[i].Feature_Image) && DisplayFeatureImage == "1")
                                        {
                                            box += "<p><img src='" + imagepath + PRodList[i].Feature_Image + "' alt='Feature Image' runat='server' /></p>";
                                        }
                                        box += "</div>";
                                        box += "<div class='col-lg-4 p-0 pt-5'>";
                                        box += "<div class='plan_price_element  col-lg-6 col-12 p-0'>";
                                        box += "<!----><span class='plan_price_value ng-star-inserted'>";
                                        string Cost = string.IsNullOrEmpty(PRodList[i].EFL_Rate) ? string.Empty : (decimal.Parse(PRodList[i].EFL_Rate)).ToString("#.0");
                                        box += Cost;
                                        box += "</span>";
                                        box += "<span class='plan_price_symbol'>¢ </span>";
                                        box += "<span class='plan_price_measure'>&nbsp;/kWh</span>";
                                        box += "</div>";
                                        box += "<div class='pricing_plan_button_container col-lg-6 col-12 p-0 '>";
                                        // box += "<button class='button solid_orange plan_order_button' data-toggle='modal' data-target='#myAddressModal' data-backdrop='static' data-keyboard='false'>Order Now</button>";
                                        box += "<button class='button solid_orange plan_order_button'  onclick='CheckSelectedAddress(" + PricePlanId + "," + ProductId + ")'>Order Now</button>";

                                        box += "</div>";
                                        box += "</div>";
                                        box += "<div class='col-lg-4 p-0 pt-4'>";
                                        box += "<ul class='text-center'>";
                                        //if (string.IsNullOrEmpty(PRodList[i].Disclaimer))
                                        //{
                                        //    box += PRodList[i].ProductDesc3;
                                        //}
                                        //else
                                        //{
                                        //    box += PRodList[i].Disclaimer;
                                        //}
                                        box += PRodList[i].ProductDesc3;
                                        box += "</ul>";
                                        box += "</div>";
                                        box += "</div>";
                                    }
                                    box += "</div>";
                                    box += "</div>";
                                    box += "</div>";
                                }
                                else
                                {
                                    box += "<div class='row' style='justify-Content:center;'>";
                                    for (int i = 0; i <= PRodList.Count - 1; i++)
                                    {

                                        enrollmentObject.ProdDetails = PRodList[i];
                                        TempData["split_deposit_yn"] = PRodList[i].split_deposit_yn.ToString();
                                        TempData["pPid"] = PRodList[i].Price_Plan_ID;
                                        TempData["ProductId"] = PRodList[i].Product_ID;
                                        TempData["TabName"] = tabId;
                                        string PricePlanId = TempData["pPid"].ToString();
                                        string ProductId = TempData["ProductId"].ToString();
                                        TempData.Keep();
                                        box += "<div class='col-xl-3 col-lg-3 col-md-6 col-12 p-0' style='display:flex;'>";
                                        box += "<div class='plan_card ng-star-inserted' style='width:100% !important;display:flex'>";
                                        box += "<div class='pricing_plan_item plan_card ng-star-inserted' style='Width:100%!important'>";
                                        box += "<div class='plan_head_text ng-star-inserted'>";
                                        if (PRodList[i].IsFeatured)
                                        {
                                            box += "<div class='special_offer_flag green_flag ng-star-inserted'><span class='special_offer_flag_internal'>Featured</span></div>";
                                        }
                                        box += "<h3>" + PRodList[i].ProductTitle + "</h3>";
                                        box += "<strong class='plan_details ng-star-inserted'>";
                                        box += PRodList[i].ProductHeader;
                                        box += "</strong>";
                                        box += "</div>";
                                        if (!string.IsNullOrEmpty(PRodList[i].Feature_Image) && DisplayFeatureImage == "1")
                                        {
                                            box += "<p><img src='" + imagepath + PRodList[i].Feature_Image + "' alt='Feature Image' /></p>";
                                        }
                                        box += "<div class='plan_price_element'>";
                                        box += "<span class='plan_price_value ng-star-inserted'>";

                                        string Cost = string.IsNullOrEmpty(PRodList[i].EFL_Rate) ? string.Empty : (decimal.Parse(PRodList[i].EFL_Rate)).ToString("#.0");
                                        box += Cost;
                                        box += "</span>";

                                        box += "<span class='plan_price_symbol'>¢ </span>";
                                        box += "<span class='plan_price_measure'> &nbsp; /kWh</span>";
                                        box += "</div>";
                                        box += "<div class='pricing_plan_button_container'>";
                                        box += "<button class='button solid_orange plan_order_button'  onclick='CheckSelectedAddress(" + PricePlanId + "," + ProductId + ")'>Order Now</button>";
                                        box += "</div>";
                                        box += "<ul class='plan_features_list plan_top_features ng-star-inserted'>";
                                        box += PRodList[i].ProductDesc3;
                                        //if (string.IsNullOrEmpty(PRodList[i].Disclaimer))
                                        //{
                                        //    box += PRodList[i].ProductDesc3;
                                        //}
                                        //else
                                        //{
                                        //    box += PRodList[i].Disclaimer;
                                        //}
                                        box += "</ul>";
                                        box += "<div class='learn_more'>";
                                        box += "<span class='learn_more_item_text' onclick='showTabsDetails(" + PRodList[i].Price_Plan_ID + "," + kwh_Choice + ")' data-toggle='modal' data-target='#ProductDetails' data-backdrop='static' data-keyboard='false'>Learn More</span>";
                                        TempData["ProductId"] = PRodList[i].Product_ID;
                                        TempData.Keep();
                                        box += "</div>";
                                        box += "</div>";
                                        box += "</div>";
                                        box += "</div>";
                                    }
                                    box += "</div>";
                                }
                            }
                            else
                            {
                                box += "<div class='container-fluid p-0' style='background-color:#efefef;'>";
                                box += "<div class='container pb-5 p-0'>";
                                box += "<div style='background-color:white;color:#212529;'>";
                                box += "<div class='col-12' style='text-align:center;padding: 1px 2px 12px 2px;' >";
                                box += "<h2>Currently there is no product on selected zipcode.</h2>";
                                box += "</div>";
                                box += "</div>";
                                box += "</div>";
                                box += "</div>";
                            }
                        }
                        else
                        {
                            box += "<div class='container-fluid p-0' style='background-color:#efefef;'>";
                            box += "<div class='container pb-5 p-0'>";
                            box += "<div style='background-color:white;color:#212529;'>";
                            box += "<div class='col-12' style='text-align:center;padding: 1px 2px 12px 2px;' >";
                            box += "<h2>Currently there is no product on selected zipcode.</h2>";
                            box += "</div>";
                            box += "</div>";
                            box += "</div>";
                            box += "</div>";
                        }
                        ReturnValue[7] = "Yes";
                    }
                    else
                    {
                        ReturnValue[7] = "No";
                        TempData["GetZIP"] = "GetZip";
                        //ProdDetReq.TDSP_Code = "CNP"; //zipResp.TDSPCode;
                        box += "<div class='container-fluid p-0' style='background-color:#efefef;'>";
                        box += "<div class='container pb-5 p-0'>";
                        box += "<div style='background-color:white;color:#212529;'>";
                        box += "<div class='row  text-center'   >";
                        if (firstLoad.ToLower() == "y")
                        {
                            box += "<h2></h2>";
                        }
                        else
                        {
                            box += "<h2>We are not serving this territory at this time.</h2>";
                        }
                        box += "</div>";
                        box += "</div>";
                        box += "</div>";
                        box += "</div>";
                    }
                    Session["enrollmentObject"] = enrollmentObject;
                    ReturnValue[2] = box;
                    if (enrollmentObject.ZipResp != null)
                    {
                        ReturnValue[3] = enrollmentObject.ZipResp.ZipCode;
                        ReturnValue[5] = enrollmentObject.ZipResp.TDSPName;
                        ReturnValue[8] = enrollmentObject.ZipResp.TDSPCode;
                    }
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("GetProductDetail::ProductDetails", Ex, -11);
            }
            return Json(ReturnValue, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProductDetail_Popup(string pPId, string tabId, string kwh_Choice)
        {
            string box = "";
            try
            {
                if (TempData["kwh_Choice"] != null)
                {
                    kwh_Choice = TempData["kwh_Choice"].ToString();
                    TempData.Keep();
                }
                TempData["pPid"] = pPId;
                if (tabId == "")
                {
                    // tabId = TempData["tabId"].ToString();
                    if (TempData["DefaultTabId"] != null)
                    {
                        tabId = TempData["DefaultTabId"].ToString();
                    }
                    TempData.Keep();
                }
                else
                {
                    TempData["tabId"] = tabId;
                }

                List<ProductDetails> PRodList = new List<ProductDetails>();
                List<ProductDetails> lstProdDetails = new List<ProductDetails>();
                ProductDetailsListResp ProdDetResp = new ProductDetailsListResp();
                var s = new JavaScriptSerializer();
                ProductDetailReq ProdDetReq = new ProductDetailReq();

                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (enrollmentObject != null)
                {
                    if (enrollmentObject.ZipResp != null)
                    {
                        ProdDetReq.BrandCode = enrollmentObject.BrandCode;
                        if (TempData["TDSPAccordAdd"] != null)
                        {
                            string tdsp = TempData["TDSPAccordAdd"].ToString();
                            TempData.Keep();
                            ProdDetReq.TDSP_Code = tdsp;
                            enrollmentObject.ZipResp.TDSPCode = tdsp;
                        }
                        else
                        {
                            ProdDetReq.TDSP_Code = enrollmentObject.ZipResp.TDSPCode;
                        }
                        PartnerDetailsResp partnerresp = new PartnerDetailsResp();
                        if (Session["PartnerDetailResp"] != null)
                        {
                            partnerresp = (PartnerDetailsResp)Session["PartnerDetailResp"];
                            ProdDetReq.TabID = partnerresp.TabID;
                        }
                        string jsonAccReq = s.Serialize(ProdDetReq);
                        string resp = "";
                        dynamic ProdDetList = "";
                        dynamic ProdDetList2 = "";
                        dynamic ProdDetList3 = "";
                        dynamic ProdDetList4 = "";
                        string requestPage = "";
                        if (Session["RequestPage"] != null)
                        {
                            requestPage = Session["RequestPage"].ToString();
                        }
                        if (requestPage != "")
                        {
                            resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetProductWithEFLKWH");
                            ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);
                            List<ProductDetails> lstProductDetails = ProdDetResp.productDetailsList;
                            if (lstProductDetails != null)
                            {
                                if (lstProductDetails.Count > 0)
                                {
                                    var PD = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                                    var PD2 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == "500")).ToList();
                                    var PD3 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == "1000")).ToList();
                                    var PD4 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == "2000")).ToList();
                                    if (PD != null && PD.Count > 0)
                                    {
                                        ProdDetList = PD;
                                    }
                                    else
                                    {
                                        ProdDetList = null;
                                    }
                                    if (PD2 != null && PD2.Count > 0)
                                    {
                                        ProdDetList2 = PD2;
                                    }
                                    else
                                    {
                                        ProdDetList2 = null;
                                    }
                                    if (PD3 != null && PD3.Count > 0)
                                    {
                                        ProdDetList3 = PD3;
                                    }
                                    else
                                    {
                                        ProdDetList3 = null;
                                    }
                                    if (PD4 != null && PD4.Count > 0)
                                    {
                                        ProdDetList4 = PD4;
                                    }
                                    else
                                    {
                                        ProdDetList4 = null;
                                    }
                                }
                            }
                        }
                        else
                        {
                            resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetAllProducts_AccordingEFL");

                            ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);

                            List<ProductDetails> lstProductDetails = ProdDetResp.productDetailsList;
                            if (lstProductDetails != null && lstProductDetails.Count > 0)
                            {
                                if (tabId == "Display All" || tabId == "Recommended")
                                {
                                    var PD = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                                    var PD2 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == "500")).ToList();
                                    var PD3 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == "1000")).ToList();
                                    var PD4 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == "2000")).ToList();
                                    if (PD != null && PD.Count > 0)
                                    {
                                        ProdDetList = PD;
                                    }
                                    else
                                    {
                                        ProdDetList = null;
                                    }
                                    if (PD2 != null && PD2.Count > 0)
                                    {
                                        ProdDetList2 = PD2;
                                    }
                                    else
                                    {
                                        ProdDetList2 = null;
                                    }
                                    if (PD3 != null && PD3.Count > 0)
                                    {
                                        ProdDetList3 = PD3;
                                    }
                                    else
                                    {
                                        ProdDetList3 = null;
                                    }
                                    if (PD4 != null && PD4.Count > 0)
                                    {
                                        ProdDetList4 = PD4;
                                    }
                                    else
                                    {
                                        ProdDetList4 = null;
                                    }
                                }
                                else
                                {
                                    var PD = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.DisplayTab == tabId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                                    var PD2 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.DisplayTab == tabId && o.EFLkwh_val == "500")).ToList();
                                    var PD3 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.DisplayTab == tabId && o.EFLkwh_val == "1000")).ToList();


                                    var PD4 = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.DisplayTab == tabId && o.EFLkwh_val == "2000")).ToList();

                                    if (PD != null && PD.Count > 0)
                                    {
                                        ProdDetList = PD;
                                    }
                                    else
                                    {
                                        ProdDetList = null;
                                    }
                                    if (PD2 != null && PD2.Count > 0)
                                    {
                                        ProdDetList2 = PD2;
                                    }
                                    else
                                    {
                                        ProdDetList2 = null;
                                    }
                                    if (PD3 != null && PD3.Count > 0)
                                    {
                                        ProdDetList3 = PD3;
                                    }
                                    else
                                    {
                                        ProdDetList3 = null;
                                    }
                                    if (PD4 != null && PD4.Count > 0)
                                    {
                                        ProdDetList4 = PD4;
                                    }
                                    else
                                    {
                                        ProdDetList4 = null;
                                    }

                                }
                            }
                        }
                        if (ProdDetList != null && ProdDetList2 != null && ProdDetList3 != null && ProdDetList4 != null)
                        {
                            ProductDetails prodDetails = ProdDetList[0];

                            string cost500 = "";
                            string cost1000 = "";
                            string cost2000 = "";

                            ProductDetails prodDetails2 = ProdDetList2[0];
                            ProductDetails prodDetails3 = ProdDetList3[0];
                            ProductDetails prodDetails4 = ProdDetList4[0];
                            if (prodDetails2 != null)
                            {
                                cost500 = string.IsNullOrEmpty(ProdDetList2[0].EFL_Rate) ? string.Empty : (Math.Round(decimal.Parse(ProdDetList2[0].EFL_Rate), 1).ToString());
                            }
                            if (prodDetails3 != null)
                            {
                                //cost1000 = string.IsNullOrEmpty(ProdDetList3[0].EFL_Rate) ? string.Empty : (decimal.Parse(ProdDetList3[0].EFL_Rate)).ToString("#.00");
                                cost1000 = string.IsNullOrEmpty(ProdDetList3[0].EFL_Rate) ? string.Empty : (Math.Round(decimal.Parse(ProdDetList3[0].EFL_Rate), 1).ToString());
                            }
                            if (prodDetails4 != null)
                            {
                                cost2000 = string.IsNullOrEmpty(ProdDetList4[0].EFL_Rate) ? string.Empty : (Math.Round(decimal.Parse(ProdDetList4[0].EFL_Rate), 1).ToString());
                            }
                            if (prodDetails != null && ProdDetList != null && ProdDetList.Count > 0)
                            {
                                enrollmentObject.ProdDetails = prodDetails;
                                box += "<script type='text/javascript'>";
                                box += "function showdiv() {";
                                box += " var x = document.getElementById('pCharge');";
                                box += "if (x.style.display === 'none') {";
                                box += "x.style.display = 'block';";
                                box += "}else{";
                                box += "x.style.display = 'none';";
                                box += "}}</script>";

                                box += "<h3 style='font-size:larger;color:black!important;margin-top:0px;'>" + ProdDetList[0].ProductTitle + "</h3>";
                                //box += "<div class='col-lg-4'>";
                                box += "<h5 style='font-size: 18px;color: #ee8d40 ;margin-top: 3%;'>Special Features</h5>";
                                //string pNote = ProdDetList[0].ProductNote;
                                //if (pNote.Contains("<br/>"))
                                //{
                                //    box += ProdDetList[0].ProductNote;
                                //}
                                //else
                                //{
                                //    box += "<ul>";
                                //    box += "<li>" + ProdDetList[0].ProductNote + "</li></ul>";

                                //}
                                //box += "</div>";

                                //box += ProdDetList[0].ProductDesc3;
                                if (string.IsNullOrEmpty(ProdDetList[0].Disclaimer))
                                {
                                    box += ProdDetList[0].ProductDesc3;
                                }
                                else
                                {
                                    box += ProdDetList[0].Disclaimer;
                                }
                                box += "<h5 style='font-size: 18px;color: #ee8d40 ;margin-top: 3%;'>Plan Documents</h5>";
                                string url = string.Empty;
                                try
                                {
                                    url = ConfigurationManager.AppSettings["efl_url"].ToString();

                                    if (!string.IsNullOrEmpty(url))
                                    {
                                        string TDSP_Code = ProdDetReq.TDSP_Code;


                                        box += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=" + Server.UrlEncode(prodDetails.Product_Code) + "&tdspcode=" + TDSP_Code + "' text='Electricity Facts Label'>Electricity Facts Label</a>";
                                        if (ProdDetList[0].Prepay_YN.ToUpper().Equals("Y"))
                                        {

                                            box += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=TOS" + "' text='Terms Of Service'>Terms Of Service</a>";
                                            box += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=YRAC" + "' text='Right as a Customer'>Your Rights as a Customer</a>";

                                        }
                                        else
                                        {

                                            box += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=TOS" + "' text='Terms of Service'>Terms Of Service</a>";
                                            box += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=YRAC" + "' text='Right as a Customer'>Your Rights as a Customer</a>";

                                        }
                                    }
                                    box += "</ul>";
                                    box += "<h5 style='font-size: 18px;color: #ee8d40;margin-top: 3%;'>Plan Details</h5>";
                                    box += "<ul>";
                                    box += "<li class='mb-2'>Term : " + ProdDetList[0].Term + "&nbsp;months</li>";
                                    box += "<li class='mb-2'>Early Cancellation Fee : $" + decimal.Parse(ProdDetList[0].EarlyTerFee).ToString("0.##") + "</li></ul>";



                                    box += "<h5 style='font-size: 18px;color: #ee8d40 ;margin-top: 3%;'>Pricing Details</h5>";
                                    box += "<div class='pricing_plan_button_container'>";

                                    box += "<table class='table table-striped '> <thead>";

                                    box += "<tr style='font-size: 18px;color: #ee8d40 !important;margin-top: 3%;'>";
                                    box += "<th style='text-align:center' scope='col'>MONTHLY USAGE</th>";
                                    box += "<th style='text-align:center' scope='col'>AVERAGE PRICE PER KWH</th>";
                                    box += "<th style='text-align:center' scope='col'>MONTHLY ESTIMATE</th>";
                                    box += "</tr></thred>";
                                    box += "<tbody>";
                                    decimal MonthlyEstimate = 0;


                                    box += "<tr>";
                                    box += "<td scope='row'>" + 500 + "&nbsp;kWh</td>";
                                    string cost5001 = decimal.Parse(cost500).ToString();
                                    box += "<td>" + cost5001 + "¢</td>";
                                    MonthlyEstimate = Convert.ToInt32(500) * Convert.ToDecimal(cost500) / 100;
                                    box += "<td>$" + MonthlyEstimate.ToString("#.00") + "</td>";
                                    box += "</tr>";

                                    box += "<tr>";
                                    box += "<td scope='row'>" + 1000 + "&nbsp;kWh</td>";
                                    //string cost10001 = decimal.Parse(cost1000).ToString("#.0");
                                    string cost10001 = decimal.Parse(cost1000).ToString();
                                    box += "<td>" + cost10001 + "¢</td>";
                                    MonthlyEstimate = Convert.ToInt32(1000) * Convert.ToDecimal(cost1000) / 100;
                                    box += "<td>$" + MonthlyEstimate.ToString("#.00") + "</td>";
                                    box += "</tr>";

                                    box += "<tr>";
                                    box += "<td scope='row'>" + 2000 + "&nbsp;kWh</td>";
                                    string cost20001 = decimal.Parse(cost2000).ToString();
                                    box += "<td>" + cost20001 + "¢</td>";
                                    MonthlyEstimate = Convert.ToInt32(2000) * Convert.ToDecimal(cost2000) / 100;
                                    box += "<td>$" + MonthlyEstimate.ToString("#.00") + "</td>";
                                    box += "</tr>";


                                    box += "</tbody>";

                                    box += "</table>";
                                    box += "</div>";
                                    box += "<a style='color: #007bff;display:block;margin-bottom:5px;cursor:pointer' class='learn_more_item_text' id='avcal' onclick='showdiv()'>How was the Average Price Calculated</a><br/>";
                                    box += "<div id='pCharge' style='display:none'>";
                                    box += "<h5 style='font-size: 18px;color: #ee8d40;margin-top: 3%;'>Plan Charges</h5>";
                                    box += "<ul>";
                                    string cost2 = string.IsNullOrEmpty(ProdDetList[0].Unit_Price) ? string.Empty : (decimal.Parse(ProdDetList[0].Unit_Price) * 100).ToString("#.000");
                                    if (cost2 == "." || cost2 == ".0" || cost2 == "0.000" || cost2 == ".000")
                                    {
                                        cost2 = "0";
                                    }
                                    string TDSPVariableRate = string.IsNullOrEmpty(ProdDetList[0].TDSPVariableRate) ? string.Empty : (decimal.Parse(ProdDetList[0].TDSPVariableRate) * 100).ToString("#.00000");
                                    string TDSPFixedRate = string.IsNullOrEmpty(ProdDetList[0].TDSPFixedRate) ? string.Empty : (decimal.Parse(ProdDetList[0].TDSPFixedRate)).ToString("#.00");
                                    string Cost = string.IsNullOrEmpty(ProdDetList[0].EFL_Rate) ? string.Empty : (decimal.Parse(ProdDetList[0].EFL_Rate)).ToString("#.000");


                                    //box += "<li class='mb-2'>Energy Charges : " + cost2 + "¢/kWh</li>";
                                    //box += "<li class='mb-2'>Est. TDU Delivery Charges:";
                                    //box += "<ul><li>";
                                    //box += "Variable : " + TDSPVariableRate + "¢/kWh</li>";
                                    //box += "<li class='mb-2'>Fixed : " + "$" + TDSPFixedRate + " per month</li></ul>";
                                    //box += "<li class='mb-2'>Rate Type : " + ProdDetList[0].Rate_Type + "</li>";
                                    //box += "<li class='mb-2'>Monthly Fee : " + "$" + ProdDetList[0].MonthlyFee + ProdDetList[0].ActiveMonthlyUsage + "</li></ul></ul>";

                                    List<ProductDetails> PRodList2 = new List<ProductDetails>();
                                    List<ProductDetails> lstProdDetails2 = new List<ProductDetails>();
                                    ProductDetailsListResp ProdDetResp2 = new ProductDetailsListResp();
                                    string resp2 = "";
                                    var s2 = new JavaScriptSerializer();
                                    ProductDetailReq ProdDetReq2 = new ProductDetailReq();
                                    ProdDetReq2.PPid = pPId;
                                    string jsonAccReq2 = s.Serialize(ProdDetReq2);
                                    resp2 = clsDtLayer.ExecuteWebService(jsonAccReq2, "/GetProductDetail");
                                    ProdDetResp2 = s.Deserialize<ProductDetailsListResp>(resp2);
                                    if (ProdDetResp2 != null && ProdDetResp2.productDetailsList.Count > 0)
                                    {
                                        List<ProductDetails> lstProductDetails2 = ProdDetResp2.productDetailsList;

                                        for (int i = 0; i <= lstProductDetails2.Count - 1; i++)
                                        {
                                            if (string.IsNullOrEmpty(lstProductDetails2[i].ChargeDesc) == false)
                                            {
                                                box += "<li class='mb-2'>" + lstProductDetails2[i].ChargeDesc.ToString() + "</li>";
                                            }
                                        }
                                    }
                                    box += "</ul>";

                                    if (enrollmentObject.BrandCode.ToLower() == "gexaix")
                                    {
                                        box += "Average Price per kWh = (Gexa charges + TDU charges) / Total Monthly Usage";
                                    }

                                    box += "</div>";

                                    if (TempData["InEnroll"] == null)
                                    {
                                        //box += "<br /><button class='button solid_orange plan_order_button'   data-toggle='modal' data-target='#myAddressModal' data-dismiss='modal' data-backdrop='static' data-keyboard='false'>Order Now</button>";
                                        box += "<button class='button solid_orange plan_order_button'  onclick='CheckSelectedAddress(" + pPId + "," + prodDetails.Product_ID + ")'>Order Now</button>";
                                    }

                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("GetProductDetail_Popup::GettingPopUp", Ex, -11, "pPId:" + pPId + ", tabId:" + tabId + ", kwh_Choice: " + kwh_Choice, "");
            }
            Session["enrollmentObject"] = enrollmentObject;
            return Json(box, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult GetServiceAddress(string serAdd, string ZipCode)
        {
            List<string> servAddr = new List<string>();
            try
            {
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (enrollmentObject != null)
                {
                    List<ServiceAddress> lstServAddr = new List<ServiceAddress>();
                    clsDataLayer clsDL = new clsDataLayer();
                    ServiceAddressReq servReq = new ServiceAddressReq();
                    AppInfo appInfo = new AppInfo();
                    appInfo = clsDL.GetAppInfo();
                    servReq.appName = appInfo.appName;
                    servReq.appVersion = appInfo.appVersion;
                    servReq.dversion = -1;
                    servReq.sessionID = clsDL.GetSessionID();
                    servReq.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    servReq.serviceAddressLike = serAdd;

                    servReq.zipCode = ZipCode;
                    var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonAccReq = s.Serialize(servReq);
                    string resp = clsDL.ExecuteWebService(jsonAccReq, "/GetServiceAddrList");
                    ServiceAddressResp servResp = new ServiceAddressResp();
                    servResp = s.Deserialize<ServiceAddressResp>(resp);

                    lstServAddr = servResp.lstServAddr;
                    if (lstServAddr != null)
                    {
                        if (servResp != null && servResp.requestStatus == 1)
                        {
                            if (enrollmentObject != null)
                            {
                                enrollmentObject.lstServAddr = lstServAddr;
                                System.Web.HttpContext.Current.Session["enrollmentObject"] = enrollmentObject;
                                for (int i = 0; i < lstServAddr.Count; i++)
                                {
                                    servAddr.Add(lstServAddr[i].serviceAddress);
                                }
                            }
                        }
                        else
                            servAddr.Add("");
                    }
                    else
                        servAddr.Add("");
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("GetServiceAddress::Loading", Ex, -11, "Service Addres: " + serAdd + ", Zipcode: " + ZipCode, "");
            }
            return Json(servAddr, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetGovermentId()
        {
            try
            {
                clsDataLayer clsDL = new clsDataLayer();
                GovermentId govId = new GovermentId();
                AppInfo appInfo = new AppInfo();
                appInfo = clsDL.GetAppInfo();
                govId.appName = appInfo.appName;
                govId.appVersion = appInfo.appVersion;
                govId.dversion = -1;
                govId.sessionID = clsDL.GetSessionID();
                govId.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonAccReq = s.Serialize(govId);
                string resp = clsDL.ExecuteWebService(jsonAccReq, "/GetGovermentId");
                GovermentId gv = new GovermentId();
                var gvValue = s.Deserialize<GovermentId[]>(resp);
                return Json(gvValue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("GetGovermentId::Loading", Ex, -11);
            }
            return null;

        }

        [HttpPost]
        public JsonResult CheckServiceAddress(string serviceAdd)
        {
            string[] responseInfo = new string[4];
            try
            {
                isPrepay = "N";
                bool sAddrFlag = false;
                string AddressMsg = "";
                string result = "";
                string addressType = "";

                ServiceAddressStatus srvAddSts = new ServiceAddressStatus();
                try
                {
                    // enrollmentObject = (EnrollmentData)System.Web.HttpContext.Current.Session["enrollmentObject"];
                    enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                    if (enrollmentObject != null && enrollmentObject.lstServAddr != null)
                    {
                        List<ServiceAddress> lstServObj = new List<ServiceAddress>();
                        lstServObj = enrollmentObject.lstServAddr;

                        foreach (ServiceAddress obj in lstServObj)
                        {
                            if (obj.serviceAddress == serviceAdd)
                            {
                                //  TempData["Addr"] = serviceAdd;
                                if (TempData["setTDSP"] != null)
                                {
                                    string tdsp = TempData["setTDSP"].ToString();
                                    TempData.Keep();
                                    if (obj.TDSPCode != tdsp)
                                    {
                                        responseInfo[2] = "Differenttdsp";
                                        responseInfo[3] = obj.TDSPCode;
                                        TempData["TDSPAccordAdd"] = obj.TDSPCode;
                                        TempData["ZipCode"] = obj.ZipCode;
                                        TempData["Address"] = obj.serviceAddress;
                                        TempData["changeTDSP"] = null;
                                    }
                                    else if (TempData["changeTDSP"] != null)
                                    {
                                        tdsp = TempData["changeTDSP"].ToString();
                                        TempData.Keep();
                                        if (obj.TDSPCode != tdsp)
                                        {
                                            responseInfo[2] = "Differenttdsp";
                                            responseInfo[3] = tdsp;
                                            TempData["setTDSP"] = obj.TDSPCode;
                                            TempData["TDSPAccordAdd"] = obj.TDSPCode;
                                            TempData["ZipCode"] = obj.ZipCode;
                                            //TempData["changeTDSP"] = obj.TDSPCode;
                                        }
                                    }
                                    //else if (TempData["newTDSP"] != null)
                                    //{
                                    //    tdsp = TempData["newTDSP"].ToString();
                                    //    TempData.Keep();
                                    //    if (obj.TDSPCode != tdsp)
                                    //    {
                                    //        responseInfo[2] = "Differenttdsp";
                                    //        responseInfo[3] = tdsp;
                                    //        TempData["setTDSP"] = obj.TDSPCode;
                                    //        TempData["TDSPAccordAdd"] = obj.TDSPCode;
                                    //        TempData["ZipCode"] = obj.ZipCode;
                                    //        //TempData["changeTDSP"] = obj.TDSPCode;
                                    //    }
                                    //} 
                                }
                                else if (enrollmentObject.ZipResp != null)
                                {
                                    string tdsp = enrollmentObject.ZipResp.TDSPCode;
                                    TempData.Keep();
                                    if (obj.TDSPCode != tdsp)
                                    {
                                        responseInfo[2] = "Differenttdsp";
                                        responseInfo[3] = obj.TDSPCode;
                                        TempData["TDSPAccordAdd"] = obj.TDSPCode;
                                        TempData["ZipCode"] = obj.ZipCode;
                                        TempData["Address"] = obj.serviceAddress;
                                    }
                                }
                                else if (TempData["newTDSP"] != null)
                                {
                                    string tdsp = TempData["newTDSP"].ToString();
                                    TempData.Keep();
                                    if (obj.TDSPCode != tdsp)
                                    {
                                        responseInfo[2] = "Differenttdsp";
                                        responseInfo[3] = tdsp;
                                        TempData["setTDSP"] = obj.TDSPCode;
                                        TempData["TDSPAccordAdd"] = obj.TDSPCode;
                                        TempData["ZipCode"] = obj.ZipCode;
                                        //TempData["changeTDSP"] = obj.TDSPCode;
                                    }
                                }
                                if (responseInfo[2] != null && responseInfo[2].ToLower() != "differenttdsp")
                                {
                                    if (TempData["kwh_Choice"] != null)
                                    {
                                        string kwh = "";
                                        kwh = TempData["kwh_Choice"].ToString();
                                        TempData.Keep();
                                        Session["kwhValueforBack"] = kwh;
                                    }
                                }
                                TempData["ESID"] = obj.ESIID;

                                TempData.Keep();
                                //******End*********
                                if (!String.IsNullOrEmpty(obj.AM_YN))//AM means 
                                {
                                    //Need to check
                                    //if (obj.AM_YN.Equals("N")) lblAMSYesNo.Text = "Non AMS";
                                    //else lblAMSYesNo.Text = "AMS";
                                    //End
                                }
                                if (!string.IsNullOrEmpty(obj.AptNum))
                                {
                                    TempData["Dwelling_Type"] = "House";
                                    enrollmentObject.dwelling_type = "House";

                                }
                                else
                                {
                                    TempData["Dwelling_Type"] = "Apartment";
                                    enrollmentObject.dwelling_type = "Apartment";

                                }
                                if (zipResp == null)
                                {
                                    enrollmentObject.ZipResp = new ZipDetailsResp();
                                    Session["enrollmentObject"] = enrollmentObject;
                                }
                                //if (TempData["TDSPCode"]==null)
                                //{
                                //    if (TempData["setTDSP"] != null && TempData["setTDSP"].ToString() != obj.TDSPCode.ToString())
                                //    {
                                //        AddressMsg = "This address is currently listed as our customer. Please call us if you have any questions.";
                                //        addressType = "Mismatch";
                                //    }
                                //}
                                //else
                                //{
                                //    if (TempData["TDSPCode"].ToString() != obj.TDSPCode.ToString())
                                //    {
                                //        AddressMsg = "This address is currently listed as our customer. Please call us if you have any questions.";
                                //        addressType = "Mismatch";
                                //    }
                                //}
                                var s = new JavaScriptSerializer();
                                if (!string.IsNullOrEmpty(obj.ESIID))
                                {
                                    sAddrFlag = true;
                                    if (obj.ServiceClass.ToUpper() == "RESIDENTIAL")
                                    {
                                        if ((isPrepay.Equals("Y") && obj.AM_YN.Equals("Y") || isPrepay.Equals("N")))
                                        {
                                            //string isAllowPrepaidForHouse = ConfigurationManager.AppSettings["AllowPrepaidForHouse"].ToString();

                                            //if (isAllowPrepaidForHouse == "0" && string.IsNullOrEmpty(obj.AptNum) && isPrepay.Equals("Y"))
                                            //{
                                            //    //Need to check
                                            //    //    lblServAddrStatus.Text = (String)GetGlobalResourceObject("MyFunResources", "Prepay_House");
                                            //    //   btnConfirm.Attributes.Add("style", "display:none;");
                                            //    // return;
                                            //    //*****End******
                                            //    AddressMsg = (String)HttpContext.GetGlobalResourceObject("MyFunResources", "Prepay_House");
                                            //    //   btnConfirm.Attributes.Add("style", "display:none;");
                                            //    // return;
                                            //    //*****End******
                                            //}
                                            //Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                            ////FillHiddenField();
                                            ////ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "servAddrStatus", "getServAddressStatus();", true);// 
                                            ////   btnConfirm.Enabled = true;
                                            //string disable_switch_hold = ConfigurationManager.AppSettings["disable_switch_hold"].ToString();
                                            //if (disable_switch_hold.ToUpper().Equals("Y"))
                                            //{
                                            //    Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                            //    //     FillHiddenField();
                                            //    //     ScriptManager.RegisterClientScriptBlock(this.page, this.GetType(), "servAddrStatus", "getServAddressStatus();", true);//
                                            //}
                                            //else
                                            //{
                                            //    if (obj.switch_hold.ToUpper().Equals("N"))
                                            //    {

                                            //        Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                            //        string esidResponse = ESIDResponse(obj.ESIID.ToString());
                                            //        if (esidResponse.ToLower() == "no")
                                            //        {
                                            //            AddressMsg = "This address is currently listed as our customer. Please call us if you have any questions.";
                                            //        }
                                            //        else if (esidResponse.ToLower() == "ok")
                                            //        {

                                            //        }
                                            //        Session["ServiceAddressMsg"] = AddressMsg;
                                            //        responseInfo[0] = addressType;
                                            //        responseInfo[1] = AddressMsg;
                                            //        return Json(responseInfo, JsonRequestBehavior.AllowGet);
                                            //        //  FillHiddenField();
                                            //        //  ScriptManager.RegisterClientScriptBlock(, this.GetType(), "servAddrStatus", "getServAddressStatus();", true);// 
                                            //    }
                                            //    else
                                            //    {
                                            //        Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                            //        //   lblServAddrStatus.Text = "This service address is on Switch Hold.";
                                            //        AddressMsg = "This service address is on Switch Hold.";
                                            //        Session["ServiceAddressMsg"] = AddressMsg;
                                            //        //lblServAddrStatus.ForeColor = Color.Blue;
                                            //        //   btnConfirm.Enabled = false;
                                            //        // FillHiddenField();
                                            //        // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "servAddrStatus", "getServAddressStatus();", true);// 
                                            //        // ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "txtservAddrFocus", "SetTxtServAddressFocus();", true);
                                            //    }
                                            //}



                                            //New Code 26-09-2019
                                            //Santosh Kumar
                                            string disable_switch_hold = ConfigurationManager.AppSettings["disable_switch_hold"].ToString();
                                            if (disable_switch_hold.ToUpper().Equals("Y"))
                                            {
                                                Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                                // FillHiddenField();
                                                string EsidResp = FillResponse(obj.ESIID.ToString(), enrollmentObject.BrandCode);
                                                // result = clsDtLayer.ExecuteWebService(EsidResp, "/GetServiceAddrStatusNew");
                                                result = clsDtLayer.ExecuteWebService(EsidResp, "/GetServiceAddrStatus_ver2");
                                                srvAddSts = s.Deserialize<ServiceAddressStatus>(result);
                                                if (srvAddSts.serviceAddrStatus.ToLower() == "no")
                                                {
                                                    AddressMsg = "This address is currently listed as our customer. Please call us if you have any questions.";
                                                }
                                                else if (srvAddSts.serviceAddrStatus.ToLower() == "ok")
                                                {
                                                    if (Convert.ToInt32(srvAddSts.Existing_Cust_Account_id) > 0)
                                                    {
                                                        enrollmentObject.Existing_Cust_No = srvAddSts.Existing_Cust_No;
                                                        enrollmentObject.Existing_Cust_Account_id = srvAddSts.Existing_Cust_Account_id;
                                                        enrollmentObject.Close_Existing_YN = "Y";
                                                    }
                                                    else
                                                    {
                                                        enrollmentObject.Close_Existing_YN = "N";
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                if (obj.switch_hold.ToUpper().Equals("N"))
                                                {
                                                    Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                                    string EsidResp = FillResponse(obj.ESIID.ToString(), enrollmentObject.BrandCode);
                                                    // result = clsDtLayer.ExecuteWebService(EsidResp, "/GetServiceAddrStatusNew");
                                                    result = clsDtLayer.ExecuteWebService(EsidResp, "/GetServiceAddrStatus_ver2");
                                                    srvAddSts = s.Deserialize<ServiceAddressStatus>(result);
                                                    if (srvAddSts.serviceAddrStatus.ToLower() == "no")
                                                    {
                                                        AddressMsg = "This address is currently listed as our customer. Please call us if you have any questions.";

                                                        responseInfo[0] = addressType;
                                                        responseInfo[1] = AddressMsg;
                                                        return Json(responseInfo, JsonRequestBehavior.AllowGet);

                                                    }
                                                    else if (srvAddSts.serviceAddrStatus.ToLower() == "ok")
                                                    {

                                                        if (Convert.ToInt32(srvAddSts.Existing_Cust_Account_id) > 0)
                                                        {
                                                            enrollmentObject.Existing_Cust_No = srvAddSts.Existing_Cust_No;
                                                            enrollmentObject.Existing_Cust_Account_id = srvAddSts.Existing_Cust_Account_id;
                                                            enrollmentObject.Close_Existing_YN = "Y";
                                                        }
                                                        else
                                                        {
                                                            enrollmentObject.Close_Existing_YN = "N";
                                                        }
                                                        responseInfo[0] = addressType;
                                                        responseInfo[1] = AddressMsg;

                                                        if (!string.IsNullOrEmpty(srvAddSts.ReasonCode))
                                                        {
                                                            // if (srvAddSts.ReasonCode.ToLower() == "baddebt")
                                                            if (srvAddSts.CurrentBalance > 0)
                                                            {
                                                                enrollmentObject.CurrentBalance = srvAddSts.CurrentBalance.ToString();
                                                                enrollmentObject.Badbebt_matched_cust_nos = srvAddSts.Badbebt_matched_cust_nos;

                                                                string internalrulemsg = "Failed in internal rule: BadDebtAmount - $" + enrollmentObject.CurrentBalance + ".";
                                                                if (enrollmentObject.Badbebt_matched_cust_nos != null)
                                                                {
                                                                    internalrulemsg += " Matched customer number(s): " + enrollmentObject.Badbebt_matched_cust_nos;
                                                                }
                                                                enrollmentObject.InternalRuleMes = internalrulemsg;
                                                            }
                                                            enrollmentObject.ReasonCode = srvAddSts.ReasonCode;
                                                            enrollmentObject.ReasonDesc = srvAddSts.ReasonCodeDesc;
                                                        }

                                                        Session["enrollmentObject"] = enrollmentObject;
                                                        return Json(responseInfo, JsonRequestBehavior.AllowGet);

                                                    }
                                                }
                                                else
                                                {
                                                    Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                                    enrollmentObject.ReasonCode = "SWITCH_HOLD";
                                                    enrollmentObject.ReasonDesc = "Switch Hold";
                                                    //  AddressMsg = "The service address is on Hold. ";
                                                    Session["ServiceAddressMsg"] = AddressMsg;
                                                    responseInfo[0] = addressType;
                                                    responseInfo[1] = AddressMsg;
                                                    Session["enrollmentObject"] = enrollmentObject;
                                                    return Json(responseInfo, JsonRequestBehavior.AllowGet);
                                                }
                                            }
                                        }
                                        else
                                        {

                                            //New code8-8-2019
                                            AddressMsg = (String)HttpContext.GetGlobalResourceObject("MyFunResources", "Mes_AmsNonAms");

                                            Session["ServiceAddressMsg"] = AddressMsg;
                                            responseInfo[0] = addressType;
                                            responseInfo[1] = AddressMsg;
                                            return Json(responseInfo, JsonRequestBehavior.AllowGet);


                                            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "txtservAddrFocus", "SetTxtServAddressFocus();", true);
                                        }

                                    }
                                    else
                                    {
                                        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "txtservAddrFocus", "SetTxtServAddressFocus();", true);

                                        AddressMsg = "This commercial address is not eligible for residential products. Please enter residential address.";
                                        addressType = "Comm";

                                        Session["ServiceAddressMsg"] = AddressMsg;
                                        Session["ServAddrObj"] = this.serviceAddressObj = obj;
                                        responseInfo[0] = addressType;
                                        responseInfo[1] = AddressMsg;
                                        Session["enrollmentObject"] = enrollmentObject;
                                        return Json(responseInfo, JsonRequestBehavior.AllowGet);
                                    }
                                    //   EnableServAddressSearch();
                                }
                                break;
                            }
                        }
                        if (!string.IsNullOrEmpty(serviceAdd))
                            //Need to check

                            //*****End********
                            AddressMsg = "We couldn't find your service address. Please check your street abbreviation and try again. ";
                        Session["ServiceAddressMsg"] = AddressMsg;
                        responseInfo[0] = addressType;
                        responseInfo[1] = AddressMsg;
                        Session["enrollmentObject"] = enrollmentObject;
                        return Json(responseInfo, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        AddressMsg = "We couldn't find your service address. Please check your street abbreviation and try again.";
                        Session["ServiceAddressMsg"] = AddressMsg;

                        responseInfo[0] = addressType;
                        responseInfo[1] = AddressMsg;
                        return Json(responseInfo, JsonRequestBehavior.AllowGet);

                    }
                    AddressMsg = "done";
                    Session["ServiceAddressMsg"] = AddressMsg;
                }
                catch
                {
                    AddressMsg = "";
                }
                Session["ServiceAddressMsg"] = AddressMsg;
                responseInfo[0] = addressType;
                responseInfo[1] = AddressMsg;
                Session["enrollmentObject"] = enrollmentObject;

            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("CheckServiceAddress::Loading", Ex, -11);
            }
            return Json(responseInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckPromoCode(string PromoCode)
        {
            string PromoCodeStatus = "";
            bool IsValid = true;
            string vval = PromoCode.Trim();
            IsValid = Validatepromocode(vval);
            if (!IsValid)
            {
                PromoCodeStatus = "The Promo code entered is invalid or has expired. Please try again with a different Promo code or close this window to continue viewing plans without a Promo code.";
            }
            else
            {
                PromoCodeStatus = "Valid";
                TempData["PromoCode"] = PromoCode;
            }
            TempData["PromoCodeStatus"] = PromoCodeStatus;

            return Json(PromoCodeStatus, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult QueryStringPromocode()
        {
            string PromocodeRevert = "";
            string vval = "";
            bool IsValid = false;
            if (Session["Promocode"] != null)
            {
                vval = Session["Promocode"].ToString();
                IsValid = Validatepromocode(vval);
                if (!IsValid)
                {
                    PromocodeRevert = "False";
                }
                else
                {
                    PromocodeRevert = "True";
                }
            }
            else
            {
                PromocodeRevert = "Not";
            }
            return Json(vval, JsonRequestBehavior.AllowGet);
        }

        public string ProductDetailInEnrollment(string zipcode, string tdspcode, string Address)
        {
            string productDetail = "";
            string StepName = "";
            try
            {
                string kwh_Choice = "";
                string pPId = "";
                string tabId = "";
                TempData["ZipCode"] = zipcode;
                TempData["Address"] = Address;
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (enrollmentObject != null)
                {
                    if (TempData["kwh_Choice"] != null)
                    {
                        kwh_Choice = TempData["kwh_Choice"].ToString();
                        TempData.Keep();
                    }
                    else
                    {
                        kwh_Choice = "500";
                    }
                    string address = TempData["Address"].ToString();
                    Session["Address"] = address;
                    pPId = TempData["pPid"].ToString();
                    TempData.Keep();
                    //tabId = (TempData["tabId"] != null && TempData["tabId"] != "") ? TempData["tabId"].ToString() : "Display All";
                    //if (tabId == "" || tabId == "0")
                    //{
                    //    tabId = TempData["DefaultTabId"].ToString();
                    //    TempData.Keep();
                    //}

                    //TempData.Keep();

                    tabId = "Display All";
                    try
                    {
                        tabId = TempData["tabId"].ToString();
                        TempData.Keep();
                    }
                    catch
                    { StepName = "Step1"; }

                    if (tabId == "" || tabId == "0")
                    {
                        try
                        {
                            tabId = TempData["DefaultTabId"].ToString();
                            TempData.Keep();
                        }
                        catch
                        { StepName = "Step2"; }
                    }

                    List<ProductDetails> PRodList = new List<ProductDetails>();
                    List<ProductDetails> lstProdDetails = new List<ProductDetails>();
                    WebEnrollmentNewDesign.Models.FUNBaseRequest basereq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                    ProductDetailsListResp ProdDetResp = new ProductDetailsListResp();
                    var s = new JavaScriptSerializer();
                    ProductDetailReq ProdDetReq = new ProductDetailReq();
                    ProdDetReq.DisplayTab = tabId;
                    ProdDetReq.ipAddress = basereq.ipAddress;
                    ProdDetReq.BrandCode = enrollmentObject.BrandCode;
                    if (TempData["TDSPCODE"] != null)
                    {
                        ProdDetReq.TDSP_Code = TempData["TDSPCODE"].ToString();
                        TempData.Keep();
                    }
                    else
                    {
                        ProdDetReq.TDSP_Code = "CNP"; //zipResp.TDSPCode;
                    }

                    ProdDetReq.sessionID = basereq.sessionID;

                    ProdDetReq.ViewAll = 1;
                    string jsonAccReq = s.Serialize(ProdDetReq);
                    string resp = "";
                    string Cost = "";
                    List<ProductDetails> lstProductDetails = new List<ProductDetails>();
                    ProductDetails prodDetails = new ProductDetails();
                    if (!(string.IsNullOrEmpty(Session["RequestPage"].ToString())))
                    {
                        string PageName = Session["RequestPage"].ToString();
                        if (PageName != "")
                        {
                            ProductDetailReq ProdDetReq2 = new ProductDetailReq();
                            querStringObj = (QueryStringObject)Session["querStringObj"];
                            string promo_code = "";
                            string ref_id = "";
                            string product_id = "";
                            if (querStringObj == null)
                            {
                                promo_code = "";
                                ref_id = "";
                                product_id = "0";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(querStringObj.ProdCode))
                                {
                                    promo_code = HttpUtility.UrlDecode(querStringObj.ProdCode);
                                }
                                if (!string.IsNullOrEmpty(querStringObj.ref_id))
                                {
                                    ref_id = querStringObj.ref_id.ToString();
                                }
                                // product_id = querStringObj.product_id.ToString();
                            }

                            //string TDSPCode = querStringObj.tdsp_code.ToString();
                            PartnerDetailsResp partnerresp = new PartnerDetailsResp();
                            if (Session["PartnerDetailResp"] != null)
                            {
                                partnerresp = (PartnerDetailsResp)Session["PartnerDetailResp"];
                            }

                            string TDSPCode = ProdDetReq.TDSP_Code;
                            ProdDetReq2.TabID = partnerresp.TabID;
                            ProdDetReq2.TDSP_Code = TDSPCode;//zipResp.TDSPCode;
                            ProdDetReq2.Promo_Code = promo_code;
                            ProdDetReq2.BrandCode = enrollmentObject.BrandCode;
                            ProdDetReq2.RefID = ref_id;
                            jsonAccReq = s.Serialize(ProdDetReq2);
                            resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetProductWithEFLKWH");
                            // resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetAllProducts_AccordingEFL");
                            ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);
                            StepName = "Step3";
                            lstProductDetails = ProdDetResp.productDetailsList;
                            // var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                            var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                            StepName = "Step4";

                            if (ProdDetList != null && ProdDetList.Count > 0)
                            {
                                int counter = ProdDetList.Count;
                                for (int i = 0; i <= counter; i++)
                                {
                                    if (ProdDetList[i].ActiveAvgUsage == null || ProdDetList[i].ActiveAvgUsage == "0")
                                    {
                                        ProdDetList = (ProdDetList.Where(o => o.EFLkwh_val == "2000")).ToList();
                                        break;
                                    }
                                    else
                                    {
                                        //ProdDetList = (ProdDetList.Where(o => o.ActiveAvgUsage == o.EFLkwh_val)).ToList();
                                        ProdDetList = (ProdDetList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList();
                                        break;
                                    }
                                }

                                prodDetails = ProdDetList[0];
                                StepName = "Step5";
                                enrollmentObject.ProdDetails = ProdDetList[0];
                                StepName = "Step6";
                                Session["ProductDetails"] = prodDetails;
                                TempData["SelectedProductDetails"] = ProdDetList[0];
                                Cost = string.IsNullOrEmpty(ProdDetList[0].EFL_Rate) ? string.Empty : (decimal.Parse(ProdDetList[0].EFL_Rate)).ToString("#.0");
                                Session["PrePayYN"] = prodDetails.Prepay_YN;
                                TempData["PrePayYN"] = prodDetails.Prepay_YN;
                            }
                        }
                        else
                        {
                            resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetAllProducts_AccordingEFL");
                            ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);
                            // resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetTestimonials");
                            lstProductDetails = ProdDetResp.productDetailsList;
                            // var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.TabID == tabId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);

                            //var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                            var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);


                            StepName = "Step7";
                            if (ProdDetList != null && ProdDetList.Count > 0)
                            {
                                int Counter = ProdDetList.Count;
                                for (int i = 0; i <= Counter; i++)
                                {
                                    if (ProdDetList[i].ActiveAvgUsage == null || ProdDetList[i].ActiveAvgUsage == "0")
                                    {
                                        ProdDetList = (ProdDetList.Where(o => o.EFLkwh_val == "2000")).ToList();
                                        break;
                                    }
                                    else
                                    {
                                        ProdDetList = (ProdDetList.Where(o => o.ActiveAvgUsage == o.EFLkwh_val && o.ActiveAvgUsage == kwh_Choice)).ToList();
                                        break;
                                    }
                                }
                                prodDetails = ProdDetList[0];
                                enrollmentObject.ProdDetails = ProdDetList[0];
                                StepName = "Step8";
                                Session["ProductDetails"] = prodDetails;
                                TempData["SelectedProductDetails"] = ProdDetList[0];
                                Cost = string.IsNullOrEmpty(ProdDetList[0].EFL_Rate) ? string.Empty : (decimal.Parse(ProdDetList[0].EFL_Rate)).ToString("#.0");
                                Session["PrePayYN"] = prodDetails.Prepay_YN;
                                TempData["PrePayYN"] = prodDetails.Prepay_YN;
                            }
                        }
                    }
                    else
                    {
                        resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetAllProducts_AccordingEFL");
                        ProdDetResp = s.Deserialize<ProductDetailsListResp>(resp);
                        // resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetTestimonials");
                        if (ProdDetResp.productDetailsList != null)
                        {
                            lstProductDetails = ProdDetResp.productDetailsList;
                            // var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.TabID == tabId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                            //       var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId && o.EFLkwh_val == kwh_Choice)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);
                            var ProdDetList = (lstProductDetails.Where(o => o.Price_Plan_ID == pPId)).ToList();  //clsDtLayer.GetSelectedPRoductDetails(ppid);


                            StepName = "Step9";
                            if (ProdDetList != null && ProdDetList.Count > 0)
                            {
                                int Counter = ProdDetList.Count;
                                for (int i = 0; i <= Counter; i++)
                                {
                                    if (ProdDetList[i].ActiveAvgUsage == null || ProdDetList[i].ActiveAvgUsage == "0")
                                    {

                                        ProdDetList = (ProdDetList.Where(o => o.EFLkwh_val == "2000")).ToList();
                                        break;
                                    }
                                    else
                                    {
                                        //ProdDetList = (ProdDetList.Where(o => o.ActiveAvgUsage == o.EFLkwh_val)).ToList();
                                        ProdDetList = (ProdDetList.Where(o => o.EFLkwh_val == kwh_Choice)).ToList();
                                        break;
                                    }
                                }
                                prodDetails = ProdDetList[0];
                                enrollmentObject.ProdDetails = ProdDetList[0];
                                StepName = "Step10";
                                Session["ProductDetails"] = prodDetails;
                                TempData["SelectedProductDetails"] = ProdDetList[0];
                                Cost = string.IsNullOrEmpty(ProdDetList[0].EFL_Rate) ? string.Empty : (decimal.Parse(ProdDetList[0].EFL_Rate)).ToString("#.0");
                                Session["PrePayYN"] = prodDetails.Prepay_YN;
                                TempData["PrePayYN"] = prodDetails.Prepay_YN;

                            }
                        }

                    }
                    if (prodDetails != null)
                    {

                        productDetail += "<div id='box20' class='f-en-left-col' style='margin-top:0px;background-color:white !important'>";

                        productDetail += "<div class='m_b_20 text-center'>";
                        StepName = "Step11";
                        productDetail += "<h3 class='size_36'>" + prodDetails.ProductTitle + "</h3>";
                        productDetail += "<strong>" + prodDetails.ProductHeader + "</strong>";
                        productDetail += "</div>";
                        productDetail += "<div class='plan_price_element m_b_20 '>";
                        productDetail += "<span  class='plan_price_value'> " + Cost + "</span>";
                        productDetail += "<span  class='plan_price_symbol'>¢</span>";
                        productDetail += "<span  class='plan_price_measure'>&nbsp;&nbsp;/kWh</span>";
                        productDetail += "</div>";
                        productDetail += "<div class='order_page_customer_service m_b_20 text-center mb-0 pt-0'>";
                        productDetail += "<h5>Call for Customer Service</h5>";
                        StepName = "Step12";
                        if (TempData["PartnerContactNo"] != null)
                        {
                            productDetail += "<strong>" + TempData["PartnerContactNo"].ToString() + "</strong>";
                            TempData.Keep();
                        }
                        else
                        {
                            StepName = "Step13";
                            if (Session["BrandTellNo"] != null)
                            {
                                //   productDetail += "<strong>1-877-437-7442</strong>";
                                productDetail += "<strong>" + Session["BrandTellNo"].ToString() + "</strong>";
                            }
                            TempData.Keep();
                        }
                        productDetail += "</div>";
                        productDetail += "<div class='order_page_customer_service col-lg-12 p-0 pt-4'>";
                        productDetail += "<ul class='text-left '>";
                        StepName = "Step14";
                        productDetail += prodDetails.ProductDesc3;

                        productDetail += "</ul>";
                        productDetail += "</div>";
                        productDetail += "<br/><div class='order_page_customer_service m_b_20 text-center mb-0 '>";
                        //productDetail += "<ul class='b_p_ul'>";
                        //productDetail += "<li>" + prodDetails.ProductNote + "</li>";
                        //productDetail += "</ul>";

                        productDetail += "<h5 style='font-size: 18px;color: #ee8d40;margin-top: 3%;'>Plan Documents</h5>";
                        string url = string.Empty;
                        try
                        {
                            url = ConfigurationManager.AppSettings["efl_url"].ToString();

                            if (!string.IsNullOrEmpty(url))
                            {
                                string TDSP_Code = ProdDetReq.TDSP_Code;


                                productDetail += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=" + Server.UrlEncode(prodDetails.Product_Code) + "&tdspcode=" + TDSP_Code + "' text='Electricity Facts Label'>Electricity Facts Label</a>";
                                StepName = "Step15";
                                if (prodDetails.Prepay_YN.ToUpper().Equals("Y"))
                                {
                                    productDetail += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=TOS" + "' text='Terms Of Service'>Terms Of Service</a>";
                                    productDetail += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=YRAC" + "' text='Right as a Customer'>Your Rights as a Customer</a>";
                                }
                                else
                                {
                                    productDetail += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=TOS" + "' text='Terms of Service'>Terms Of Service</a>";
                                    productDetail += "<a style='color: #007bff;display:block;margin-bottom:5px;' class='learn_more_item_text' target='_blank' href='" + url + "?lang=EN&prodcode=YRAC" + "' text='Right as a Customer'>Your Rights as a Customer</a>";
                                }
                            }
                        }
                        catch
                        {
                        }
                        productDetail += "</ul>";

                        productDetail += "<div class='plan_item_links text-center'>";

                        productDetail += "</div>";
                        productDetail += "<div class='right_col_learn text-center'>";
                        productDetail += "<span class='learn_more_link' onclick='showTabsDetails(" + prodDetails.Price_Plan_ID + "," + kwh_Choice + ")' data-toggle='modal' data-target='#ProductDetails' data-backdrop='static' data-keyboard='false'>Learn More</span>";
                        productDetail += "</div>";
                        productDetail += "</div>";
                        if (string.IsNullOrEmpty(prodDetails.Disclaimer))
                        {
                            TempData["Disclaimer"] = "";
                        }
                        else
                        {
                            TempData["Disclaimer"] = prodDetails.Disclaimer;
                        }
                        StepName = "Step16";
                        //InsertMileStone("OrderNow:ProductSelected", prodDetails.ProductTitle.ToString());
                        InsertMileStone("OrderNow:ProductSelected", "PricePlanID:" + pPId);
                    }

                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("Brand: " + enrollmentObject.BrandCode + "Excption at: " + StepName + "ProductDetailInEnrollment::Loading", Ex, -11);
            }
            return productDetail;
        }

        [HttpPost]
        public JsonResult GetSelectedProductDetails(string Address, string NewZipCode)
        {
            string box = "";
            string tdspcode = "";
            try
            {
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];

                if (Session["RequestPage"] != null)
                {
                    string PageName = Session["RequestPage"].ToString();
                    if (TempData["setTDSP"] != null)
                    {
                        tdspcode = TempData["setTDSP"].ToString();
                        TempData.Keep();
                    }
                    else
                    {
                        tdspcode = getTDSP(NewZipCode);
                    }
                    if (enrollmentObject != null && enrollmentObject.ZipResp != null)
                    {
                        if (tdspcode == enrollmentObject.ZipResp.TDSPCode.ToUpper())
                        {
                            TempData.Keep();
                            TempData["TDSPCODE"] = tdspcode;
                            //TempData["TDSPName"] = zipcodeRESP.zipDetailResp[0].tdspName;
                            box = ProductDetailInEnrollment(NewZipCode, enrollmentObject.ZipResp.TDSPCode.ToString(), Address);
                        }
                        else if (NewZipCode == enrollmentObject.ZipResp.ZipCode)
                        {
                            box = ProductDetailInEnrollment(NewZipCode, enrollmentObject.ZipResp.TDSPCode.ToString(), Address);

                        }
                        else
                        {
                            TempData["ZipCode"] = NewZipCode;
                            box = "NewZip";
                        }
                    }
                    else
                    {
                        TempData["ZipCode"] = NewZipCode;
                        box = "NewZip";
                    }
                    enrollmentObject.ZipResp.TDSPCode = tdspcode;
                    enrollmentObject.ZipResp.ZipCode = NewZipCode;
                    TempData["TDSPCODE"] = tdspcode;





                }
                else
                {
                    if (enrollmentObject != null && enrollmentObject.ZipResp != null)
                    {
                        string Prvzipcode = enrollmentObject.ZipResp.ZipCode;
                        if (Prvzipcode.Trim() == NewZipCode.Trim())
                        {

                            box = ProductDetailInEnrollment(NewZipCode, enrollmentObject.ZipResp.TDSPCode.ToString(), Address);
                        }
                        else
                        {

                            tdspcode = getTDSP(NewZipCode);
                            if (tdspcode != "")
                            {
                                if (tdspcode == enrollmentObject.ZipResp.TDSPCode.ToUpper())
                                {
                                    TempData["TDSPCODE"] = tdspcode;
                                    box = ProductDetailInEnrollment(NewZipCode, enrollmentObject.ZipResp.TDSPCode.ToString(), Address);
                                }
                                else
                                {
                                    box = "NewZip";
                                }
                                // ZipInfo[0] = tdspcode;
                                //ZipInfo[1] = zipcode;
                                enrollmentObject.ZipResp.TDSPCode = tdspcode;
                                enrollmentObject.ZipResp.ZipCode = NewZipCode;

                            }
                            else
                            {

                                enrollmentObject.ZipResp.TDSPCode = "CNP";
                                enrollmentObject.ZipResp.ZipCode = System.Configuration.ConfigurationManager.AppSettings["ZipCode"].ToString();
                            }

                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                // clsDtLayer.WriteErrLog("GetSelectedProductDetails::Loading", Ex, -11);
                clsDtLayer.WriteErrorLog("GetSelectedProductDetails::Loading", Ex, -11, clsDtLayer.ObjectToJson(enrollmentObject), NewZipCode);
            }
            TempData["planInfo"] = box;
            Session["enrollmentObject"] = enrollmentObject;
            return Json(box, JsonRequestBehavior.AllowGet);
        }

        //Start EditServiceAddress
        [HttpPost]
        public JsonResult UpdateEditServiceAddress(string userAddress, string zipcode)
        {
            string box = "";
            EditserviceResponse objEditService = new EditserviceResponse();
            try
            {
                string[] UserInfo = new string[3];
                string tdspcode = "";
                TempData["Address"] = userAddress;
                TempData["ZipCode"] = zipcode;
                UserInfo[0] = userAddress;
                enrollmentObject = (EnrollmentData)System.Web.HttpContext.Current.Session["enrollmentObject"];
                if (enrollmentObject.lstServAddr != null)
                {
                    List<ServiceAddress> lstServObj = new List<ServiceAddress>();
                    lstServObj = enrollmentObject.lstServAddr;

                    foreach (ServiceAddress obj in lstServObj)
                    {
                        if (obj.serviceAddress == userAddress)
                        {
                            TempData["ESID"] = obj.ESIID;
                            TempData.Keep();
                            UserInfo[1] = obj.ESIID;
                            TempData["TDSPAccordAdd2"] = obj.TDSPCode;
                        }
                    }
                    if (TempData["TDSPAccordAdd2"] != null)
                    {
                        tdspcode = TempData["TDSPAccordAdd2"].ToString();
                    }
                    string prvTDSP = "";
                    if (TempData["setTDSP"] != null)
                    {
                        prvTDSP = TempData["setTDSP"].ToString();
                        TempData.Keep();
                    }
                    if (TempData["setTDSP"] != null && tdspcode == TempData["setTDSP"].ToString().ToUpper())
                    {
                        TempData.Keep();
                        TempData["TDSPCODE"] = tdspcode;
                        box = ProductDetailInEnrollment(zipcode, tdspcode, userAddress);
                    }
                    else
                    {
                        box = "NewZip";
                    }

                    //clsDataLayer clsDL = new clsDataLayer();
                    //ZipCodeDetail zipCodeDetail = new ZipCodeDetail();
                    //GovermentId govId = new GovermentId();
                    //AppInfo appInfo = new AppInfo();
                    //appInfo = clsDL.GetAppInfo();
                    //zipCodeDetail.appName = appInfo.appName;
                    //zipCodeDetail.appVersion = appInfo.appVersion;
                    //zipCodeDetail.dversion = -1;
                    //zipCodeDetail.sessionID = clsDL.GetSessionID();
                    //zipCodeDetail.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    //zipCodeDetail.zipcode = zipcode;
                    //ZipDetailsListResp zipcodeResp = new ZipDetailsListResp();
                    //var ss = new System.Web.Script.Serialization.JavaScriptSerializer();
                    //string jsonAccReq0 = ss.Serialize(zipCodeDetail);
                    //string res = clsDL.ExecuteWebService(jsonAccReq0, "/GetTDSP");
                    //if (res != null && res != "")
                    //{
                    //    //  zipcodeDretailres = ss.Deserialize<ZipCodeDetail>(res);
                    //    zipcodeResp = ss.Deserialize<ZipDetailsListResp>(res);
                    //    if (zipcodeResp != null)
                    //    {
                    //        string tdspcode = zipcodeResp.zipDetailResp[0].tdsp;
                    //        if (tdspcode == enrollmentObject.ZipResp.TDSPCode.ToUpper())
                    //        {
                    //            TempData["TDSPCODE"] = zipcodeResp.zipDetailResp[0].tdsp;
                    //            box = ProductDetailInEnrollment(zipcode, enrollmentObject.ZipResp.TDSPCode.ToString(), userAddress);
                    //        }
                    //        else
                    //        {
                    //            box = "NewZip";
                    //        }
                    //        //enrollmentObject.ZipResp.TDSPCode = tdspcode;
                    //        //enrollmentObject.ZipResp.ZipCode = zipcode;
                    //    }
                    //    else
                    //    {
                    //        enrollmentObject.ZipResp.TDSPCode = "CNP";
                    //        enrollmentObject.ZipResp.ZipCode = System.Configuration.ConfigurationManager.AppSettings["ZipCode"].ToString();
                    //    }
                    //}
                    objEditService.UserInfo = UserInfo;
                    objEditService.ProductBox = box;
                    UserInfo[2] = tdspcode;
                    TempData["TDSPCODE"] = tdspcode;
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("UpdateEditServiceAddress::Loading", Ex, -11, clsDtLayer.ObjectToJson(objEditService), "");
            }
            return Json(objEditService, JsonRequestBehavior.AllowGet);
        }
        // End EditServiceAddress


        [HttpPost]
        public JsonResult EditServiceAddress(string userAddress, string zipcode)
        {
            string[] UserInfo = new string[4];
            TempData["Address"] = userAddress;
            TempData["ZipCode"] = zipcode;

            UserInfo[0] = userAddress;
            enrollmentObject = (EnrollmentData)System.Web.HttpContext.Current.Session["enrollmentObject"];
            if (userAddress != "")
            {
                if (enrollmentObject.lstServAddr != null)
                {
                    List<ServiceAddress> lstServObj = new List<ServiceAddress>();
                    lstServObj = enrollmentObject.lstServAddr;

                    foreach (ServiceAddress obj in lstServObj)
                    {
                        if (obj.serviceAddress == userAddress)
                        {
                            TempData["ESID"] = obj.ESIID;
                            UserInfo[1] = obj.ESIID;

                            UserInfo[2] = TempData["TDSPAccordAdd"].ToString();
                            TempData.Keep();

                            if (TempData["changeTDSP"] != null)
                            {
                                string changeTDSP = TempData["changeTDSP"].ToString();
                                TempData.Keep();
                                TempData["setTDSP"] = changeTDSP;
                            }
                        }
                    }
                }
            }
            else
            {
                if (TempData["changeTDSP"] != null)
                {
                    string changeTDSP = TempData["changeTDSP"].ToString();
                    TempData.Keep();
                    TempData["setTDSP"] = changeTDSP;
                    UserInfo[2] = changeTDSP;
                }

                // UserInfo[2] = TempData["TDSPAccordAdd2"].ToString();
            }
            string refid = "";
            string kwh = "";
            PartnerDetailsResp partnerresp = new PartnerDetailsResp();
            querStringObj = (QueryStringObject)Session["queryStringObj"];
            if (querStringObj != null && querStringObj.ref_id != null)
            {
                refid = querStringObj.ref_id;
            }
            if (Session["UL"] != null)
            {
                kwh = Session["UL"].ToString();
            }
            if (kwh != "")
            {
                if (refid != "")
                {
                    UserInfo[3] = "refid=" + refid + "&ul=" + kwh;
                }
                else
                {
                    UserInfo[3] = "ul=" + kwh;
                }
            }
            else
            {
                if (refid != "")
                {
                    UserInfo[3] = "refid=" + refid;
                }
                else
                {
                    UserInfo[3] = "";
                }
            }
            return Json(UserInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetProvider()
        {
            List<SelectListItem> Providers = new List<SelectListItem>();
            try
            {
                var s = new JavaScriptSerializer();
                var resp = clsDtLayer.ExecuteWebService("", "/GetProviders");

                var testi = s.Deserialize<MobileProvidersResponse>(resp);
                if (testi != null && testi.lst != null)
                {
                    for (int i = 0; i <= testi.lst.Count - 1; i++)
                    {
                        Providers.Add(new SelectListItem
                        {
                            Value = testi.lst[i].mobile_provider_id.ToString(),
                            Text = testi.lst[i].mobile_provider_desc
                        });
                    }
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("GetProvider::Loading", Ex, -11);
            }
            return Json(Providers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CheckSelAddress(string pricePlanId, string productId)
        {
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            TempData["Ppid"] = pricePlanId;
            TempData["ProductId"] = productId;
            string[] AddressInfo = new string[3];
            AddressInfo[0] = "No";
            AddressInfo[1] = "No";
            AddressInfo[2] = "No";
            try
            {
                if (TempData["ZipCode"] != null)
                {

                    AddressInfo[0] = TempData["ZipCode"].ToString();

                    TempData.Keep();
                    if (TempData["Address"] != null)
                    {
                        string serviceAdd = TempData["Address"].ToString();
                        TempData.Keep();
                        if (TempData["Address"] != "")
                        {
                            if (enrollmentObject.lstServAddr == null)
                            {
                                GetServiceAddress(serviceAdd, AddressInfo[0].ToString());
                                //   var checkAddress = CheckServiceAddress(serviceAdd);
                                // serviceAdd = TempData["Address"].ToString();
                            }

                            //*****New Code 7-6-2020********
                            if (serviceAdd != "")
                            {
                                string addr = TempData["Address"].ToString();
                                AddressInfo[1] = addr;
                                if (TempData["kwh_Choice"] != null)
                                {
                                    string kwh = "";
                                    kwh = TempData["kwh_Choice"].ToString();
                                    TempData.Keep();
                                    Session["kwhValueforBack"] = kwh;
                                }
                                if (TempData["ESID"] != null)
                                {
                                    AddressInfo[2] = TempData.Peek("ESID").ToString();
                                    TempData.Keep();

                                }
                                TempData.Keep();
                            }
                            else
                            {
                                AddressInfo[1] = "No";
                            }
                            //****End****

                            //Previous code 7-6-2020
                            //var checkAddress = CheckServiceAddress(serviceAdd);

                            //var checkIndexOne = ((string[])(checkAddress.Data))[0];
                            //var checkIndexTwo = ((string[])(checkAddress.Data))[1];
                            //if (checkIndexOne == "" && checkIndexTwo == "")
                            //{
                            //    string addr = TempData["Address"].ToString();
                            //    AddressInfo[1] = addr;
                            //    AddressInfo[2] = TempData.Peek("ESID").ToString();
                            //    TempData.Keep();
                            //}
                            //else
                            //{
                            //    AddressInfo[1] = "No";
                            //}
                            //End
                        }
                    }
                    else
                    {
                        AddressInfo[1] = "No";
                    }
                }
                else
                {
                    AddressInfo[0] = "No";
                    AddressInfo[1] = "No";
                }

            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("CheckSelAddress", Ex, -11, pricePlanId, clsDtLayer.ObjectToJson(enrollmentObject));
                //   clsDtLayer.WriteErrLog("CheckSelAddress", Ex, -11);
            }
            return Json(AddressInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetServiceAddress(string address, string zipcode)
        {
            string[] ZipInfo = new string[9];
            try
            {

                if (address != null && address != "")
                {
                    var checkAddress = CheckServiceAddress(address);
                    var checkIndexOne = ((string[])(checkAddress.Data))[0];
                    var checkIndexTwo = ((string[])(checkAddress.Data))[1];


                    if (checkIndexTwo.Contains("We couldn't find"))
                    {
                        ZipInfo[2] = "Error";
                        return Json(ZipInfo, JsonRequestBehavior.AllowGet);
                    }
                    if (checkIndexOne != "" || checkIndexTwo != "")
                    {
                        ZipInfo[3] = checkIndexOne;
                        ZipInfo[4] = checkIndexTwo;
                    }

                }


                //string[] ZipInfo = new string[3];
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                TempData["Address"] = address;
                TempData["ZipCode"] = zipcode;
                clsDataLayer clsDL = new clsDataLayer();
                ZipCodeDetail zipCodeDetail = new ZipCodeDetail();
                GovermentId govId = new GovermentId();
                AppInfo appInfo = new AppInfo();
                ZipDetailsResp Zip_Resp = new ZipDetailsResp();
                //ZipDetailsReq Zip_Req = new ZipDetailsReq();

                //Zip_Req.lattitude = 0; Zip_Req.lattitude = 0;


                //if (string.IsNullOrEmpty(Zip_Req.zipCode))
                //{
                //        Zip_Req.zipCode = zipcode;

                //       // GetProductsWithZipCode(Zip_Req);
                //        bool IsTx=UpdateZipInfoObj(Zip_Req, "Set_ServiceAdddress");
                //        if (IsTx == false)
                //        {
                //            ZipInfo[0] = "false";
                //            Session["enrollmentObject"] = enrollmentObject;
                //            return Json(ZipInfo, JsonRequestBehavior.AllowGet);

                //        }
                //}
                appInfo = clsDL.GetAppInfo();
                zipCodeDetail.appName = appInfo.appName;
                zipCodeDetail.appVersion = appInfo.appVersion;
                zipCodeDetail.dversion = -1;
                zipCodeDetail.sessionID = clsDL.GetSessionID();
                zipCodeDetail.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (TempData["TDSPAccordAdd"] != null)
                {
                    string tdsp = TempData["TDSPAccordAdd"].ToString();
                    TempData["setTDSP"] = tdsp;
                    TempData.Keep();
                    if (TempData["kwh_Choice"] != null)
                    {
                        string kwh = "";
                        kwh = TempData["kwh_Choice"].ToString();
                        TempData.Keep();
                        Session["kwhValueforBack"] = kwh;
                    }
                }
                else if (TempData["changeTDSP"] != null)
                {
                    TempData["setTDSP"] = TempData["changeTDSP"].ToString(); ;
                    TempData.Keep();
                }
                string tdspvalue = "";
                if (TempData["setTDSP"] != null)
                {
                    tdspvalue = TempData["setTDSP"].ToString();
                    TempData.Keep();
                    if (TempData["kwh_Choice"] != null)
                    {
                        string kwh = "";
                        kwh = TempData["kwh_Choice"].ToString();
                        TempData.Keep();
                        Session["kwhValueforBack"] = kwh;
                    }

                    //zipCodeDetail.tdsp = tdsp;

                }

                zipCodeDetail.zipcode = zipcode;
                ZipDetailsListResp zipcodeRESP = new ZipDetailsListResp();
                var ss = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonAccReq0 = ss.Serialize(zipCodeDetail);
                //DefaultSettings();
                string res = clsDL.ExecuteWebService(jsonAccReq0, "/GetTDSP");

                //  enrollmentObject.ZipResp = new ZipDetailsResp();

                if (res != null && res != "")
                {
                    //zipcodeDretailres = ss.Deserialize<ZipCodeDetail>(res);
                    zipcodeRESP = ss.Deserialize<ZipDetailsListResp>(res);
                    if (zipcodeRESP.zipDetailResp.Count > 0)
                    {
                        // string tdspcode = zipcodeDretailres.tdsp.ToString();
                        if (zipcodeRESP.zipDetailResp.Count < 2)
                        {
                            string tdspcode = zipcodeRESP.zipDetailResp[0].tdsp;
                            if (enrollmentObject != null)
                            {
                                if (enrollmentObject.ZipResp != null)
                                {
                                    if (tdspcode == enrollmentObject.ZipResp.TDSPCode.ToUpper())
                                    {
                                        ZipInfo[2] = "Same";
                                        zipResp.ZipCode = zipcode;
                                    }
                                    else
                                    {
                                        ZipInfo[2] = "New";
                                        zipResp.ZipCode = zipcode;
                                    }
                                }
                                else
                                {
                                    ZipInfo[2] = "New";
                                    zipResp.ZipCode = zipcode;
                                }
                                ZipInfo[0] = tdspcode;
                                ZipInfo[1] = zipcode;
                                zipResp.TDSPCode = zipcodeRESP.zipDetailResp[0].tdsp;
                                zipResp.TDSPName = zipcodeRESP.zipDetailResp[0].tdspName; ;
                                //  zipResp.ZipCode = zipcodeRESP.zipDetailResp[0].zipcode;
                                ZipInfo[5] = zipcodeRESP.zipDetailResp[0].tdspName;
                                if (TempData["setTDSP"] == null)
                                {
                                    TempData["setTDSP"] = zipcodeRESP.zipDetailResp[0].tdsp;
                                }
                                TempData["TDSPCODE"] = zipcodeRESP.zipDetailResp[0].tdsp;
                                TempData["TDSPName"] = zipcodeRESP.zipDetailResp[0].tdspName;
                                Session["TDSPCODE"] = zipcodeRESP.zipDetailResp[0].tdsp;
                                Session["TDSPName"] = zipcodeRESP.zipDetailResp[0].tdspName;
                                ZipInfo[7] = zipcodeRESP.zipDetailResp[0].tdsp;
                                ZipInfo[8] = zipcodeRESP.zipDetailResp[0].tdspName;
                                enrollmentObject.ZipResp = zipResp;
                                if (TempData["ESID"] != null)
                                {
                                    ZipInfo[6] = TempData["ESID"].ToString();
                                    TempData.Keep();
                                }
                            }
                        }

                        else if (zipcodeRESP.zipDetailResp.Count > 1)
                        {
                            for (int i = 0; i <= zipcodeRESP.zipDetailResp.Count - 1; i++)
                            {
                                if (zipcodeRESP.zipDetailResp[i].tdsp == tdspvalue)
                                {
                                    string tdspcode = zipcodeRESP.zipDetailResp[i].tdsp;
                                    if (enrollmentObject != null)
                                    {
                                        if (enrollmentObject.ZipResp != null)
                                        {
                                            if (tdspcode == enrollmentObject.ZipResp.TDSPCode.ToUpper())
                                            {
                                                ZipInfo[2] = "Same";
                                                zipResp.ZipCode = zipcode;
                                            }
                                            else
                                            {
                                                ZipInfo[2] = "New";
                                                zipResp.ZipCode = zipcode;
                                            }
                                        }
                                        else
                                        {
                                            ZipInfo[2] = "New";
                                            zipResp.ZipCode = zipcode;
                                        }
                                        ZipInfo[0] = tdspcode;
                                        ZipInfo[1] = zipcode;
                                        zipResp.TDSPCode = zipcodeRESP.zipDetailResp[i].tdsp;
                                        zipResp.TDSPName = zipcodeRESP.zipDetailResp[i].tdspName;
                                        //  zipResp.ZipCode = zipcodeRESP.zipDetailResp[0].zipcode;
                                        ZipInfo[5] = zipcodeRESP.zipDetailResp[i].tdspName;
                                        if (TempData["setTDSP"] == null)
                                        {
                                            TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                        }
                                        TempData["TDSPCODE"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                        TempData["TDSPName"] = zipcodeRESP.zipDetailResp[i].tdspName;
                                        Session["TDSPCODE"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                        Session["TDSPName"] = zipcodeRESP.zipDetailResp[i].tdspName;
                                        ZipInfo[7] = zipcodeRESP.zipDetailResp[i].tdsp;
                                        ZipInfo[8] = zipcodeRESP.zipDetailResp[i].tdspName;
                                        enrollmentObject.ZipResp = zipResp;
                                        if (TempData["ESID"] != null)
                                        {
                                            ZipInfo[6] = TempData["ESID"].ToString();
                                        }

                                    }
                                }

                            }

                        }

                    }
                    else
                    {
                        ZipInfo[2] = "Error";
                        // enrollmentObject.ZipResp.TDSPCode = "CNP";
                        //enrollmentObject.ZipResp.ZipCode = System.Configuration.ConfigurationManager.AppSettings["ZipCode"].ToString();                        
                    }
                }




                // GetProductDetail("1", "500");
                Session["enrollmentObject"] = enrollmentObject;
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("SetServiceAddress::Loading", Ex, -11);
            }
            return Json(ZipInfo, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSessionObject()
        {
            PCITransactionLog objlog = new PCITransactionLog();
            objlog.PCISessionId = GetUniqueKey(30);
            return Json(objlog, JsonRequestBehavior.AllowGet);
        }

        public static string GetUniqueKey(int size)
        {
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }

        # region Personal Information
        protected void InsertPersonalInformation(CustomerEnrollInfo custEnrollInfo)
        {
            try
            {
                clsDtLayer.InsertMileStoneDetails("PersonalInformation");
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];

                var s = new JavaScriptSerializer();

                WebEnrollmentNewDesign.Models.FUNBaseRequest basereq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                basereq = clsDtLayer.FillBaseRequestInfo();

                FUNWebSession_PersonalInformation objFUNWebSession_PersonalInformation = new FUNWebSession_PersonalInformation();
                objFUNWebSession_PersonalInformation.sessionID = basereq.sessionID;
                if (TempData["PromoCode"] != null)
                {
                    objFUNWebSession_PersonalInformation.PromoCode = TempData["PromoCode"].ToString();
                }
                else
                {
                    objFUNWebSession_PersonalInformation.PromoCode = "";
                }
                //objFUNWebSession_PersonalInformation.Name = custEnrollInfo.Customer_FirstName.Trim() + "  " + custEnrollInfo.Customer_LastName.Trim();
                //objFUNWebSession_PersonalInformation.PhoneNumber = custEnrollInfo.Customer_PhoneNumber;
                //objFUNWebSession_PersonalInformation.AlternatePhoneNumber = "";
                //objFUNWebSession_PersonalInformation.Email = custEnrollInfo.Customer_EmailId;
                //objFUNWebSession_PersonalInformation.PreferredCoummunication = "";
                //objFUNWebSession_PersonalInformation.PreferredLanguage = custEnrollInfo.Preferred_Language; //ddlLang.SelectedValue;
                objFUNWebSession_PersonalInformation.Name = enrollmentObject.personalInfo.first_name.Trim() + " " + enrollmentObject.personalInfo.last_name.Trim();
                objFUNWebSession_PersonalInformation.PhoneNumber = enrollmentObject.contactInfo.contactno;
                objFUNWebSession_PersonalInformation.AlternatePhoneNumber = "";
                objFUNWebSession_PersonalInformation.Email = enrollmentObject.contactInfo.email;
                objFUNWebSession_PersonalInformation.PreferredCoummunication = "";
                objFUNWebSession_PersonalInformation.PreferredLanguage = custEnrollInfo.Preferred_Language; //ddlLang.SelectedValue;
                if (custEnrollInfo.GoPaperLess == true)
                {
                    objFUNWebSession_PersonalInformation.ReceiveEBills = true;

                }
                else
                {
                    //enrollmentObject.IsPaperless = true;
                    //   Session["enrollmentObject"] = enrollmentObject;
                    objFUNWebSession_PersonalInformation.ReceiveEBills = false;
                }
                if (custEnrollInfo.IS_Bill_Add_Same_Serv == false)
                {
                    objFUNWebSession_PersonalInformation.BillingAddressSameAsService = false;

                    objFUNWebSession_PersonalInformation.StreetName = enrollmentObject.serviceAddressObj.StreetName;
                    objFUNWebSession_PersonalInformation.ApartmentNumber = enrollmentObject.serviceAddressObj.AptNum;
                    objFUNWebSession_PersonalInformation.City = enrollmentObject.serviceAddressObj.CityName;
                    objFUNWebSession_PersonalInformation.State = enrollmentObject.serviceAddressObj.StateName;
                    objFUNWebSession_PersonalInformation.ZipCode = enrollmentObject.serviceAddressObj.ZipCode;
                    if (custEnrollInfo.Is_PO_Box == true)
                    {
                        objFUNWebSession_PersonalInformation.POBoxNo = custEnrollInfo.PO_Box_Num;
                    }
                }
                else
                {
                    objFUNWebSession_PersonalInformation.BillingAddressSameAsService = false;
                }
                objFUNWebSession_PersonalInformation.IsPOBox = custEnrollInfo.Is_PO_Box;
                string strAddress = custEnrollInfo.Diff_Service_Address;
                objFUNWebSession_PersonalInformation.Address = strAddress;

                string jsonRequest = s.Serialize(objFUNWebSession_PersonalInformation);
                clsDtLayer.ExecuteWebService(jsonRequest, "/InsertPersonalInformation");
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("InsertPersonalInformation", ex, -11);
            }
        }
        #endregion

        #region Insert Service Information

        protected void InsertServiceInformation(CustomerEnrollInfo custEnrollInfo)
        {
            var s = new JavaScriptSerializer();
            try
            {
                clsDtLayer.InsertMileStoneDetails("ServiceInformation");

                WebEnrollmentNewDesign.Models.FUNBaseRequest basereq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                basereq = clsDtLayer.FillBaseRequestInfo();

                FUNWebSession_ServiceInformation objFUNWebSession_ServiceInformation = new FUNWebSession_ServiceInformation();
                objFUNWebSession_ServiceInformation.sessionID = basereq.sessionID;
                string str = custEnrollInfo.Switching_Moving;
                if (str.ToLower() == "yes")
                {
                    objFUNWebSession_ServiceInformation.IsCurrentLocation = true;
                }
                else
                {
                    objFUNWebSession_ServiceInformation.IsCurrentLocation = false;
                }
                objFUNWebSession_ServiceInformation.StartDate = custEnrollInfo.Move_Or_Switch_Date;
                objFUNWebSession_ServiceInformation.DateOfBirth = DateTime.ParseExact(custEnrollInfo.DateOfBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                objFUNWebSession_ServiceInformation.HashDOB = objclsEncryptDecrypter.SHA1Hash((objFUNWebSession_ServiceInformation.DateOfBirth).ToString("MM/dd/yyyy"));
                bool creditCheckb = false;
                if (TempData["CreditCheck"].ToString().ToLower() == "yes")
                {
                    creditCheckb = true;
                }
                else
                {
                    creditCheckb = false;
                }
                objFUNWebSession_ServiceInformation.AgreeCreditCheck = creditCheckb;
                TempData.Keep();
                //objFUNWebSession_ServiceInformation.SSN = txtSSN1.Value + txtSSN2.Value + txtSSN3.Value;
                objFUNWebSession_ServiceInformation.HashSSN = objclsEncryptDecrypter.SHA1Hash(objclsEncryptDecrypter.lastFourDigits(custEnrollInfo.SSN));

                string jsonServiceRequest = s.Serialize(objFUNWebSession_ServiceInformation);
                clsDtLayer.ExecuteWebService(jsonServiceRequest, "/InsertServiceInformation");
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("InsertServiceInformation::Loading", Ex, -11);
            }
        }

        #endregion

        #region CreditScore

        private CreditScoreResponse GetCreditScore(WebEnrollmentNewDesign.Models.CreditScoreReqData csReqData)
        {
            CreditScoreResponse crScrResp = new CreditScoreResponse();
            try
            {
                WebEnrollmentNewDesign.Models.FUNBaseRequest FunReq = clsDtLayer.FillBaseRequestInfo();
                if (csReqData != null)
                {
                    var s = new JavaScriptSerializer();
                    csReqData.ipAddress = FunReq.ipAddress;
                    csReqData.sessionID = FunReq.sessionID;
                    csReqData.deviceID = FunReq.deviceID;
                    csReqData.dversion = FunReq.dversion;
                    csReqData.appName = FunReq.appName;
                    csReqData.appVersion = FunReq.appVersion;
                    //  csReqData.language = rdblLanguage.SelectedValue;
                    //string IsFrontierCommonService = ConfigurationManager.AppSettings["IsFrontierCommonService"].ToString();
                    //if (IsFrontierCommonService == "0")
                    //{
                    //    string jsonAccReq = s.Serialize(csReqData);
                    //    string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetCreditScore");
                    //    crScrResp = s.Deserialize<CreditScoreResponse>(resp);
                    //}
                    //else if (IsFrontierCommonService == "1")
                    //{
                    WebEnrollmentNewDesign.CommonService.CreditScoreReqData commreq = FillCommonServiceCreditScore((WebEnrollmentNewDesign.Models.CreditScoreReqData)csReqData, (WebEnrollmentNewDesign.Models.FUNBaseRequest)FunReq);
                    CommonService.CreditScoreResponse commresp = new CommonService.CreditScoreResponse();
                    //commresp = obj.RunCreditCheck(commreq);
                    commresp = obj.RunCreditCheck_NEW(commreq);

                    crScrResp = FillCommonServiceCreditResp(commresp);
                    //}
                }
                else
                {
                    // SessionExpired();
                }
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("CreditScore:serv call ", ex, -11);
            }
            return crScrResp;
        }
        private CreditScoreResponse FillCommonServiceCreditResp(CommonService.CreditScoreResponse commresp)
        {
            CreditScoreResponse crScrResp = new CreditScoreResponse();
            try
            {
                crScrResp.depositAmount = commresp.depositAmount;
                crScrResp.credit_score = commresp.credit_score;
                crScrResp.credit_module = commresp.credit_module;
                crScrResp.result_code = commresp.result_code;
                crScrResp.result_msg = commresp.result_msg;
                crScrResp.fraud_alert_details = commresp.fraud_alert_details;
                crScrResp.pv_credit_check_id = commresp.pv_credit_check_id;
                crScrResp.pv_fraud_alert_flag = commresp.pv_fraud_alert_flag;
                // crScrResp.pv_fraud_alert_flag = "Y";
                crScrResp.pn_seq_int_crd_chk_id = commresp.pn_seq_int_crd_chk_id;
                crScrResp.pv_BadDebtAmt = commresp.pv_BadDebtAmt;
                crScrResp.pv_OpenAccounts = commresp.pv_OpenAccounts;
                crScrResp.pv_ControlCount = commresp.pv_ControlCount;
                crScrResp.pv_matched_cust_nos = commresp.pv_matched_cust_nos;
                crScrResp.responseData = commresp.responseData;
                crScrResp.freeze_flag = commresp.freeze_flag;
                crScrResp.fraud_alert_type = commresp.fraud_alert_type;
                crScrResp.requestStatus = commresp.requestStatus;
                crScrResp.requestMessage = commresp.requestMessage;
                crScrResp.creditcheck_status_id = commresp.creditcheck_status_id;
                crScrResp.EnrollmentStep = commresp.EnrollmentStep;
                crScrResp.IsDepositRequired = commresp.IsDepositRequired;
                crScrResp.ReasonCode = commresp.ReasonCode;
            }
            catch (Exception ex)
            {

                clsDtLayer.WriteErrorLog("FillCommonServiceCreditScoreResp", ex, -11, clsDtLayer.ObjectToJson(commresp), clsDtLayer.ObjectToJson(crScrResp));
            }
            return crScrResp;
        }

        private WebEnrollmentNewDesign.CommonService.CreditScoreReqData FillCommonServiceCreditScore(WebEnrollmentNewDesign.Models.CreditScoreReqData csReqData, WebEnrollmentNewDesign.Models.FUNBaseRequest FunReq)
        {
            WebEnrollmentNewDesign.CommonService.CreditScoreReqData commreq = new CommonService.CreditScoreReqData();
            try
            {
                commreq.IPAddress = FunReq.ipAddress;
                commreq.SessionID = FunReq.sessionID;
                commreq.DeviceID = FunReq.deviceID;
                commreq.DeviceVersion = FunReq.dversion;
                commreq.AppName = FunReq.appName;
                commreq.AppVersion = FunReq.appVersion;
                commreq.first_name = csReqData.first_name;
                commreq.middle_initial = csReqData.middle_initial;
                commreq.last_name = csReqData.last_name;
                if (!string.IsNullOrEmpty(csReqData.ssn))
                {
                    commreq.ssn = csReqData.ssn;
                    commreq.hashssn = csReqData.hashssn;
                }
                else
                {
                    commreq.DriverLicense = csReqData.DriverLicense;
                    commreq.DriverLicenseState = csReqData.DriverLicenseState;
                    commreq.ssn = "";
                    commreq.hashssn = "";
                }
                commreq.addresss = csReqData.addresss;
                commreq.addr_no = csReqData.addr_no;
                commreq.apt_no = csReqData.apt_no;
                commreq.street = csReqData.street;
                commreq.city = csReqData.city;
                commreq.state = csReqData.state;
                commreq.zip_code = csReqData.zip_code;
                commreq.date_of_birth = csReqData.date_of_birth;
                commreq.hashDOB = csReqData.hashDOB;
                commreq.score_module = csReqData.score_module;
                commreq.dwelling_tpye = csReqData.dwelling_tpye;
                commreq.site_identifier = csReqData.site_identifier;
                commreq.phone = csReqData.phone;
                commreq.email_id = csReqData.email_id;
                commreq.credit_module_type = ConfigurationManager.AppSettings["credit_module_type"].ToString();
                commreq.source = ConfigurationManager.AppSettings["source"].ToString();
                commreq.BrandCode = csReqData.Brandcode;
                if (Convert.ToString(Session["RequestPage"]) == "PartnerPage")
                {
                    commreq.insert_user = Session["UserName"].ToString();
                }
                else
                {
                    commreq.insert_user = !string.IsNullOrEmpty(enrollmentObject.Agent_Code) ? enrollmentObject.Agent_Code : System.Configuration.ConfigurationManager.AppSettings["SalespersonCode"];
                }
                commreq.callerId = csReqData.callerId;
                commreq.language = csReqData.language;
                commreq.product_id = Convert.ToInt32(enrollmentObject.ProdDetails.Product_ID);
                commreq.price_plan_id = Convert.ToInt32(enrollmentObject.ProdDetails.Price_Plan_ID);
                if (TempData["PromoCode"] != null)
                {
                    commreq.promo_code = TempData["PromoCode"].ToString();
                    TempData.Keep();
                }
                else
                {
                    commreq.promo_code = "";
                }
            }
            catch (Exception ex)
            {

                clsDtLayer.WriteErrorLog("FillCommonServiceCreditScore", ex, -11, clsDtLayer.ObjectToJson(csReqData), clsDtLayer.ObjectToJson(commreq));
            }
            return commreq;
        }

        private void PopulateDepositAmoutObject(WebEnrollmentNewDesign.Models.CreditScoreReqData DepAmtReq, bool isPreviousAddress, CustomerEnrollInfo customerInfoReceive)
        {

            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            ServiceAddress saObj = new ServiceAddress();
            saObj = (ServiceAddress)Session["ServAddrObj"];
            try
            {


                if (enrollmentObject != null)
                {
                    string dateOfBirth = enrollmentObject.personalInfo.date_of_birth;
                    try
                    {
                        //DepAmtReq.date_of_birth = customerInfoReceive.DateOfBirth.ToString();//lblDOB_Lpanel.Text; ;

                        //DepAmtReq.hashDOB = objclsEncryptDecrypter.SHA1Hash(DateTime.ParseExact(customerInfoReceive.DateOfBirth.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                        DepAmtReq.date_of_birth = dateOfBirth;
                        //DepAmtReq.hashDOB = objclsEncryptDecrypter.SHA1Hash(DateTime.ParseExact(dateOfBirth.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                        DepAmtReq.hashDOB = objclsEncryptDecrypter.SHA1Hash(DateTime.ParseExact(enrollmentObject.personalInfo.date_of_birth, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                    }
                    catch (Exception Ex)
                    {
                        clsDtLayer.WriteErrorLog("DateOfBirthErrorinpopulate ", Ex, -11, clsDtLayer.ObjectToJson(DepAmtReq), "");
                    }
                    DepAmtReq.site_identifier = enrollmentObject.serviceAddressObj.ESIID;
                    //DepAmtReq.ssn = customerInfoReceive.SSN;
                    DepAmtReq.ssn = customerInfoReceive.ActualSSN;
                    if (string.IsNullOrEmpty(customerInfoReceive.ActualSSN))
                    {
                        DepAmtReq.hashssn = "";
                        if (customerInfoReceive.Govt_Id_type != null && customerInfoReceive.Govt_Id_type.ToLower() == "drivers license")
                        {
                            DepAmtReq.DriverLicense = customerInfoReceive.Govt_ID_Number;
                            DepAmtReq.DriverLicenseState = customerInfoReceive.Govt_ID_State;
                        }
                    }
                    else
                    {
                        DepAmtReq.hashssn = objclsEncryptDecrypter.SHA1Hash(objclsEncryptDecrypter.lastFourDigits(customerInfoReceive.SSN));
                    }
                    DepAmtReq.addresss = customerInfoReceive.Service_Address;
                    DepAmtReq.dwelling_tpye = enrollmentObject.dwelling_type;
                    //DepAmtReq.score_module = "transunion";
                    DepAmtReq.first_name = customerInfoReceive.Customer_FirstName;
                    //DepAmtReq.middle_initial = enrollmentObject.personalInfo.middle_initial; // txtMInitial.Text.Trim();
                    DepAmtReq.last_name = customerInfoReceive.Customer_LastName;
                    DepAmtReq.phone = customerInfoReceive.Customer_PhoneNumber;
                    DepAmtReq.email_id = customerInfoReceive.Customer_EmailId;

                }
                else
                {
                    //    SessionExpired();
                    return;
                }

                if (isPreviousAddress)
                {
                    //DepAmtReq.addr_no = customerInfoReceive.Moving_StreetName;
                    //DepAmtReq.addresss = customerInfoReceive.Moving_Address;
                    DepAmtReq.apt_no = customerInfoReceive.Moving_Apt;
                    DepAmtReq.city = customerInfoReceive.Moving_City;
                    DepAmtReq.state = customerInfoReceive.Moving_State;
                    DepAmtReq.street = customerInfoReceive.Moving_StreetName;
                    DepAmtReq.zip_code = customerInfoReceive.Moving_ZipCode;

                    BillingAddress prevAddr = new BillingAddress();
                    prevAddr.AptNum = customerInfoReceive.Moving_Apt;
                    prevAddr.CityName = customerInfoReceive.Moving_City;
                    prevAddr.StateName = customerInfoReceive.Moving_State;
                    prevAddr.StreetName = customerInfoReceive.Moving_StreetName;
                    //prevAddr.StreetNum = txtPrevStreetNo.Value.Trim();
                    prevAddr.ZipCode = customerInfoReceive.Moving_ZipCode;
                    enrollmentObject.previousAddress = prevAddr;
                    enrollmentObject.isPreviousAddress = "TRUE";
                }
                else
                {

                    if (saObj != null)
                    {
                        //previous code
                        //DepAmtReq.addr_no = serviceAddressObj.StreetNum;
                        //DepAmtReq.apt_no = serviceAddressObj.AptNum;
                        //DepAmtReq.city = serviceAddressObj.CityName;
                        //DepAmtReq.state = serviceAddressObj.StateName;
                        //DepAmtReq.street = serviceAddressObj.StreetName;
                        //DepAmtReq.zip_code = serviceAddressObj.ZipCode;

                        DepAmtReq.addr_no = saObj.StreetNum;
                        DepAmtReq.apt_no = saObj.AptNum;
                        DepAmtReq.city = saObj.CityName;
                        DepAmtReq.state = saObj.StateName;
                        DepAmtReq.street = saObj.Address;//saObj.StreetName;
                        DepAmtReq.zip_code = saObj.ZipCode;
                    }
                    enrollmentObject.previousAddress = null;
                    enrollmentObject.isPreviousAddress = "FALSE";
                }
                DepAmtReq.callerId = null;
                DepAmtReq.Brandcode = enrollmentObject.BrandCode;
                DepAmtReq.ProductId = Convert.ToInt32(enrollmentObject.ProdDetails.Product_ID);
                DepAmtReq.PricePlanId = Convert.ToInt32(enrollmentObject.ProdDetails.Price_Plan_ID);
                if (TempData["PromoCode"] != null)
                {
                    DepAmtReq.PromoCode = TempData["PromoCode"].ToString();
                    TempData.Keep();
                }
                else
                {
                    DepAmtReq.PromoCode = "";
                }
                Session["enrollmentObject"] = enrollmentObject;

            }
            catch (Exception ex) { clsDtLayer.WriteErrorLog("DepAmtObj:Populate ", ex, -11, clsDtLayer.ObjectToJson(DepAmtReq), ""); }

        }

        #endregion

        #region Credit Check
        private void fillBillingAddress(CustomerEnrollInfo custEnrollInfo)
        {
            serviceAddressObj = (ServiceAddress)Session["ServAddrObj"];
            try
            {
                if (!custEnrollInfo.IS_Bill_Add_Same_Serv)
                {
                    if (custEnrollInfo.Is_PO_Box == true)
                    {
                        billingAddress.StreetNum = string.Empty;
                        billingAddress.StreetName = "PO Box # " + custEnrollInfo.PO_Box_Num;
                    }
                    else
                    {
                        billingAddress.StreetNum = custEnrollInfo.Diff_StreetNo;
                        billingAddress.StreetName = custEnrollInfo.Diff_StreeName;
                        billingAddress.AptNum = custEnrollInfo.Diff_AptNo;

                    }
                    billingAddress.AptNum = custEnrollInfo.Diff_AptNo;
                    billingAddress.CityName = custEnrollInfo.Diff_City;
                    billingAddress.StateName = custEnrollInfo.Diff_State;
                    billingAddress.ZipCode = custEnrollInfo.Diff_ZipCode;
                }
                else
                {
                    if (serviceAddressObj != null)
                    {

                        billingAddress.StreetNum = serviceAddressObj.StreetNum;
                        billingAddress.AptNum = serviceAddressObj.AptNum;
                        billingAddress.CityName = serviceAddressObj.CityName;
                        billingAddress.StateName = serviceAddressObj.StateName;
                        billingAddress.StreetName = serviceAddressObj.StreetName;
                        billingAddress.ZipCode = serviceAddressObj.ZipCode;

                    }
                }
                enrollmentObject.billingAddress = billingAddress;
                Session["enrollmentObject"] = enrollmentObject;
            }
            catch (Exception ex) { clsDtLayer.WriteErrLog("BillingAddress:Fill ", ex, -11); }
        }
        private void PopupDepAmount(string amt)
        {
            try
            {
                string lblPopmsg = "You qualify for our service with a deposit of $" + string.Format("{0:f2}", amt) + ". <br/><br/>.";
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                this.DepAmtRes.depositAmount = decimal.Parse(amt);
                enrollmentObject.depAmtResp = this.DepAmtRes;
                Session["enrollmentObject"] = enrollmentObject;

                decimal DepAmt = decimal.Parse(amt);

            }
            catch (Exception ex)
            { clsDtLayer.WriteErrLog("PopupDepAmount", ex, -11); }
        }
        private CreditScoreResponse GetDepositAmount()
        {
            CreditScoreResponse DepAmt_Res = new CreditScoreResponse();
            CommonService.DespositAmountReqData DepAmtReq = new CommonService.DespositAmountReqData();
            WebEnrollmentNewDesign.Models.FUNBaseRequest BaseReq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
            try
            {

                BaseReq = clsDtLayer.FillBaseRequestInfo();
                DepAmtReq.AppName = BaseReq.appName;
                DepAmtReq.AppVersion = BaseReq.appVersion;
                DepAmtReq.DeviceVersion = BaseReq.dversion;
                DepAmtReq.IPAddress = BaseReq.ipAddress;
                DepAmtReq.SessionID = BaseReq.sessionID;
                if (TempData["Dwelling_Type"] != null)
                {
                    DepAmtReq.dwelling_tpye = TempData["Dwelling_Type"].ToString();
                    TempData.Keep();
                }
                DepAmtReq.credit_score = this.DepAmtRes.credit_score;

                //DepAmtReq.product_id = Int32.Parse(enrollmentObject.ProdDetails.Product_ID);
                //DepAmtReq.price_plan_id = Int32.Parse(enrollmentObject.ProdDetails.Price_Plan_ID);
                DepAmtReq.product_id = Int32.Parse(TempData["ProductId"].ToString()); TempData.Keep();
                DepAmtReq.price_plan_id = Int32.Parse(TempData["Ppid"].ToString());

                if (TempData["PromoCode"] != null)
                {
                    DepAmtReq.promo_code = TempData["PromoCode"].ToString();//enrollmentObject.ProdDetails.Promo_Code;
                }
                else
                {
                    DepAmtReq.promo_code = "";
                }
                DepAmtReq.creditcheck_status_id = enrollmentObject.creditcheck_status_id;
                var s = new JavaScriptSerializer();
                string jsonAccReq = s.Serialize(DepAmtReq);
                //string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetDepositAmountDetails");
                DepAmt_Res = obj.GetDepositAmountDetails(DepAmtReq);
                if (DepAmt_Res.requestStatus != 1 && DepAmt_Res.depositAmount < 0)
                {
                    //msgPopup_Format("Error while getting Deposit Amount.", "Deposit Amount Error.", false, true, "", "Retry");
                    //mpext_msg.Show(); IE_Fix();
                }
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrorLog("DepositAmount:serv call", ex, -11, "DepAmtReq: " + clsDtLayer.ObjectToJson(DepAmtReq) + " , BaseReq: " + clsDtLayer.ObjectToJson(BaseReq), "");
            }
            return DepAmt_Res;
        }
        private decimal GetDepoitAmountFromCommonService()
        {
            decimal ret = -2;
            CommonService.CreditScoreResponse depResp = new CommonService.CreditScoreResponse();
            CommonService.DespositAmountReqData depReq = new CommonService.DespositAmountReqData();
            try
            {
                depReq.product_id = Int32.Parse(enrollmentObject.ProdDetails.Product_ID);
                depReq.price_plan_id = Int32.Parse(enrollmentObject.ProdDetails.Price_Plan_ID);
                depReq.credit_score = 0;
                depReq.dwelling_tpye = enrollmentObject.dwelling_type;
                if (TempData["PromoCode"] != null)
                {
                    depReq.promo_code = TempData["PromoCode"].ToString();
                    TempData.Keep();
                }
                else
                {
                    depReq.promo_code = "";
                }
                CommonServiceClient csClient = new CommonServiceClient();
                depResp = csClient.GetDepositAmountDetails(depReq);
                ret = depResp.depositAmount;
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("GetDepoitAmountFromCommonService", ex, -11, clsDtLayer.ObjectToJson(depReq), clsDtLayer.ObjectToJson(depResp));
            }
            return ret;
        }

        private void FillEnrollmentWithCreditResponse(CreditScoreResponse crScrResp, string method, WebEnrollmentNewDesign.Models.CreditScoreReqData csReqData = null)
        {
            try
            {
                if (crScrResp.credit_module == "ALERT" || crScrResp.pv_fraud_alert_flag == "Y" || this.DepAmtRes == null)
                    this.DepAmtRes = crScrResp;

                enrollmentObject.CsReqData = csReqData;
                enrollmentObject.depAmtResp = this.DepAmtRes;
                enrollmentObject.CrScrResp = crScrResp;
                if (!string.IsNullOrEmpty(this.DepAmtRes.fraud_alert_details))
                {
                    if (!string.IsNullOrEmpty(enrollmentObject.InternalRuleMes))
                    {
                        enrollmentObject.InternalRuleMes += this.DepAmtRes.fraud_alert_details;
                    }
                    else
                    {
                        enrollmentObject.InternalRuleMes = this.DepAmtRes.fraud_alert_details;
                    }
                }
                Session["enrollmentObject"] = enrollmentObject;

                if (this.DepAmtRes.depositAmount > 0)
                {
                    string amt = this.DepAmtRes.depositAmount.ToString("##.#"); //

                    if (this.DepAmtRes.credit_module == "ALERT" || this.DepAmtRes.pv_fraud_alert_flag == "Y")
                    {
                        enrollmentObject.ReasonCode = string.IsNullOrEmpty(enrollmentObject.ReasonCode) ? "FA101" : enrollmentObject.ReasonCode + " and " + "FA101";
                    }
                }
                else if (crScrResp.depositAmount == 0)
                {
                    if (this.DepAmtRes.credit_module == "ALERT" || this.DepAmtRes.pv_fraud_alert_flag == "Y")
                    {
                        enrollmentObject.ReasonCode = string.IsNullOrEmpty(enrollmentObject.ReasonCode) ? "FA101" : enrollmentObject.ReasonCode + " and " + "FA101";
                        Session["enrollmentObject"] = enrollmentObject;

                    }
                }
            }
            catch (Exception ex)
            { clsDtLayer.WriteErrorLog("FillEnrollmentWithCreditResponse:" + method, ex, -11, clsDtLayer.ObjectToJson(csReqData), clsDtLayer.ObjectToJson(crScrResp)); }
        }

        public CommonService.CreditScoreResponse VerifyInternalRule()
        {
            CommonService.CreditScoreResponse internalRuleResp = new CommonService.CreditScoreResponse();
            try
            {

                CommonService.CreditScoreReqData internalRuleReqData = new CommonService.CreditScoreReqData();
                internalRuleReqData.first_name = enrollmentObject.personalInfo.first_name;
                internalRuleReqData.last_name = enrollmentObject.personalInfo.last_name;
                internalRuleReqData.date_of_birth = enrollmentObject.personalInfo.date_of_birth;

                internalRuleReqData.hashDOB = objclsEncryptDecrypter.SHA1Hash(DateTime.ParseExact(internalRuleReqData.date_of_birth, "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture).ToString());
                // internalRuleReqData.hashDOB = objclsEncryptDecrypter.SHA1Hash(DateTime.ParseExact(internalRuleReqData.date_of_birth, "dd/MM/yy", CultureInfo.InvariantCulture).ToString("MM/dd/yy", CultureInfo.InvariantCulture).ToString());
                internalRuleReqData.hashssn = enrollmentObject.personalInfo.hashSSN;
                internalRuleReqData.phone = enrollmentObject.contactInfo.mobile_no;
                internalRuleReqData.email_id = enrollmentObject.contactInfo.email;
                internalRuleReqData.addr_no = enrollmentObject.serviceAddressObj.StreetNum;
                internalRuleReqData.apt_no = enrollmentObject.serviceAddressObj.AptNum;
                internalRuleReqData.street = enrollmentObject.serviceAddressObj.StreetName;
                internalRuleReqData.city = enrollmentObject.serviceAddressObj.CityName;
                internalRuleReqData.state = enrollmentObject.serviceAddressObj.StateName;
                internalRuleReqData.zip_code = enrollmentObject.serviceAddressObj.ZipCode;
                internalRuleReqData.site_identifier = enrollmentObject.serviceAddressObj.ESIID;
                internalRuleReqData.callerId = "";

                internalRuleReqData.BrandCode = enrollmentObject.BrandCode;
                internalRuleReqData.product_id = Convert.ToInt32(enrollmentObject.ProdDetails.Product_ID);
                internalRuleReqData.price_plan_id = Convert.ToInt32(enrollmentObject.ProdDetails.Price_Plan_ID);
                internalRuleReqData.dwelling_tpye = enrollmentObject.dwelling_type;
                internalRuleReqData.source = ConfigurationManager.AppSettings["source"].ToString();
                if (TempData["PromoCode"] != null)
                {
                    internalRuleReqData.promo_code = TempData["PromoCode"].ToString();
                    TempData.Keep();
                }
                else
                {
                    internalRuleReqData.promo_code = "";
                }
                internalRuleResp = CheckInternalRule(internalRuleReqData);
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("VerifyInternalRule", ex, -11);
            }
            return internalRuleResp;
        }
        public CommonService.CreditScoreResponse CheckInternalRule(CommonService.CreditScoreReqData csReqData)
        {
            CommonService.CreditScoreResponse crScrResp = new CommonService.CreditScoreResponse();
            try
            {
                var s = new JavaScriptSerializer();
                WebEnrollmentNewDesign.Models.FUNBaseRequest FunReq = clsDtLayer.FillBaseRequestInfo();
                csReqData.IPAddress = FunReq.ipAddress;
                csReqData.SessionID = FunReq.sessionID;
                csReqData.DeviceID = FunReq.deviceID;
                csReqData.DeviceVersion = FunReq.dversion;
                csReqData.AppName = FunReq.appName;
                csReqData.AppVersion = FunReq.appVersion;
                //string jsonAccReq = s.Serialize(csReqData);
                //string resp = string.Empty;               
                crScrResp = obj.CheckInternalRule(csReqData);
                //  crScrResp = s.Deserialize<CreditScoreResponse>(resp);
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("CheckInternalRule", ex, -11);
            }
            return crScrResp;
        }

        public void CustomerHold(string value)
        {

            WebEnrollmentNewDesign.Models.CreditResultCompareResponse creditResultCompareResponse = new WebEnrollmentNewDesign.Models.CreditResultCompareResponse();
            try
            {
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                enrollmentObject.ReasonCode = string.IsNullOrEmpty(enrollmentObject.ReasonCode) ? value : (!string.IsNullOrEmpty(value) ? enrollmentObject.ReasonCode + " and " + value : enrollmentObject.ReasonCode);
                Session["enrollmentObject"] = enrollmentObject;
                if ((enrollmentObject.ProdDetails != null && enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("N") && enrollmentObject.ProdDetails.CCheckFree.ToUpper().Equals("N"))
                    || (enrollmentObject.ProdDetails != null
                    && enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("N")))
                {
                    //         continueWithVerification();

                    if (this.DepAmtRes == null)
                        this.DepAmtRes = new CreditScoreResponse();
                    this.DepAmtRes.credit_score = 0;
                    CreditScoreResponse crScrResp = new CreditScoreResponse();
                    crScrResp = GetDepositAmount();
                    if (crScrResp.requestStatus == 1)
                    {
                        //SaveDepAmtAndPromtAccordingly(crScrResp, "No CreditCheck");
                        this.DepAmtRes.depositAmount = crScrResp.depositAmount;
                        this.DepAmtRes.pv_credit_check_id = (enrollmentObject.CrScrResp != null) ? enrollmentObject.CrScrResp.pv_credit_check_id : 0;
                        enrollmentObject.depAmtResp = this.DepAmtRes;
                        Session["enrollmentObject"] = enrollmentObject;

                        if (this.DepAmtRes.depositAmount > 0)
                        {
                            string amt = this.DepAmtRes.depositAmount.ToString("##.#"); //

                            decimal DepAmt = decimal.Parse(amt);
                        }

                    }
                    else
                    { clsDtLayer.WriteErrInfo("No Credit Check:DepAmt", "Service Response error", 11); }
                }

            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("CustomerHold", ex, -11);
            }
        }
        private bool DefaultSettings()
        {

            bool isAllowIP = true;
            ZipDetailsReq Zip_Req = new ZipDetailsReq();
            try
            {
                ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                try
                {
                    isAllowIP = clsDtLayer.IsAllowIPAddress();
                }
                catch (Exception ex)
                {
                    if ((ex.Message != "Thread was being aborted."))
                        clsDtLayer.WriteErrLog("Default::IsAllowIPAddress", ex, -11);
                }
                if (isAllowIP)
                {
                    Zip_Req.lattitude = 0;
                    Zip_Req.longitude = 0;
                    querStringObj = (QueryStringObject)Session["queryStringObj"];
                    if (querStringObj != null && (!string.IsNullOrEmpty(querStringObj.zip_Code)))
                    {
                        Zip_Req.zipCode = querStringObj.zip_Code;
                        string TDSP = "";
                        if (TempData["TDSPCODE"] != null)
                        {
                            TDSP = TempData["TDSPCODE"].ToString();
                        }
                        if (string.IsNullOrEmpty(TDSP))
                        {
                            UpdateZipDetails(Zip_Req);
                        }
                    }
                    else if (querStringObj != null && (!string.IsNullOrEmpty(querStringObj.tdsp_code)))
                    {
                        ZipDetailsResp Zip_Resp = new ZipDetailsResp();
                        zipResp.TDSPCode = querStringObj.tdsp_code;
                        if (TempData["TDSPName"] != null)
                        {
                            zipResp.TDSPName = TempData.Peek("TDSPName").ToString();
                        }
                        enrollmentObject.ZipResp = zipResp;
                        Session["enrollmentObject"] = enrollmentObject;
                    }
                    else
                    {
                        GetZipByIPAddress(Zip_Req);
                    }
                }
            }
            catch (Exception ex)
            {
                if ((ex.Message != "Thread was being aborted."))
                    clsDtLayer.WriteErrorLog("Default::QueryString", ex, -11, clsDtLayer.ObjectToJson(querStringObj), "");
            }
            return isAllowIP;
        }

        private void GetZipByIPAddress(ZipDetailsReq Zip_Req)
        {
            WebEnrollmentNewDesign.Models.FUNBaseRequest baseReq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
            browser = Request.Browser;
            baseReq = clsDtLayer.FillBaseRequestInfo();
            string IPAddress = baseReq.ipAddress.ToString();
            IpInfo ipInfo = new IpInfo();
            var s = new JavaScriptSerializer();
            string resp = string.Empty;
            try
            {
                resp = clsDtLayer.GetZipByIP(ipAddress);
                clsDtLayer.FunTracker_Log("GetZipByIPAddress", "IPAddress: " + IPAddress + ";Response: " + resp);
                if (resp != "")
                {
                    ipInfo = s.Deserialize<IpInfo>(resp);
                    clsDtLayer.FunTracker_Log("GetZipByIPAddress1", "IPAddress: " + IPAddress + "; IPINFO Zip: " + ipInfo.zip);
                }
            }
            catch
            {
                clsDtLayer.FunTracker_Log("GetZipByIPAddress-catch", "IPAddress: " + IPAddress + "; Response: " + resp);
            }
            if (!(string.IsNullOrEmpty(ipInfo.zip)))
            {
                Zip_Req.zipCode = ipInfo.zip;
                TempData["ZipByIP"] = ipInfo.zip;

            }

            if (Zip_Req.zipCode != null)
            {
                UpdateZipDetails(Zip_Req);
            }
        }

        private void UpdateZipDetails(ZipDetailsReq Zip_Req) // Calls from default settings
        {
            try
            {

                if (GetZipDetails(Zip_Req))
                {
                    TempData["OLDTDSP"] = this.oldTDSPCode = zipResp.TDSPCode;
                }

            }
            catch (Exception ex)
            { clsDtLayer.WriteErrLog("Default::Prods with Zip", ex, -11); }
        }


        //Get Zip details & Assign to Session Object
        private bool GetZipDetails(ZipDetailsReq Zip_Req)
        {
            bool ret = false;
            ZipDetailsResp Zip_Resp = new ZipDetailsResp();
            try
            {
                var s = new JavaScriptSerializer();
                string jsonAccReq = s.Serialize(Zip_Req);
                string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetZipDetails");

                Zip_Resp = s.Deserialize<ZipDetailsResp>(resp);
                if (Zip_Resp != null)
                {
                    if (Zip_Resp.requestStatus == 1)
                    {
                        if (!string.IsNullOrEmpty(Zip_Resp.StateName))
                        {
                            if (Zip_Resp.StateName == "TX")
                            {
                                if (!string.IsNullOrEmpty(Zip_Resp.TDSPCode))
                                {
                                    zipResp = Zip_Resp;
                                    enrollmentObject.ZipResp = Zip_Resp;
                                    //   TempData["setTDSP"] = Zip_Resp.TDSPCode;
                                    Session["enrollmentObject"] = enrollmentObject;
                                    ret = true;
                                }
                                else
                                {
                                    string proptstr = (String)HttpContext.GetGlobalResourceObject("MyFunResources", "TDSP_NullPrompt");
                                }
                            }
                            if (Zip_Resp.StateName.ToUpper() == "IL" || Zip_Resp.StateName.ToUpper() == "PA" || Zip_Resp.StateName.ToUpper() == "NJ" || Zip_Resp.StateName.ToUpper() == "OH")
                            {
                                enrollmentObject.ZipResp = null;
                                Session["enrollmentObject"] = enrollmentObject;

                                //if (!Response.IsRequestBeingRedirected)
                                if (TempData["ZipCodeResponse"] == null)
                                {
                                    System.Web.HttpContext.Current.Response.Redirect(ConfigurationManager.AppSettings["NEEnrollmentPortal"].ToString() + Zip_Resp.ZipCode);
                                }
                                TempData["ZipCodeResponse"] = Zip_Resp.StateName.ToString();
                            }
                        }
                    }
                    else
                    {
                        //   clsDtLayer.WriteErrInfo("Zip Info", "service Response error", 11);
                    }
                }
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrorLog("Default:TDSP", ex, -11, clsDtLayer.ObjectToJson(Zip_Req), clsDtLayer.ObjectToJson(Zip_Resp));
            }
            return ret;
        }

        public ActionResult Confirmation(string resp)
        {
            try
            {
                string revertCustInfo = "";
                string revertMsg = "";
                string BrandTellNo = "";
                string brandtellNo = "";
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                enrollmentResp = (EnrollResult)Session["enrollmentResp"];
                FunEnrollment funObj = new FunEnrollment();

                if (enrollmentObject != null)
                {
                    //Clear back button info
                    Session["pInfo"] = null;
                    Session["cInfo"] = null;
                    Session["enrollmentObject2"] = null;
                    //End

                    funObj.InternalRuleMes = enrollmentObject.InternalRuleMes;
                    funObj.ReasonCode = enrollmentObject.ReasonCode;
                    funObj.ProdDetails = enrollmentObject.ProdDetails;
                    if (enrollmentObject.depAmtResp != null)
                    {
                        funObj.depAmtResp = enrollmentObject.depAmtResp;
                    }
                    if (enrollmentObject.serviceAddressObj != null)
                    {
                        funObj.serviceAddressObj = enrollmentObject.serviceAddressObj;
                    }
                    if (Session["BrandTellNo"] != null)
                    //  if (!(string.IsNullOrEmpty(Session["BrandTellNo"].ToString())))
                    {
                        brandtellNo = Session["BrandTellNo"].ToString();
                        var result = Regex.Match(brandtellNo, @"\d+").Value;
                        TempData["BrandTellNo"] = brandtellNo;
                    }

                    if (TempData["PendingMsg"] != null)
                    {
                        string PndMsg = TempData["PendingMsg"].ToString();
                        TempData.Keep();
                        if (PndMsg != "")
                        {
                            TempData["BrandTellNo"] = brandtellNo;
                            TempData["PendingMsgData"] = TempData["PendingMsg"].ToString();
                            TempData.Keep();
                            if (TempData["Reject"] != null)
                            {
                                string reject = TempData["Reject"].ToString();
                                if (reject.ToLower() == "yes")
                                {
                                    TempData["RejectMsg"] = "At this time we cannot process your enrollment. Please contact our Customer Care department at " + brandtellNo + " to continue your enrollment.";
                                }
                            }
                        }
                    }
                    if (funObj.depAmtResp == null)
                    {
                        TempData["DepositAmount"] = "0";
                    }
                    else
                    {
                        TempData["DepositAmount"] = funObj.depAmtResp.depositAmount;
                        if (funObj.depAmtResp.depositAmount > 0)
                        {
                            InsertMileStone("DepositRequired", funObj.depAmtResp.depositAmount.ToString());
                        }
                    }
                    funObj.creditCardInfo = enrollmentObject.creditCardInfo;
                    funObj.personalInfo = enrollmentObject.personalInfo;
                    funObj.contactInfo = enrollmentObject.contactInfo;

                    if (enrollmentResp != null)
                    {

                        string ret_str = "";
                        funObj.enrRes = enrollmentResp;
                        TempData["CustomerNumber"] = funObj.enrRes.pv_customer_no;
                        TempData["CustomerId"] = funObj.enrRes.recordID;
                        if (funObj.enrRes.pv_customer_no == "0")
                        {
                            var s = new JavaScriptSerializer();
                            WebEnrollmentNewDesign.Models.FUNBaseRequest bReq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                            if (Session["ReferralCode"] != null)
                            {
                                funObj.refAccountNumber = Session["ReferralCode"].ToString();
                            }

                            if (Session["RequestPage"] != null)
                            {
                                if (Session["RequestPage"].ToString() == "PartnerPage")
                                {
                                    funObj.SalespersonCode = Session["UserName"].ToString();
                                }
                                else
                                {
                                    funObj.SalespersonCode = System.Configuration.ConfigurationManager.AppSettings["SalespersonCode"].ToString();
                                }
                            }
                            else
                            {
                                // FunObj.SalespersonCode = !string.IsNullOrEmpty(enrollmentObject.Agent_Code) ? enrollmentObject.Agent_Code : System.Configuration.ConfigurationManager.AppSettings["SalespersonCode"];
                                funObj.SalespersonCode = System.Configuration.ConfigurationManager.AppSettings["SalespersonCode"].ToString();
                            }
                            bReq = clsDtLayer.FillBaseRequestInfo();
                            funObj.appName = bReq.appName;
                            funObj.appVersion = bReq.appVersion;
                            funObj.deviceID = bReq.deviceID;
                            funObj.dversion = bReq.dversion;
                            funObj.ipAddress = bReq.ipAddress;
                            funObj.sessionID = bReq.sessionID;
                            //PendingEnrollmentRequest objPendingEnrollmentRequest = new PendingEnrollmentRequest();
                            try
                            {
                                clsDtLayer.InsertMileStoneDetails("PendingEnroll");
                                enrollmentObject.EnrollmentType = "REGULAR";
                                //objPendingEnrollmentRequest.Sent_XML = funObj;
                                //objPendingEnrollmentRequest.UserName = funObj.SalespersonCode;
                                //objPendingEnrollmentRequest.PhoneNumber = funObj.contactInfo.mobile_no;
                                //objPendingEnrollmentRequest.ReasonCode = "System Error";
                                //objPendingEnrollmentRequest.Source = ConfigurationManager.AppSettings["source"].ToString();
                                //objPendingEnrollmentRequest.CallerId = "";
                                //objPendingEnrollmentRequest.BrandCode = funObj.BrandCode;
                                //objPendingEnrollmentRequest.Sent_XML.EnrollmentType = enrollmentObject.EnrollmentType;
                            }
                            catch (Exception ex)
                            {
                                revertMsg = "Problem at data";
                            }
                            //try
                            //{
                            //    ret_str = clsDtLayer.ExecuteWebService(s.Serialize(objPendingEnrollmentRequest), "/SubmitPendingEnrollInfo");
                            //}
                            //catch (Exception ex)
                            //{
                            //    if (!(ex is System.Threading.ThreadAbortException))
                            //        clsDtLayer.WriteErrorLog("Enroll", ex, -11, "FunObj: " + clsDtLayer.ObjectToJson(funObj) + ", bReq: " + clsDtLayer.ObjectToJson(bReq), clsDtLayer.ObjectToJson(enrollmentResp));
                            //    revertMsg = "Problem at submit";
                            //}
                            WebEnrollmentNewDesign.Models.FUNBaseResponse resp2 = s.Deserialize<WebEnrollmentNewDesign.Models.FUNBaseResponse>(ret_str);

                            if (resp2.requestStatus != 0 && resp2.requestStatus != -1)
                            {
                                revertMsg = string.Format((String)HttpContext.GetGlobalResourceObject("MyFunResources", "PendnfEnroll_Mes"), "P_" + resp2.requestStatus + "");
                                TempData["PendingMsg"] = revertMsg;
                            }
                        }
                        else
                        {
                            clsDtLayer.InsertMileStoneDetails("Enroll", funObj.enrRes.pv_customer_no);
                        }
                    }

                    return View(funObj);
                }
                else
                {
                    //  return View();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("Confirmation::Loading", Ex, -11);
            }
            return RedirectToAction("Index");

        }
        public JsonResult SubmitCustomerEnrollInfo()
        {
            string[] ResponseInfo = new string[3];
            try
            {

                string revertCustInfo = "";
                string revertMsg = "";
                var s = new JavaScriptSerializer();
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                enrollmentResp = (EnrollResult)Session["enrollmentResp"];
                FunEnrollment FunObj = new FunEnrollment();
                custEnrollInfo = (CustomerEnrollInfo)Session["CustomerEnrollInfo"];
                //   if(custEnrollInfo!=null && enrollmentObject!=null && enrollmentObject.depAmtResp!=null
                if (custEnrollInfo != null)
                {
                    FillSwitchMoveRespObj(custEnrollInfo);
                    FillFunEnrollObject(FunObj, custEnrollInfo);
                }

                enrollmentResp = new EnrollResult();
                WebEnrollmentNewDesign.Models.FUNBaseRequest bReq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                bReq = clsDtLayer.FillBaseRequestInfo();
                FunObj.appName = bReq.appName;
                FunObj.appVersion = bReq.appVersion;
                FunObj.deviceID = bReq.deviceID;
                FunObj.dversion = bReq.dversion;
                FunObj.ipAddress = bReq.ipAddress;
                FunObj.sessionID = bReq.sessionID;

                if (Session["ReferralCode"] != null)
                {
                    FunObj.refAccountNumber = Session["ReferralCode"].ToString();
                }
                if (Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    FunObj.SalespersonCode = Session["UserName"].ToString();
                }
                else
                {
                    if (Session["IsMobileBrowserResponsive"] != null)
                    {
                        if (Session["IsMobileBrowserResponsive"].ToString().ToLower() == "no")
                        {
                            FunObj.SalespersonCode = System.Configuration.ConfigurationManager.AppSettings["SalespersonCode"];
                        }
                        else
                        {
                            FunObj.SalespersonCode = System.Configuration.ConfigurationManager.AppSettings["MobileSalesPersonCode"];
                        }
                    }
                }
                string ser_str = s.Serialize(FunObj);
                string ret_str = "";
                if (FunObj.depAmtResp != null)
                {
                    InsertMileStone("DepositPayment", FunObj.depAmtResp.depositAmount.ToString());
                }
                ret_str = clsDtLayer.ExecuteWebService(ser_str, "/SubmitEnrollInfoWithoutPayment");
                enrollmentResp = s.Deserialize<EnrollResult>(ret_str);  //clsDtLayer.EnrollCustomer(retXml);
                FunObj.enrRes = enrollmentResp;
                Session["enrollmentResp"] = enrollmentResp;
                clsDtLayer.InsertMileStoneDetails("Enrolled:PaymentPending");
                ResponseInfo[0] = enrollmentResp.pv_customer_no;
                ResponseInfo[1] = enrollmentResp.recordID;
                if (enrollmentResp.pv_customer_no == "0")
                {

                    bReq = clsDtLayer.FillBaseRequestInfo();
                    FunObj.appName = bReq.appName;
                    FunObj.appVersion = bReq.appVersion;
                    FunObj.deviceID = bReq.deviceID;
                    FunObj.dversion = bReq.dversion;
                    FunObj.ipAddress = bReq.ipAddress;
                    FunObj.sessionID = bReq.sessionID;
                    //PendingEnrollmentRequest objPendingEnrollmentRequest = new PendingEnrollmentRequest();
                    try
                    {
                        clsDtLayer.InsertMileStoneDetails("PendingEnroll");
                        enrollmentObject.EnrollmentType = "REGULAR";
                        //objPendingEnrollmentRequest.Sent_XML = FunObj;
                        //objPendingEnrollmentRequest.UserName = FunObj.SalespersonCode;
                        //objPendingEnrollmentRequest.PhoneNumber = FunObj.contactInfo.mobile_no;
                        //objPendingEnrollmentRequest.ReasonCode = "System Error";
                        //objPendingEnrollmentRequest.Source = ConfigurationManager.AppSettings["source"].ToString();
                        //objPendingEnrollmentRequest.CallerId = "";
                        //objPendingEnrollmentRequest.BrandCode = FunObj.BrandCode;
                        //objPendingEnrollmentRequest.Sent_XML.EnrollmentType = enrollmentObject.EnrollmentType;

                        //clsDtLayer.WriteErrorLog("SubmitCustomerEnrollInfo::PendingEnroll", null, -11, s.Serialize(FunObj), "");
                    }
                    catch (Exception ex)
                    {
                        revertMsg = "Problem at data";
                    }
                    //try
                    //{
                    //    ret_str = clsDtLayer.ExecuteWebService(s.Serialize(objPendingEnrollmentRequest), "/SubmitPendingEnrollInfo");
                    //}
                    //catch (Exception ex)
                    //{
                    //    if (!(ex is System.Threading.ThreadAbortException))
                    //        clsDtLayer.WriteErrorLog("Enroll", ex, -11, "FunObj: " + clsDtLayer.ObjectToJson(FunObj) + ", bReq: " + clsDtLayer.ObjectToJson(bReq), clsDtLayer.ObjectToJson(enrollmentResp));
                    //    revertMsg = "Problem at submit";
                    //}
                    WebEnrollmentNewDesign.Models.FUNBaseResponse resp2 = s.Deserialize<WebEnrollmentNewDesign.Models.FUNBaseResponse>(ret_str);

                    if (resp2.requestStatus != 0 && resp2.requestStatus != -1)
                    {
                        revertMsg = string.Format((String)HttpContext.GetGlobalResourceObject("MyFunResources", "PendnfEnroll_Mes"), "P_" + resp2.requestStatus + "");
                        TempData["PendingMsg"] = revertMsg;
                    }
                    TempData["ErrorMsg"] = enrollmentResp.requestMessage;
                    ResponseInfo[2] = enrollmentResp.requestMessage;
                }
                else
                {
                    clsDtLayer.InsertMileStoneDetails("Enroll", enrollmentResp.pv_customer_no);
                }
                TempData["ZipCodeInfo"] = FunObj.serviceAddressObj.ZipCode;
                // return Content(ResponseInfo.ToString());
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("SubmitCustomerEnrollInfo::Loading", Ex, -11);
            }
            return Json(ResponseInfo, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetFeeInfo()
        {

            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            List<AgentFeeInfo> lstAgentFee = new List<AgentFeeInfo>();
            decimal Totamt = 0;
            JavaScriptSerializer s = new JavaScriptSerializer();
            FeeInfo feeInfo = new FeeInfo();
            try
            {
                int purchasedAmt = 200;
                WebEnrollmentNewDesign.Models.FUNBaseRequest breq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                breq = clsDtLayer.FillBaseRequestInfo();
                AgentFeeReq feeReq = new AgentFeeReq();
                feeReq.appName = breq.appName;
                feeReq.appVersion = breq.appVersion;
                feeReq.deviceID = breq.deviceID;
                feeReq.dversion = breq.dversion;
                feeReq.ipAddress = breq.ipAddress;
                feeReq.sessionID = breq.sessionID;
                feeReq.ProductID = enrollmentObject.ProdDetails.Product_ID;//"1067";
                feeReq.TDSP_Code = enrollmentObject.ProdDetails.TDSP_Code;
                //feeReq.ChargeType = enrollmentObject.SwitchMoveResp.chargeType;//STDMOVEIN
                feeReq.ChargeType = "STDMOVEIN";

                string str_req = s.Serialize(feeReq);
                string resp = clsDtLayer.ExecuteWebService(str_req, "/GetAgentFees");
                AgentFeeResp lstFTInfo = new AgentFeeResp();
                lstFTInfo = s.Deserialize<AgentFeeResp>(resp);
                if (lstFTInfo.requestStatus == 1)
                {
                    decimal amt = purchasedAmt * decimal.Parse(enrollmentObject.ProdDetails.Unit_Price);
                    lstAgentFee = lstFTInfo.lstAgentFee;
                    AgentFeeInfo totfee = new AgentFeeInfo();
                    foreach (AgentFeeInfo afi in lstAgentFee)
                    {
                        totfee.Charges += afi.Charges;
                    }
                    totfee.fee_desc = "Total";
                    Session["TotCharges"] = totfee.Charges.ToString();
                    // TotCharges_HF.Value = totfee.Charges.ToString();
                    lstAgentFee.Add(totfee);


                    feeInfo.TotalAmount = Totamt;
                    enrollmentObject.lstAgentFee = lstAgentFee;


                    Session["enrollmentObject"] = enrollmentObject;
                    List<gvTaxInfo> AgentFeeAdd = new List<gvTaxInfo>();
                    var AgentFee = new List<gvTaxInfo>();
                    for (int i = 0; i <= lstAgentFee.Count - 1; i++)
                    {
                        AgentFee = new List<gvTaxInfo>(new[]
                        {
                            
                            new gvTaxInfo { fee_desc=lstAgentFee[i].fee_desc,Charges="$"+lstAgentFee[i].Charges.ToString()}
                            
                        });
                        AgentFeeAdd.Add((gvTaxInfo)AgentFee[0]);
                    }
                    return Json(new
                    {
                        aaData = AgentFeeAdd.Select(x => new[] { x.fee_desc, x.Charges })
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    clsDtLayer.WriteErrInfo("GetAgentFees", "Service Response error", 11);
                    return Json(new
                    {
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("GetAgentFeesAndBindGrid:fill ", ex, -11);
                return Json(new
                {

                }, JsonRequestBehavior.AllowGet);
            }

        }
        private void updateDeposit()
        {
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            if (enrollmentObject.ProdDetails != null)
            {
                if (enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("N"))
                {
                    if (enrollmentObject.ProdDetails.split_deposit_yn.ToUpper().Equals("N"))
                    {
                        if (enrollmentObject.depAmtResp != null && enrollmentObject.depAmtResp.depositAmount != null)
                        {
                            enrollmentObject.paymentAmount = Convert.ToDecimal(enrollmentObject.depAmtResp.depositAmount);
                            Session["enrollmentObject"] = enrollmentObject;
                        }
                    }
                    else
                    {
                        if (enrollmentObject.depAmtResp != null && enrollmentObject.depAmtResp.depositAmount != null)
                        {
                            enrollmentObject.paymentAmount = Convert.ToDecimal(enrollmentObject.ProdDetails.split_percentage * enrollmentObject.depAmtResp.depositAmount);
                        }
                    }
                    if (enrollmentObject.depAmtResp != null && enrollmentObject.depAmtResp.depositAmount != null)
                    {
                    }
                }
                else
                {
                    TempData["DepAmount"] = "0";
                    //   HiddenFieldDepAmount.Value = "0";

                }
            }
        }
        public ActionResult fillTaxAndUsage(string purchaseAmt)
        {
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            updateDeposit();

            // string purchaseAmt = "100";
            string Split = "No";
            int purchasedAmt = Convert.ToInt32(purchaseAmt);


            List<AgentFeeInfo> lstAgentFee = new List<AgentFeeInfo>();
            try
            {
                JavaScriptSerializer s = new JavaScriptSerializer();

                WebEnrollmentNewDesign.Models.FUNBaseRequest breq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
                breq = clsDtLayer.FillBaseRequestInfo();
                TaxRequest feeReq = new TaxRequest();
                feeReq.appName = breq.appName;
                feeReq.appVersion = breq.appVersion;
                feeReq.deviceID = breq.deviceID;
                feeReq.dversion = breq.dversion;
                feeReq.ipAddress = breq.ipAddress;
                feeReq.sessionID = breq.sessionID;
                //feeReq.AmountPurchased = purchasedAmt;// decimal.Parse(txtkWhPurchase.Text); //ProductID = "1067";
                decimal Totcharge = 0;
                if (Session["TotCharges"] != null)
                {
                    Totcharge = Convert.ToDecimal(Session["TotCharges"].ToString());
                }
                if (Totcharge > purchasedAmt)
                {
                    feeReq.AmountPurchased = purchasedAmt + Totcharge;// decimal.Parse(txtkWhPurchase.Text); //ProductID = "1067";
                    //  txtkWhPurchase.Text = feeReq.AmountPurchased.ToString();
                    initialAmount = feeReq.AmountPurchased.ToString();
                }
                else
                {
                    feeReq.AmountPurchased = purchasedAmt;// decimal.Parse(txtkWhPurchase.Text); //ProductID = "1067";
                    int initAmount = Convert.ToInt32(ConfigurationManager.AppSettings["InitialPurchaseAmount"]);
                    if (Totcharge > initAmount)
                        initialAmount = (initAmount + Totcharge).ToString();
                    else
                        initialAmount = initAmount.ToString();
                }
                feeReq.TotalFees = Totcharge;// TDSP_Code = enrollmentObject.ProdDetails.TDSP_Code;
                feeReq.Unit_Rate = decimal.Parse(enrollmentObject.ProdDetails.Unit_Price);// ChargeType = "";
                feeReq.ProductType = enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y") ? "PREPAID" : "POSTPAID";
                decimal kWkpurhased = 0; decimal Totamt = 0;
                //if (enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y"))
                //{
                FeeInfo feeInfo = new FeeInfo();
                string str_req = s.Serialize(feeReq);
                string resp = clsDtLayer.ExecuteWebService(str_req, "/GetTaxAndUsage");
                AgentFeeResp lstFTInfo = new AgentFeeResp();
                lstFTInfo = s.Deserialize<AgentFeeResp>(resp);
                if (lstFTInfo.requestStatus == 1)
                {
                    decimal amt = purchasedAmt * decimal.Parse(enrollmentObject.ProdDetails.Unit_Price);
                    lstAgentFee = lstFTInfo.lstAgentFee;
                    decimal depAmt = 0;
                    if (TempData["DepAmount"] != null)
                    {
                        string depAmount = TempData["DepAmount"].ToString();
                        depAmt = decimal.Parse(depAmount);
                        //depAmt = decimal.Parse(enrollmentObject.depAmtResp.depositAmount.ToString());
                    }
                    //  decimal depAmt = decimal.Parse((370).ToString());
                    if (enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y"))
                    {
                        // divCheckOutDetails_header.Visible = false;
                        // divCheckOutDetails_body.Visible = false;
                        // divPaymentPrepaid.Visible = true;
                    }
                    else
                    {
                        //  divCheckOutDetails_header.Visible = true;
                        //  divCheckOutDetails_body.Visible = true;
                        // divPaymentPrepaid.Visible = false;
                        AgentFeeInfo depAmtRow = new AgentFeeInfo();
                        //if (split)
                        //{
                        //    depAmtRow.fee_desc = "Deposit Due Now";
                        //    decimal splitentered = 0.0m;
                        //    if (decimal.TryParse(txtWaiverAmt.Text, out splitentered))
                        //    {
                        //        depAmtRow.Charges = splitentered;
                        //    }
                        //    else
                        //    {
                        //        depAmtRow.Charges = 0.0m;
                        //    }
                        //}
                        //else
                        //{
                        depAmtRow.fee_desc = "Deposit Amount";
                        decimal depAmt2 = 0;
                        if (TempData["DepAmount"] != null)
                        {
                            string depAmount = TempData["DepAmount"].ToString();
                            depAmt2 = decimal.Parse(depAmount);
                        }
                        //  decimal depAmt2 = decimal.Parse((370).ToString());
                        depAmtRow.Charges = depAmt2;//rbWaiver.Checked? 0 : depAmt2; 
                        //}
                        lstAgentFee.Add(depAmtRow);
                    }

                    AgentFeeInfo totfee = new AgentFeeInfo();
                    foreach (AgentFeeInfo afi in lstAgentFee)
                    {
                        totfee.Charges += afi.Charges;
                    }
                    totfee.fee_desc = "Total";

                    lstAgentFee.Add(totfee);

                    decimal charges = totfee.Charges;

                    // gvPaymentFees.DataSource = lstAgentFee;//gvtaxPaymentFees.DataSource = lstAgentFee;// lstFeeTax_grid;
                    // gvPaymentFees.DataBind(); //gvPaymentFees.DataBind();

                    //decimal totConnCharges = string.IsNullOrEmpty(TotCharges_HF.Value) ? 0 : decimal.Parse(TotCharges_HF.Value);
                    Totamt = charges; // +totConnCharges;
                    enrollmentObject.lstAgentFee = lstAgentFee;
                    feeInfo.PurchasedAmount = Totamt;
                    feeInfo.TotalAmount = Totamt;
                    enrollmentObject.feeInfo = feeInfo;

                    Session["enrollmentObject"] = enrollmentObject;

                    //   lblConnectionFee_Lpanel.Text = "$" + (totConnCharges).ToString("0.00");

                    if (enrollmentObject.ProdDetails.Prepay_YN.ToUpper().Equals("Y"))
                    {
                        //  lblOtherCharges_Lpanel.Text = "$" + (charges - totConnCharges).ToString("0.00");
                        // divCheckOutDetails_header.Visible = false;
                        // divCheckOutDetails_body.Visible = false;
                    }
                    else
                    {

                    }
                    // if (enrollmentObject.ProdDetails.split_deposit_yn.ToUpper().Equals("Y"))
                    //   txtCcDeposit.Text = "" + (Totamt).ToString("0.00");
                    // else
                    // txtCcDeposit.Text = "" + (Totamt).ToString("0.00");

                    // lblTotal_Lpanel.Text = "$" + Totamt.ToString("0.00");
                    List<gvTaxInfo> AgentFeeAdd = new List<gvTaxInfo>();
                    var AgentFee = new List<gvTaxInfo>();
                    for (int i = 0; i <= lstAgentFee.Count - 1; i++)
                    {
                        AgentFee = new List<gvTaxInfo>(new[]
                        {
                            
                            new gvTaxInfo { fee_desc=lstAgentFee[i].fee_desc,Charges="$"+lstAgentFee[i].Charges.ToString()}
                            
                        });
                        AgentFeeAdd.Add((gvTaxInfo)AgentFee[0]);
                    }
                    return Json(new
                    {
                        aaData = AgentFeeAdd.Select(x => new[] { x.fee_desc, x.Charges })
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    clsDtLayer.WriteErrInfo("GetTaxAndUsage", "Service Response error", 11);
                    return Json(new
                    {

                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("fillTaxAndUsage:fill ", ex, -11);
                return Json(new
                {

                }, JsonRequestBehavior.AllowGet);
            }
        }
        public class gvTaxInfo
        {
            public string fee_desc { get; set; }
            public string Charges { get; set; }
            //   public List<PhoneNumber> PhoneNumber { get; set; }
        }
        private void GetAgentFeesAndBindGrid(int purchasedAmt, int kwhAmtDefault)
        {

        }

        protected void FillCreditScoreReqData(CommonService.CreditScoreReqData creditScoreReqData, WebEnrollmentNewDesign.Models.CreditScoreReqData csReqData)
        {
            try
            {
                creditScoreReqData.first_name = csReqData.first_name;
                creditScoreReqData.middle_initial = csReqData.middle_initial;
                creditScoreReqData.last_name = csReqData.last_name;
                creditScoreReqData.ssn = csReqData.ssn;
                creditScoreReqData.addresss = csReqData.addresss;
                creditScoreReqData.addr_no = csReqData.addr_no;
                creditScoreReqData.apt_no = csReqData.apt_no;
                creditScoreReqData.street = csReqData.street;
                creditScoreReqData.city = csReqData.city;
                creditScoreReqData.state = csReqData.state;
                creditScoreReqData.date_of_birth = csReqData.date_of_birth;
                creditScoreReqData.hashDOB = objclsEncryptDecrypter.SHA1Hash(csReqData.date_of_birth);
                creditScoreReqData.zip_code = csReqData.zip_code;
                creditScoreReqData.score_module = csReqData.score_module;
                creditScoreReqData.dwelling_tpye = csReqData.dwelling_tpye;
                creditScoreReqData.site_identifier = csReqData.site_identifier;
                creditScoreReqData.phone = csReqData.phone;
                creditScoreReqData.email_id = csReqData.email_id;
            }
            catch (Exception ex)
            {

                clsDtLayer.WriteErrLog("FillCreditScoreReqData:", ex, -11);
            }
        }

        #endregion

        #region MoveIn or Switch

        public JsonResult GetSwitchMoveInfo(string Switch_Or_Move)
        {
            List<string> ddlSwitchDates = new List<string>();
            List<string> ddlDates = new List<string>();
            try
            {
                string expiryalert = "";
                string lblMoveSwitchDate = "";
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["contract_expiry_alert"].ToString()))
                    expiryalert = ConfigurationManager.AppSettings["contract_expiry_alert"].ToString();
                else
                    expiryalert = "Y";
                if (TempData["ProductId"] != null)
                {
                    TempData.Keep();
                    if (Session["PrePayYN"] != null && Session["PrePayYN"] != "" && Session["PrePayYN"].ToString().Equals("Y"))
                    {
                        TempData.Keep();
                        switchMoveInReq.ProductType = "PREPAID";
                    }
                    else
                    {
                        switchMoveInReq.ProductType = "POSTPAID";
                    }

                    ServiceAddress addr = (ServiceAddress)Session["ServAddrObj"];
                    if (addr != null)
                        switchMoveInReq.AMS_YN = !string.IsNullOrEmpty(addr.AM_YN) ? addr.AM_YN : "Y";
                    else
                        switchMoveInReq.AMS_YN = "Y";

                    //TempData.Keep();
                    switchMoveInReq.ASAP_On = "ONDATE";
                    if (Switch_Or_Move.ToUpper().Equals("YES") && expiryalert.ToUpper().Equals("Y"))
                    {
                        //  contractExpiryNote.Visible = true;
                        switchMoveInReq.SwitchMoveType = "Y";
                    }
                    else
                    {
                        switchMoveInReq.SwitchMoveType = "N";
                        //contractExpiryNote.Visible = false;
                    }
                    //if (TempData["TDSPCODE"] != null)
                    //{
                    //    switchMoveInReq.TDSPCode = TempData["TDSPCODE"].ToString();
                    //    TempData.Keep();
                    //}
                    if (Session["enrollmentObject"] != null)
                    {
                        enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                        if (enrollmentObject.ZipResp != null)
                        {

                            switchMoveInReq.TDSPCode = enrollmentObject.ZipResp.TDSPCode;

                            //switchMoveResp = new SwitchMoveResp();
                            SwitchDataArray arrSwitchData = new SwitchDataArray();
                            JavaScriptSerializer s = new JavaScriptSerializer();
                            string scr_str = s.Serialize(switchMoveInReq);
                            string resp = clsDtLayer.ExecuteWebService(scr_str, "/GetMoveSwitchInfo");
                            if (resp != null)
                                arrSwitchData = s.Deserialize<SwitchDataArray>(resp);

                            if (arrSwitchData.requestStatus == 1)
                            {
                                Session["lstSwMovData"] = arrSwitchData.lstSwitchData;
                                if (arrSwitchData.lstSwitchData != null)
                                {
                                    for (int i = 0; i < arrSwitchData.lstSwitchData.Count; i++)
                                    {
                                        SwitchMoveData swmovData = new SwitchMoveData();
                                        swmovData = arrSwitchData.lstSwitchData[i];
                                        if (swmovData != null)
                                        {
                                            string Date = swmovData.ToString();

                                            DateTime smDate = DateTime.ParseExact(swmovData.SCalDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                            ddlSwitchDates.Add(smDate.ToString("ddd, ") + smDate.ToString("MMM dd,yyyy")); //+ "     - $" + swmovData.Fee.ToString("#0.00"));
                                            DateTime date = smDate.Date;
                                            ddlDates.Add(date.ToString("dd-MM-yyyy"));
                                        }
                                    }
                                    SwitchMoveData swmovData1 = new SwitchMoveData();
                                    swmovData1 = arrSwitchData.lstSwitchData[0];
                                    DateTime smDate1 = DateTime.ParseExact(swmovData1.SCalDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                    lblMoveSwitchDate = smDate1.AddDays(13).ToString("dddd,MMM,dd yyyy");
                                }

                            }
                            else
                            {
                                clsDtLayer.WriteErrInfo("ServInfo::GetInfo", "Service Response error", 11);
                            }
                        }
                    }
                }
                else
                {

                }
                var dates = lblMoveSwitchDate;
            }
            catch (Exception ex) { clsDtLayer.WriteErrorLog("ServInfo:GetInfo", ex, -11, clsDtLayer.ObjectToJson(switchMoveInReq), ""); }
            var dateValues = ddlSwitchDates;
            // return Json(dateValues, JsonRequestBehavior.AllowGet);
            return Json(ddlDates, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fill_Switch_MoveResp()
        public void FillSwitchMoveRespObj(CustomerEnrollInfo customerEnrollInfo)
        {
            try
            {
                SwitchMoveData swmovData = new SwitchMoveData();
                lstSwMovData = (List<SwitchMoveData>)Session["lstSwMovData"];
                string switchMoveDate = TempData["SwitchMoveDate"].ToString();
                TempData.Keep();
                switchMoveDate = switchMoveDate.Replace("-", "/");
                //int index = lstSwMovData.FindIndex(a => a.MoveDate == switchMoveDate);
                int index = lstSwMovData.FindIndex(a => a.SCalDate == switchMoveDate);
                if (index >= 0)
                {
                    swmovData = lstSwMovData[index];

                    DateTime smdate = DateTime.ParseExact(swmovData.SCalDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                    string swmovdate = smdate.ToString("dddd,MMM,dd yyyy");
                    //lblMoveSwitchDate.Text = swmovdate;
                    // lblMoveSwitchDate.Text = DateTime.Parse(swmovdate).AddDays(13).ToString("dddd,MMM,dd yyyy");
                    //swithmoveDateHF.Value = lblMoveSwitchDate.Text;

                    SwitchMoveResp switchMoveResp = new SwitchMoveResp();
                    switchMoveResp.Fee = swmovData.Fee;
                    if (!(string.IsNullOrEmpty(customerEnrollInfo.Switching_Moving)) && customerEnrollInfo.Switching_Moving.ToString().ToLower() == "movein")
                    {
                        switchMoveResp.SwitchMoveType = "Y";
                    }
                    else
                    {
                        switchMoveResp.SwitchMoveType = "N";
                    }

                    if (switchMoveResp.SwitchMoveType == "N")
                        switchMoveResp.PriorityCode = swmovData.PriorityCode;
                    else
                        switchMoveResp.PriorityCode = string.Empty;
                    enrollmentObject.SwitchMoveResp = switchMoveResp;
                    enrollmentObject.SwitchMoveResp.SelectedSwitchMoveDate = swmovData.MoveDate;//.ToShortDateString();//swmovData.MoveDate != null ? swmovData.MoveDate.ToShortDateString() : string.Empty;
                    enrollmentObject.SwitchMoveResp.chargeType = swmovData.chargeType;
                    //  Session["switchMoveDate"] = enrollmentObject;
                    Session["enrollmentObject"] = enrollmentObject;
                }

            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrLog("FillSwitchMoveRespObj:", ex, -11);
                // clsDtLayer.WriteErrorLog("FillSwitchMoveRespObj", ex, -11, clsDtLayer.ObjectToJson(BaseReq), "");
            }
        }
        #endregion

        #region FillResponse
        public string FillResponse(string ESID, string BrandCode)
        {
            string result = "";
            var s = new JavaScriptSerializer();
            ServiceAddressStatusReq servStatusReq = new ServiceAddressStatusReq();
            servStatusReq.dversion = 1;
            servStatusReq.esiid = ESID.ToString();
            WebEnrollmentNewDesign.Models.FUNBaseRequest breq = new WebEnrollmentNewDesign.Models.FUNBaseRequest();
            breq = clsDtLayer.FillBaseRequestInfo();
            servStatusReq.appName = breq.appName;
            servStatusReq.appVersion = breq.appVersion;
            servStatusReq.deviceID = breq.deviceID;
            servStatusReq.dversion = breq.dversion;
            servStatusReq.ipAddress = breq.ipAddress;
            servStatusReq.sessionID = breq.sessionID;
            servStatusReq.BrandCode = BrandCode;
            string jsonAccReq = s.Serialize(servStatusReq);
            string esidValue = jsonAccReq;

            return esidValue;
        }
        #endregion

        #region CheckURL

        #region IndexPage
        public ActionResult OrderNow()
        {
            return RedirectToAction("Index");
        }
        public ActionResult DisplayAll()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Recommended()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Fixed()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Green()
        {
            return RedirectToAction("Index");
        }
        public ActionResult UsageCredit()
        {
            return RedirectToAction("Index");
        }

        public ActionResult kwh500()
        {
            return RedirectToAction("Index");
        }
        public ActionResult kwh1000()
        {
            return RedirectToAction("Index");
        }
        public ActionResult kwh2000()
        {
            return RedirectToAction("Index");
        }
        public ActionResult ChangeZipCode()
        {
            return RedirectToAction("Index");
        }
        public ActionResult PromoCode()
        {
            return RedirectToAction("Index");
        }
        public ActionResult ProductDetail()
        {
            return RedirectToAction("Index");
        }
        #endregion

        #region ConfirmationPage()
        public ActionResult Continuewithdeposit()
        {
            return RedirectToAction("Confirmation");
        }
        public ActionResult Continuewithoutdeposit()
        {
            return RedirectToAction("Confirmation");
        }
        #endregion

        #endregion

        #region MileStone

        [HttpPost]
        public JsonResult InsertMileStone(string Milestone, string value = "", string BrandCode = null)
        {
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            if (enrollmentObject != null)
            {
                if (value == "")
                {
                    if (enrollmentObject.ZipResp != null)
                    {
                        clsDtLayer.InsertMileStoneDetails(Milestone, enrollmentObject.ZipResp.ZipCode);
                    }
                }
                else
                {
                    clsDtLayer.InsertMileStoneDetails(Milestone, value);
                }
            }
            return Json("Done", JsonRequestBehavior.AllowGet);
        }

        #endregion

        [HttpPost]
        public ActionResult ValidatePCITransactionLog(string ChildId, string PCISessionId)
        {
            try
            {
                var pcireq = new PCITransactionLog
                {
                    ChildUniqueId = ChildId,
                    SessionId = PCISessionId,
                    Source = "OEFWeb"
                };
                var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonAccReq = s.Serialize(pcireq);

                string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/ValidatePCITransactionLog");

                // var requestResponse = JsonHelper.FromJson<FUNAccountResponse>(resp);
                var requestResponse = s.Deserialize<WebEnrollmentNewDesign.Models.FUNBaseResponse>(resp);

                //InsertPCILog("ValidatePCITransactionLog: ChildId: " + pcireq.ChildUniqueId + ", Response:" + requestResponse.resultMessage, "");
                return Json(requestResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("ValidatePCITransactionLog::Loading", Ex, -11);
            }
            return null;
        }

        [HttpGet]
        public JsonResult GetBrandInfo()
        {
            string[] BrandDetail = new string[6];
            try
            {
                clsDataLayer clsDL = new clsDataLayer();
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (enrollmentObject != null)
                {
                    string BrandCode = enrollmentObject.BrandCode;
                    BrandInfoReq brandInfoRequest = new BrandInfoReq();
                    AppInfo appInfo = new AppInfo();
                    appInfo = clsDL.GetAppInfo();
                    brandInfoRequest.appName = appInfo.appName;
                    brandInfoRequest.appVersion = appInfo.appVersion;
                    brandInfoRequest.dversion = -1;
                    brandInfoRequest.sessionID = clsDL.GetSessionID();
                    brandInfoRequest.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                    brandInfoRequest.BrandCode = BrandCode.ToString();
                    var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonAccReq = s.Serialize(brandInfoRequest);
                    string resp = clsDL.ExecuteWebService(jsonAccReq, "/GetBrandInfo");
                    BrandInfoResp brandInfoResp = new BrandInfoResp();
                    BrandInfo brandInfo = new BrandInfo();
                    brandInfoResp = s.Deserialize<BrandInfoResp>(resp);

                    if (brandInfoResp != null && brandInfoResp.requestStatus == 1)
                    {
                        BrandDetail[0] = brandInfoResp.BrandInfo[0].BrandName;
                        BrandDetail[1] = brandInfoResp.BrandInfo[0].BrandDescription;
                        BrandDetail[2] = brandInfoResp.BrandInfo[0].TollfreeNumber;
                        BrandDetail[3] = brandInfoResp.BrandInfo[0].CopyrightNote;
                        BrandDetail[4] = brandInfoResp.BrandCode;
                        BrandDetail[5] = BrandCode;
                        Session["BrandCode"] = BrandCode;
                        Session["BrandTellNo"] = BrandDetail[2];
                        TempData["BrandName"] = BrandDetail[0];
                    }
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("GetBrandInfo::Loading", Ex, -11);
            }
            return Json(BrandDetail, JsonRequestBehavior.AllowGet);

        }

        public void VerifyForMobileBrowser()
        {
            try
            {
                if (IsMobileBrowser())
                {
                    Session["IsMobileBrowserResponsive"] = "yes";
                    // Session["IsMobileBrowserResponsive"] = System.Configuration.ConfigurationManager.AppSettings["MobileSalesPersonCode"].ToString();

                    string IsRedirectOldMobileSite = System.Configuration.ConfigurationManager.AppSettings["IsOldMobileTX"].ToString();
                    if (IsRedirectOldMobileSite == "1")
                    {
                        try
                        {
                            string[] queryStr = { };
                            string reDirectUrl = string.Empty;

                            if (Request.QueryString.ToString().Length > 0)
                            {
                                queryStr = Convert.ToString(Request.Url).Split('?');
                                reDirectUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MobileEnrollmentURL"]) + "?" + queryStr[1];
                            }
                            else
                            {
                                reDirectUrl = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["MobileEnrollmentURL"]);
                            }

                            Response.Redirect(reDirectUrl, false);
                        }
                        catch
                        {
                        }

                    }
                }
                else
                {
                    Session["IsMobileBrowserResponsive"] = "no";
                }
            }
            catch
            {

            }
        }

        public bool IsMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            // HttpContext context = (HttpContext)base.Request.RequestContext.HttpContext;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (Request.ServerVariables["HTTP_ACCEPT"] != null &&
                Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                    new[]
                {
                    "midp", "j2me", "avant", "docomo", 
                    "novarra", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "blackberry", "mib/", "symbian", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio", 
                    "SIE-", "SEC-", "samsung", "HTC", 
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx", 
                    "NEC", "philips", "mmm", "xx", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java", 
                    "pt", "pg", "vox", "amoi", 
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", 
                    "moto", "iphone","android","iPad","iPod","windowsphone","mobile"
                };

                //Loop through each item in the list created above 
                //and check if the header contains that text
                foreach (string s in mobiles)
                {
                    if (Request.ServerVariables["HTTP_USER_AGENT"].ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //Testing Method
        public ActionResult TestConformation(string resp)
        {
            try
            {
                string revertCustInfo = "";
                string revertMsg = "";
                string BrandTellNo = "";
                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                enrollmentResp = (EnrollResult)Session["enrollmentResp"];
                FunEnrollment funObj = new FunEnrollment();
                if (enrollmentObject != null)
                {
                    funObj.InternalRuleMes = enrollmentObject.InternalRuleMes;
                    funObj.ReasonCode = enrollmentObject.ReasonCode;
                    funObj.ProdDetails = enrollmentObject.ProdDetails;
                    funObj.depAmtResp = enrollmentObject.depAmtResp;
                    funObj.serviceAddressObj = enrollmentObject.serviceAddressObj;


                    if (TempData["PendingMsg"] != null)
                    {
                        if (!string.IsNullOrEmpty(TempData["PendingMsg"].ToString()))
                        {

                            clsDataLayer clsDL = new clsDataLayer();


                            string BrandCode = enrollmentObject.BrandCode;
                            BrandInfoReq brandInfoRequest = new BrandInfoReq();
                            AppInfo appInfo = new AppInfo();
                            appInfo = clsDL.GetAppInfo();
                            brandInfoRequest.appName = appInfo.appName;
                            brandInfoRequest.appVersion = appInfo.appVersion;
                            brandInfoRequest.dversion = -1;
                            brandInfoRequest.sessionID = clsDL.GetSessionID();
                            brandInfoRequest.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                            brandInfoRequest.BrandCode = BrandCode.ToString();
                            var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                            string jsonAccReq = s.Serialize(brandInfoRequest);
                            string resp2 = clsDL.ExecuteWebService(jsonAccReq, "/GetBrandInfo");
                            BrandInfoResp brandInfoResp = new BrandInfoResp();
                            BrandInfo brandInfo = new BrandInfo();
                            brandInfoResp = s.Deserialize<BrandInfoResp>(resp2);
                            if (brandInfoResp.requestStatus == 1)
                            {
                                BrandTellNo = brandInfoResp.BrandInfo[0].TollfreeNumber;
                            }
                            if (BrandTellNo.Contains("\r\n"))
                            {
                                BrandTellNo = BrandTellNo.Replace("\r\n", "");
                            }
                            TempData["BrandTellNo"] = BrandTellNo;
                            TempData["PendingMsgData"] = TempData["PendingMsg"].ToString();
                            TempData.Keep();
                        }
                    }
                    if (funObj.depAmtResp == null)
                    {
                        TempData["DepositAmount"] = "0";
                    }
                    else
                    {
                        TempData["DepositAmount"] = funObj.depAmtResp.depositAmount;
                    }
                    funObj.creditCardInfo = enrollmentObject.creditCardInfo;
                    funObj.personalInfo = enrollmentObject.personalInfo;
                    funObj.contactInfo = enrollmentObject.contactInfo;

                    if (enrollmentResp != null)
                    {
                        funObj.enrRes = enrollmentResp;
                        TempData["CustomerNumber"] = funObj.enrRes.pv_customer_no;
                        TempData["CustomerId"] = funObj.enrRes.recordID;
                    }

                    return View(funObj);
                }
                else
                {
                    //  return View();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrLog("Confirmation::Loading", Ex, -11);
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public JsonResult ClearPromo()
        {
            querStringObj.PromotionalCode = "";
            Session["Promocode"] = querStringObj.PromotionalCode;
            Session["RequestPage"] = "";
            TempData["Ref_ID"] = querStringObj.PromotionalCode;
            querStringObj.ref_id = enrollmentObject.Ref_Code = querStringObj.PromotionalCode;

            PropertyInfo isreadonly = typeof(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance | BindingFlags.NonPublic);
            // make collection editable
            isreadonly.SetValue(this.Request.QueryString, false, null);
            // remove
            this.Request.QueryString.Remove("PC");

            RedirectToAction("Index");
            return Json("Done", JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetAllTDSP(string zipcode)
        {
            TempData["newZipCode"] = zipcode;
            ZipCodeDetail zipcodeDretailres = new ZipCodeDetail();
            ZipDetailsListResp zipcodeRESP = new ZipDetailsListResp();
            ArrayList tdsp = new ArrayList();
            ArrayList tdspcode = new ArrayList();
            try
            {
                string PrevTDSP = "";

                enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
                if (enrollmentObject != null && enrollmentObject.ZipResp != null)
                {
                    PrevTDSP = enrollmentObject.ZipResp.TDSPCode.ToString();
                }
                TempData["NZipCode"] = zipcode;
                clsDataLayer clsDL = new clsDataLayer();
                ZipCodeDetail zipCodeDetail = new ZipCodeDetail();

                GovermentId govId = new GovermentId();
                AppInfo appInfo = new AppInfo();
                ZipDetailsResp Zip_Resp = new ZipDetailsResp();
                appInfo = clsDL.GetAppInfo();
                zipCodeDetail.appName = appInfo.appName;
                zipCodeDetail.appVersion = appInfo.appVersion;
                zipCodeDetail.dversion = -1;
                zipCodeDetail.sessionID = clsDL.GetSessionID();
                zipCodeDetail.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                zipCodeDetail.zipcode = zipcode;
                var ss = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonAccReq0 = ss.Serialize(zipCodeDetail);
                string res = clsDL.ExecuteWebService(jsonAccReq0, "/GetTDSP");

                if (res != null && res != "")
                {
                    zipcodeRESP = ss.Deserialize<ZipDetailsListResp>(res);
                }
                if (zipcodeRESP.zipDetailResp != null)
                {
                    for (int i = 0; i <= zipcodeRESP.zipDetailResp.Count - 1; i++)
                    {
                        //tdsp.Add(zipcodeRESP.zipDetailResp[i].tdspName.ToLower());
                        tdsp.Add(zipcodeRESP.zipDetailResp[i].tdspName);
                    }
                }
                if (!(tdsp.Count > 1))
                {
                    if (enrollmentObject != null && enrollmentObject.ZipResp != null)
                    {

                        if (!(tdsp.Contains(enrollmentObject.ZipResp.TDSPName.ToString().ToLower())))
                        {
                            tdsp.Clear();
                            tdsp.Add("diff");
                        }


                    }
                }

            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("GetTDSP::Load", Ex, -11, "Zipcode: " + zipcode, "");
            }
            Session["zipDetail"] = (ZipDetailsListResp)zipcodeRESP;
            return Json(tdsp, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult setTDSP(string TDSP)
        {
            string newTDSP = "";
            string resp = "";
            ZipDetailsListResp zipcodeRESP = new ZipDetailsListResp();
            try
            {
                zipcodeRESP = (ZipDetailsListResp)System.Web.HttpContext.Current.Session["zipDetail"];
                if (zipcodeRESP != null && zipcodeRESP.zipDetailResp != null)
                {
                    for (int i = 0; i <= zipcodeRESP.zipDetailResp.Count - 1; i++)
                    {
                        if (zipcodeRESP.zipDetailResp[i].tdspName.ToUpper() == TDSP.ToUpper())
                        {
                            TempData["newTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp.ToString();
                            if (TempData["setTDSP"] != null)
                            {
                                string prevtdsp = TempData["setTDSP"].ToString();
                                TempData.Keep();
                                //    if (prevtdsp != TDSP)
                                if (zipcodeRESP.zipDetailResp[i].tdsp != prevtdsp)
                                {
                                    resp = "diff";
                                    //   TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    // TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    TempData["changeTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp; ;
                                }

                                else
                                {
                                    resp = "same";
                                    TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    break;

                                }
                            }
                            else if (TempData["TDSPCODE"] != null)
                            {
                                string prevtdsp = TempData["TDSPCODE"].ToString();
                                TempData.Keep();
                                //    if (prevtdsp != TDSP)
                                if (zipcodeRESP.zipDetailResp[i].tdsp.ToLower() != prevtdsp.ToLower())
                                {
                                    resp = "diff";
                                    //TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    //TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    TempData["changeTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    //TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                }

                                else
                                {
                                    resp = "same";
                                    TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    break;

                                }
                            }
                            else if (TempData["newZipCode"] != null)
                            {
                                string prevtdsp = TempData["newZipCode"].ToString();
                                TempData.Keep();
                                //    if (prevtdsp != TDSP)
                                if (zipcodeRESP.zipDetailResp[i].tdsp != prevtdsp)
                                {
                                    resp = "diff";
                                    //TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    //TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    TempData["changeTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp; ;
                                }

                                else
                                {
                                    resp = "same";
                                    TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                                    break;

                                }
                            }
                            //else if (TempData["newZipCode"] != null)
                            //{
                            //}
                            //if (TempData["newZipCode"] != null)
                            //{
                            //    string newzipcode = TempData["newZipCode"].ToString();
                            //    if (zipcodeRESP.zipDetailResp[i].zipcode != null)
                            //    {
                            //        if (newzipcode != zipcodeRESP.zipDetailResp[i].zipcode)
                            //        {
                            //            resp = "diff";
                            //            TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //            TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //        }

                            //        else
                            //        {
                            //            resp = "same";
                            //            TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //        }
                            //    }
                            //    //else if (TempData["ZipCode"] != null)
                            //    //{
                            //    //    string Prevzipcode = TempData["ZipCode"].ToString();
                            //    //    if (newzipcode != Prevzipcode)
                            //    //    {
                            //    //        resp = "diff";
                            //    //        TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //    //        TempData["TDSPAccordAdd2"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //    //    }
                            //    //}
                            //    else
                            //    {

                            //        resp = "same";
                            //        TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //    }
                            // }

                            //else
                            //{
                            //    resp = "same";
                            //    TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;
                            //}

                            //}
                            //break;
                            //}
                            //else
                            //{
                            //    resp = "same";
                            //    //TempData["setTDSP"] = zipcodeRESP.zipDetailResp[i].tdsp;

                            //}

                            //TempData.Keep();
                        }
                        else
                        {
                            resp = "diff";

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                clsDtLayer.WriteErrorLog("setTDSP", ex, -11, TDSP, clsDtLayer.ObjectToJson(zipcodeRESP.zipDetailResp));
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ClearPromocode()
        {
            querStringObj.PromotionalCode = "";
            Session["Promocode"] = "";
            Session["RequestPage"] = "";
            TempData["Ref_ID"] = "";
            querStringObj.ref_id = enrollmentObject.Ref_Code = "";

            TempData["kWh"] = "";
            return Json("Done", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult CheckPromoValue(string pcode)
        {
            //string returnValue = "";
            string[] returnValue = new string[2];
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            if (enrollmentObject != null)
            {
                PartnerDetailsReq partnerDetailsReq = new PartnerDetailsReq();
                PartnerDetailsResp partnerDetailsResp = new PartnerDetailsResp();
                partnerDetailsReq.PartnerName = pcode;
                partnerDetailsReq.BrandCode = enrollmentObject.BrandCode;
                var s = new JavaScriptSerializer();
                string jsonAccReq = s.Serialize(partnerDetailsReq);
                string resp = clsDtLayer.ExecuteWebService(jsonAccReq, "/GetPartnerDetails");
                partnerDetailsResp = s.Deserialize<PartnerDetailsResp>(resp);

                if (partnerDetailsResp.requestMessage != "failure")
                {
                    returnValue[0] = "Done";
                }
                else
                {
                    returnValue[0] = "Fail";
                }

                string qs = "";
                if (TempData["kWh"] != null)
                {
                    string ul = TempData["kWh"].ToString();
                    TempData.Keep();
                    if (ul != "")
                    {
                        qs = "&ul=" + ul;
                    }

                }
                else
                {
                    qs = "";
                }
                if (TempData["TDSPAccordAdd"] != null)
                {
                    TempData["setTDSP"] = TempData["TDSPAccordAdd"].ToString();
                    if (qs != "")
                    {
                        qs += "&tdsp=" + TempData["TDSPAccordAdd"].ToString();
                    }
                    else
                    {
                        qs = "&tdsp=" + TempData["TDSPAccordAdd"].ToString();
                    }
                    TempData.Keep();
                }
                else
                {

                    if (TempData["TDSPCODE"] != null)
                    {
                        TempData["setTDSP"] = TempData["TDSPCODE"].ToString();
                        if (qs != "")
                        {
                            qs += "&tdsp=" + TempData["TDSPCODE"].ToString();
                        }
                        else
                        {
                            qs = "&tdsp=" + TempData["TDSPCODE"].ToString();
                        }
                    }
                    TempData.Keep();
                }

                returnValue[1] = qs;

            }
            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]

        public JsonResult AddEnrollinfo(FormCollection formCollection)
        {
            PersonalInformation pInfo = new PersonalInformation();
            ContactInformation cInfo = new ContactInformation();
            BillingAddress ba = new BillingAddress();
            BillingAddress pba = new BillingAddress();
            string firstname = "";
            string lastname = "";
            string emailid = "";
            string phoneno = "";
            string dob = "";
            string PlessComm = "";
            string preferLang = "";
            string isBill = "";
            firstname = formCollection["firstname"].Trim();
            lastname = formCollection["lastname"].Trim();
            emailid = formCollection["emailid"].Trim();
            phoneno = formCollection["phoneno"].Trim();
            dob = formCollection["dob"].Trim();
            PlessComm = formCollection["PlessComm"].Trim();
            preferLang = formCollection["preferLang"].Trim();
            isBill = formCollection["isbill"].Trim();
            string gopaperless = formCollection["IsPLessBilling"].Trim();
            string StreetName = formCollection["StreetName"].Trim();
            string StreetNum = formCollection["StreetNum"].Trim();
            string AptNum = formCollection["AptNum"].Trim();
            string CityName = formCollection["CityName"].Trim();
            string StateName = formCollection["StateName"].Trim();
            string BilllingZipCode = formCollection["ZipCode"].Trim();
            string ZipCode = BilllingZipCode;
            string IsPoBox = formCollection["IsPoBox"].Trim();
            string PoBox = formCollection["PoBox"].Trim();

            string PreferedLanguage = formCollection["PreferedLanguage"].Trim();

            string LocationByName = formCollection["LocationByName"];
            string IsMoving = formCollection["IsMoving"];
            string StreetNamePrevious = formCollection["StreetNamePrevious"].Trim();
            string APTPrevious = formCollection["APTPrevious"].Trim();
            string CityPrevious = formCollection["CityPrevious"].Trim();
            string StatePrevious = formCollection["StatePrevious"].Trim();
            string ZipCodePrevious = formCollection["ZipCodePrevious"].Trim();
            string switchDate = formCollection["switchDate"].Trim();
            string BillingZipCode = formCollection["BillingZipCode"].Trim();
            string ddlContactOptions = formCollection["ddlContactOptions"];
            string AnotherAuthrizeduser = formCollection["AnotherAuthrizedUser"].Trim();
            string AuthrizeduserFirstName = formCollection["AuthrizedUserFirstName"].Trim();
            string AuthrizeduserLastName = formCollection["AuthrizedUserLastName"].Trim();
            string AuthrizeduserContactNumber = formCollection["AuthrizedUserContactNumber"].Trim();
            string CarrierType = formCollection["ProviderType"].Trim();
            //string CreditCheck = formCollection["CreditCheck"];
            //string SocialSecurityType = formCollection["SocialSecurity"];
            //string ssn = formCollection["ssn"];
            //string GovermentIdNumber = formCollection["GovermentIdNumber"];
            //string GovIdType = formCollection["GovIdType"];
            //string GovIdState = formCollection["GovIdState"];
            string IsTCPa = formCollection["IsTCPA"];
            bool comm = false;
            bool Bill = false;
            if (PlessComm != null)
            {
                if (PlessComm.ToLower() == "yes")
                {
                    comm = true;
                }
                else
                {
                    comm = false;
                }
            }
            if (!(string.IsNullOrEmpty(isBill)))
            {
                if (isBill.ToLower() == "yes")
                {
                    Bill = true;
                }
                else
                {
                    Bill = false;
                }
            }
            bool Plbilling = false;

            if (gopaperless.ToLower() == "yes")
            {
                Plbilling = true;
            }
            else if (gopaperless.ToLower() == "no")
            {
                Plbilling = false;
            }
            pInfo.first_name = firstname;
            pInfo.last_name = lastname;
            pInfo.date_of_birth = dob;
            Session["pInfo"] = pInfo;
            cInfo.email = emailid;
            cInfo.contactno = phoneno;
            cInfo.eBill = comm;
            cInfo.langCode = preferLang;
            Session["cInfo"] = cInfo;
            enrollmentObject.personalInfo = pInfo;
            enrollmentObject.contactInfo = cInfo;
            enrollmentObject.isBillingSame = Bill;

            ba.AptNum = AptNum;
            ba.CityName = CityName;
            ba.StreetName = StreetName;
            ba.StreetNum = StreetNum;
            ba.StateName = StateName;
            ba.ZipCode = ZipCode;
            if (IsPoBox.ToLower() == "yes")
            {
                Session["IsPoBox"] = "yes";
                Session["PoBoxno"] = PoBox;
            }
            else
            {
                Session["IsPoBox"] = "no";
            }
            enrollmentObject.billingAddress = ba;
            if (IsMoving.ToLower() == "yes")
            {
                enrollmentObject.isPreviousAddress = "Yes";
                pba.AptNum = APTPrevious;
                pba.StreetName = StreetNamePrevious;
                pba.CityName = CityPrevious;
                pba.StateName = StatePrevious;
                pba.ZipCode = ZipCodePrevious;
            }
            else
            {
                enrollmentObject.isPreviousAddress = "No";
                pba.AptNum = "";
                pba.StreetName = ""; ;
                pba.CityName = "";
                // pba.StreetName = "";
                pba.ZipCode = "";
            }
            if (switchDate != null)
            {
                Session["switchDate"] = switchDate.ToString();
            }
            else
            {
                Session["switchDate"] = "";
            }
            Session["AuthrizeUser"] = AnotherAuthrizeduser;
            if (AnotherAuthrizeduser.ToLower() == "yes")
            {
                enrollmentObject.AuthrizedUserFirstName = AuthrizeduserFirstName;
                enrollmentObject.AuthrizeduserLastName = AuthrizeduserLastName;
                enrollmentObject.AuthrizeduserContactNumber = AuthrizeduserContactNumber;
            }
            //if (CreditCheck.ToLower() == "yes")
            //{
            //    enrollmentObject.creditCheckYesNo = true;

            //}
            //else
            //{
            //    enrollmentObject.creditCheckYesNo = false;
            //}
            enrollmentObject.IsPaperless = Plbilling;
            enrollmentObject.previousAddress = pba;
            Session["enrollmentObject2"] = enrollmentObject;
            EnrollmentData enrollmentObject2 = new EnrollmentData();
            enrollmentObject2 = (EnrollmentData)Session["enrollmentObject"];
            if (enrollmentObject2 != null && enrollmentObject2.ZipResp != null)
            {
                TempData["ZipCode"] = enrollmentObject2.ZipResp.ZipCode;

            }
            return Json("Done", JsonRequestBehavior.AllowGet);
        }
        public string getTDSP(string NewZipCode)
        {
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            string tdspcode = "";
            try
            {
                clsDataLayer clsDL = new clsDataLayer();
                ZipCodeDetail zipCodeDetail = new ZipCodeDetail();
                ZipDetailsListResp zipcodeRESP = new ZipDetailsListResp();
                GovermentId govId = new GovermentId();
                AppInfo appInfo = new AppInfo();

                appInfo = clsDL.GetAppInfo();
                zipCodeDetail.appName = appInfo.appName;
                zipCodeDetail.appVersion = appInfo.appVersion;
                zipCodeDetail.dversion = -1;
                zipCodeDetail.sessionID = clsDL.GetSessionID();
                zipCodeDetail.ipAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                zipCodeDetail.zipcode = NewZipCode;
                var ss = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonAccReq0 = ss.Serialize(zipCodeDetail);

                string res = clsDL.ExecuteWebService(jsonAccReq0, "/GetTDSP");

                if (res != null && res != "")
                {
                    zipcodeRESP = ss.Deserialize<ZipDetailsListResp>(res);
                    if (zipcodeRESP != null && zipcodeRESP.zipDetailResp != null && zipcodeRESP.zipDetailResp.Count > 0)
                    {
                        tdspcode = zipcodeRESP.zipDetailResp[0].tdsp;
                    }
                    else
                    {
                        tdspcode = "";
                    }
                }
            }
            catch (Exception Ex)
            {
                clsDtLayer.WriteErrorLog("GetTDSP::ByZip", Ex, -11, "ZipCode:" + NewZipCode, "");
            }

            return tdspcode;
        }
        [HttpPost]
        public JsonResult checkDifferTDSP(string address, string zipcode)
        {
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            string revert = "";
            string tdspcode = getTDSP(zipcode);
            if (enrollmentObject != null && enrollmentObject.ZipResp != null)
            {
                if (tdspcode == enrollmentObject.ZipResp.TDSPCode.ToUpper())
                {
                    revert = "same";
                }
                else
                {
                    revert = "new";
                    TempData["changeTDSP"] = tdspcode;
                }
            }
            else
            {
                revert = "first";
            }

            return Json(revert.ToLower(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult changeQstring(string zipcode)
        {
            string revert = "";
            string tdsp = getTDSP(zipcode);
            enrollmentObject = (EnrollmentData)Session["enrollmentObject"];
            //string qs = "";
            string[] qs = new string[3];

            //TempData["Address"] = null;
            try
            {
                if (enrollmentObject != null && enrollmentObject.QueryString != null && enrollmentObject.QueryString != "")
                {
                    string url = Server.HtmlDecode(enrollmentObject.QueryString.ToString().ToLower());
                    if (TempData["URLTDSP"] != null)
                    {
                        string urltdsp = TempData["URLTDSP"].ToString().ToLower();
                        if (TempData["changeTDSP"] != null)
                        {
                            string changeTDSP = TempData["changeTDSP"].ToString();
                            TempData.Keep();
                            TempData["setTDSP"] = changeTDSP;
                            TempData["Address"] = null;
                            qs[0] = url.Replace("tdsp=" + urltdsp, "tdsp=" + changeTDSP);
                            TempData.Keep();

                        }

                        else if (TempData["setTDSP"] != null)
                        {

                            tdsp = TempData["setTDSP"].ToString();
                            if (tdsp == "")
                            {
                                if (TempData["TDSPAccordAdd"] != null)
                                {
                                    tdsp = TempData["TDSPAccordAdd"].ToString();

                                    TempData["setTDSP"] = tdsp;

                                }
                            }
                            else if (TempData["TDSPAccordAdd"] != null)
                            {
                                tdsp = TempData["TDSPAccordAdd"].ToString();

                                TempData["setTDSP"] = tdsp;

                            }
                            qs[0] = url.Replace("tdsp=" + urltdsp, "tdsp=" + tdsp);
                            TempData.Keep();
                        }
                        else
                        {
                            if (TempData["TDSPAccordAdd"] != null)
                            {
                                tdsp = TempData["TDSPAccordAdd"].ToString();
                                qs[0] = url.Replace("tdsp=" + urltdsp, "tdsp=" + tdsp);
                                //qs[0] = url + "&tdsp=" + TempData["TDSPAccordAdd"].ToString();
                                TempData.Keep();
                                //TempData["Address"] = TempData["Addr"].ToString();
                                //TempData["setTDSP"] = TempData["URLTDSP"].ToString();
                            }
                            else if (TempData["setTDSP"] != null)
                            {
                                qs[0] = url + "&tdsp=" + TempData["setTDSP"].ToString();
                                TempData.Keep();
                            }
                            else if (TempData["newTDSP"] != null)
                            {
                                qs[0] = url + "&tdsp=" + TempData["newTDSP"].ToString();
                                TempData.Keep();
                            }
                            else
                            {
                                urltdsp = TempData["URLTDSP"].ToString().ToLower();
                                TempData["setTDSP"] = tdsp;
                                qs[0] = url.Replace("tdsp=" + urltdsp, "tdsp=" + tdsp);
                            }
                            //TempData["setTDSP"] = TempData["TDSPAccordAdd"].ToString();
                            // TempData["Address"] = TempData["Addr"].ToString();
                            TempData.Keep();
                            //TempData["ZipCode"]
                        }

                    }
                    else
                    {
                        if (TempData["TDSPAccordAdd"] != null)
                        {
                            qs[0] = url + "&tdsp=" + TempData["TDSPAccordAdd"].ToString();
                            TempData.Keep();
                            //TempData["Address"] = TempData["Addr"].ToString();
                            //TempData["setTDSP"] = TempData["URLTDSP"].ToString();
                        }
                        else if (TempData["setTDSP"] != null)
                        {
                            qs[0] = url + "&tdsp=" + TempData["setTDSP"].ToString();
                            TempData.Keep();
                        }
                        else if (TempData["newTDSP"] != null)
                        {
                            qs[0] = url + "&tdsp=" + TempData["newTDSP"].ToString();
                            TempData.Keep();
                        }
                    }
                    if (TempData["URLZipCode"] != null)
                    {
                        string urlzipcode = TempData["URLZipCode"].ToString().ToLower();
                        string newzipcode = zipcode;
                        //string url = Server.HtmlDecode(enrollmentObject.QueryString.ToString().ToLower());
                        if (!(string.IsNullOrEmpty(qs[0])))
                        {
                            qs[0] = qs[0].Replace("zip=" + urlzipcode, "zip=" + newzipcode);
                        }
                        // TempData["setTDSP"] = TempData["URLTDSP"].ToString();
                        //TempData["setTDSP"] = TempData["TDSPAccordAdd"].ToString();
                        // TempData["Address"] = TempData["Addr"].ToString();
                        qs[1] = tdsp;
                        querStringObj = (QueryStringObject)Session["queryStringObj"];
                        if (querStringObj != null)
                        {
                            string ref_id = querStringObj.ref_id;
                            if (!(string.IsNullOrEmpty(ref_id)))
                            {
                                qs[2] = ref_id;
                            }
                        }
                        TempData.Keep();

                    }
                    else
                    {


                        qs[0] = qs[0] + "&zip=" + zipcode;
                        TempData.Keep();


                    }
                }
                else
                {
                    if (TempData["changeTDSP"] != null)
                    {
                        string settdsp = TempData["changeTDSP"].ToString();
                        TempData["setTDSP"] = settdsp;
                        TempData.Keep();
                        qs[1] = settdsp;
                        TempData["Address"] = null;
                    }
                    else if (TempData["TDSPAccordAdd"] != null)
                    {
                        string settdsp = TempData["TDSPAccordAdd"].ToString();
                        TempData["setTDSP"] = settdsp;
                        TempData.Keep();
                        qs[1] = settdsp;
                    }


                    else if (TempData["TDSPAccordAdd2"] != null)
                    {
                        string settdsp = TempData["TDSPAccordAdd2"].ToString();
                        TempData["setTDSP"] = settdsp;
                        TempData.Keep();
                        qs[1] = settdsp;
                    }
                }
            }
            catch (Exception ex)
            {

                clsDtLayer.WriteErrorLog("changeQstring", ex, -11, zipcode, clsDtLayer.ObjectToJson(enrollmentObject));
            }
            return Json(qs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckSessionValidity()
        {
            if (Session["enrollmentObject"] == null)
            {
                Session.RemoveAll();
                Session.Abandon();
                return Json("False");
            }

            return Json("True");
        }
        public ActionResult cleanAddress()
        {
            if (TempData["Address"] != null)
            {
                TempData["Address"] = null;
            }

            return Json("True");
        }
    }

}

