﻿using System.Web.Http;
using Owin;

namespace WorkingWithHttp.OwinHosting
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder p_appBuilder)
        {
            // Configure Web API for self‐host.
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
            p_appBuilder.UseWebApi(config);
        }
    }
}