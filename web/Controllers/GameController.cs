using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using Henge.Data.Entities;
using Henge.Engine;


namespace Henge.Web.Controllers
{
	public class GameViewModel
	{
		public string Clan 			{ get; set; }
		public Avatar Avatar 		{ get; set; }
		public IList<Avatar> Others { get; set; }
		
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
			if (this.avatar != null) 
			{
				return View (new GameViewModel(this.avatar, this.user.Clan, this.avatar.Location.Inhabitants.Where(i=> i.Name != this.avatar.Name).ToList()));
			}
			
			return RedirectToAction("Account", "User");
		}
		
		/// <summary>Action to create a new user</summary>
		/*[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Move(string button)
		{
			if (this.avatar != null)
			{
				Coordinates coordinates = new Coordinates(this.avatar.Location.Coordinates);
				//int x = this.avatar.Location.Coordinates.X;
				//int y = this.avatar.Location.Coordinates.Y;
				//int z = this.avatar.Location.Coordinates.Z;
				
				if (button.Contains("North"))	coordinates.Y--;
				if (button.Contains("South"))	coordinates.Y++;
				if (button.Contains("East"))	coordinates.X++;
				if (button.Contains("West"))	coordinates.X--;
				if (button.Contains("Up"))		coordinates.Z++;
				if (button.Contains("Down"))	coordinates.Z--;
				
					
				//Location location = this.db.Get<Location>(l => l.X == x && l.Y == y && l.Z == z && l.Map == m);
				Location location = this.avatar.Location.Map.GetLocation(coordinates);
				
				if (location != null) Interactor.Instance.Interact(this.avatar, location, "Move.Run");
				
				return RedirectToAction("Index");
			}
			
			return RedirectToAction("Account", "User");
		}*/
	}	
}
