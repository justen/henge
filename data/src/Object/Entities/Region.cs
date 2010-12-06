using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Region : TraitfulEntity
	{
	    
	    public virtual string Description			{get; set;}
	    public virtual Region Parent				{get; set;}
	    public virtual IList<Location> Locations	{get; set;}	
		
		
		public Region()
		{
			this.Locations = new List<Location>();
		}
	}
}