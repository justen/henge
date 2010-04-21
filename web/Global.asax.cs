
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Henge.Web
{
	public class HengeApp: System.Web.HttpApplication
	{
		public static Henge.Data.DataProvider dataprovider;
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
			
			routes.MapRoute ("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });
			
		}

		protected void Application_Start ()
		{
			dataprovider = new Henge.Data.DataProvider();
			dataprovider.Initialise("mysql","Server=127.0.0.1;Uid=hengine;Pwd=hengine;Database=henge" , true);
			dataprovider.UpdateSchema();
			RegisterRoutes (RouteTable.Routes);
		}
		
				/// <summary>This is called at the beginning of each web request and represents a single user session.</summary>
		protected void Application_BeginRequest()
		{
			// Start new a NHibernate session. Sessions are lightweight and taken from a pool, but they are
			// not thread safe, so a session must be created for each concurrent web request.
			HengeApp.dataprovider.RegisterContext();
		}
		
		
		/// <summary>This is called at the end of each web request.</summary>
		protected void Application_EndRequest()
		{
			// Unbind the NHibernate session from the thread. This returns the session to the pool, allowing it
			// to be re-used by later web requests.
			// No need to close the session as it is already automatically closed at this point
			HengeApp.dataprovider.ReleaseContext();
		}
		
	}
}
