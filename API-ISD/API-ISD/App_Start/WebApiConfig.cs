using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API_ISD
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

			// This needs to be here to write circular references, probably not the best option, there ware other ways to fix the self-referencing loop problem
			// if it arises with the multco site schema
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling
				 = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
			config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling
				 = Newtonsoft.Json.PreserveReferencesHandling.Objects;
		}
	}
}
