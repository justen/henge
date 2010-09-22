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
						//this.db.Store<LogEntry>(new LogEntry { Occurred = DateTime.Now, Entry = "We moved!" });
						return Json(new { Valid = true, X = location.X - origin.X, Y = location.Y - origin.Y, Energy = this.avatar.Traits["Energy"].Value, Message = result.Conclusion });
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
				var messages = (from l in this.db.Query<LogEntry>() where l.AvatarId == this.avatar.Id select l).ToList();
    			List<string> log = messages.Select<LogEntry, string>(l => this.DescribeTime(l.Occurred) + l.Entry).ToList();
				this.db.Delete(messages);
				return Json(new { 
					Health 			= this.avatar.Traits["Health"].Value, 
					Reserve			= this.avatar.Traits["Reserve"].Percentage(), 
					Constitution 	= this.avatar.Traits["Constitution"].Percentage(),
					Energy			= Interactor.Instance.Modifier("Energy").Apply(this.avatar).Value,
					Messages		= log,
				});
				
			}

			return Json(new { Message = "Error: You are not connected to an avatar" });
		}
		
		protected string DescribeTime(DateTime time)
		{	
			string result = "A long time ago in a galaxy far, far away... ";
			TimeSpan span = DateTime.Now - time;
			if (span.TotalSeconds < 60)
			{
			  result = "A moment ago ";
			}
			else
			{
				if (span.TotalMinutes < 15)	result = "A little earlier ";
				else
				{
					if (span.TotalMinutes <60)	result = "A short time ago ";
					else
					{
						if (span.TotalHours <6)	result = "A little while ago ";
						else
						{
							if (span.TotalHours < 24) result = "Some time ago ";
							else
							{
								if (span.TotalHours < 48) result = "A long time ago ";
								else
								{
									if (span.TotalDays < 28) result = "Many days earlier ";
									else
									{
										if (span.TotalDays < 56) result = "Moons ago ";
										else if (span.TotalDays < 180) result = "Many moons ago ";
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
	

}
