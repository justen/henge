using System;
using System.Collections.Generic;


namespace ConcurrencyLayer.Entities
{
	public class Role
	{
		public virtual string Name	{ get; set; }
		public virtual Role Rolex { get; set; }
	}
	
	
	public class User
	{
	    public virtual string Name				{ get; set; }
		public virtual string Password			{ get; set; }	
		public virtual DateTime Time			{ get; set; }
		
		public virtual Role Role { get; set; }
		
		public virtual IList<Role> Roles { get; set; }
		public virtual IList<int> Numbers { get; set; }
		public virtual IDictionary<string, Role> RoleDictionary { get; set; }

		//public Avatar CurrentAvatar		{ get; set; }
		
		
		public User()
		{
			this.Roles			= new List<Role>();
			this.Numbers		= new List<int>();
			this.RoleDictionary = new Dictionary<string, Role>();
			//this.Avatars 	= new List<Avatar>();
		}
		
		public void Test()
		{
			Console.WriteLine("Test string");
		}
	}
}
