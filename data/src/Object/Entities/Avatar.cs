using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Avatar : Actor
	{
	    public virtual IList<Npc> Pets 			{ get; set; }
	    public virtual IList<LogEntry> Log		{ get; set; }
	    public virtual User User				{ get; set; }
		
		//not sure if we need this but if we want to track avatars through generations...
		public virtual IList<string> Ancestors	{ get; set; }
		
		
		public Avatar(ComponentType type) : base(type)
		{
			this.Pets	= new List<Npc>();
			this.Log	= new List<LogEntry>();
			this.Ancestors = new List<string>();
		}
		
		
		public Avatar()
		{
			this.Pets	= new List<Npc>();
			this.Log	= new List<LogEntry>();
			this.Ancestors = new List<string>();
		}
	}
}