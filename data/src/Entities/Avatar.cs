using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Avatar : Actor
	{
	    public IList<Npc> Pets 			{ get; set; }
	    public IList<LogEntry> Log		{ get; set; }
	    public User User				{ get; set; }
	}
}