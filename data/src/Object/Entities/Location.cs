using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Location : Component
	{
	    public IList<Avatar> Inhabitants		{ get; set; }
		public IList<Edifice> Structures		{ get; set; }
		public IList<Npc> Fauna					{ get; set; }
		public Coordinates Coordinates			{ get; set; }

	    //List of the Regions this Location is in
	    public IList<Region> Regions			{ get; set; }
	    //the Map this location exists in
	    public Map Map							{ get; set; }
		
		public IList<Location> Visible			{ get; set; }
		public bool InvertVisibility			{ get; set; }
		
		
		public Location(int x, int y, int z)
		{
			this.Coordinates		= new Coordinates { X = x, Y = y, Z = z };
			this.Inhabitants 		= new List<Avatar>();
			this.Structures			= new List<Edifice>();
			this.Fauna				= new List<Npc>();
			this.Regions 			= new List<Region>();
			this.Visible			= new List<Location>();
			this.InvertVisibility	= true;
		}
		
		
		public bool CanSee(Location target)
		{
			return this.Visible.Contains(target) ^ this.InvertVisibility;	
		}
	}
}