using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Location : HengeEntity
	{
	    public IList<Avatar> Inhabitants		{ get; set; }
		public IList<Edifice> Structures		{ get; set; }
		public IList<Npc> Fauna					{ get; set; }
		public Coordinates Coordinates			{ get; set; }
		//public int X							{get; set;}			
	    //public int Y							{get; set;}
	    //public int Z							{get; set;}
	    //List of the Regions this Location is in
	    public IList<Region> Regions			{get; set;}
	    //the Map this location exists in
	    public Map Map							{get; set;}
		
		
		public Location(int x, int y, int z)
		{
			this.Coordinates	= new Coordinates { X = x, Y = y, Z = z };
			this.Inhabitants 	= new List<Avatar>();
			this.Structures		= new List<Edifice>();
			this.Fauna			= new List<Npc>();
		}
	}
}