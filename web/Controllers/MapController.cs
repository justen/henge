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
						result.Add(new { Type = 0, Name = appearance.Type, Colour = appearance.Parameters["colour"] });
					}
					else result.Add(new { Type = -1 });
				}
			}
			
			return Json(result);
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
