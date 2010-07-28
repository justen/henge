using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Henge.Data.Entities;
using Henge.Engine;
using Henge.Rules;


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
				Location location			= this.db.Get<Location>(l => l.X == 50 && l.Y == 50);
				ComponentType avatarType 	= db.Get<ComponentType>(x => x.Id == "avatar");
				Avatar avatar				= new Avatar(avatarType) {Name = name , User  = this.user,  Location = location};
				
				using (this.db.Lock(this.user.Avatars)) this.user.Avatars.Add(avatar);

				IInteraction result = Interactor.Instance.Interact(avatar, location, "Spawn.Character", null);

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
