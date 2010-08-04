using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Henge.Data.Entities;


namespace Henge.Web.Controllers
{
	[Authorize][HandleError]
	public class MapController : MasterController
	{
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Tile(int [] x, int [] y)
		{
			List<object> result = new List<object>();
			
			if (x != null && y != null)
			{
				Location origin = Session["Origin"] as Location;
				
				for (int i=0; i<x.Length; i++)
				{
					ulong index			= ((ulong)(origin.X + x[i]) << 32) | (ulong)(origin.Y + y[i]);
					Location location	= this.db.Get<Location>(l => l.Index == index);
					
					if (location != null)
					{
						Appearance appearance = location.Appearance();
						result.Add(new { Type = 0, Name = appearance.Type, Colour = appearance.Parameters["colour"], Priority = this.GetPriority(appearance.Type) });
					}
					else result.Add(new { Type = -1 });
				}
			}
			
			return Json(result);
		}
		
		
		// This is a temporary hack until priorities are properly encoded in the map appearance information (and types are handled separately with the client).
		private int GetPriority(string name)
		{
			switch (name)
			{
				case "cliff":		return 8;
				case "forest":		return 7;
				case "woodland":	return 6;
				case "shallows":	return 5;
				case "ocean":		return 4;
				case "plain":		return 3;
				case "dunes":		return 2;
				case "beach":		return 1;
			}
			
			return 0;
		}
		
		
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult AssetList()
		{
			return Json(this.Find(new List<string>(), Path.Combine(Server.MapPath("~"), "Content/interface/images"))); 
		}
		
		
		private List<string> Find(List<string> list, string directory)//, string basePath)
		{
			foreach (string f in Directory.GetFiles(directory, "*.png"))	list.Add(Url.Content(f.Replace(Server.MapPath("~"), "~/")));  
			foreach (string d in Directory.GetDirectories(directory))		this.Find(list, d);
			
			return list;
		}
	}
}
