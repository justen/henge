using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Map : Entity
	{
	    public IList<Location> Locations 	{ get; set; }
	    public string Name					{ get; set; }
		
		
		public Map()
		{
			this.Locations = new List<Location>();
		}
	}
}