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
	public class CreateCharacterController : MasterController
	{
		public ActionResult Index ()
		{
			return View ();
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(string name)
		{
			//if (this.db.CreateCriteria<Avatar>().CreateAlias("BaseAppearance", "A").Add(Restrictions.Eq("A.Name", name)).UniqueResult<Avatar>() == null)
			if ( (from a in this.db.Query<Avatar>() where a.Name == name select true).Count() == 0 )
			{
				Location location	= this.db.Get<Location>(x => x.Coordinates.X == 0 && x.Coordinates.Y == 0);
				Avatar avatar		= new Avatar {Name = name, Type = db.Get<ComponentType>(x => x.Id == "avatar"), User  = this.user,  Location = location};
				this.user.Avatars.Add(avatar);
				location.Inhabitants.Add(avatar);

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
