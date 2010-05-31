using System;
using System.Linq;
using System.Web.Security;

using Henge.Web;
using Henge.Data.Entities;


namespace Henge.Web.Providers
{
	/// <summary>
	/// A custom membership provider specific to this application. It is based on the standard membership system
	/// provided by the .NET framework, but it uses NHibernate as the storage instead. Only the necessary functions
	/// have actually been implemented.
	/// </summary>
	public class Membership : MembershipProvider
	{
		public override string ApplicationName 
		{
			get { throw new System.NotImplementedException(); }
			set { throw new System.NotImplementedException(); }
		}
		
		// Set some hard-coded defaults here. Should probably allow these to be properly configured in a larger scale application
		public override bool EnablePasswordReset					{ get { return false; 							}}
		public override bool EnablePasswordRetrieval				{ get { return false; 							}}	
		public override int MaxInvalidPasswordAttempts				{ get { return 5; 								}}
		public override int MinRequiredNonAlphanumericCharacters	{ get { return 0; 								}}
		public override int MinRequiredPasswordLength				{ get { return 6; 								}}
		public override int PasswordAttemptWindow					{ get { return 30;								}}
		public override MembershipPasswordFormat PasswordFormat		{ get { return MembershipPasswordFormat.Hashed;	}}
		public override string PasswordStrengthRegularExpression	{ get { return string.Empty;					}}
		public override bool RequiresQuestionAndAnswer				{ get { return false; 							}}
		public override bool RequiresUniqueEmail					{ get { return false; 							}}
		

		/// <summary>Change the password for the specified user, making sure that the old password is validated first</summary>
		public override bool ChangePassword (string name, string oldPwd, string newPwd)
		{
			bool result	= false;
			
			//User user = HengeApplication.DataProvider.CreateCriteria<User>().Add(Restrictions.Eq("Name", name)).UniqueResult<User>();
			User user =	HengeApplication.DataProvider.Get<User>(x => x.Name == name);
			
			if (user != null) 
			{
				// Check that the hash of the old password matches what is currently stored in the database
				if (FormsAuthentication.HashPasswordForStoringInConfigFile(oldPwd, "sha1") == user.Password)
				{
					// Update the user with the hashed new password and persist the changes
					user.Password 	= FormsAuthentication.HashPasswordForStoringInConfigFile(newPwd, "sha1");
					result 			= true;
				}
			}
			
			return result;
		}
		

		/// <summary>Create a new user. Because our user model is quite simple, most of the extra parameters are ignored</summary>
		public override MembershipUser CreateUser(string username, string password, string email, string pwdQuestion, string pwdAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			User user = HengeApplication.DataProvider.Get<User>(x => x.Name == username);
			
			if (user == null)
			{
				HengeApplication.DataProvider.Store(
					new User() { Name = username, Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1") }
				);
			
				status = MembershipCreateStatus.Success;
			
				// Return a new MembershipUser with most of the extra values empty or set to Now
				return new MembershipUser("HengeMembershipProvider", username, 0 /*user.Id*/, "", "", "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
			}
			
			status = MembershipCreateStatus.DuplicateUserName;
			return null;
		}
		
		
		/// <summary>Delete a user from persistent storage</summary>
		public override bool DeleteUser(string name, bool deleteAllRelatedData)
		{
			bool result = false;
			User user	=	HengeApplication.DataProvider.Get<User>(x => x.Name == name);
			
			// Check that the user exists
			if (user != null)
			{
				// Delete the user from the database
				HengeApplication.DataProvider.Delete(user);
				result = true;
			}
			
			return result;
		}
		
		
		/// <summary>Request a user from persitent storage</summary>
		public override MembershipUser GetUser(string name, bool userIsOnline)
		{
			User user =	HengeApplication.DataProvider.Get<User>(x => x.Name == name);
			
			// If the user exists return a new MembershipUser with most of hte extra values empty or set to Now
			return (user == null) ? null : new MembershipUser("HengeMembershipProvider", name, 0/*user.Id*/, "", "", "", true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
		}
		
		
		/// <summary>Check that the supplied password is valid for the user</summary>
		public override bool ValidateUser (string name, string password)
		{
			User user =	HengeApplication.DataProvider.Get<User>(x => x.Name == name);
			
			// If the user exists, return true if the passwords match and false otherwise. If the user does not exist return false.
			return (user != null) ? FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1") == user.Password : false;
		}
		
		
		public override bool ChangePasswordQuestionAndAnswer (string name, string password, string newPwdQuestion, string newPwdAnswer)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override MembershipUserCollection FindUsersByEmail (string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override MembershipUserCollection FindUsersByName (string nameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override MembershipUserCollection GetAllUsers (int pageIndex, int pageSize, out int totalRecords)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override int GetNumberOfUsersOnline ()
		{
			throw new System.NotImplementedException();
		}
		
		
		public override string GetPassword (string name, string answer)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override string GetUserNameByEmail (string email)
		{
			throw new System.NotImplementedException();
		}

				
		public override MembershipUser GetUser (object providerUserKey, bool userIsOnline)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override string ResetPassword (string name, string answer)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override bool UnlockUser (string userName)
		{
			throw new System.NotImplementedException();
		}
		
		
		public override void UpdateUser (MembershipUser user)
		{
			throw new System.NotImplementedException();
		}
	}
}





