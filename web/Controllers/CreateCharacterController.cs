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
				Location location		= this.db.Get<Location>((long)5);
				Appearance appearance	= this.db.UpdateAndRefresh<Appearance>(new Appearance { Name = name });
				Avatar avatar			= this.db.UpdateAndRefresh<Avatar>(new Avatar {Name = name, BaseAppearance = appearance, User  = this.user,  Location = location});
				//avatar.BaseAppearance = this.db.UpdateAndRefresh(new Appearance {Name = name});
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
