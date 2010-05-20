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
			Interactor.Instance.Interact(this.db.Get<Avatar>(this.avatarId), this.db.Get<Location>((long)12), "Move.Run" );
			return RedirectToAction("Index");
		}
	}	
}
