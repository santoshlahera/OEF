using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using System.Text;


/// <summary>
/// Summary description for EventLog
/// </summary>
/// 
// v1.0.0 - 09/23/2012  Baseline
// Added - File logger (eventLogger.WriteToFileLog("message" );)
// v1.0.1 - 09/24/2012
// Added - Log folder creation

public class EventLogger
{
    public EventLogger()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static string MappedApplicationPath
    {
        get
        {
            string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
            if (APP_PATH == "/")      //a site
                APP_PATH = "/";
            else if (!APP_PATH.EndsWith(@"/")) //a virtual
                APP_PATH += @"/";

            string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
            if (!it.EndsWith(@"\"))
                it += @"\";
            return it;
        }
    }

    public void WriteToFileLog(string strLogEntry)
    {

        string sErrorTime;

        string sYear = DateTime.Now.Year.ToString();
        string sMonth = DateTime.Now.Month.ToString();
        string sDay = DateTime.Now.Day.ToString();
        sErrorTime = sMonth + sDay + sYear;
        string filepath = MappedApplicationPath;

        if (!Directory.Exists(filepath + "Log"))
        {
            Directory.CreateDirectory(filepath + "Log");
        }

        string strLogFile = MappedApplicationPath + "Log/" + "Log" + sErrorTime + ".txt";

        StreamWriter swLog;


        strLogEntry = string.Format("{0}: {1}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"), strLogEntry);
        if (!File.Exists(strLogFile))
        {
            swLog = new StreamWriter(strLogFile);
        }
        else
        {
            swLog = File.AppendText(strLogFile);
        }

        swLog.WriteLine(strLogEntry);

        swLog.Flush();
        swLog.Close();
    }

    public void WriteToEventLog(string strLogEntry, EventLogEntryType eType)
    {
        string strSource = "MyFUN Account";
        string strLogType = "Application";
        string strMachine = ".";
        EventLog eLog;
        try
        {
            if (!EventLog.SourceExists(strSource))
                EventLog.CreateEventSource(strSource, strLogType);

            eLog = new EventLog(strLogType, strMachine, strSource);
            eLog.WriteEntry(strLogEntry, eType, 1000);
        }
        catch
        {

        }

    }


}