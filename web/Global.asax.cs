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

		
		protected void Application_Start()
		{
			string path	= Path.Combine(Server.MapPath("~"), "Data");
			string yap	= Path.Combine(path, "henge.yap");
			
			if (!Directory.Exists(path))	Directory.CreateDirectory(path);
			if (File.Exists(yap))			File.Delete(yap);
			
			DataProvider = new Henge.Data.DataProvider();
			DataProvider.Initialise(yap, "mysql", "Server=127.0.0.1;Uid=henge;Pwd=henge;Database=henge", true);
			DataProvider.UpdateSchema();
			
			Avebury.Loader avebury = new Avebury.Loader(path);
			DataProvider.Bootstrap(avebury.Data);
			User user = DataProvider.Store(new User { 
				Name = "test", 
				Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("test", "sha1") 
			});
			
			Location location			= DataProvider.Get<Location>(x => x.Coordinates.X == 25 && x.Coordinates.Y == 25);
			ComponentType avatarType 	= DataProvider.Get<ComponentType>(x => x.Id == "avatar");
			Avatar avatar				= DataProvider.Store(new Avatar(avatarType) { Name = "Og" , User  = user,  Location = location });
			
			using (DataProvider.Lock(user.Avatars)) user.Avatars.Add(avatar);
			
			Henge.Engine.Interactor.Instance.Initialise(Path.Combine(Server.MapPath("~"), "bin"));
			
			Henge.Engine.Interactor.Instance.Interact(DataProvider, avatar, location, "Spawn.Character", null);
			
			RegisterRoutes(RouteTable.Routes);
		}
		
		
		protected void Application_End()
		{
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
