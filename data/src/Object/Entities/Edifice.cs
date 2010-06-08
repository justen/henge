using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Edifice : MapEntity
	{
	    //use this if the Edifice is to act as a portal between two maps
	    private Location InnerLocation 		{get; set;}
	    //Things inhabiting the Edifice itself (as distinct from having passed
	    //into the map it links to)
	    private List<Actor> Inhabitants	{get; set;}
		
		public Edifice()
		{
		 	this.Inhabitants = new List<Actor>();	
		}
	}
}