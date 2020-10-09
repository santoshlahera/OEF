using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using WebEnrollmentNewDesign.Models;
using WebEnrollmentNewDesign.PayWSDLService;
using System.Net;
namespace WebEnrollmentNewDesign.Controllers
{
    public class OEFPaymentFlowController : Controller
    {
        //
        // GET: /Payment/

        public ActionResult OEFPayment()
        {
            Session["PaymentSessionId"] = GetUniqueKey(10);
            return View();
        }
        public ActionResult MobileQuickPay()
        {
            Session["PaymentSessionId"] = GetUniqueKey(10);
            return View();
        }

        public ActionResult GetLocations(string AccountNo, string zip, [DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                return Json(GetAlllocations(AccountNo, zip).ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
              //  Common.InsertClientErrorLog(AccountNo, string.Empty, "MyAccountWeb - PaymentUI", exp, "PaymentUI - GetLocations");
                return null;
            }
        }

        public IEnumerable<PaymentLocation> GetAlllocations(string AccountNo, string zip)
        {
            var resp = new List<PaymentLocation>();
            var payment = new MyPaymentLocationsRequest
            {
                longitude = 0,
                latitude = 0,
                zip = zip
            };
            if (AccountNo != null)
            {
                PayWSDBClient objFPClient = new PayWSDBClient();
                try
                {
                    resp = objFPClient.MyPaymentLocations(AccountNo, zip);
                }
                catch (Exception exp)
                {
                   // Common.InsertClientErrorLog(AccountNo, string.Empty, "MyAccountWeb", exp, "PaymentUI - GetAlllocations");
                }
            }
            return resp;
        }

        [HttpPost]
        public ActionResult MakePayment(CreditCardPaymentRequest objCCReq)
        {
            var resp = new PaymentResponse();
            if (HttpContext.Session["PaymentSessionId"] != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                objCCReq.AuthKey = System.Configuration.ConfigurationManager.AppSettings["MyAccountAuthKey"];
                if (objCCReq.PaymentorAutopay == "PayNow")
                {
                    try
                    {
                        resp = Common.FrontierPayMakePayment(objCCReq);
                        if (objCCReq.SetupAutoPay && resp.ResultCode == 1)
                        {
                            PaymentResponse profileResp = new PaymentResponse();
                            profileResp = Common.ManagePaymentProfile(objCCReq);
                            resp.profileStatusCode = profileResp.profileStatusCode;
                            resp.profileStatusMessage = profileResp.profileStatusMessage;
                            return Json(resp, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            resp.profileStatusCode = -1;
                            resp.profileStatusMessage = "";
                            return Json(resp, JsonRequestBehavior.AllowGet);
                        }

                    }
                    catch (Exception exp)
                    {
                        resp.ResultCode = -3; //Exception
                        resp.ResultMessage = exp.Message;
                        Common.InsertClientErrorLog(objCCReq.CustomerNumber, string.Empty, objCCReq.Source, exp, "PaymentUI - MakePayment");
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (objCCReq.PaymentorAutopay == "AutoPay")
                {
                    try
                    {
                        resp = Common.ManagePaymentProfile(objCCReq);
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception exp)
                    {
                        resp.profileStatusCode = -3; //Exception
                        resp.profileStatusMessage = exp.Message;
                        Common.InsertClientErrorLog(objCCReq.CustomerNumber, string.Empty, objCCReq.Source, exp, "PaymentUI - MakePayment(AuthoPay)");
                        return Json(resp, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(resp, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resp.ResultCode = -100;
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FrontierPayCheckPayment(CheckInfo req)
        {
            var payResponse = new PaymentResponse();
            if (HttpContext.Session["PaymentSessionId"] != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                req.AuthKey = System.Configuration.ConfigurationManager.AppSettings["MyAccountAuthKey"];
                try
                {
                    if (req.PaymentorAutopay == "PayNow")
                    {
                        payResponse = Common.FrontierCheckPayment(req);
                        return Json(payResponse, JsonRequestBehavior.AllowGet);
                    }
                    else if (req.PaymentorAutopay == "AutoPay")
                    {
                        payResponse = Common.SetupACHAutoPay(req);
                        return Json(payResponse, JsonRequestBehavior.AllowGet);
                    }
                    return Json(payResponse, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exp)
                {
                    payResponse.ResultCode = -3; //Exception
                    payResponse.ResultMessage = exp.Message;
                    Common.InsertClientErrorLog(req.CustomerNumber, string.Empty, req.Source, exp, "PaymentUI - FrontierPayCheckPayment");
                    return Json(payResponse, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                payResponse.ResultCode = -100;
                return Json(payResponse, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FrontierPayDeletePaymentProfile(ProfilePaymentRequest request)
        {
            var resp = new PaymentResponse();
            if (HttpContext.Session["PaymentSessionId"] != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AuthKey = System.Configuration.ConfigurationManager.AppSettings["MyAccountAuthKey"];
                try
                {
                    if (request.Cust_no != null)
                    {
                        resp = Common.FrontierPayDeletePaymentProfile(request);
                    }
                    else
                    {
                        resp.profileStatusCode = -1;
                        resp.profileStatusMessage = "Invalid Session";
                    }

                    return Json(resp, JsonRequestBehavior.AllowGet);

                }
                catch (Exception exp)
                {
                    resp.profileStatusCode = -3; //Exception
                    resp.profileStatusMessage = exp.Message;
                    Common.InsertClientErrorLog(request.Cust_no, string.Empty, request.Source, exp, "PaymentUI - FrontierPayDeletePaymentProfile");
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                resp.ResultCode = -100;
                return Json(resp, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult FrontierPayByProfile(ProfilePaymentRequest request)
        {
            var resp = new PaymentResponse();
            if (HttpContext.Session["PaymentSessionId"] != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AuthKey = System.Configuration.ConfigurationManager.AppSettings["MyAccountAuthKey"];
                try
                {
                    resp = Common.FrontierPayByProfile(request);
                    return Json(resp, JsonRequestBehavior.AllowGet);

                }
                catch (Exception exp)
                {
                    resp.profileStatusCode = -3; //Exception
                    resp.profileStatusMessage = exp.Message;
                    Common.InsertClientErrorLog(request.Cust_no, string.Empty, request.Source, exp, "PaymentUI - FrontierPayByProfile");
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                resp.ResultCode = -100;
                return Json(resp, JsonRequestBehavior.AllowGet);
            }
        }

        #region"Prepay Wizard"

        [HttpPost]
        public ActionResult PrepayWizad(string accountno)
        {
            PayWSDBClient objFPClient = new PayWSDBClient();
            var resp = new PrepayResponse();
            try
            {
                resp = objFPClient.PrepayWizad(accountno);
            }
            catch (Exception ex)
            {
                Common.InsertClientErrorLog(accountno, "", "MyAccountWeb", ex, "PrepayWizad");
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult MobilePay()
        {
            Session["PaymentSessionId"] = GetUniqueKey(10);
            return View();
        }

        public ActionResult UnRegisterPay()
        {
            Session["PaymentSessionId"] = GetUniqueKey(10);
            return View();
        }

        [HttpPost]
        public ActionResult FrontierPayCreateProfile(CheckPaymentRequest request)
        {
            var resp = new PaymentResponse();
            if (HttpContext.Session["PaymentSessionId"] != null)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                request.AuthKey = System.Configuration.ConfigurationManager.AppSettings["MyAccountAuthKey"];
                try
                {
                    resp = Common.CreateProfile(request);
                    return Json(resp, JsonRequestBehavior.AllowGet);

                }
                catch (Exception exp)
                {
                    resp.profileStatusCode = -3; //Exception
                    resp.profileStatusMessage = exp.Message;
                    Common.InsertClientErrorLog("", string.Empty, request.Source, exp, "PaymentUI - FrontierPayCreateProfile");
                    return Json(resp, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                resp.ResultCode = -100;
                return Json(resp, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public ActionResult InsertPCITransactionLog(string Source, string PCISessionId, string CustomerNumber)
        {
            string ChildId = Guid.NewGuid().ToString();
            Common.InsertPCITransactionLog(Source, PCISessionId, ChildId, CustomerNumber);
            PCITransactionLog objpcilog = new PCITransactionLog();
            objpcilog.ChildUniqueId = ChildId;
            objpcilog.PCISessionId = PCISessionId;
            objpcilog.Source = Source;
            objpcilog.CustomerNumber = CustomerNumber;
            return Json(objpcilog, JsonRequestBehavior.AllowGet);
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
    }
}
