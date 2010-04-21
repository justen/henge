using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Map : Entity
	{
	    public virtual IList<Location> Locations 	{get; set;}
	    public virtual string Name					{get; set;}
	}
}