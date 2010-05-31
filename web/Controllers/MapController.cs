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
					Location location = this.avatar.Location.Map.GetLocation(x[i], y[i], 0);
					
					if (location != null)
					{
						result.Add(new { Type = 0, Name = location.BaseAppearance.Name });
					}
					else result.Add(new { Type = -1 });
				}
			}
			
			return Json(result);
		}
	}
}
