using System;


namespace Henge.Data.Entities
{
	public class Npc : Actor
	{
		public virtual Avatar Master { get; set; }
		
		
		public Npc(ComponentType type) : base(type)
		{
		}
		
		
		public Npc()
		{
		}
		
	   
	}
}