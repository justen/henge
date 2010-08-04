using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Henge.Web.Controllers
{

	[Authorize][HandleError]
	public class HomeController : MasterController
	{
		public ActionResult Index ()
		{
			if (this.avatar != null)
			{
				Session["Origin"] = this.avatar.Location;
				return View ();
			}
			
			return RedirectToAction("Account", "User");
		}
	}
}
