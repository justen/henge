using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Edifice : MapComponent
	{
	    // Use this if the Edifice is to act as a portal between two maps
	    public Location Portal { get; set; }
		
	    // Things inhabiting the Edifice itself (as distinct from having passed into the map it links to)
	    public IList<Actor> Inhabitants	{ get; set; }
		
		
		public Edifice(ComponentType type) : base (type)
		{
		 	this.Inhabitants = new List<Actor>();	
		}
		
		public Edifice()
		{
			
		}
	}
}