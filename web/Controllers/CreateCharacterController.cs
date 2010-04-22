using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Henge.Data.Entities;
using NHibernate.Criterion;

namespace Henge.Web.Controllers
{

	[Authorize][HandleError]
	public class CreateCharacterController : MasterController
	{
		public ActionResult Index ()
		{
			return View ();
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(string name)
		{
			if (this.db.CreateCriteria<Avatar>().CreateAlias("BaseAppearance", "A").Add(Restrictions.Eq("A.Name", name)).UniqueResult<Avatar>() == null)
			{
				Appearance appearance	= (Appearance)this.db.UpdateAndRefresh(new Appearance {Name = name});
				Location location		= this.db.Get<Location>((long)5);
				Avatar avatar			= (Avatar)this.db.UpdateAndRefresh(new Avatar {Name = name, BaseAppearance = appearance, User  = this.currentUser,  Location = location});

				location.Inhabitants.Add(avatar);
				this.db.Flush();
				
				return RedirectToAction("Account", "User");
			}
			else
			{
				this.SetError("A character with the same name already exists");
				return RedirectToAction("Index");
			}
	
		}
	}
	
}
