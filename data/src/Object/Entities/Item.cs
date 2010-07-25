using System;


namespace Henge.Data.Entities
{
	//a carry-able item which may be used or placed in an inventory
	public class Item : PhysicalComponent
	{
		public virtual Component Owner { get; set; }
		
		
		public Item (ComponentType type) : base(type)
		{	 
		}
		
		
		public Item()
		{
		}
	}
}