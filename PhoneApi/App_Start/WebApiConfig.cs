using PhoneApi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PhoneApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            JsonContent.SetupNullIgnore(GlobalConfiguration.Configuration);
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
