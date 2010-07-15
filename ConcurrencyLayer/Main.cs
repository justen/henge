using System;
using System.IO;
using System.Linq;


using ConcurrencyLayer.Entities;


namespace ConcurrencyLayer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			/*User user = Persistence.Create<User>();
			user.Name = "Dan";
			user.Password = "qwerty";
			Console.WriteLine(user.Name);
			Console.WriteLine(user.Password);
			user.Test();*/
			
			//if (File.Exists("test.yap")) File.Delete("test.yap");
			
			ConcurrencyDataProvider db = new ConcurrencyDataProvider();

			db.Initialise("test.yap", 1);
			
			/*User user = new User { Name = "Dan", Password = "test", Role = new Role { Name = "Code Monkey", Rolex = new Role { Name = "Boss" } } };
			user.Roles.Add(new Role { Name = "Bread" });
			user.RoleDictionary.Add("bread", new Role { Name = "Bread" });
			user.Numbers.Add(42);
			user.Numbers.Add(2);
			user.RoleDictionary.Add("another", user.Roles[0]);
			db.Store<User>(user);//*/

				
			User user	= db.Get<User>(u => u.Name == "Dan");
			
			if (user != null)
			{
				if (db.Lock(user)) Console.WriteLine("Achieved user write lock");
				user.Time = DateTime.Now;
				db.Unlock(user);
				
				Console.WriteLine("user.Name = " + user.Name);
				
				Console.WriteLine("user.Password = " + user.Password);
				Console.WriteLine("user.Time = " + user.Time);
				
				if (db.Lock(user, user.Roles)) Console.WriteLine("Achieved user write lock");
				user.Name = "Thing";
				Console.WriteLine("user.Name = " + user.Name);
				user.Roles.Add(user.Role);
				db.Unlock(user);
				

				if (user.Role != null) 
				{
					Console.WriteLine("user.Role.Name = " + user.Role.Name);
					
					if (user.Role.Rolex != null) Console.WriteLine("user.Role.Rolex.Name = " + user.Role.Rolex.Name);
					
					if (db.Lock(user.Role)) Console.WriteLine("Achieved user.Role write lock");
					user.Role.Rolex = user.Role.Rolex;
					user.Role.Rolex = new Role { Name = "Something" };
					db.Unlock(user.Role);
				}
				
				if (user.Roles != null)
				{
					Console.WriteLine("user.Roles:");
					foreach (Role r in user.Roles)
					{
						Console.WriteLine("  " + r.Name);
					}
					
					Console.WriteLine("user.Numbers:");
					foreach(int n in user.Numbers)
					{
						Console.WriteLine("  " + n);
					}
					
					Console.WriteLine("LINQ for user.Roles:");
					Console.WriteLine("  " + user.Roles.Where(r => r.Name.StartsWith("Code")).Single().Name);
					
				}
				//if (user.RoleDictionary != null) Console.WriteLine("Accessed dictionary");
			}//*/
		}
	}
}

