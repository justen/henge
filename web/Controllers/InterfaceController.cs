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
	public class InterfaceController : MasterController
	{
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult Move(int dx, int dy)
		{
			string error = "Error: You are not connected to an avatar";
			
			if (this.avatar != null)
			{
				Coordinates coordinates = new Coordinates(this.avatar.Location.Coordinates);
				
				coordinates.X += dx;
				coordinates.Y += dy;

				Location location = this.avatar.Location.Map.GetLocation(coordinates);
				
				if (location != null) 
				{
					IInteraction result = Interactor.Instance.Interact(this.db, this.avatar, location, "Move.Run");
					
					if (result.Succeeded)
					{
						//this.db.Store<LogEntry>(new LogEntry { Occurred = DateTime.Now, Entry = "We moved!" });
						return Json(new { Valid = true, X = coordinates.X, Y = coordinates.Y });
					}
					else error = result.Conclusion;
				}
				else error = "Error: Invalid location specified"; 
			}
			
			return Json(new { Valid = false, Message = error });
		}
	}
}
