using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

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
		public ActionResult Index ()
		{
			return View ();
		}
		
		public ActionResult Users()
		{
			IList<User> accounts = this.db.Query<User>().ToList(); 
			
			              
			return View(new UsersViewModel(accounts));
		}
		
		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UserDetail(string name)
		{
			User user = this.db.Get<User>(u => u.Name == name);
			
			return View(new UserDetailViewModel(user));
		}
	}
}

