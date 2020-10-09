using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
namespace WebEnrollmentNewDesign
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Default",
              url: "{controller}/{action}/{id}",
                //   url: "{*url}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                
           );
           
           
            routes.MapAttributeRoutes();
        }
    }
}