using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
namespace WebEnrollmentNewDesign
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);
        }
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            
            string BrandCode = System.Web.Configuration.WebConfigurationManager.AppSettings["BrandCode"];
            if (BrandCode.ToLower() == "gexaix")
            {

                bundles.Add(new StyleBundle("~/css/Indexcss").Include("~/IndexCSS/styles1.css",
                                                                       "~/IndexCSS/styles1.css",
                                                                       "~/IndexCSS/styles2.css",
                                                                       "~/IndexCSS/styles-blessed1.css",
                                                                       "~/IndexCSS/styles-blessed2.css",
                                                                       "~/IndexCSS/styles-blessed3.css",
                                                                       "~/IndexCSS/styles-blessed4.css",
                                                                       "~/IndexCSS/styles-blessed5.css",
                                                                       "~/css/styles11.css",
                                                                       "~/css/gexa-theme.css"
                                                                       ));
            

                bundles.Add(new StyleBundle("~/css/Enrollmentcss").Include("~/css/styles.css",
                                                                      "~/css/styles11.css",
                                                                      "~/css/styles1.css",
                                                                      "~/css/styles2.css",
                                                                      "~/css/styles-blessed1.css",
                                                                      "~/css/styles-blessed2.css",
                                                                      "~/css/styles-blessed3.css",
                                                                      "~/css/styles-blessed4.css",
                                                                      "~/css/styles-blessed5.css",
                                                                      "~/css/Brand.css",
                                                                       "~/css/gexa-theme.css"
                                                                      ));
               
                bundles.Add(new StyleBundle("~/css/Confirmationcss").Include("~/css/styles.css",
                                                                      "~/css/styles11.css",
                                                                      "~/css/styles1.css",
                                                                      "~/css/styles2.css",
                                                                      "~/css/styles-blessed1.css",
                                                                      "~/css/styles-blessed2.css",
                                                                      "~/css/styles-blessed3.css",
                                                                      "~/css/styles-blessed4.css",
                                                                      "~/css/styles-blessed5.css",
                                                                      "~/css/Brand.css",
                                                                      "~/ConfirmationCSS/blessed1.css",
                                                                      "~/ConfirmationCSS/style.css",
                                                                       "~/css/gexa-theme.css"
                                                                      ));
             

            }
            else
            {
                bundles.Add(new StyleBundle("~/css/Indexcss").Include("~/IndexCSS/styles1.css",
                                                                       "~/IndexCSS/styles1.css",
                                                                       "~/IndexCSS/styles2.css",
                                                                       "~/IndexCSS/styles-blessed1.css",
                                                                       "~/IndexCSS/styles-blessed2.css",
                                                                       "~/IndexCSS/styles-blessed3.css",
                                                                       "~/IndexCSS/styles-blessed4.css",
                                                                       "~/IndexCSS/styles-blessed5.css",
                                                                       "~/css/styles11.css",
                                                                       "~/css/Brand.css"
             
                                                                       ));
               

                bundles.Add(new StyleBundle("~/css/Enrollmentcss").Include("~/css/styles.css",
                                                                      "~/css/styles11.css",
                                                                      "~/css/styles1.css",
                                                                      "~/css/styles2.css",
                                                                      "~/css/styles-blessed1.css",
                                                                      "~/css/styles-blessed2.css",
                                                                      "~/css/styles-blessed3.css",
                                                                      "~/css/styles-blessed4.css",
                                                                      "~/css/styles-blessed5.css",
                                                                      "~/css/frontier-theme.css",
                                                                      "~/css/Brand.css"
                                                                     
                                                                      ));
               

                bundles.Add(new StyleBundle("~/css/Confirmationcss").Include("~/css/styles.css",
                                                                      "~/css/styles11.css",
                                                                      "~/css/styles1.css",
                                                                      "~/css/styles2.css",
                                                                      "~/css/styles-blessed1.css",
                                                                      "~/css/styles-blessed2.css",
                                                                      "~/css/styles-blessed3.css",
                                                                      "~/css/styles-blessed4.css",
                                                                      "~/css/styles-blessed5.css",
                                                                      "~/css/frontier.css",
                                                                      "~/ConfirmationCSS/blessed1.css",
                                                                      "~/ConfirmationCSS/style.css"
                                                                      ));
              

            }
            bundles.Add(new ScriptBundle("~/scripts/Indexjs").Include("~/scripts/enroll_util_funcs.js",
                                                                 "~/scripts/ProductsInfo.js"
                                                                 ));
            bundles.Add(new ScriptBundle("~/scripts/Confirmationjs").Include("~/scripts/showhide_containers.js",
                                                                       "~/scripts/contactinfo_validation.js",
                                                                       "~/scripts/dob_validation.js",
                                                                       "~/scripts/cc_validation.js",
                                                                       "~/scripts/tracking-header.js",
                                                                      "~/scripts/ProductsInfo.js",
                                                                       "~/scripts/tracking-header.js",
                                                                       "~/scripts/enroll_misc_funcs.js",
                                                                       "~/scripts/busyindicator.js"
                                                                       ));
            bundles.Add(new ScriptBundle("~/scripts/Enrollmentjs").Include("~/scripts/showhide_containers.js",
                                                                        "~/scripts/contactinfo_validation.js",
                                                                        "~/scripts/dob_validation.js",
                                                                        "~/scripts/cc_validation.js",
                                                                        "~/scripts/tracking-header.js",
                                                                        "~/scripts/ProductsInfo.js",
                                                                        "~/scripts/tracking-header.js",
                                                                        "~/scripts/enroll_misc_funcs.js",
                                                                        "~/scripts/busyindicator.js",
                                                                        "~/scripts/onBlur.js"
                                                                        ));

        }
    }
}