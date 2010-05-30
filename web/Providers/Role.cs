using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Security;

using Henge.Web;
using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Web.Providers
{
	/// <summary>
	/// A custom role provider specific to this application. It is based on the standard role system
	/// provided by the .NET framework, but it uses NHibernate as the storage instead.
	/// </summary>
	public class Role : RoleProvider
	{
		public override string ApplicationName 
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}
		
		
		/// <summary>Add the specified list of roles to each of the given users</summary>
		public override void AddUsersToRoles (string [] usernames, string[] rolenames)
		{
			// Db: Get all roles where "Name" is in rolenames
			/*IList<Data.Entities.Role> roles = HengeApplication.DataProvider.CreateCriteria<Data.Entities.Role>()
				.Add(Restrictions.In("Name", rolenames))
				.List<Data.Entities.Role>();
			
			// Db: Get all users where "Name" is in usernames
			IList<User> users = HengeApplication.DataProvider.CreateCriteria<User>()
				.Add(Restrictions.In("Name", usernames))
				.List<User>();*/
			
			var users = from u in HengeApplication.DataProvider.Query<User>()
						where ((IList<string>)usernames).Contains(u.Name)
						select u;
			
			foreach (User user in users)
			{
				// For each of the users found add the set of roles (if they do not already have them)
				foreach (string role in rolenames) if (!user.Roles.Contains(role)) user.Roles.Add(role);
			}
			
			// Persist the changes to the database
			HengeApplication.DataProvider.Flush();
		}
		
		
		/// <summary>Create a new type of role</summary>
		public override void CreateRole(string rolename)
		{
			// Save a new transient role instance to the database			
			//HengeApplication.DataProvider.Update(new Data.Entities.Role() { Name = rolename });
			
		}
		
		
		/// <summary>Delete a role type - warning, this could break database integrity!</summary>
		public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
		{
			/*bool result = false;			
			// Db: "SELECT * FROM Role WHERE Name=rolename"
			Data.Entities.Role role = HengeApplication.DataProvider.CreateCriteria<Data.Entities.Role>()
				.Add(Restrictions.Eq("Name", rolename))
				.UniqueResult<Data.Entities.Role>();
			
			// Check that role exists in the database
			if (role != null)
			{
				// Delete the role and persist the changes
				HengeApplication.DataProvider.Delete(role);
				result = true;
			}
			
			return result;*/
			
			return false;
		}
		
		
		/// <summary>Find all users that have a name similar to usernameToMatch and have the specified role</summary>
		public override string [] FindUsersInRole (string roleName, string usernameToMatch)
		{
			List<string> result	= new List<string>();
			
			// Db: Get user where "Name" is like usernameToMatch and the user's role list contains roleName
			/*IList<User> users = HengeApplication.DataProvider.CreateCriteria<User>()
				.Add(Restrictions.Like("Name", usernameToMatch))
				.CreateCriteria("Roles")
				.Add(Restrictions.Eq("Name", roleName))
				.List<User>();*/
			
			var users = from u in HengeApplication.DataProvider.Query<User>()
						where u.Roles.Contains(roleName)
						select u;
			
			// For each of the relevant users found in the database, add their name to the list
			foreach (User user in users) result.Add(user.Name);
			
			return result.ToArray();
		}
		
		
		/// <summary>Get all roles stored in the database</summary>
		public override string [] GetAllRoles ()
		{
			//List<string> result	= new List<string>();
			
			// For each role in "SELECT * FROM Role" add the name to the list
			//foreach (Data.Entities.Role role in HengeApplication.DataProvider.CreateCriteria<Data.Entities.Role>().List<Data.Entities.Role>()) result.Add(role.Name);
			
			var roles = from u in HengeApplication.DataProvider.Query<User>()
						from string r in u.Roles
						select r;
			
			return roles.Distinct().ToArray();
		}
		
		
		/// <summary>Get all roles belonging to a specific user</summary>
		public override string [] GetRolesForUser (string username)
		{
			/*List<string> result = new List<string>();
			
			// Db: "SELECT * FROM User WHERE Name=username", the role list for that user is joined automatically
			User user = HengeApplication.DataProvider.CreateCriteria<User>()
				.Add(Restrictions.Eq("Name", username))
				.UniqueResult<User>();
			
			// If the user is found then add each of their roles to the list
			if (user != null) foreach (Data.Entities.Role role in user.Roles) result.Add(role.Name);
			
			return result.ToArray();*/
			
			var user =	from u in HengeApplication.DataProvider.Query<User>()
						where u.Name == username 
						select u;
			
			return user.SingleOrDefault().Roles.ToArray();
		}
		
		
		/// <summary>Get all users that have the specified role</summary>
		public override string[] GetUsersInRole (string rolename)
		{
			//List<string> result = new List<string>();
			
			// For each user that has a role named rolename, add their name to the list
			//foreach (User user in HengeApplication.DataProvider.CreateCriteria<User>().CreateCriteria("Roles").Add(Restrictions.Eq("Name", rolename)).List<User>()) result.Add(user.Name);
			
			var users = from u in HengeApplication.DataProvider.Query<User>()
						where u.Roles.Contains(rolename)
						select u.Name;
			
			return users.ToArray();	
		}
		
		
		/// <summary>Check if the specified user has the specified role</summary>
		public override bool IsUserInRole (string username, string rolename)
		{
			// If user exists with "Name=username" and has a role with "Name=rolename" return true
			/*return HengeApplication.DataProvider.CreateCriteria<User>()
				.Add(Restrictions.Eq("Name", username))
				.CreateCriteria("Roles")
				.Add(Restrictions.Eq("Name", rolename))
				.UniqueResult<User>() != null;*/
			
			var user =	from u in HengeApplication.DataProvider.Query<User>()
						where u.Name == username 
						select u;
			
			return user.SingleOrDefault().Roles.Contains(rolename);
		}
		
		
		/// <summary>Remove the specified list of roles from each of the given users</summary>
		public override void RemoveUsersFromRoles (string[] usernames, string[] rolenames)
		{
			// Db: Get all roles where "Name" is in rolenames
			/*IList<Data.Entities.Role> roles = HengeApplication.DataProvider.CreateCriteria<Data.Entities.Role>()
				.Add(Restrictions.In("Name", rolenames))
				.List<Data.Entities.Role>();
			
			// Db: Get all users where "Name" is in usernames
			IList<User> users = HengeApplication.DataProvider.CreateCriteria<User>()
				.Add(Restrictions.In("Name", usernames))
				.List<User>();*/
			
			var users = from u in HengeApplication.DataProvider.Query<User>()
						where ((IList<string>)usernames).Contains(u.Name)
						select u;
			
			foreach (User user in users)
			{
				// For each of the users found remove the set of roles
				foreach (string role in rolenames) user.Roles.Remove(role);
			}
			
			HengeApplication.DataProvider.Flush();
		}
		
		
		/// <summary>Check that a specific role type exists</summary>
		public override bool RoleExists (string rolename)
		{			
			// Get the role called rolename from the database, if it exists return true
			/*return HengeApplication.DataProvider.CreateCriteria<Data.Entities.Role>()
				.Add(Restrictions.Eq("Name", rolename))
				.UniqueResult<Data.Entities.Role>() != null;*/
			
			var roles = from u in HengeApplication.DataProvider.Query<User>()
						where u.Roles.Contains(rolename)
						select true;
			
			return roles.Count() > 0;
		}
	}
}
