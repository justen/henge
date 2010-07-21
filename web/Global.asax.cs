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
		
		public static void RegisterRoutes (RouteCollection routes)
		{
			routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
			routes.MapRoute ("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });
		}

		
		protected void Application_Start ()
		{
			string path = Path.Combine(Server.MapPath("~"), "Data");
			if (!Directory.Exists(path)) Directory.CreateDirectory(path);
			
			DataProvider = new Henge.Data.DataProvider();
			//DataProvider.Initialise(Path.Combine(path, "henge.yap"), true);
			DataProvider.Initialise(Path.Combine(path, "henge.yap"), "mysql", "Server=127.0.0.1;Uid=henge;Pwd=henge;Database=henge", true);
			Avebury.Loader avebury = new Avebury.Loader(path);
			List<Entity> data = avebury.Maps;
			ComponentType avatarType = new ComponentType(){ Id = "avatar"};
			avatarType.Appearance.Add( new Appearance() { Type = "avatar", ShortDescription = "A person", Description = "A nondescript person" });
			data.Add(avatarType);     
			DataProvider.Bootstrap(data);

			DataProvider.UpdateSchema();
			Henge.Engine.Interactor.Instance.Initialise(Path.Combine(Server.MapPath("~"), "bin"));
			RegisterRoutes (RouteTable.Routes);
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
