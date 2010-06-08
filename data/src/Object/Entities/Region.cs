using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Region : ObjectEntity
	{
	    public string Name				{get; set;}
	    public string Description		{get; set;}
	    public Region Parent			{get; set;}
	    public List<Location> Locations	{get; set;}	
		
		public Region()
		{
			this.Locations = new List<Location>();
		}
	}
}