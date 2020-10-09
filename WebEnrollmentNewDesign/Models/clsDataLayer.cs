using FUNWebEnrollment;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace WebEnrollmentNewDesign.Models
{

    public class clsDataLayer
    {
        public string Constr = string.Empty;
        public string ExecuteWebService(string reqString, string methodName)
        {
            string url = System.Configuration.ConfigurationManager.AppSettings["WCFServiceURL"].ToString();
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            string result = string.Empty;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(url + methodName);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.Timeout = 90000;
                //req.ContentLength = reqString.Length;
                req.ContentLength = Encoding.UTF8.GetByteCount(reqString);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(req.GetRequestStream());
                sw.Write(reqString);
                sw.Flush();
                sw.Close();
                res = (HttpWebResponse)req.GetResponse();
                if (res != null && res.CharacterSet != null)
                {
                    Encoding responseEncoding = Encoding.GetEncoding(res.CharacterSet);
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), responseEncoding))
                    {
                        result = sr.ReadToEnd();
                    }
                }

            }
            catch (Exception ex)
            {
                //WriteErrLog("WSCall:" + methodName, ex, -11, reqString, res.ToString());
                WriteErrLog("WSCall:ExecuteWebService", ex, -11, "", "");
            }
            return result;
        }
        public void WriteErrLog(string method, Exception exp, int errType, string request = null, string response = null)
        {
            try
            {
                if ((exp.Message != "Thread was being aborted."))
                {
                    string BrandCode = System.Configuration.ConfigurationManager.AppSettings["BrandCode"].ToString();
                    string IsMobile = "";
                    
                    if (HttpContext.Current.Session["IsMobileBrowserResponsive"] != null)
                    {
                        if (HttpContext.Current.Session["IsMobileBrowserResponsive"].ToString() == "no")
                        {
                            IsMobile = "Web";
                        }
                        else
                        {
                            IsMobile = "Mobile";
                        }
                    }
                    
                    int line = (new StackTrace(exp, true)).GetFrame(0).GetFileLineNumber();
                    FUNBaseRequest baseReq = new FUNBaseRequest();
                    baseReq = FillBaseRequestInfo();
                    ErrorLogger errLog = new ErrorLogger();
                    errLog.FunName = method + "::" + BrandCode + "::" + IsMobile;
                    errLog.ExceptionText = exp.Message + " @Line no:" + line + exp.StackTrace;
                    errLog.errorType = errType;
                    errLog.AccountNo = null;
                    errLog.appName = baseReq.appName;
                    errLog.appVersion = baseReq.appVersion;
                    errLog.deviceID = baseReq.deviceID;
                    errLog.ipAddress = baseReq.ipAddress;
                    errLog.dversion = baseReq.dversion;
                    errLog.ResData = response;
                    errLog.ReqData = request;
                    JavaScriptSerializer j = new JavaScriptSerializer();
                    string strSer = j.Serialize(errLog);
                    string strRet = ExecuteWebService(strSer, "/ErrorLogger");
                }
            }
            catch (Exception ex)
            {
                WriteErrLog("DataLayer:WriteErrLog", ex, -11, "", "");
            }
           
        }
        public string browserInfo(System.Web.HttpBrowserCapabilitiesBase browser)
        {
            string s = "Type=" + browser.Type + ";"
                + "Name=" + browser.Browser + ";"
                + "Version=" + browser.Version + ";"
                + "Platform=" + browser.Platform;
            return s;
        }
        public FUNBaseRequest FillBaseRequestInfo()
        {
            FUNBaseRequest BaseReq = new FUNBaseRequest();
            AppInfo appinfo = GetAppInfo();
            BaseReq.appName = appinfo.appName;
            BaseReq.appVersion = appinfo.appVersion;
            //BaseReq.deviceID = "";
            BaseReq.sessionID = GetSessionID();
            BaseReq.dversion = -1;
            BaseReq.ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return BaseReq;
        }
        //public class AppInfo
        //{
        //    public string appName { get; set; }
        //    public string appVersion { get; set; }
        //}
        public AppInfo GetAppInfo()
        {
            var appInfo = new AppInfo();
            appInfo.appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            appInfo.appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                 System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
                                 System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.MajorRevision.ToString() + "." +
                                 System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.MinorRevision.ToString();
            return appInfo;
        }
        public string GetSessionID()
        {
            HttpSessionState ss = HttpContext.Current.Session;
            //HttpContext.Current.Session[SessKey] = "test";
            return ss.SessionID;
        }
        public string ObjectToJson(object obj)
        {
            if (obj != null)
            {
                var json = new JavaScriptSerializer().Serialize(obj);
                return json;
            }
            else
            {
                var json = "";
                return json;
            }
        }
        #region Utils

        string _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";

        public bool IsUsZipCode(string zipCode)
        {
            bool validZipCode = true;
            if (!Regex.Match(zipCode, _usZipRegEx).Success)
            {
                validZipCode = false;
            }
            return validZipCode;
        }
        public bool IsAllowIPAddress()
        {
            bool isAllow = true;
            try
            {
                FUNBaseRequest baseReq = new FUNBaseRequest();
                baseReq.ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                var serialize = new JavaScriptSerializer();
                string jsonReq = serialize.Serialize(baseReq);
                string resp = ExecuteWebService(jsonReq, "/IsAllowIPAddress");
                var isAllowIP = new IsAllowIPAddressResponse();
                if (resp != null)
                {
                    isAllowIP = serialize.Deserialize<IsAllowIPAddressResponse>(resp);
                    isAllow = isAllowIP.IsAllowIPAddress;
                }

                return isAllow;
            }
            catch (Exception ex)
            {
                WriteErrLog("DataLayer:IsAllowIPAddress", ex, -11, "", "");
            }
            return isAllow;
        }
        #endregion

        #region MileStone
        public void InsertMileStoneDetails(string MileStone, string Value = null, string Source = "WebTX")
        {
            try
            {
                string Brandcode = System.Configuration.ConfigurationManager.AppSettings["BrandCode"].ToString();
                FUNBaseRequest bReq = FillBaseRequestInfo();
                OpenDBConnectionFUNWeb("FUNEnrollment");
                SqlCommand command = new SqlCommand();
                command.CommandType = CommandType.StoredProcedure;
              //  command.CommandText = "sp_InsertMileStone";
                //command.CommandText = "sp_InsertMileStone_ver2";
                command.CommandText = "sp_InsertMileStone_New";
                command.Parameters.Add("@Source", SqlDbType.VarChar).Value = Source;
                command.Parameters.Add("@SessionId", SqlDbType.VarChar).Value = bReq.sessionID;
                command.Parameters.Add("@MileStone", SqlDbType.VarChar).Value = MileStone;
                command.Parameters.Add("@Value", SqlDbType.VarChar).Value = Value;
                command.Parameters.Add("@Brandcode", SqlDbType.VarChar).Value = Brandcode;
                command.Connection = mDBConnectionFUNWeb;
                command.ExecuteNonQuery();
                mDBConnectionFUNWeb.Close();
            }
            catch (Exception ex)
            {
                mDBConnectionFUNWeb.Close();
                WriteErrorLog("InsertMileStoneDetails", ex, -11, "", "");
            }
        }
        #endregion

        #region Connection
        SqlConnection mDBConnectionFUNWeb;
        private void OpenDBConnectionFUNWeb(string connectionName)
        {
            try
            {
                if (mDBConnectionFUNWeb == null || mDBConnectionFUNWeb.State != ConnectionState.Open)
                {
                    string connectionString;
                    connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
                    mDBConnectionFUNWeb = new SqlConnection(connectionString);
                    mDBConnectionFUNWeb.Open();
                }
            }
            catch (Exception exp)
            {
                //eventLogger.WriteToFileLog("mDBConnectionFUNWeb:: Exception: " + exp.Message);
            }
            //finally
            //{
            //    try
            //    {
            //        if (mDBConnectionFUNWeb == null || mDBConnectionFUNWeb.State != ConnectionState.Open)
            //        {
            //            string connectionString;
            //            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            //            mDBConnectionFUNWeb = new SqlConnection(connectionString);
            //            mDBConnectionFUNWeb.Open();
            //        }
            //    }
            //    catch (Exception exp)
            //    {
            //        //eventLogger.WriteToFileLog("mDBConnectionFUNWeb:: Exception: " + exp.Message);
            //    }
            //}
        }
        #endregion

        public void FunTracker_Log(string method, string reqData)
        {
            try
            {
                FUNTrackerInfo funTrac = new FUNTrackerInfo();
                JavaScriptSerializer s = new JavaScriptSerializer();
                FUNBaseRequest bReq = FillBaseRequestInfo();
                funTrac.appName = bReq.appName;
                funTrac.appVersion = bReq.appVersion;
                funTrac.deviceID = bReq.deviceID;
                funTrac.dversion = bReq.dversion;
                funTrac.ipAddress = bReq.ipAddress;
                funTrac.sessionID = bReq.sessionID;
                funTrac.methodName = method;
                funTrac.requestData = reqData;
                string ser_str = s.Serialize(funTrac);
                string ret_str = ExecuteWebService(ser_str, "/FUNTracker");
                FUNBaseResponse tracResp = s.Deserialize<FUNBaseResponse>(ret_str);  //clsDtLayer.EnrollCustomer(retXml);
                if (tracResp != null)
                {

                    if (tracResp.requestStatus == 0)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("FunTracker_Log", ex, -11, "", "");
            }
        }

        #region ErrorLogger
        public void WriteErrInfo(string method, string errorMsg, int errType)
        {
            try
            {
                FUNBaseRequest baseReq = new FUNBaseRequest();
                baseReq = FillBaseRequestInfo();
                ErrorLogger errLog = new ErrorLogger();
                errLog.FunName = method;
                errLog.ExceptionText = errorMsg;
                errLog.errorType = errType;
                errLog.AccountNo = null;
                errLog.appName = baseReq.appName;
                errLog.appVersion = baseReq.appVersion;
                errLog.deviceID = baseReq.deviceID;
                errLog.dversion = baseReq.dversion;
                JavaScriptSerializer j = new JavaScriptSerializer();
                string strSer = j.Serialize(errLog);
                string strRet = ExecuteWebService(strSer, "/ErrorLogger");
            }
            catch (Exception ex)
            {
                WriteErrorLog("WriteErrInfo", ex, -11, "", "");
            }
           
        }
        //public void WriteErrLog(string method, Exception exp, int errType, string request = null, string response = null)
        //{
        //    if ((exp.Message != "Thread was being aborted."))
        //    {
        //        int line = (new StackTrace(exp, true)).GetFrame(0).GetFileLineNumber();
        //        FUNBaseRequest baseReq = new FUNBaseRequest();
        //        baseReq = FillBaseRequestInfo();
        //        ErrorLogger errLog = new ErrorLogger();
        //        errLog.FunName = method;
        //        errLog.ExceptionText = exp.Message + " @Line no:" + line + exp.StackTrace;
        //        errLog.errorType = errType;
        //        errLog.AccountNo = null;
        //        errLog.appName = baseReq.appName;
        //        errLog.appVersion = baseReq.appVersion;
        //        errLog.deviceID = baseReq.deviceID;
        //        errLog.ipAddress = baseReq.ipAddress;
        //        errLog.dversion = baseReq.dversion;
        //        errLog.ResData = response;
        //        errLog.ReqData = request;
        //        JavaScriptSerializer j = new JavaScriptSerializer();
        //        string strSer = j.Serialize(errLog);
        //        string strRet = ExecuteWebService(strSer, "/ErrorLogger");
        //    }
        //}
        public void WriteErrorLog(string method, Exception exp, int errType, string request, string response)
        {
            try
            {
                string expmessage = exp != null ? exp.Message : "";
                if ((expmessage != "Thread was being aborted."))
                {
                    int line = 0;
                    if (exp != null)
                    {
                        line = (new StackTrace(exp, true)).GetFrame(0).GetFileLineNumber();
                    }
                    FUNBaseRequest baseReq = new FUNBaseRequest();
                    baseReq = FillBaseRequestInfo();
                    ErrorLogger errLog = new ErrorLogger();
                    errLog.FunName = method;
                    if (exp != null)
                    {
                        errLog.ExceptionText = exp.Message + "@Line No: " + line + "Exception: " + exp.StackTrace; 
                    }
                    errLog.errorType = errType;
                    errLog.AccountNo = null;
                    errLog.appName = baseReq.appName;
                    errLog.appVersion = baseReq.appVersion;
                    errLog.deviceID = baseReq.deviceID;
                    errLog.ipAddress = baseReq.ipAddress;
                    errLog.dversion = baseReq.dversion;
                    errLog.ReqData = request;
                    errLog.ResData = response;
                    JavaScriptSerializer j = new JavaScriptSerializer();
                    string strSer = j.Serialize(errLog);
                    string strRet = ExecuteWebService(strSer, "/ErrorLogger");
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog("WriteErrorLog", ex, -11, "", "");
            }          
        }
        #endregion
        #region GetZIPBYIP
        public string GetZipByIP(string ipAddress)
        {
            //string url = System.Configuration.ConfigurationManager.AppSettings["WCFServiceURL"].ToString();
           // "98.196.174.47"

            //ipAddress = "98.196.174.47";
           //ipAddress="66.194.222.66";
            //ipAddress = "172.58.99.190";
            string url = System.Configuration.ConfigurationManager.AppSettings["IptoZip"].ToString();
            string key = System.Configuration.ConfigurationManager.AppSettings["KeyforZip"].ToString();
            string completeURL = url + ipAddress + "?access_key=" + key;
            HttpWebRequest req = null;
            HttpWebResponse res = null;
            string result = string.Empty;
            try
            {
                req = (HttpWebRequest)WebRequest.Create(completeURL);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.Timeout = 90000;
                req.ReadWriteTimeout = 99000;
                //req.ContentLength = reqString.Length;
                //req.ContentLength = Encoding.UTF8.GetByteCount(reqString);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(req.GetRequestStream());
                //sw.Write(reqString);
                sw.Flush();
                sw.Close();
                res = (HttpWebResponse)req.GetResponse();
                if (res != null && res.CharacterSet != null)
                {
                    Encoding responseEncoding = Encoding.GetEncoding(res.CharacterSet);
                    using (StreamReader sr = new StreamReader(res.GetResponseStream(), responseEncoding))
                    {
                       
                        result = sr.ReadToEnd();
                    }
                }

            }
            catch (Exception ex)
            {
                WriteErrLog("WSCall:GetZipByIP", ex, -11, "", "");
            }
            return result;
        }
        #endregion
    }
}