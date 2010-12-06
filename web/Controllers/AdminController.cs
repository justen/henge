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
		protected Avatar currentAvatar {get; set;}
		
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
		public ActionResult EditAvatar(string userName, int avatarId)
		{
			if(this.currentUser == null || this.currentUser.Name != userName) {
				this.currentUser = this.db.Get<User>(u => u.Name == userName);
			}
			
			this.currentAvatar = this.currentUser.Avatars.ElementAt(avatarId);
			
			return View(new UserDetailViewModel(currentUser));
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult DeleteUser(string name)
		{
			if(this.user.Name == name) {
				this.SetError("You cannot delete yourself with the Admin interface.");
			} else {
				User user	=	HengeApplication.DataProvider.Get<User>(x => x.Name == name);
				bool res = false;
				// Check that the user exists
				if (user != null)
				{
					res = UserService.Instance.DeleteUser(user);
				}
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
		
		public ActionResult DeleteDuplicateUsers()
		{
			
			ICollection<string> names = new HashSet<string>();
			ICollection<string> deleted = new HashSet<string>();
			foreach(User user in this.db.Query<User>()) {
				if(names.Contains(user.Name)) {
					deleted.Add(user.Name);
					this.db.Delete(user);
				} else {
					names.Add(user.Name);
				}
			}
			if(deleted.Count > 0) {
				this.SetMessage("Deleted users: " + String.Join(", ", deleted.ToArray()));
			} else {
				this.SetMessage("No errors found.");
			}
			return RedirectToAction("");
		}
	}
}

