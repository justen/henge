using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Avatar : Actor
	{
	    public List<Npc> Pets 			{ get; set; }
	    public List<LogEntry> Log		{ get; set; }
	    public User User				{ get; set; }
		
		public Avatar()
		{
			this.Pets = new List<Npc>();
			this.Log = new List<LogEntry>();
		}
	}
}