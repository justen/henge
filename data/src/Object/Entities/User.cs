using System;
using System.Collections.Generic;
using Db4objects.Db4o;


namespace Henge.Data.Entities
{
	public class User : ObjectEntity
	{
		[Indexed]
	    public string Name				{ get; set; }
		public string Password			{ get; set; }	
		public string Clan				{ get; set; }
	    public string Email				{ get; set; }
		public bool Enabled 			{ get; set; }
		public IList<Avatar> Avatars 	{ get; set; }
		public IList<string> Roles		{ get; set; }
		public Avatar CurrentAvatar		{ get; set; }
		//public IList<Account> Account	{ get; set; }
		
		
		public User()
		{
			this.Avatars 	= new List<Avatar>();
			this.Roles		= new List<string>();
		}
	}
}
