using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Henge.Data.Entities;
using NHibernate.Criterion;
using Henge.Engine;

namespace Henge.Web.Controllers
{
	public class GameViewModel
	{
		public string Clan {get; set;}
		public Avatar Avatar {get; set;}
		public IList<Avatar> Others {get; set;}
		
		public GameViewModel(Avatar avatar, string clan, IList<Avatar> others)
		{
			this.Clan = clan;
			this.Avatar = avatar;
			this.Others = others;
		}
	}
	
	[Authorize][HandleError]
	public class GameController : MasterController
	{
		public ActionResult Index ()
		{
			return View (new GameViewModel(this.avatar, this.user.Clan, this.avatar.Location.Inhabitants.Where(i=> i.Id != this.avatar.Id).ToList()));
		}
		
		/// <summary>Action to create a new user</summary>
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Move(string button)
		{
			if (this.avatar != null)
			{
				int x = this.avatar.Location.X;
				int y = this.avatar.Location.Y;
				int z = this.avatar.Location.Z;
				
				if (button.Contains("North"))	y--;
				if (button.Contains("South"))	y++;
				if (button.Contains("East"))	x++;
				if (button.Contains("West"))	x--;
				if (button.Contains("Up"))		z++;
				if (button.Contains("Down"))	z--;
				
				Location location = this.db.CreateCriteria<Location>()
					.Add(Restrictions.Eq("X", x))
					.Add(Restrictions.Eq("Y", y))
					.Add(Restrictions.Eq("Z", z))
					.Add(Restrictions.Eq("Map", this.avatar.Location.Map))
					.UniqueResult<Location>();
				
				Interactor.Instance.Interact(this.avatar, location, "Move.Run");
				
				return RedirectToAction("Index");
			}
			
			return RedirectToAction("Account", "User");
		}
	}	
}
