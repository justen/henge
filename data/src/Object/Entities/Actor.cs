using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Actor : MapEntity
	{
		public Actor()
		{
			this.Inventory = new List<Item>();
		}
		
	    public List<Item> Inventory 	{ get; set; }
	}
}