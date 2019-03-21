using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RSS_Reader
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "CancelNewsletter",
                "Reader/ConfirmationOfCancellingNewsletter",
                new { controller = "Reader", action = "ConfirmationOfCancellingNewsletter"}
             );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Reader", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
