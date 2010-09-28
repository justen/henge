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
				// Can't compare references at the moment (since they are proxied)
				Location origin		= Session["Origin"] as Location;
				Location current	= this.avatar.Location;
				ulong index			= ((ulong)(current.X + dx) << 32) | (ulong)(current.Y + dy);
				Location location	= this.db.Get<Location>(l => l.Index == index);
				
				if (location != null && location != this.avatar.Location) 
				{
					IInteraction result = Interactor.Instance.Interact(this.avatar, location, "Move.Autodetect", null);
					
					if (result.Succeeded)
					{
						this.cache.Local.Clear();
						
						return Json(new { 
							Valid		= true, 
							X			= location.X - origin.X, 
							Y			= location.Y - origin.Y, 
							Energy		= this.avatar.Traits["Energy"].Value, 
							Message		= result.Conclusion,
							Contents	= this.GetDiffs(true)
						});
					}
					else error = result.Conclusion;
				}
				else error = "Error: Invalid location specified"; 
			}
			
			return Json(new { Valid = false, Message = error });
		}
		
		
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult DefendLocation(int dx, int dy, int duration)
		{
			string error = "Error: You are not connected to an avatar";
			
			if (this.avatar != null)
			{
				Dictionary<string, object> arguments = new Dictionary<string, object>();
				arguments.Add("expiry", null);
				if (duration > 0)
				{
					arguments["expiry"] = DateTime.Now.AddHours(duration);	
				}
				arguments.Add("dx", dx);
				arguments.Add("dy", dy);
				Location location = this.avatar.Location;
				if (location != null) 
				{
					IInteraction result = Interactor.Instance.Interact(this.avatar, location, "Defend.Guard", arguments);
					
					if (result.Succeeded)
					{
						//this.db.Store<LogEntry>(new LogEntry { Occurred = DateTime.Now, Entry = "We moved!" });
						return Json(new { Valid = true, Message = "you are now defending"});
					}
					else error = result.Conclusion;
				}
				else error = "Error: Invalid location specified"; 
			}
			
			return Json(new { Valid = false, Message = error });
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public JsonResult GetStatus()
		{
			if (this.avatar != null)
			{
				var messages		= (from l in this.db.Query<LogEntry>() where l.AvatarId == this.avatar.Id select l).ToList();
    			List<string> log	= messages.Select<LogEntry, string>(l => this.DescribeTime(l.Occurred) + l.Entry).ToList();
				this.db.Delete(messages);
			
				return Json(new { 
					Health 			= this.avatar.Traits["Health"].Value, 
					Reserve			= this.avatar.Traits["Reserve"].Percentage(), 
					Constitution 	= this.avatar.Traits["Constitution"].Percentage(),
					Energy			= Interactor.Instance.Modifier("Energy").Apply(this.avatar).Value,
					Messages		= log,
					Contents		= this.GetDiffs(true)
				});
				
			}

			return Json(new { Message = "Error: You are not connected to an avatar" });
		}
		
		
		private List<string> GetDiffs(bool local)
		{
			Dictionary<long, Component> cache 	= local ? this.cache.Local : this.cache.Remote;
			Location location 					= this.avatar.Location;
			List<string> diffs					= new List<string>();
			
			foreach (Avatar avatar in location.Inhabitants) 
			{
				if (avatar != this.avatar)
				{
					if (!cache.ContainsValue(avatar)) 
					{
						long id = Generator.Id;
						cache.Add(id, avatar);
						diffs.Add(string.Format("+a{0}", id));
					}
				}
			}
			
			foreach (Npc npc in location.Fauna)
			{
				if (!cache.ContainsValue(npc))
				{
					long id = Generator.Id;
					cache.Add(id, npc);
					diffs.Add(string.Format("+n{0}", id));
				}
			}
			
			foreach (Edifice edifice in location.Structures)
			{
				if (!cache.ContainsValue(edifice))
				{
					long id = Generator.Id;
					cache.Add(id, edifice);
					diffs.Add(string.Format("+s{0}", id));
				}
			}
			
			if (local)
			{
				foreach (Item item in location.Inventory)
				{
					if (!cache.ContainsValue(item))
					{
						long id = Generator.Id;
						cache.Add(id, item);
						diffs.Add(string.Format("+i{0}", id));
					}
				}
			}
			
			List<long> removable = new List<long>();
			
			foreach (KeyValuePair<long, Component> item in cache)
			{
				if (item.Value is Item)
				{
					if ((item.Value as Item).Owner != location) 
					{
						removable.Add(item.Key);
						diffs.Add(string.Format("-i{0}", item.Key));
					}
				}
				else
				{
					if ((item.Value as MapComponent).Location != location)
					{
						removable.Add(item.Key);
						if (item.Value is Avatar) 		diffs.Add(string.Format("-a{0}", item.Key));
						else if (item.Value is Npc)		diffs.Add(string.Format("-n{0}", item.Key));
						else if (item.Value is Edifice)	diffs.Add(string.Format("-s{0}", item.Key));
					}
				}
			}
			
			foreach (long id in removable) cache.Remove(id);
			
			return diffs;
		}
					          
					

		protected string DescribeTime(DateTime time)
		{	
			double span = (DateTime.Now - time).TotalSeconds;
			
			if (span < 60)			return "A moment ago ";			// 1 min
			if (span < 900)			return "A little earlier ";		// 15 mins
			if (span < 3600)		return "A short time ago ";		// 60 mins
			if (span < 21600)		return "A little while ago ";	// 6 hours
			if (span < 86400)		return "Some time ago ";		// 24 hours
			if (span < 172800)		return "A long time ago ";		// 48 hours
			if (span < 2419200)		return "Many days earlier ";	// 28 days
			if (span < 4838400) 	return "Moons ago ";			// 56 days
			if (span < 15552000)	return "Many moons ago ";		// 180 days
			
			return "A long time ago in a galaxy far, far away... ";
		}
	}
	

}
