using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using WebHttpClient.UserSecurity;


namespace WebHttpClient
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
                   
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            //config.Routes.MapHttpRoute(
            //    name: "SqlGetById",
            //    routeTemplate: "api/{controller}/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            //config.Routes.MapHttpRoute(
            //    name: "GetByTitle",
            //    routeTemplate: "api/{controller}/{action}/{title}",
            //    defaults: new { title = RouteParameter.Optional }
            //);


        }
    }
}
