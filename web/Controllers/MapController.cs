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
		private static int RANGE = 10;
		
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Tile(int [] x, int [] y)
		{
			List<object> result = new List<object>();
			
			if (this.avatar != null && x != null && y != null)
			{
				Location origin 	= Session["Origin"] as Location;
				Location current 	= this.avatar.Location;
				
				for (int i=0; i<x.Length; i++)
				{
					int ax = origin.X + x[i];
					int ay = origin.Y + y[i];
					
					if (Math.Abs(current.X - ax) <= RANGE && Math.Abs(current.Y - ay) <= RANGE)
					{
						ulong index			= ((ulong)ax << 32) | (ulong)ay;
						Location location	= this.db.Get<Location>(l => l.Index == index);
						
						if (location != null) 	result.Add(new { Type = location.Type.Id, Z = location.Z });
						else 					result.Add(new { Type = 0 });
					}
					else throw new Exception("Attempted to access tile outside of permitted range");
				}
			}
			
			return Json(result);
		}
		
		
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult TypeList()
		{
			Dictionary<string, object> result = new Dictionary<string, object>();
			
			if (this.avatar != null)
			{
				foreach (ComponentType type in this.avatar.Location.Map.LocationTypes)
				{
					Appearance appearance = type.Appearance.FirstOrDefault();
					result.Add(type.Id, new { Name = appearance.Type, Colour = appearance.Parameters["colour"], Priority = appearance.Priority });
				}
			}
			
			return Json(result);
		}
		
		
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult AssetList()
		{
			return Json(this.Find(new List<string>(), Path.Combine(Server.MapPath("~"), "Content/interface/images"))); 
		}
		
		
		/*public JsonResult Constants()
		{
			return Json(new {
				MapRange	= RANGE,
				Energy		= new {*/
		
		
		private List<string> Find(List<string> list, string directory)
		{
			foreach (string f in Directory.GetFiles(directory, "*.png"))	list.Add(Url.Content(f.Replace(Server.MapPath("~"), "~/")));  
			foreach (string d in Directory.GetDirectories(directory))		this.Find(list, d);
			
			return list;
		}
	}
}
