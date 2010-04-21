using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;
using NHibernate;
using NHibernate.Criterion;
using Henge.Web;


namespace Henge.Web.Controllers
{
	/// <summary>
	/// The base controller for the project - all other controllers should descend from this.
	/// Provides standard members and utility functions.
	/// </summary>
	public class MasterController : Controller
	{	
		/// <summary>Used to store the current database session for simple access within all inherited controllers.</summary>
	//	protected ISession db;
		protected Henge.Data.DataProvider db;
		protected Henge.Data.Entities.User currentUser;
		protected long avatarId;
		
		// Called before the action in the inherited controller is executed, allowing certain members to be set
		protected override void OnActionExecuting(ActionExecutingContext ctx) 
		{
		    base.OnActionExecuting(ctx);
		    db = HengeApp.dataprovider;
		    	    
		    // If the user has logged in then add their name to the view data
		    if (this.User.Identity.IsAuthenticated)
			{
				this.ViewData["User"] = this.User.Identity.Name;
				long key = (long)Membership.GetUser(false).ProviderUserKey;
				this.currentUser = this.db.Get<Henge.Data.Entities.User>(key);
				if (Session["Avatar"]!=null)
				{
					this.avatarId = (long)Session["Avatar"];
				}
			}
			else
			{
				this.currentUser = null;
			}
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
