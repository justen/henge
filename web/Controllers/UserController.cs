using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Security;

using Henge.Data.Entities;


namespace Henge.Web.Controllers
{
	public class AccountViewModel
	{
		public string Clan {get; set;}
		public IList<Avatar> Avatars {get; set;}
		
		public AccountViewModel(string clan, IList<Avatar> avatars)
		{
			this.Clan = clan;
			this.Avatars = avatars;
		}
	}
	
	/// <summary>
	/// Controller for handling user login/logout. It also allows a logged in user
	/// to change their password.
	/// </summary>
	[HandleError]
	public class UserController : MasterController
	{
		/// <summary>The index action for this controller</summary>
		public ActionResult Index()
		{
/*			if (this.currentUser!=null)
			{
				this.ViewData["Clan"] = currentUser.Clan;	
			}*/
			return View();
		}
		
		
		/// <summary>Action to attempt user login</summary>
		public ActionResult Login(string username, string password)
		{
			// Use the membership provider to validate the user
			if (Membership.ValidateUser(username, password))
			{
				// Use forms authentication sessions to login the user
				FormsAuthentication.SetAuthCookie(username, false);
			}
			else
			{
				// Set an error message and redirection to the index action of this controller
				this.SetError("Invalid Username or Password");
				return RedirectToAction("Index");
			}
			
			// Login was successful so redirect to the default action of the Home controller
			return RedirectToAction("", "Home");
		}
		
		
		/// <summary>Action to logout the current user</summary>
		public ActionResult Logout()
		{
			// Use forms authentication sessions to logout the user
			FormsAuthentication.SignOut();
			this.Session.Remove("user");
			
			// Redirect to the index action of this controller
			return RedirectToAction("Index");
		}
		
		
		/// <summary>Action to create a new user</summary>
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Create(string name, string password, string passwordRepeat)
		{
			// Make sure that something has at least been entered for the name and password, and that the passwords match
			if (name.Length > 0 && password.Length > 0 && password == passwordRepeat)
			{
				// Make sure the user doesn't already exist
				User user = this.db.Get<User>(u => u.Name == name);
				if(user != null) {
					this.SetError("There is already a user with the name " + name);
				} else {
					// Save a new user to the database, simply by creating a new transient User object and passing it to nhibernate
					this.db.Store(new User { Name = name, Clan = name, Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1") });
					this.SetMessage("User added successfully");
				}
			}
			else this.SetError("Please fill in all details and ensure the passwords match");
			
			// Redirect back to the index page of this controller
			return RedirectToAction("Index");
		}
		
		
		/// <summary>
		///	Action to allow the user to enter new password details for their account.
		/// The accept verbs filter allows this action to be chosen when there are no
		/// POST parameters present (i.e., the user has not submitted a form)
		/// </summary>
		[Authorize][AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Account()
		{
			return View(new AccountViewModel(this.user.Clan, this.user.Avatars));
		}
		
		
		/// <summary>
		/// Action to actually attempt changing the password details for the current user.
		/// The accept verbs filter allows this action to be chosen when POST parameters
		/// are present (i.e., the user has submitted a form)
		/// </summary>
		[Authorize][AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Account(string current, string password, string passwordRepeat)
		{
			// Check that the new passwords match
			if (password == passwordRepeat)
			{
				// Get the user via the membership provider
				MembershipUser user = Membership.GetUser();
				
				// Request a password change
				if (user.ChangePassword(current, password))
				{
					this.SetMessageNow("Password changed successfully");
				}
				else
				{
					this.SetErrorNow("There was a problem changing the password");
				}
			}
			else this.SetErrorNow("Please ensure the new passwords match");
			
			return View();
		}
		
		
		[Authorize][AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Clan(string clan)
		{
			if (!this.db.Query<User>().Where(u => u.Clan == clan).Any())
			{
				using (this.db.Lock(this.user)) this.user.Clan = clan;
				this.SetMessage("Clan name successfully updated");
			}
			else this.SetError("A clan with the same name already exists");	

			return RedirectToAction ("Account");
		}

		
		[Authorize][AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteAvatar(int index)
		{
			Avatar avatar = this.user.Avatars.ElementAtOrDefault(index);
			
			if (avatar != null)
			{
				if (this.avatar == avatar) Session.Remove("Avatar");
				
				using (this.db.Lock(avatar.Location.Inhabitants, this.user.Avatars))
				{
					avatar.Location.Inhabitants.Remove(avatar);
					this.user.Avatars.Remove(avatar);	
				}
				
				this.db.Delete(avatar);
			}
			
			return RedirectToAction ("Account");	
		}
	
		
		[Authorize][AcceptVerbs(HttpVerbs.Post)]
		public ActionResult ConnectAvatar(int index)
		{
			Session["Avatar"] 	= this.user.Avatars.ElementAtOrDefault(index);
			Session["Cache"]	= new Cache();
			return RedirectToAction ("", "");	
		}
		
		
		[Authorize]
		public ActionResult DisconnectAvatar()
		{
			Session.Remove("Avatar");
			return RedirectToAction("Account");
		}
		
		
		[Authorize][AcceptVerbs(HttpVerbs.Post)]
		public ActionResult CreateAvatar()
		{
			return RedirectToAction ("", "CreateCharacter");	
		}
		
	}
}