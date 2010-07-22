using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Avatar : Actor
	{
	    public IList<Npc> Pets 			{ get; set; }
	    public IList<LogEntry> Log		{ get; set; }
	    public User User				{ get; set; }
		
		//not sure if we need this but if we want to track avatars through generations...
		public Avatar Parent			{ get; set; }
		
		public Avatar(ComponentType type) : base (type)
		{
			this.Pets = new List<Npc>();
			this.Log = new List<LogEntry>();
		}
		
		public Avatar()
		{
			
		}
	}
}