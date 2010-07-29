using System;
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
				for (int i=0; i<x.Length; i++)
				{
					ulong index			= ((ulong)x[i] << 32) | (ulong)y[i];
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
	}
}
