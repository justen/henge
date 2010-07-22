using System;


namespace Henge.Data.Entities
{
	public class Npc : Actor
	{
		public Npc(ComponentType type) : base (type)
		{
		}
		
		public Npc()
		{
		}
		
	    public Avatar Master {get; set;}
	}
}