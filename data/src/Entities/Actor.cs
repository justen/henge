using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Actor : MapEntity
	{
	    public virtual IList<Item> Inventory {get; set;}
		//"learned" skills - i.e., not common to everything of this type
		public virtual IList<Skill> Skills 		{get; set;}
		
	}
}