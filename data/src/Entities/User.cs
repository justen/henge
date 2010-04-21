using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class User : Entity
	{
	    public virtual IList<Avatar> Avatars 	{get; set;}
		public virtual bool Enabled 			{get; set;}
	    public virtual string Name				{get; set;}
	    public virtual string Email				{get; set;}
	    public virtual IList<Account> Account	{get; set;}
		//many-to-many
		public virtual IList<Role> Roles		{get; set;}
		public virtual string Password			{get; set;}	
		public virtual string Clan				{get; set;}
	}
}
