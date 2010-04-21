using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Avatar : Actor
	{
	    public virtual IList<Npc> Pets 			{get; set;}
	    public virtual IList<LogEntry> Log			{get; set;}
	    public virtual User User					{get; set;}
	}
}