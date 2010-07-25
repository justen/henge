using System;
using System.Collections.Generic;
//using Db4objects.Db4o;


namespace Henge.Data.Entities
{
	public class User : ObjectEntity
	{
		//[Indexed]
	    public virtual string Name				{ get; set; }
		public virtual string Password			{ get; set; }	
		public virtual string Clan				{ get; set; }
	    public virtual string Email				{ get; set; }
		public virtual bool Enabled 			{ get; set; }
		public virtual IList<Avatar> Avatars 	{ get; set; }
		public virtual IList<string> Roles		{ get; set; }
		public virtual Avatar CurrentAvatar		{ get; set; }
		//public IList<Account> Account	{ get; set; }
		
		
		public User()
		{
			this.Avatars 	= new List<Avatar>();
			this.Roles		= new List<string>();
		}
	}
}
