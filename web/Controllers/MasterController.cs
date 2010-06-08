using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;

using Henge.Web;
using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Web.Controllers
{
	/// <summary>
	/// The base controller for the project - all other controllers should descend from this.
	/// Provides standard members and utility functions.
	/// </summary>
	public class MasterController : Controller
	{	
		protected DataProvider db;
		protected User user;
		protected Avatar avatar;
		
		
		// Called before the action in the inherited controller is executed, allowing certain members to be set
		protected override void OnActionExecuting(ActionExecutingContext ctx) 
		{
		    base.OnActionExecuting(ctx);
			
		    this.db = HengeApplication.DataProvider;
			
		    // If the user has logged in then add their name to the view data
		    if (this.User.Identity.IsAuthenticated)
			{
				this.user					= this.db.Get<User>(x => x.Name == this.User.Identity.Name);
				this.avatar					= this.user.CurrentAvatar;
				this.ViewData["User"] 		= this.User.Identity.Name;
				this.ViewData["Character"]	= (this.avatar != null) ? string.Format("{0} of {1}", this.avatar.Name, this.user.Clan) : null;
			}
			else
			{
				this.user 	= null;
				this.avatar	= null;
			}
		}
		
		
		protected override void OnActionExecuted (ActionExecutedContext filterContext)
		{
			this.db.Flush();
			
			base.OnActionExecuted (filterContext);
		}

		
		/// <summary>
		/// Make the given message available to the view, but after a single redirect.
		/// </summary>
		/// <param name="message">
		/// Message to be displayed <see cref="System.String"/>
		/// </param>
		protected void SetMessage(string message)
		{
			this.TempData["Message"] 	= message;
			this.TempData["Error"]		= false;
		}
		
		
		/// <summary>
		/// Make the given message available to the current view (immediate effect).
		/// </summary>
		/// <param name="message">
		/// Message to be displayed <see cref="System.String"/>
		/// </param>
		protected void SetMessageNow(string message)
		{
			this.ViewData["Message"] 	= message;
			this.ViewData["Error"]		= false;
		}
		
		
		/// <summary>
		/// Make the given error message available to the view, but after a single redirect.
		/// </summary>
		/// <param name="message">
		/// Error message to be displayed <see cref="System.String"/>
		/// </param>
		protected void SetError(string message)
		{
			this.TempData["Message"] 	= message;
			this.TempData["Error"]		= true;
		}
		
		
		/// <summary>
		/// Make the given error message available to the current view (immediate effect)
		/// </summary>
		/// <param name="message">
		/// Error message to be displayed <see cref="System.String"/>
		/// </param>
		protected void SetErrorNow(string message)
		{
			this.ViewData["Message"] 	= message;
			this.ViewData["Error"]		= true;
		}
	}
}
