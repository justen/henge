using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Edifice : MapComponent, IInhabitable
	{
	    // Use this if the Edifice is to act as a portal between two maps
	    public virtual Location Portal { get; set; }
		
	    // Things inhabiting the Edifice itself (as distinct from having passed into the map it links to)
	    public virtual IList<Avatar> Inhabitants	{ get; set; }
		public virtual IList<Npc> Fauna				{ get; set; }
		
		public Edifice(ComponentType type) : base(type)
		{
		 	this.Inhabitants = new List<Avatar>();
			this.Fauna = new List<Npc>();
		}
		
		public Edifice()
		{
			this.Inhabitants = new List<Avatar>();
			this.Fauna = new List<Npc>();
		}
	}
}