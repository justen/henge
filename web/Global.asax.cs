using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Web
{
	public class HengeApplication : System.Web.HttpApplication
	{
		public static Henge.Data.DataProvider DataProvider { get; private set; }
		public static Henge.Data.Entities.Global Globals { get; private set; }
		private static Henge.Daemon.Heart Heartbeat { get; set; }
		
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
			routes.MapRoute ("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });
		}

		
		protected void Application_Start()
		{
			string path		= Path.Combine(Server.MapPath("~"), "Data");
			string yap		= Path.Combine(path, "henge.yap");
			bool bootstrap	= true;
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			
			if (File.Exists(yap))
			{
				if (bootstrap) File.Delete(yap);
			}
			else bootstrap = true;
			Console.WriteLine("Starting up");
			DataProvider = new Henge.Data.DataProvider();
			DataProvider.Initialise(yap, "mysql", "Server=127.0.0.1;Uid=henge;Pwd=henge;Database=henge");
			DataProvider.UpdateSchema();
			
			if (bootstrap)
			{
				Globals = DataProvider.Store<Global>(new Global());
				
				using (DataProvider.Lock(Globals))
				{
					Avebury.Loader avebury = new Avebury.Loader(path, Globals);
					DataProvider.Bootstrap(avebury.Data);
				}
			}
			else Globals = DataProvider.Get<Global>(g => true);
			
			Henge.Engine.Interactor.Instance.Initialise(Path.Combine(Server.MapPath("~"), "bin"), DataProvider);
			UserService.Instance.Initialise(DataProvider);
			
			Heartbeat = new Henge.Daemon.Heart(500);
			Heartbeat.Start();
			
			RegisterRoutes(RouteTable.Routes);
		}
		
		
		protected void Application_End()
		{
			Heartbeat.Arrest();
			DataProvider.Dispose();
		}
		
		
		/// <summary>This is called at the beginning of each web request and represents a single user session.</summary>
		protected void Application_BeginRequest()
		{
			DataProvider.RegisterContext();
		}
		
		
		/// <summary>This is called at the end of each web request.</summary>
		protected void Application_EndRequest()
		{
			DataProvider.ReleaseContext();
		}
		
	}
}
