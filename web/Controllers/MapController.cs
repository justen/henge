using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;


namespace Henge.Web.Controllers
{
	[Authorize][HandleError]
	public class MapController : MasterController
	{
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Tile(int [] x, int [] y)
		{
			List<string> result = new List<string>();
			
			if (x != null && y != null)
			{
				for (int i=0; i<x.Length; i++)
				{
					result.Add(string.Format("Test - {0}, {1}", x[i], y[i]));
				}
			}
			
			return Json(result);
		}
	}
}
