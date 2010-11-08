using System;

using Henge.Data;
using Henge.Data.Entities;

namespace Henge.Web
{
	public class UserService
	{
		private static readonly UserService instance = new UserService();

		public static UserService Instance { get { return instance; }}
		
		private DataProvider db;
		
		private UserService () {}
		static UserService() {}
		
		public void Initialise(DataProvider db) {
			this.db = db;
		}
		
		public bool DeleteUser(User user) {
			// Clean up the avatars
			foreach(Avatar avatar in user.Avatars) {
				DeleteAvatar(avatar);
			}
			return this.db.Delete(user);
		}
		
		public bool DeleteAvatar(Avatar avatar) {
			using (this.db.Lock(avatar.Location.Inhabitants, avatar.User.Avatars))
			{
				avatar.Location.Inhabitants.Remove(avatar);
				avatar.User.Avatars.Remove(avatar);	
			}
				
			return this.db.Delete(avatar);
		}
	}
}

