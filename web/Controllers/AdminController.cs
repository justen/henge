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
	public class UsersViewModel
	{
		public IList<User> accounts {get; set;}
		
		public UsersViewModel(IList<User> accounts)
		{
			this.accounts = accounts;
		}
	}
	
	public class UserDetailViewModel
	{
		public User user { get; set;}
		
		public UserDetailViewModel(User user)
		{
			this.user = user;
		}
	}
	
	[Authorize][HandleError]
	public class AdminController : MasterController
	{
		protected User currentUser {get; set;}
		
		public ActionResult Index ()
		{
			return View ();
		}
		
		public ActionResult Users()
		{
			IList<User> accounts = this.db.Query<User>().ToList(); 
			this.currentUser = null;
			              
			return View(new UsersViewModel(accounts));
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UserDetail(string name)
		{
			this.currentUser = this.db.Get<User>(u => u.Name == name);
			
			return View(new UserDetailViewModel(currentUser));
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteUser(string name)
		{
			if(this.user.Name == name) {
				this.SetError("You cannot delete yourself with the Admin interface.");
			} else {
				bool res = Membership.DeleteUser(name);
				if(res) {
					this.SetMessage("User Deleted");
				} else {
					this.SetMessage("Could not delete user.");
				}
			}
			return RedirectToAction("Users");	
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
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
					this.db.Delete(avatar);
				}
			}
			
			return RedirectToAction ("Account");	
		}
	}
}

