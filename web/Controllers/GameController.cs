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
			Avatar avatar = this.db.Get<Avatar>(this.avatarId);
			return View (new GameViewModel(avatar, this.currentUser.Clan, avatar.Location.Inhabitants.Where(i=> i.Id != avatar.Id).ToList()));
		}
		
		/// <summary>Action to create a new user</summary>
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Move(string button)
		{
			Actor protagonist		= this.db.Get<Avatar>(this.avatarId);
			if (protagonist!=null)
			{
				int x = protagonist.Location.X;
				int y = protagonist.Location.Y;
				int z = protagonist.Location.Z;
				if (button.Contains("North"))
				{
					y--;
				}
				if (button.Contains("South"))
				{
					y++;
				}
				if (button.Contains("East"))
				{
					x++;
				}
				if (button.Contains("West"))
				{
					x--;
				}
				if (button.Contains("Up"))
				{
					z++;
				}
				if (button.Contains("Down"))
				{
					z--;
				}
				Location location = this.db.CreateCriteria<Location>().Add(Restrictions.Eq("X", x)).Add(Restrictions.Eq("Y", y)).Add(Restrictions.Eq("Z", z)).Add(Restrictions.Eq("Map", protagonist.Location.Map)).UniqueResult<Location>();
				Interactor.Instance.Interact(protagonist, location, "Move.Run");
				this.db.Flush();
			}
			else
			{
				return RedirectToAction ("Account", "User");
			}
			
			return RedirectToAction("Index");
		}
	}	
}
