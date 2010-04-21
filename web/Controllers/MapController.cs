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
		public JsonResult Tile(int x, int y)
		{
			return Json(string.Format("Test - {0}, {1}", x, y));
		}
	}
}
