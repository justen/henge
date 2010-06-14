using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Actor : MapComponent
	{
		public IList<Item> Inventory { get; set; }
		public IDictionary<string, Skill> Skills {get; set;}
		
		public Actor()
		{
			this.Inventory = new List<Item>();
		}    
	}
}